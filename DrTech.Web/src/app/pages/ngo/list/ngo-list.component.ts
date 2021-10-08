import { Component, OnInit } from '@angular/core';
import { NbDialogService } from '@nebular/theme';
import { ActivatedRoute } from '@angular/router';
import { RegistrationRequestService } from '../../registration-request/service/registration-request-service';

@Component({
  selector: 'ngx-ngo-list',
  templateUrl: './ngo-list.component.html',
  styles: [`
    nb-card {
      transform: translate3d(0, 0, 0);
    }
  `],
})
export class NgoListComponent implements OnInit {
  listViewModel: any[] = [];
  userId: any;
  constructor(public requestService: RegistrationRequestService, private dialogService: NbDialogService, private route: ActivatedRoute) {
    this.userId = route.snapshot.paramMap.get('id');
  }

 async ngOnInit() {
   const response = await this.requestService.GetNgoList();

    if (response.statusCode === 0) {
      this.listViewModel = response.data;
    }
  }
}
