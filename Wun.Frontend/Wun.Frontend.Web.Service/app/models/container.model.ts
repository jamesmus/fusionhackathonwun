export interface IContainer {
	type: string;
	deleted: boolean;
};

class Container implements IContainer {
	type: string = '';
	deleted: boolean = false;
}

export class RealtimeTweetsContainer extends Container {
	type: string = 'realtime-tweets';
}

export class UserTweetsContainer extends Container {
	type: string = 'user-tweets';
}