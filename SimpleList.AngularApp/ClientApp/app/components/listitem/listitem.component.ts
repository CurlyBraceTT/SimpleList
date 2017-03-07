import { Component, Input, Output, EventEmitter } from '@angular/core';
import { ListItem } from '../../models/listitem.model';

@Component({
    selector: 'list-item',
    template: require('./listitem.component.html'),
    styles: [require('./listitem.component.css')]
})
export class ListItemComponent {
    editMode: boolean = false;

    @Input()
    item: ListItem;

    @Output()
    itemUpdated = new EventEmitter<ListItem>();
    @Output()
    itemDeleted = new EventEmitter<ListItem>();

    toggle(): void {
        this.item.done = !this.item.done;
        this.itemUpdated.emit(this.item);
    };

    delete(): void {
        this.itemDeleted.emit(this.item);
    };

    edit(element: HTMLInputElement): void {
        this.editMode = true;
        setTimeout(() => { element.focus(); }, 0);
    };

    cancelEdit(element: HTMLInputElement): void {
        this.editMode = false;
        element.value = this.item.description;
    };

    commitEdit(newDescription: string): void {
        this.editMode = false;
        this.item.description = newDescription;
        this.itemUpdated.emit(this.item);
    };
}
