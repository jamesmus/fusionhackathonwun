import WebMainComponent from './web';
import DesktopMainComponent from './desktop';
import ToolbarComponent from './toolbar';
import DashboardComponent from './dashboard';
import TweetComponent from './tweet';
import TweetListComponent from './tweet-list';
import ContainerComponent from './container';

export default {
	declarations: [
		WebMainComponent,
		DesktopMainComponent,
		ToolbarComponent,
		DashboardComponent,
		TweetComponent,
		TweetListComponent,
		ContainerComponent
	],
	web: WebMainComponent,
	desktop: DesktopMainComponent
};