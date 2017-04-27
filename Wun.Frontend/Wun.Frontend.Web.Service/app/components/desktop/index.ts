import { Component } from '@angular/core';
import ContainerService from '../../services/container.service';

@Component({
	selector: 'desktop-app',
	templateUrl: 'desktop.component.html',
	styleUrls: ['desktop.component.scss']
})

export default class DesktopMainComponent {

	constructor(private containerService: ContainerService) { }

	addRTContainerClicked() {
		this.containerService.addRTContainer();
	}

	addUserContainerClicked() {
		this.containerService.addUserContainer();
	}

	removeContainer(container){
		this.containerService.removeContainer(container);
	}

	get containers() {
		return this.containerService.containers;
	}
}