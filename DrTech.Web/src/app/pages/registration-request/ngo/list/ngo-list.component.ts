import { Component, OnInit, TemplateRef } from '@angular/core';
import { LocalDataSource } from 'ng2-smart-table';
import { RegistrationRequestService } from '../../service/registration-request-service';
import { NbDialogService, NbDialogRef } from '@nebular/theme';
import { RecycleDTO } from '../../dto';
import { DropdownDTO } from '../../../../common/dropdown-dto';
import { LocationLinkComponent, UserDetailLinkComponent } from '../../../../common/custom-control';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'ngx-ngo-list',
  templateUrl: './ngo-list.component.html',
  styles: [`
    nb-card {
      transform: translate3d(0, 0, 0);
    }
  `],
})
export class NGOListComponent implements OnInit {
  listViewModel: any[] = [];
  userId: any;
  constructor(public requestService: RegistrationRequestService, private dialogService: NbDialogService, private route: ActivatedRoute) {
    this.userId = route.snapshot.paramMap.get("id");
  }

 async ngOnInit() {
   var response = await this.requestService.GetNgoList();

    if (response.statusCode == 0) {
      this.listViewModel = response.data;
    }
  }
}
