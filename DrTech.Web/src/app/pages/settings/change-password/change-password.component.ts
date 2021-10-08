import { Component, OnInit, Output, EventEmitter, TemplateRef } from '@angular/core';
import { GridDataResult, PageChangeEvent } from '@progress/kendo-angular-grid';
import { NbDialogService, NbDialogRef } from '@nebular/theme';
import { compileFilter } from '@progress/kendo-data-query';
import { ActivatedRoute, Router } from '@angular/router';
import { SettingsService } from '../settings.service'

@Component({
  selector: 'ngx-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.scss']
})
export class ChangePasswordComponent implements OnInit {
  _viewModel: any = {};
ResponseMessage : string = "";
  constructor(public settingsService: SettingsService, private route: ActivatedRoute, private router: Router) { }

  ngOnInit() {
  }

  async onSubmit() {
    var formResponse = await this.settingsService.ChangePassword(this._viewModel);
    if (formResponse.statusCode == 0 && formResponse.data == true)
    {
      this.router.navigateByUrl('/auth/logout');
    }
    else{
      this.ResponseMessage = formResponse.statusMessage;
    }
     // this.router.navigate(["/pages/settings/setting"]);

  }


}
