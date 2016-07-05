import { Swish, $  } from './../../ui/swish';
import Module        from './../../ui/module';
import Popup         from './../../ui/popup';
import * as Tooltip  from './../../ui/tooltip';
import * as Defer    from './../../defer';
import * as Assets   from './../../assets/assets';

import * as Service  from './service';

const template = (
    <module class="masteries-page">
        <div class="left">
            <x-flexpadd/>
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
            <x-flexpadd style="flex: 6"/>
        </div>
        <div class="body" data-ref="treeContainer">
        </div>
    </module>
);

const pageTemplate = (
    <div class="mastery-page">
        <span data-ref="name"/>
    </div>
);

const rowTemplate = (
    <div class="mastery-row"></div>
);

const iconTemplate = (
    <div class="mastery-icon">
        <img data-ref="icon"/>
        <span class="mastery-icon-points"><span data-ref="current">0</span>/<span data-ref="max"/></span>
    </div>
);

let currentBook: Domain.Collection.MasteryBook;
let currentPage: Domain.Collection.MasteryPage;

Defer.auth(() => {
    Service.get().then(book => currentBook = book);
});

export function selected() {
    return currentBook.selected;
}

export function list() {
    return currentBook.pages;
}

export function popup() {
    let popup = new MasteriesPopup();
    popup.show();
    return popup;
}

export function select(page: Domain.Collection.MasteryPage) {
    currentBook.selected = page.id;

    Service.select(page);
}

export class Page extends Module {
    private icons: { [id: number]: any } = {};

    constructor() {
        super(template);

        this.$('#save-masteries').on('click', (e: MouseEvent) => this.onSaveMasteriesClick(e));
        this.$('#revert-masteries').on('click', (e: MouseEvent) => this.onRevertMasteriesClick(e));

        this.$('#reset-masteries').on('click', (e: MouseEvent) => this.onResetMasteriesClick(e));

        this.load();
    }

    private load() {
        if (!currentBook) {
            setTimeout(() => this.load(), 500);
            return;
        }
        for (var group of Assets.gamedata.masteries.tree.groups) {
            var node = document.createElement('div');
            node.classList.add('mastery-tree');
            this.refs.treeContainer.add(node);
            this.createTree(group, $(node));
        }

        let active: Domain.Collection.MasteryPage;
        for (let i = 0; i < currentBook.pages.length; i++) {
            let page = currentBook.pages[i];
            if (page.id == currentBook.selected)
                active = page;
        }
        this.renderPage(active);
    }

    private renderPageList() {
        let list = this.$('#mastery-page-list');
        list.empty();
        for (let i = 0; i < currentBook.pages.length; i++) {
            let page = currentBook.pages[i];
            let node = Module.create(pageTemplate);
            node.refs.name.text = page.name;
            node.node.on('click', e => this.renderPage(page));
            node.render(list);
            node.node.setClass(page.id == currentBook.selected, 'active');
        }
    }

    private renderPage(page?: Domain.Collection.MasteryPage) {
        if (page) {
            select(page);
            currentPage = page;
        }

        this.renderPageList();

        for (var id in this.icons) {
            let icon = this.icons[id];
            icon.refs.current.text = '0';
        }

        for (var id in Assets.gamedata.masteries.data) {
            var info = Assets.gamedata.masteries.data[id];
            let icon = this.icons[info.id];

            let rank = currentPage.masteries[info.id] || 0;

            icon.node.setClass(rank != 0, 'active')
            if (rank == 0)
                delete currentPage.masteries[id];
            icon.refs.current.text = rank;
        }
    }

    private onMasteryChange(info: Domain.GameData.Mastery, tree: Domain.GameData.MasteryGroup, row: number, delta: number) {
        let changed = (currentPage.masteries[info.id] || 0) + delta;

        if (changed > info.maxRank || changed < 0)
            return;

        //Steal from other icons in row//
        let currentRow = getRowSum(tree.rows[row].masteries);
        currentRow += delta;

        //Steal from other icons in row//
        if (currentRow > tree.rows[row].maxPointsInRow) {
            let other = tree.rows[row].masteries.filter(n => n != info.id && !!currentPage.masteries[n]);
            currentPage.masteries[other[0]]--;
        }

        if (delta < 0) {
            //Check for masteries in higher rows//
            for (let y = row + 1; y < tree.rows.length; y++) {
                for (let x = 0; x < tree.rows[y].masteries.length; x++) {
                    if (currentPage.masteries[tree.rows[y].masteries[x]])
                        return;
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
            for (let key in currentPage.masteries)
                count += currentPage.masteries[key];
            if (count >= 30) return;
        }

        currentPage.masteries[info.id] = changed;

        this.renderPage();
    }

    private onSaveMasteriesClick(e: MouseEvent) {
        Service.save(currentPage);
        this.renderPage();
    }

    private onRevertMasteriesClick(e: MouseEvent) {
        this.renderPage(currentPage);
    }

    private onResetMasteriesClick(e: MouseEvent) {
        currentPage.masteries = {};
        this.renderPage();
    }

    private createTree(src: Domain.GameData.MasteryGroup, dst: Swish) {
        for (let y = 0; y < src.rows.length; y++) {
            let row = Module.create(rowTemplate);
            row.node.setClass(src.rows[y].maxPointsInRow == 1, 'single');
            for (let id of src.rows[y].masteries) {
                let info = Assets.gamedata.masteries.data[id];
                let icon = Module.create(iconTemplate);
                icon.refs.icon.src = Assets.masteries.icon(info.id);
                icon.refs.max.text = info.maxRank;

                icon.node.setClass(info.maxRank == 1, 'single');

                icon.render(row.node);
                icon.node.on('wheel', (e: WheelEvent) => this.onMasteryChange(info, src, y, -e.deltaY / Math.abs(e.deltaY)));

                Tooltip.top(icon.node, new MasteryTooltip(info));

                this.icons[info.id] = icon;
            }
            row.render(dst);
        }
    }
}

function getRowSum(row: number[]) {
    var sum = 0;
    for (var id of row) {
        sum += currentPage.masteries[id] || 0;
    }
    return sum;
}

class MasteriesPopup extends Popup {
    constructor() {
        super("Masteries", new Page());
    }
}

const tooltipTemplate = (
    <module class="masteries-tooltip">
        <span class="title" data-ref="title"/>
        <span class="description" data-ref="description"/>
    </module>
);

class MasteryTooltip extends Tooltip.Content {
    private info: Domain.GameData.Mastery;

    constructor(info: Domain.GameData.Mastery) {
        super(tooltipTemplate);

        this.info = info;

        this.refs.title.text = info.name;
    }

    public onshow() {
        let rank = currentPage.masteries[this.info.id] || 0;
        this.refs.description.html = this.info.description[Math.max(rank - 1, 0)];
    }
}