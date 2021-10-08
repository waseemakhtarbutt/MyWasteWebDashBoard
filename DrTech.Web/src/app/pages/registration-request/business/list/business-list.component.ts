import { Component, OnInit } from '@angular/core';
import { RegistrationRequestService } from '../../service/registration-request-service';
import { NbDialogService } from '@nebular/theme';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'ngx-business-list',
  templateUrl: './business-list.component.html',
  styles: [`
    nb-card {
      transform: translate3d(0, 0, 0);
    }
  `],
})
export class BusinessListComponent implements OnInit {
  listViewModel: any[] = [];
  userId: any;
  constructor(public requestService: RegistrationRequestService, private dialogService: NbDialogService, private route: ActivatedRoute) {
    this.userId = route.snapshot.paramMap.get("id");
  }

  async ngOnInit() {
    var response = await this.requestService.GetBusinessList();

    if (response.statusCode == 0) {
      this.listViewModel = response.data;
    }
  }
}
