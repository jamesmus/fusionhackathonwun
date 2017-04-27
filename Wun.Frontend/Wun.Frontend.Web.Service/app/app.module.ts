import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialModule } from '@angular/material';
import { FlexLayoutModule } from '@angular/flex-layout';
import { FormsModule } from '@angular/forms';

import components from './components';
import services from './services';
import hubs from './hubs';

const bootstrap = TARGET == 'web' ? components.web : components.desktop;

@NgModule({
	imports:
	[
		BrowserModule,
		BrowserAnimationsModule,
		MaterialModule,
		FormsModule,
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
	bootstrap: [bootstrap]
})
export default  class AppModule { }