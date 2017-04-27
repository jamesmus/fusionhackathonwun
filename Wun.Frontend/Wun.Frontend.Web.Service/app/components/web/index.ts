import { Component } from '@angular/core';
import ContainerService from '../../services/container.service';

@Component({
	selector: 'web-app',
	templateUrl: 'web.component.html',
	styleUrls: ['web.component.scss']
})

export default class WebMainComponent {

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