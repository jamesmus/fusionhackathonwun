import { Component, NgZone } from '@angular/core';
import Tweet from '../../models/tweet.model';
import TweetHub from '../../hubs/tweet.hub';

import { IContainer, RealtimeTweetsContainer, UserTweetsContainer } from '../../models/container.model';

@Component({
	selector: 'main',
	templateUrl: 'main.component.html',
	styleUrls: ['main.component.scss']
})

export default class MainComponent{
	containers: Array<IContainer> = [];

	addRTContainerClicked() {
		const container = new RealtimeTweetsContainer(); 
		this.containers.push(container);
	}

	addUserContainerClicked() {
		const container = new UserTweetsContainer(); 
		this.containers.push(container);
	}

	removeContainer(container){
		this.containers = this.containers.filter(item => item != container);
	}
}