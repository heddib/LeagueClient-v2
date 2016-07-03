import { Swish, $  } from './../../ui/swish';
import * as Module   from './../../ui/module';
import * as Defer    from './../../defer';
import * as Assets   from './../../assets/assets';

import * as Service  from './service';

const template = (
    <module id="masteries-popup" class="popup">
        <div class="header">
            <span class="window-button exit-button" id="close-masteries"></span>
        </div>
        <div class="content">
            <div class="left">
                <div id="mastery-page-list">
                </div>
                <div class="button-group">
                    <button id="save-masteries">Save</button>
                    <button id="revert-masteries">Revert</button>
                </div>
                <div class="button-group">
                    <button id="reset-masteries">Reset</button>
                    <button id="delete-masteries">Delete</button>
                </div>
            </div>
            <div class="body" data-ref="treeContainer">
            </div>
        </div>
    </module>
);

const pageTemplate = (
    <div class="mastery-page">
        <span data-ref="name"/>
    </div>
);

const rowTemplate = (
    <div class="mastery-row">

    </div>
);

const iconTemplate = (
    <div class="mastery-icon">
        <div class="popup">
            <span class="title" data-ref="title"/>
            <span class="description" data-ref="description"/>
        </div>
        <img data-ref="icon"/>
        <span class="mastery-icon-points"><span data-ref="current">0</span>/<span data-ref="max"/></span>
    </div>
);

let currentBook: Domain.Masteries.MasteryBookDTO;
let currentPage: Domain.Masteries.MasteryBookPageDTO;
let talents: { [id: number]: Domain.Masteries.TalentEntry } = {};
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

export function select(page: Domain.Masteries.MasteryBookPageDTO) {
    currentBook.bookPages.forEach(b => b.current = false);
    page.current = true;
    Service.select(page);
}

function getRowSum(row: number[]) {
    var sum = 0;
    for (var id of row) {
        var entry = talents[id];
        if (entry) sum += entry.rank;
    }
    return sum;
}

class Masteries extends Module.default {
    private icons: { [id: number]: Module.AnonymousModule } = {};

    constructor() {
        super(template);

        this.$('#save-masteries').on('click', (e: MouseEvent) => this.onSaveMasteriesClick(e));
        this.$('#revert-masteries').on('click', (e: MouseEvent) => this.onRevertMasteriesClick(e));

        this.$('#reset-masteries').on('click', (e: MouseEvent) => this.onResetMasteriesClick(e));
        this.$('#close-masteries').on('click', (e: MouseEvent) => this.onCloseMasteriesClick(e));

        Defer.auth(() => {
            Service.get().then(book => this.onBook(book));
        });
    }

    private onBook(book: Domain.Masteries.MasteryBookDTO) {
        currentBook = book;

        for (var group of Assets.gamedata.masteries.tree.groups) {
            var node = document.createElement('div');
            node.classList.add('mastery-tree');
            this.refs.treeContainer.add(node);
            this.createTree(group, $(node));
        }

        let active: Domain.Masteries.MasteryBookPageDTO;
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
            let node = Module.default.create(pageTemplate);
            node.refs.name.text = page.name;
            node.node.on('click', e => this.renderPage(page));
            node.render(list);
            node.node.setClass(page.current, 'active');
        }
    }

    private renderPage(page?: Domain.Masteries.MasteryBookPageDTO) {
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

        for (var id in this.icons) {
            let icon = this.icons[id];
            icon.refs.current.text = '0';
        }

        for (var id in Assets.gamedata.masteries.data) {
            var info = Assets.gamedata.masteries.data[id];
            let icon = this.icons[info.id];
            var talent = talents[info.id];
            var rank = 0;
            icon.node.setClass(talent && talent.rank != 0, 'active')
            if (!talent || !talent.rank) {
                delete talents[id];
            } else {
                rank = talent.rank;
            }
            icon.refs.current.text = rank;
            icon.refs.description.html = info.description[Math.max(rank - 1, 0)];
        }
    }

    private onMasteryChange(info: Domain.GameData.Mastery, tree: Domain.GameData.MasteryGroup, row: number, delta: number) {
        let entry = talents[info.id];
        let changed = (entry ? entry.rank : 0) + delta;

        if (changed > info.maxRank || changed < 0)
            return;

        //Steal from other icons in row//
        let currentRow = getRowSum(tree.rows[row].masteries);
        currentRow += delta;

        //Steal from other icons in row//
        if (currentRow > tree.rows[row].maxPointsInRow) {
            let other = tree.rows[row].masteries.filter(n => n != info.id && !!talents[n]);
            talents[other[0]].rank--;
        }

        if (delta < 0) {
            //Check for masteries in higher rows//
            for (let y = row + 1; y < tree.rows.length; y++) {
                for (let x = 0; x < tree.rows[y].masteries.length; x++) {
                    if (talents[tree.rows[y].masteries[x]]) return;
                }
            }
        } else {
            //Check for masteries in lower rows//
            for (let y = 0; y < row; y++) {
                let above = getRowSum(tree.rows[y].masteries);
                if (above < tree.rows[y].maxPointsInRow) return;
            }

            //Check for mastery limit//
            let count = 0;
            for (let key in talents)
                count += talents[key].rank;
            if (count >= 30) return;
        }

        talents[info.id] = { rank: changed, talentId: info.id } as Domain.Masteries.TalentEntry;

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

    private createTree(src: Domain.GameData.MasteryGroup, dst: Swish) {
        for (let y = 0; y < src.rows.length; y++) {
            let row = Module.default.create(rowTemplate);
            row.node.setClass(src.rows[y].maxPointsInRow == 1, 'single');
            for (let id of src.rows[y].masteries) {
                let info = Assets.gamedata.masteries.data[id];
                let icon = Module.default.create(iconTemplate);
                icon.refs.icon.src = Assets.masteries.icon(info.id);
                icon.refs.max.text = info.maxRank;
                icon.refs.title.text = info.name;

                icon.node.setClass(info.maxRank == 1, 'single');

                icon.render(row.node);
                icon.node.on('wheel', (e: WheelEvent) => this.onMasteryChange(info, src, y, -e.deltaY / Math.abs(e.deltaY)));

                this.icons[info.id] = icon;
            }
            row.render(dst);
        }
    }
}