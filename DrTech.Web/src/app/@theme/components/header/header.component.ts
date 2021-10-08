import { Component, Input, OnInit, AfterContentInit } from '@angular/core';

import { NbMenuService, NbSidebarService } from '@nebular/theme';
import { NbAuthService } from '../../../common/auth';
import { UserService } from '../../../@core/data/users.service';
import { AnalyticsService } from '../../../@core/utils/analytics.service';
import { LayoutService } from '../../../@core/data/layout.service';
import { filter } from 'rxjs/operators'
import { Router } from '@angular/router';

@Component({
  selector: 'ngx-header',
  styleUrls: ['./header.component.scss'],
  templateUrl: './header.component.html',
})
export class HeaderComponent implements OnInit, AfterContentInit {

  @Input() position = 'normal';

  user: any;

  userMenu = [{ title: 'Log out' },{ title: 'Change Password' }];

  constructor(private sidebarService: NbSidebarService,
    private menuService: NbMenuService,
    private userService: UserService,
    private analyticsService: AnalyticsService,
    private layoutService: LayoutService,
    private authService: NbAuthService,
    private router: Router) {
  }
  ngAfterContentInit() {
    this.menuService.onItemClick()
      .subscribe((tag) => {
        if (tag != null && tag.item != null && tag.item.title === "Log out") {
          this.router.navigateByUrl('/auth/logout');
        }
        else  if (tag != null && tag.item != null && tag.item.title === "Change Password") {
          this.router.navigateByUrl('/pages/settings/config');
        }
      });
  }
  ngOnInit() {
    this.user = this.authService.getTokenItem().value;
    //debugger;
    //this.userService.getUsers()
    //  .subscribe((users: any) => this.user = users.nick);
  }

  toggleSidebar(): boolean {
    this.sidebarService.toggle(true, 'menu-sidebar');
    this.layoutService.changeLayoutSize();

    return false;
  }

  toggleSettings(): boolean {
    this.sidebarService.toggle(false, 'settings-sidebar');

    return false;
  }

  goToHome() {
    this.menuService.navigateHome();
  }

  startSearch() {
    this.analyticsService.trackEvent('startSearch');
  }
}
