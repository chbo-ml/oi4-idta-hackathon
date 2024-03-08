import { Component } from '@angular/core';
import { DiscoveryService } from './discovery.service';
import { UrlBase } from '../base/url-base';

@Component({
  selector: 'app-discovery-component',
  templateUrl: './discovery.component.html'
})
export class DiscoveryComponent extends UrlBase {
  assetId: string = '';
  aasIds: string[] = [];

  constructor(private discoveryService: DiscoveryService) {
    super();
  }

  async queryLocal() {
    this.aasIds = await this.discoveryService.query(this.localRegistryUrl, this.assetId);
  }
  async queryRemote() {
    this.aasIds = await this.discoveryService.query(this.remoteRegistryUrl, this.assetId);
  }
}
