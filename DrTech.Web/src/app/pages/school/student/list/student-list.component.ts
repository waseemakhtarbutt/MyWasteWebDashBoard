import { Component, OnInit, Input} from '@angular/core';
import { NbDialogService} from '@nebular/theme';
import { ActivatedRoute } from '@angular/router';
import { DropdownDTO } from '../../../../common/dropdown-dto';
import { RegistrationRequestService } from '../../../registration-request/service/registration-request-service';

@Component({
  selector: 'ngx-student-list',
  templateUrl: './student-list.component.html',
  styles: [`
    nb-card {
      transform: translate3d(0, 0, 0);
    }
  `],
})
export class StudentListComponent implements OnInit {
  listViewModel: any[] = [];

  statusList: Array<DropdownDTO> = new Array<DropdownDTO>();
  updatedStatus: number = -1;

  constructor(public requestService: RegistrationRequestService, protected route: ActivatedRoute) { }

  async ngOnInit() {

    const id = this.route.snapshot.paramMap.get('id');

    const response = await this.requestService.GetStudentList(id);
    if (response.statusCode === 0) {
      this.listViewModel = response.data;
    }
  }

  
}
