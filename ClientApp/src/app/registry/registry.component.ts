import { Component } from '@angular/core';
import { RegistryService } from './registry.service';
import { UrlBase } from '../base/url-base';

@Component({
  selector: 'app-registry-component',
  templateUrl: './registry.component.html',
})
export class RegistryComponent extends UrlBase {
  aasId: string = '';
  descriptors: any[] = [];

  constructor(private registryService: RegistryService) {
    super();
  }

  async queryLocal() {
    this.descriptors = [];
    this.descriptors = await this.registryService.loadShells(
      this.localRegistryUrl,
      this.aasId
    );
  }
  async queryRemote() {
    this.descriptors = [];
    this.descriptors = await this.registryService.loadShells(
      this.remoteRegistryUrl,
      this.aasId
    );
  }

  getSubmodels(descriptor: any) {
    return descriptor.submodels?.map((submodel: any) =>
      submodel?.keys?.map((key: any) => key.value)
    );
  }

  getSpecificAssetIds(descriptor: any) {
    return descriptor.assetInformation?.specificAssetIds?.map(
      (a: any) => a.name + ': ' + a.value
    );
  }
}
