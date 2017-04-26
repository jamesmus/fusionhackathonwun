import MainComponent from './main';
import ToolbarComponent from './toolbar';
import DashboardComponent from './dashboard';
import TweetComponent from './tweet/tweet.component';

export default {
	declarations: [
		MainComponent,
		ToolbarComponent,
		DashboardComponent,
		TweetComponent
	],
	bootstrap: MainComponent
};