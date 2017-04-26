import { Component, NgZone } from '@angular/core';
import Tweet from '../../models/tweet.model';
import TweetHub from '../../hubs/tweet.hub';

import { IContainer, RealtimeTweetsContainer } from '../../models/container.model';

@Component({
	selector: 'main',
	templateUrl: 'main.component.html',
	styleUrls: ['main.component.scss']
})

export default class MainComponent{
	containers: Array<IContainer> = [];

	public addRTContainerClicked() {
		const container = new RealtimeTweetsContainer(); 
		this.containers.push(container);
	}
}