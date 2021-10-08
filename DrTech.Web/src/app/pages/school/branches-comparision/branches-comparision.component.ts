import { Component, OnInit } from '@angular/core';
import { SchoolService } from '../service/school.service'
@Component({
  selector: 'ngx-branches-comparision',
  templateUrl: './branches-comparision.component.html',
  styleUrls: ['./branches-comparision.component.scss']
})
export class BranchesComparisionComponent implements OnInit {
  // options
  gradient: boolean = true;
  showLegend: boolean = true;
  showLabels: boolean = true;
  isDoughnut: boolean = false;


  studentsingle: any[];
  single: any[];
  view: any[] = [500, 400];
  colorScheme = {
    domain: ['#5AA454', '#E44D25', '#CFC0BB', '#7aa3e5', '#a8385d', '#aae3f5']
  };
  constructor(public schoolService: SchoolService) {
   // Object.assign(this, { single });
  }

  ngOnInit() {
    this.callAPI();
    this.studentsAPICall();
  }
  async callAPI() {
    let response = this.schoolService.GetSchoolsBranchesComparisionPieChartBySchoolAdmin();
    if ((await response).statusCode == 0) {
      this.single = [];
      this.single = (await response).data;
    }
  }
  async studentsAPICall() {
    let response = this.schoolService.GetSchoolsBranchesStudentsPieChartBySchoolAdmin();
    if ((await response).statusCode == 0) {
      this.studentsingle = [];
      this.studentsingle = (await response).data;
    }
  }
  onSelect(event) {
  }
  onActivate(data): void {
  }

  onDeactivate(data): void {
  }
}
