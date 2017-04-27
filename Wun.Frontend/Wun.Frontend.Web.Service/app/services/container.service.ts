import { IContainer, RealtimeTweetsContainer, UserTweetsContainer } from './../models/container.model';
import { Injectable } from '@angular/core';

@Injectable()
export default class ContainerService {

	public containers: Array<IContainer> = [];

	addRTContainer() {
		const container = new RealtimeTweetsContainer(); 
		this.containers.push(container);
	}

	addUserContainer() {
		const container = new UserTweetsContainer(); 
		this.containers.push(container);
	}

	removeContainer(container){
		this.containers = this.containers.filter(item => item != container);
	}
}
