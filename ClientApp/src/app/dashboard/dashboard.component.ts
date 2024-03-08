import { Component, OnInit } from '@angular/core';
import { DashboardService } from './dashboard.service';
import { UrlBase } from '../base/url-base';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
})
export class DashboardComponent extends UrlBase implements OnInit {
  countRepo = 0;
  countReg: any;

  constructor(private dashboardService: DashboardService) {
    super();
  }

  ngOnInit(): void {
    this.count();
  }

  async count() {
    this.countRepo = await this.dashboardService.countShellsRepository(this.localRegistryUrl);
    this.countReg = await this.dashboardService.countShellsRegistry(this.localRegistryUrl);
  }
}
