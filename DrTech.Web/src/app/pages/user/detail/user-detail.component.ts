import { Component, OnInit } from '@angular/core';
import { UserService } from '../service/user-service';
import { UserDTO } from '../dto';
import { ActivatedRoute } from "@angular/router";
import { AssociationDTO } from '../dto/user-dto';

@Component({
  selector: 'ngx-user-detail',
  templateUrl: './user-detail.component.html',
  styleUrls: ['./user-detail.component.scss'],
})
export class UserDetailComponent implements OnInit {

  _userModel: UserDTO = new UserDTO();
  _associationDTO: AssociationDTO = new AssociationDTO();
  isRegistered: boolean = false;
  userId: string = "";
  Model: any[] = [];
  //schools:string[]=null;
  constructor(public userService: UserService, private route: ActivatedRoute) { }

  ngOnInit() {
    this.userId = this.route.snapshot.paramMap.get("id");
    this.userService.GetUserDetail(this.userId).subscribe(result => {
      if (result.statusCode == 0) {
        if (result.data != null && (result.data.fileName == "" || result.data.fileName.length <= 0))
          result.data.fileName = "/assets/images/default-user.png";
        this._userModel = result.data;
        console.log(result.data)
       // this.schools = this._userModel.strSchools.split("-");
     //  console.log(this.schools)
       // var organizationsss = this._userModel.strOrganiztions.split("-");

        if (this._userModel.userType.toLowerCase() == "registered")
          this.isRegistered = true;
      }
    });
    ///////////////////////////////
    this.userService.GetUserDetailAssoList(this.userId).subscribe(result => {
      if (result.statusCode == 0) {
        this._associationDTO = result.data;
        console.log("lllllllllllllllllllllllllllllllllll")
        console.log(this._associationDTO)


      }
    });
  }
}
