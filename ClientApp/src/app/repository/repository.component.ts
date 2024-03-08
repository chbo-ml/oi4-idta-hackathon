import { Component } from '@angular/core';
import { RepositoryService } from './repository.service';
import { AssetAdministrationShell } from '@aas-core-works/aas-core3.0-typescript/types';
import { UrlBase } from '../base/url-base';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './repository.component.html',
})
export class RepositoryComponent extends UrlBase{

  shells: (AssetAdministrationShell | null)[] = [];
  isLocal = false;
  aasId = '';

  constructor(private repositoryService: RepositoryService) {
    super();
  }

  async loadLocalRegistry() {
    if (this.aasId === '') {
    this.shells = await this.repositoryService.loadShells(
      this.localRegistryUrl
    );
    } else {
      this.shells = await this.repositoryService.loadShell(
        this.localRegistryUrl, this.aasId
      );
    }
    this.isLocal = true;
  }

  async loadRemoteRegistry() {
    if (this.aasId === '') {
      this.shells = await this.repositoryService.loadShells(
        this.remoteRegistryUrl
      );
      } else {
        this.shells = await this.repositoryService.loadShell(
          this.remoteRegistryUrl, this.aasId
        );
      }
      this.isLocal = false;
  }

  import(aasId: string) {
    this.repositoryService.import(
      this.localRegistryUrl,
      this.remoteRegistryUrl,
      aasId
    );
  }
}
