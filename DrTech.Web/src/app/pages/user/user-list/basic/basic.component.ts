
import { Component, OnInit, TemplateRef } from '@angular/core';
import { GridDataResult, PageChangeEvent } from '@progress/kendo-angular-grid';
import { Router } from '@angular/router';
import { UserService } from '../../service/user-service';
import { LocalDataSource } from 'ng2-smart-table';
import { NbDialogService, NbDialogRef } from '@nebular/theme';
import { compileFilter, SortDescriptor, orderBy } from '@progress/kendo-data-query';
// import { LocationLinkComponent, UserDetailLinkComponent } from '../../../common/custom-control';
import { ActivatedRoute } from '@angular/router';
import { UserRequestDto } from '../../dto/user-dto';
@Component({
  selector: 'ngx-user-list-basic',
  templateUrl: './basic.component.html',
  styleUrls: ['./basic.component.scss']
})
export class BasicComponent implements OnInit {
  listViewModel: any[] = [];
  loading = false;

  public gridView: GridDataResult;
  public pageSize = 8;
  public skip = 0;
  source: LocalDataSource = new LocalDataSource();
  updatedStatus: number = -1;
  points: number = 0;
  userId: any;
  statusId: any;
  // sort by Created Date 
  // public sort: SortDescriptor[] = [{
  //   field: 'fullName',
  //   dir: 'asc'
  // }];
  public multiple = false;
  public allowUnsort = true;
  userRequestDto : UserRequestDto = new UserRequestDto();
  constructor(public userService: UserService, private router: Router) {
    this.userRequestDto.type = 'basic';
   }
  async ngOnInit() {
    this.loading = true;
    this.LoadData();
    this.loading = false
  }

  LoadData() {
    this.userService.GetUserList(this.userRequestDto).subscribe(result => {
      if (result.statusCode == 0) {
        result.data.forEach(p => p.pinType = "user");
        this.source.load(result.data);
        this.listViewModel = result.data;
        this.loadItems();
      }
    });
  }

  public pageChange(event: PageChangeEvent): void {
    this.skip = event.skip;
    this.loadItems();
  }
  public sortChange(sort: SortDescriptor[]): void {
    //this.sort = sort;
    this.loadItems();
  }
  private loadItems(): void {
    if (this.skip == this.listViewModel.length)
      this.skip = this.skip - this.pageSize;
    this.gridView = {
      data: this.listViewModel.slice(this.skip, this.skip + this.pageSize),
      total: this.listViewModel.length
    };

  }
  public onSelect(e) {
    // this.router.navigate(["/pages/driver/tasklist/" + this.gridView.data[e.index % this.pageSize].id])
  }
  // xyz:RefuseDTO

  searchGrid(search) {

    const predicate = compileFilter(
      {
        logic: "or",
        filters: [
          { field: "fullName", operator: "contains", value: search },
          { field: "email", operator: "contains", value: search },
          { field: "phone", operator: "contains", value: search },
        ]
      });

    if (search) {
      this.gridView = {
        data: this.listViewModel.filter(predicate),
        total: this.listViewModel.length
      };
    }
    else {
      this.gridView = {
        data: this.listViewModel.filter(predicate).slice(this.skip, this.skip + this.pageSize),
        total: this.listViewModel.length
      };
    }
  }
}
