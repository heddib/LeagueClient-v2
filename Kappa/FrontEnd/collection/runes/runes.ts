import * as Defer    from './../../defer';
import * as Assets   from './../../assets/assets';

import * as Service  from './service';

let currentBook: Domain.Collection.RuneBook;
let currentPage: Domain.Collection.RunePage;
let callbacks: Function[] = [];

let root: Swish, popup: Swish;

// window.addEventListener('load', () => {
//     root = UI.module('masteries');
//     popup = root.firstChild();
//     $(document.body).add(popup);

//     popup.$('#save-runes').on('click', e => onSaveMasteriesClick(e));
//     popup.$('#revert-runes').on('click', e => onRevertMasteriesClick(e));

//     popup.$('#reset-runes').on('click', e => onResetMasteriesClick(e));
//     popup.$('#close-runes').on('click', e => onCloseMasteriesClick(e));
// });

Defer.auth(() => {
    Service.get().then(book => {
        currentBook = book;

        // createTree(Assets.ddragon.masteries.tree.Ferocity, popup.$('#ferocity-tree'));
        // createTree(Assets.ddragon.masteries.tree.Cunning, popup.$('#cunning-tree'));
        // createTree(Assets.ddragon.masteries.tree.Resolve, popup.$('#resolve-tree'));

        // let active: SpellBookPageDTO;
        // for (let i = 0; i < currentBook.bookPages.length; i++) {
        //     let page = currentBook.bookPages[i];
        //     if (page.current)
        //         active = page;
        // }
        // loadPage(active);
    })
});

export function selected() {
    return currentBook.selected;
}

export function list() {
    return currentBook.pages;
}

export function show(callback?: Function) {
    if (callback) callbacks.push(callback);
    popup.addClass('shown');
}

export function select(page: Domain.Collection.RunePage) {
    currentBook.selected = page.id;

    Service.select(page);
}
/*
function loadPages() {
    let list = popup.$('#mastery-page-list');
    list.empty();
    for (let i = 0; i < currentBook.bookPages.length; i++) {
        let page = currentBook.bookPages[i];
        let node = UI.template(root, 'mastery-page', { name: page.name });
        node.on('click', e => loadPage(page));
        list.add(node);
        if (page.current) {
            node.addClass('active');
        }
    }
}

function loadPage(page?: SpellBookPageDTO) {
    if (page) {
        select(page);
        currentPage = page;
        talents = {};
        for (var i = 0; i < currentPage.talentEntries.length; i++) {
            var entry = currentPage.talentEntries[i];
            talents[entry.talentId] = entry;
        }
    }

    loadPages();

    popup.$('.mastery-icon').do(n => {
        $(n, '.current-points').text = '0';
        n.removeClass('active');
    })

    for (var id in Assets.ddragon.masteries.data) {
        var info = Assets.ddragon.masteries.data[id];
        var node = popup.$('#mastery-icon-' + id);
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

function onMasteryChange(info, tree, row: number, delta: number) {
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

    loadPage();
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

function createTree(src, dst: Swish) {
    for (let y = 0; y < src.length; y++) {
        let row = UI.template(root, 'mastery-row', {});
        for (let x = 0; x < src[y].length; x++) {
            if (src[y][x]) {
                let info = Assets.ddragon.masteries.data[src[y][x].masteryId];
                let icon = UI.template(root, 'mastery-icon', {
                    id: info.id,
                    iconurl: Assets.image('mastery', info.id),
                    ranks: info.ranks,
                    title: info.name,
                });
                if (info.ranks == 1)
                    icon.addClass('single');
                row.add(icon);
                icon.on('wheel', e => onMasteryChange(info, src, y, -e.deltaY / Math.abs(e.deltaY)));
            } else {
                var blank = UI.template(root, 'mastery-icon', { iconurl: '' });
                blank.addClass('blank');
                row.add(blank);
            }
        }
        dst.add(row);
    }
}

function onSaveMasteriesClick(e: MouseEvent) {
    currentPage.talentEntries = [];
    for (var id in talents) {
        currentPage.talentEntries.push(talents[id]);
    }
    Service.save(currentPage);
    loadPage();
}

function onRevertMasteriesClick(e: MouseEvent) {
    loadPage(currentPage);
}

function onResetMasteriesClick(e: MouseEvent) {
    talents = {};
    loadPage();
}

function onCloseMasteriesClick(e: MouseEvent) {
    popup.removeClass('shown');
    loadPage(currentPage);
    while (callbacks[0])
        callbacks.shift()();
}*/