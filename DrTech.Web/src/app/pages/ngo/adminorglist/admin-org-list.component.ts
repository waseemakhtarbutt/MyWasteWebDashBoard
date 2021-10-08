import { Component, OnInit, TemplateRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgoService } from '../service/ngo-service';
import { GridDataResult, PageChangeEvent } from '@progress/kendo-angular-grid';
import { NbDialogService, NbDialogRef } from '@nebular/theme';

@Component({
  selector: 'ngx-admin-org-list',
  templateUrl: './admin-org-list.component.html',
  styleUrls: ['./admin-org-list.component.scss'],
  styles: [`
    nb-card {
      transform: translate3d(0, 0, 0);
    }
  `],
})
export class AdminOrgListComponent implements OnInit {
  listViewModel: any[] = [];
  loading=false;
  public memberId = 0;

  public gridView: GridDataResult;
  public pageSize = 10;
  public skip = 0;

  constructor(public ngoService: NgoService, protected route: ActivatedRoute, protected router: Router, private dialogService: NbDialogService) { }

  async ngOnInit() {
    this.LoadData();
  }

  LoadData() {
    this.loading = true;
  
    this.ngoService.GetMemberListByRole(false).subscribe(result => {
      if (result.statusCode == 0) {
        this.listViewModel = result.data;

        this.loadItems();
      }
    });

    this.loading = false
  }

  private loadItems(): void {
    if(this.skip == this.listViewModel.length)
        this.skip = this.skip - this.pageSize;
    this.gridView = {
        data: this.listViewModel.slice(this.skip, this.skip + this.pageSize),
        total: this.listViewModel.length
    };
  }
  
  public pageChange(event: PageChangeEvent): void {
    this.skip = event.skip;
    this.loadItems();
  }

  async SuspendMember(dialog: NbDialogRef<any>) {    
    var response = await this.ngoService.SuspendMember(this.memberId);
    
    if (response.statusCode == 0) {
      dialog.close();
      this.LoadData();      
    }
  }

  onComplaintAction(dialog: TemplateRef<any>, event: any, id: any) {
    const dialogRef = this.dialogService.open(
      dialog,
      {
        context: event.data,
        closeOnBackdropClick: false,
        closeOnEsc: false,
      });

      this.memberId = id;
  }
}
