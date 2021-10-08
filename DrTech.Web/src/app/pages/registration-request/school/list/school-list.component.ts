import { Component, OnInit} from '@angular/core';
import { RegistrationRequestService } from '../../service/registration-request-service';
import { NbDialogService} from '@nebular/theme';
import { DropdownDTO } from '../../../../common/dropdown-dto';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'ngx-school-list',
  templateUrl: './school-list.component.html',
  styles: [`
    nb-card {
      transform: translate3d(0, 0, 0);
    }
  `],
})
export class SchoolListComponent implements OnInit {

  listViewModel: any[] = [];

  statusList: Array<DropdownDTO> = new Array<DropdownDTO>();
  updatedStatus: number = -1;
  userId: any;

  constructor(public requestService: RegistrationRequestService, private dialogService: NbDialogService, private route: ActivatedRoute) {
    this.userId = route.snapshot.paramMap.get("id");
  }

  async ngOnInit() {
    var response = await this.requestService.GetSchoolList();

    if (response.statusCode == 0) {
      this.listViewModel = response.data;
    }
  }

  
}
