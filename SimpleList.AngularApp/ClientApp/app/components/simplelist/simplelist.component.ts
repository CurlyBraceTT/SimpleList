import { Component, OnInit } from '@angular/core';
import { ListItem } from '../../models/listitem.model';
import { ListItemsService } from '../../services/listitems.service';

@Component({
    selector: 'simple-list',
    template: require('./simplelist.component.html'),
    styles: [require('./simplelist.component.css')]
})
export class SimpleListComponent implements OnInit {
    items: ListItem[];
    newItem: ListItem = new ListItem();

    constructor(private listItemService: ListItemsService) { }

    ngOnInit(): void {
        this.listItemService.getAll()
            .then(items => this.items = items);
    };

    addItem(): void {
        if (!this.newItem.description) {
            return;
        }

        this.listItemService.create(this.newItem)
            .then(item => this.items.push(item));
        this.newItem = new ListItem();
    };

    itemDeleted(item: ListItem): void {
        this.listItemService
            .delete(item.id)
            .then(() => {
                this.items = this.items.filter(i => i !== item);
            });
    };
}
