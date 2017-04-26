import { Component, NgZone, Input } from '@angular/core';
import { IContainer } from '../../models/container.model';

@Component({
	selector: 'dashboard',
	templateUrl: 'dashboard.component.html'
})

export default class DashboardComponent {
	@Input()
	containers: Array<IContainer> = [];
}