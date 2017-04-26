import { Component, NgZone } from '@angular/core';
import Tweet from '../../models/tweet.model';
import TweetHub from '../../hubs/tweet.hub';

@Component({
	selector: 'tweet-list',
	templateUrl: 'tweet-list.component.html',
	styleUrls: ['tweet-list.component.scss']
})

export default class TweetListComponent {

	tweets: Array<Tweet> = [];

	constructor(private tweetHub: TweetHub, private zone: NgZone) {
		this.tweetHub.newTweet.subscribe((tweet) => {
			this.zone.run(() => {
				this.tweets = [tweet, ...this.tweets.slice(0, 9)];
			});
		});
	}
}