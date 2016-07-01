import { Swish, $  } from './../../ui/swish';
import Module        from './../../ui/module';
import * as Defer    from './../../defer';
import * as Assets   from './../../assets/assets';

import * as Service  from './service';

const html = Module.import('collection/masteries');

let currentBook: MasteryBookDTO;
let currentPage: MasteryBookPageDTO;
let talents: { [id: number]: TalentEntry } = {};
let callbacks: Function[] = [];

let masteries: Masteries;

window.addEventListener('load', () => {
    masteries = new Masteries();
    $(document.body).add(masteries.node);
});

export function list() {
    return currentBook.bookPages;
}

export function show(callback?: Function) {
    if (callback) callbacks.push(callback);
    masteries.node.addClass('shown');
}

export function select(page: MasteryBookPageDTO) {
    currentBook.bookPages.forEach(b => b.current = false);
    page.current = true;
    Service.select(page);
}

function getRowPoints(row: any[]) {
    var rowSum = 0, rowMax = 0;
    for (var x = 0; x < row.length; x++) {
        if (row[x]) {
            var id = row[x].masteryId;
            var entry = talents[id];
            if (entry) {
                rowSum += entry.rank;
            }
            rowMax = Assets.ddragon.masteries.data[id].ranks;
        }
    }
    return { sum: rowSum, max: rowMax };
}

class Masteries extends Module {
    constructor() {
        super(html);

        this.$('#save-masteries').on('click', (e: MouseEvent) => this.onSaveMasteriesClick(e));
        this.$('#revert-masteries').on('click', (e: MouseEvent) => this.onRevertMasteriesClick(e));

        this.$('#reset-masteries').on('click', (e: MouseEvent) => this.onResetMasteriesClick(e));
        this.$('#close-masteries').on('click', (e: MouseEvent) => this.onCloseMasteriesClick(e));

        Defer.auth(() => {
            Service.get().then(book => this.onBook(book));
        });
    }

    private onBook(book: MasteryBookDTO) {
        currentBook = book;

        this.createTree(Assets.ddragon.masteries.tree.Ferocity, this.$('#ferocity-tree'));
        this.createTree(Assets.ddragon.masteries.tree.Cunning, this.$('#cunning-tree'));
        this.createTree(Assets.ddragon.masteries.tree.Resolve, this.$('#resolve-tree'));

        let active: MasteryBookPageDTO;
        for (let i = 0; i < currentBook.bookPages.length; i++) {
            let page = currentBook.bookPages[i];
            if (page.current)
                active = page;
        }
        this.renderPage(active);
    }

    private renderPageList() {
        let list = this.$('#mastery-page-list');
        list.empty();
        for (let i = 0; i < currentBook.bookPages.length; i++) {
            let page = currentBook.bookPages[i];
            let node = this.template('mastery-page', { name: page.name });
            node.on('click', e => this.renderPage(page));
            list.add(node);
            if (page.current) {
                node.addClass('active');
            }
        }
    }

    private renderPage(page?: MasteryBookPageDTO) {
        if (page) {
            select(page);
            currentPage = page;
            talents = {};
            for (var i = 0; i < currentPage.talentEntries.length; i++) {
                var entry = currentPage.talentEntries[i];
                talents[entry.talentId] = entry;
            }
        }

        this.renderPageList();

        this.$('.mastery-icon').do(n => {
            $(n, '.current-points').text = '0';
            n.removeClass('active');
        })

        for (var id in Assets.ddragon.masteries.data) {
            var info = Assets.ddragon.masteries.data[id];
            var node = this.$('#mastery-icon-' + id);
            var talent = talents[id];
            var rank = 0;
            if (!talent || !talent.rank) {
                node.removeClass('active');
                delete talents[id];
            } else {
                rank = talent.rank;
                node.addClass('active');
            }
            $(node, '.current-points').text = rank;
            $(node, '.description').html = info.description[Math.max(rank - 1, 0)];
        }
    }

    private onMasteryChange(info, tree, row: number, delta: number) {
        let entry = talents[info.id];
        let changed = (entry ? entry.rank : 0) + delta;

        if (changed > info.ranks || changed < 0)
            return;

        //Steal from other icons in row//
        let currentRow = getRowPoints(tree[row]);
        currentRow.sum += delta;

        //Steal from other icons in row//
        if (currentRow.sum > currentRow.max) {
            let other = tree[row].filter(n => n && n.masteryId != info.id && talents[n.masteryId]);
            talents[other[0].masteryId].rank--;
        }

        if (delta < 0) {
            //Check for masteries in higher rows//
            for (let y = row + 1; y < tree.length; y++) {
                for (let x = 0; x < tree[y].length; x++) {
                    if (tree[y][x] && talents[tree[y][x].masteryId])
                        return;
                }
            }
        } else {
            //Check for masteries in lower rows//
            for (let y = 0; y < row; y++) {
                let above = getRowPoints(tree[y]);
                if (above.sum < above.max) return;
            }

            //Check for mastery limit//
            let count = 0;
            for (let key in talents)
                count += talents[key].rank;
            if (count >= 30) return;
        }

        talents[info.id] = <TalentEntry>{ rank: changed, talentId: info.id };

        this.renderPage();
    }

    private onSaveMasteriesClick(e: MouseEvent) {
        currentPage.talentEntries = [];
        for (var id in talents) {
            currentPage.talentEntries.push(talents[id]);
        }
        Service.save(currentPage);
        this.renderPage();
    }

    private onRevertMasteriesClick(e: MouseEvent) {
        this.renderPage(currentPage);
    }

    private onResetMasteriesClick(e: MouseEvent) {
        talents = {};
        this.renderPage();
    }

    private onCloseMasteriesClick(e: MouseEvent) {
        this.node.removeClass('shown');
        this.renderPage(currentPage);
        while (callbacks[0])
            callbacks.shift()();
    }

    private createTree(src, dst: Swish) {
        for (let y = 0; y < src.length; y++) {
            let row = this.template('mastery-row', {});
            for (let x = 0; x < src[y].length; x++) {
                if (src[y][x]) {
                    let info = Assets.ddragon.masteries.data[src[y][x].masteryId];
                    let icon = this.template('mastery-icon', {
                        id: info.id,
                        iconurl: Assets.image('mastery', info.id),
                        ranks: info.ranks,
                        title: info.name,
                    });
                    if (info.ranks == 1)
                        icon.addClass('single');
                    row.add(icon);
                    icon.on('wheel', (e: WheelEvent) => this.onMasteryChange(info, src, y, -e.deltaY / Math.abs(e.deltaY)));
                } else {
                    var blank = this.template('mastery-icon', { iconurl: '' });
                    blank.addClass('blank');
                    row.add(blank);
                }
            }
            dst.add(row);
        }
    }
}