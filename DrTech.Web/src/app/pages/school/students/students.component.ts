

import { SchoolService } from '../service/school.service'
import { Component, OnInit, TemplateRef} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { GridDataResult, PageChangeEvent } from '@progress/kendo-angular-grid';
import { NbDialogService, NbDialogRef } from '@nebular/theme';
import { compileFilter, CompositeFilterDescriptor, filterBy, SortDescriptor, orderBy } from '@progress/kendo-data-query';
import { BranchRequest } from '../models/schools-comparision-criteria';

@Component({
  selector: 'ngx-students',
  templateUrl: './students.component.html',
 // styleUrls: ['./students.component.css']
})

export class StudentsComponent implements OnInit {
  request : BranchRequest = new BranchRequest();
  public schoolId:any = 0;
  listViewModel: any[] = [];
  loading=false;
  public studentId = 0;
  schoolBranches: any[];
  public gridView: GridDataResult;
  public pageSize = 8;
  public skip = 0;
  public filter: CompositeFilterDescriptor;
  public studentName:string=""
  public sort: SortDescriptor[] = [{
    field: 'name',
    dir: 'asc'
  }];
  public multiple = false;
  public allowUnsort = true;
  constructor(public schoolService: SchoolService, protected route: ActivatedRoute, protected router: Router, private dialogService: NbDialogService) {
   }

  async ngOnInit() {
    this.schoolId.toString();
    this.LoadSchoolBranches();
    this.LoadData();
  }

 async LoadData() {
   debugger;
    this.loading = true;
    let response = this.schoolService.StudentsBySchool(this.request);
    if ((await response).statusCode == 0) {
      this.listViewModel = [];
      this.listViewModel = (await response).data;
     // this.gridView = this.listViewModel;
      console.log('banches mapped',this.listViewModel)
      this.loading = false;
    }

    // this.schoolService.GetSchoolStudentsBySchoolId(this.schoolId).subscribe(result => {
    //   if (result.statusCode == 0) {
    //     this.listViewModel = result.data;

    //     this.loadItems();
    //   }
    // });

    this.loading = false
  }
  loadGrid(value):void{
    this.schoolId = value;
    this.LoadData();
  }
async  LoadSchoolBranches() {
    this.loading = true;
    let response = this.schoolService.GetSchoolBranchesByUserId();
    if ((await response).statusCode == 0) {
      this.schoolBranches = [];
      this.schoolBranches = (await response).data;
      console.log('banches mapped',this.schoolBranches)
      this.loading = false;
    }
  }
  valueChange(value):void{
  if(parseInt(value) === 0)
  {
    this.request.all = true;
  }
  else{
    this.request.id = parseInt(value);
    this.request.all = false;
  }
  
    debugger;
    this.LoadData();
  }
  private loadItems(): void {
    if(this.skip == this.listViewModel.length)
        this.skip = this.skip - this.pageSize;
    this.gridView = {
        data: orderBy(this.listViewModel.slice(this.skip, this.skip + this.pageSize),this.sort),
        total: this.listViewModel.length
    };
  }
  public sortChange(sort: SortDescriptor[]): void {
    this.sort = sort;
    this.loadItems();
}
  public pageChange(event: PageChangeEvent): void {
    this.skip = event.skip;
    this.loadItems();
  }

  async ActivateStudent(dialog: NbDialogRef<any>) {
    var response = await this.schoolService.ActivateStudent(this.studentId);

    if (response.statusCode == 0) {
      dialog.close();
      this.LoadData();
    }
  }

  onComplaintAction(dialog: TemplateRef<any>, event: any, id: any) {
    this.studentName = this.listViewModel.find(x=>x.id==id).name;
    const dialogRef = this.dialogService.open(
      dialog,
      {
        context: event.data,
        closeOnBackdropClick: false,
        closeOnEsc: false,
      });

      this.studentId = id;
  }
  searchGrid(search) {
    const predicate = compileFilter(
      { logic: "or",
       filters: [
        { field: "name", operator: "contains", value: search },
        { field: "schoolname", operator: "contains", value: search },
        { field: "clas", operator: "contains", value: search},
        { field: "section", operator: "contains", value: search},
        { field: "contactno", operator: "contains", value: search},
        ]});

        if(search)
        {
          this.gridView = {
            data: this.listViewModel.filter(predicate),
             total: this.listViewModel.length
         };
        }
        else
        {
          this.gridView = {
            data: this.listViewModel.filter(predicate).slice(this.skip, this.skip + this.pageSize),
             total: this.listViewModel.length
         };
        }
  }

  public filterChange(filter: CompositeFilterDescriptor): void {
    this.filter = filter;
    this.gridView = {
        data: filterBy(this.listViewModel, filter),
        total: this.listViewModel.length
    };
  }
   // async getSchoolsDropdown() {
  //   let response = this.schoolService.GetSchoolBranchesByUserId();
  //   if ((await response).statusCode == 0) {
  //     this.schoolBranches = [];
  //     this.schoolBranches = (await response).data;
  //   }
  // }
  // async getSchoolStudents() {
  //   let response = this.schoolService.GetSchoolBranchesByUserId();
  //   if ((await response).statusCode == 0) {
  //     this.schoolBranches = [];
  //     this.schoolBranches = (await response).data;
  //   }
  // }

}
