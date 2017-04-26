import { Component, NgZone } from '@angular/core';
import Tweet from '../../models/tweet.model';
import TweetHub from '../../hubs/tweet.hub';

@Component({
	selector: 'dashboard',
	templateUrl: 'dashboard.component.html'
})

export default class DashboardComponent {
	tweet: Tweet = <Tweet>{
		displayName: 'Donald Trump',
		content: 'Some stupid tweet from DT',
		created: '4/26/2017'
	};

	constructor(private tweetHub: TweetHub, private zone: NgZone) {
		this.tweetHub.newTweet.subscribe((tweet) => {
			this.zone.run(() => {
				this.tweet = tweet;
			});
		});
	}
}