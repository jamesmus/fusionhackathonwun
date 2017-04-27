import Tweet from '../models/tweet.model';

import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, Subject } from 'rxjs/Rx';

@Injectable()
export default class TweetHub {

	private hubLink: string = HUB + 'signalr';
	private hubName: string = 'realtimeTweetsHub';

	private newTweetSubject = new Subject<Tweet>();

	private tweetHub: any;
	
	constructor() {
		this.connect();
	}

	connect() {
		$.connection.hub.url = this.hubLink;

		const connection = $.hubConnection(this.hubLink);
		const tweetHub = connection.createHubProxy(this.hubName);

		tweetHub.on('newTweet', (tweet) => {
			this.newTweetSubject.next(<Tweet>tweet);
		});

		connection.start()
			.done(() => { console.log('Now connected, connection ID=' + connection.id); })
			.fail(() => { console.log('Could not connect'); });
	}

	get newTweet(): Subject<Tweet> {
		return this.newTweetSubject;
	};
}