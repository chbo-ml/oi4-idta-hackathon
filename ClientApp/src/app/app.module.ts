import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { DiscoveryComponent } from './discovery/discovery.component';
import { RepositoryComponent } from './repository/repository.component';
import { RegistryComponent } from './registry/registry.component';

import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { TableModule } from 'primeng/table';
import { MenubarModule } from 'primeng/menubar';
import { CardModule } from 'primeng/card';
import { ToastModule } from 'primeng/toast';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ConfirmationService, MessageService } from 'primeng/api';
import { OverlayModule } from 'primeng/overlay';
import { OverlayPanelModule } from 'primeng/overlaypanel';
import { NgxJsonViewerModule } from 'ngx-json-viewer';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    DashboardComponent,
    DiscoveryComponent,
    RepositoryComponent,
    DiscoveryComponent,
    RegistryComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    ButtonModule,
    InputTextModule,
    TableModule,
    MenubarModule,
    CardModule,
    ToastModule,
    ConfirmDialogModule,
    OverlayModule,
    OverlayPanelModule,
    NgxJsonViewerModule,
    RouterModule.forRoot([
      { path: '', component: DashboardComponent, pathMatch: 'full' },
      { path: 'repository', component: RepositoryComponent },
      { path: 'discovery', component: DiscoveryComponent },
      { path: 'registry', component: RegistryComponent },
    ]),
  ],
  providers: [MessageService, ConfirmationService],
  bootstrap: [AppComponent],
})
export class AppModule {}
