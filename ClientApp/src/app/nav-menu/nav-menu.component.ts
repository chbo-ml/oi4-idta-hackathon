import { Component, OnDestroy, OnInit } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { MenuItem } from 'primeng/api';
import { Subscription, filter } from 'rxjs';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.scss']
})
export class NavMenuComponent implements OnInit, OnDestroy {
  items: MenuItem[] = [];
  subscriptions: Subscription[] = [];

  constructor(private router: Router) { }

  ngOnDestroy(): void {
    this.subscriptions.forEach(subscription => subscription.unsubscribe());
  }

  ngOnInit(): void {

    this.subscriptions.push( this.router.events.pipe(filter((event) => event instanceof NavigationEnd)).subscribe(() => {
      this.items.forEach(item => {
        item.styleClass = this.router.url === item.routerLink ? 'activeItem' : '';
        console.log(item);
      });
      console.log(this.router.url);
      if (this.router.url === '/') {
        this.items[0].styleClass = 'activeItem';
      }
    }));

    this.items = [
      {
          label: 'Dashboard',
          icon: 'pi pi-fw pi-th-large',
          routerLink: '/',

      },
      {
          label: 'AAS Discovery',
          icon: 'pi pi-fw pi-file',
          routerLink: '/discovery'
      },
      {
          label: 'AAS Registry',
          icon: 'pi pi-fw pi-file',
          routerLink: '/registry'
      },
      {
          label: 'AAS Repository',
          icon: 'pi pi-fw pi-file',
          routerLink: '/repository'
      },
      {
          label: 'Events',
          icon: 'pi pi-fw pi-calendar',
      },
  ];
  }
}
