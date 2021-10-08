import { Component, OnInit} from '@angular/core';
import { NbDialogService} from '@nebular/theme';
import { ActivatedRoute } from '@angular/router';
import { DropdownDTO } from '../../../common/dropdown-dto';
import { RegistrationRequestService } from '../../registration-request/service/registration-request-service';

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
    this.userId = route.snapshot.paramMap.get('id');
  }

  async ngOnInit() {
    const response = await this.requestService.GetSchoolList();

    if (response.statusCode === 0) {
      this.listViewModel = response.data;
    }
  }

  
}
