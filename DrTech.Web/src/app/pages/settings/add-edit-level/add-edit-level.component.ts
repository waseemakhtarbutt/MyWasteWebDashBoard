import { Component, OnInit, Output, EventEmitter, TemplateRef } from '@angular/core';
import { GridDataResult, PageChangeEvent } from '@progress/kendo-angular-grid';
import { NbDialogService, NbDialogRef } from '@nebular/theme';
import { compileFilter } from '@progress/kendo-data-query';
import { ActivatedRoute, Router } from '@angular/router';
import { SettingsService } from '../settings.service';

@Component({
  selector: 'ngx-add-edit-level',
  templateUrl: './add-edit-level.component.html',
  styleUrls: ['./add-edit-level.component.scss']
})

export class AddEditLevelComponent implements OnInit {

  _viewModel: any = {};
  itemslist: any;
  subitemslist: any;
  show: boolean = false;
  donationFlag = false;

  constructor(public settingsService: SettingsService, private route: ActivatedRoute, private router: Router) { }

  async ngOnInit() {
    var ID = this.route.snapshot.paramMap.get("id");
    if (ID != null) {
      // this.showHide=true;
      var listresponse = await this.settingsService.GetGPNLevelById(ID);
      this.show = true;
      if (listresponse.statusCode == 0)

        this._viewModel = listresponse.data;
      this._viewModel.title = "Update GPN Level";
    }
    else {
      this.show = false;
      this._viewModel.title = "Add GPN Level";
    }
  }
  async onSubmit() {
    var formResponse = await this.settingsService.SaveGPNLevel(this._viewModel);
    if (formResponse.statusCode == 0)
      this.router.navigate(["/pages/settings/setting"]);

  }
  getSelectedOptionText(event: Event) {
    let selectElementText = event.target['options']
    [event.target['options'].selectedIndex].text;
    if (selectElementText == "Other") {
      this.donationFlag = true;
      this.subitemslist = null;
    }
    else {
      this._viewModel.typeDescription = "";
      this._viewModel.subTypeDescription = "";
      this.donationFlag = false;

    }

  }
  numberOnly(event): boolean {
    const charCode = (event.which) ? event.which : event.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
      return false;
    }
    return true;

  }
}
