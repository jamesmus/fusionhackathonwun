export interface IContainer {
	type: string;
};

export class RealtimeTweetsContainer implements IContainer {
	type: string = 'realtime-tweets';
}

export class UserTweetsContainer implements IContainer {
	type: string = 'user-tweets';
}