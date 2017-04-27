import { Component, NgZone, Input, Output, EventEmitter } from '@angular/core';
import { IContainer } from '../../models/container.model';

@Component({
	selector: 'dashboard',
	templateUrl: 'dashboard.component.html',
	styleUrls: ['dashboard.component.scss']
})

export default class DashboardComponent {
	@Input()
	containers: Array<IContainer> = [];
	@Input()
	columnSize: number = 30;
	@Output()
   public removed = new EventEmitter();

	removeContainer(container) {
		if (this.removed) {
			this.removed.emit(container);
		}
	}
}