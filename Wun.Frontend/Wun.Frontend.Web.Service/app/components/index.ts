import MainComponent from './main';
import ToolbarComponent from './toolbar';
import DashboardComponent from './dashboard';
import TweetComponent from './tweet';
import TweetListComponent from './tweet-list';
import ContainerComponent from './container';

export default {
	declarations: [
		MainComponent,
		ToolbarComponent,
		DashboardComponent,
		TweetComponent,
		TweetListComponent,
		ContainerComponent
	],
	bootstrap: MainComponent
};