import { Component, OnInit, TemplateRef} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SchoolService } from '../service/school.service';
import { GridDataResult, PageChangeEvent } from '@progress/kendo-angular-grid';
import { NbDialogService, NbDialogRef } from '@nebular/theme';
import { compileFilter, CompositeFilterDescriptor, filterBy } from '@progress/kendo-data-query';

import {NbProgressBarModuleModule} from "../../../../app/nb-progress-bar-module/nb-progress-bar-module.module";
@Component({
  selector: 'ngx-progress',
  templateUrl: './progress.component.html',
  styleUrls: ['./progress.component.scss']
})
export class ProgressComponent implements OnInit {
  listViewModel: any[] = [];
  loading=false;
  public studentId = 0;

  public gridView: GridDataResult;
  public pageSize = 8;
  public skip = 0;
  public filter: CompositeFilterDescriptor;
  public studentName: string =""

  constructor(public schoolService: SchoolService, protected route: ActivatedRoute, protected router: Router, private dialogService: NbDialogService) { }

  async ngOnInit() {
    this.LoadData();
  }

  LoadData() {
    this.loading = true;
  
    this.schoolService.GetStudentListByRoleWithGreenPointsProgress(false).subscribe(result => {
      if (result.statusCode == 0) {
        this.listViewModel = result.data;
        console.log(this.listViewModel)
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

  async SuspendChild(dialog: NbDialogRef<any>) {
    var response = await this.schoolService.SuspendChild(this.studentId);
    
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
}





