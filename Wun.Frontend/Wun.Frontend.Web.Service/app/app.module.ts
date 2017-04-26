import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialModule } from '@angular/material';
import { FlexLayoutModule } from '@angular/flex-layout';

import components from './components';
import services from './services';
import hubs from './hubs';

@NgModule({
	imports:
	[
		BrowserModule,
		BrowserAnimationsModule,
		MaterialModule,
		FlexLayoutModule
	],
	declarations:
	[
		...components.declarations
	],
	providers: [
		...services,
		...hubs
	],
	bootstrap: [components.bootstrap]
})
export default  class AppModule { }