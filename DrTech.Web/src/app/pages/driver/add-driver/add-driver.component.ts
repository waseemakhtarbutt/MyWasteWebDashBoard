import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DriverService } from '../../driver/service/driver.service';
import { CommonService } from '../../../common/service/common-service';
import { Http, Response, Headers, RequestOptions, ResponseContentType } from '@angular/http';
import { HttpClient } from '@angular/common/http';
import { from } from 'rxjs';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';


@Component({
  selector: 'ngx-add-driver',
  templateUrl: './add-driver.component.html',
  styleUrls: ['./add-driver.component.scss']
})
export class AddDriverComponent implements OnInit {
  imageToUpload : File;  
  fileToUpload : File;  
  VechileList:any;
  GreenShopList:any;
  Files : any;
  file: File[] = [];
  picSize: boolean = true;
  picType: boolean = true;
  rowClass: boolean = true;
  isNumberExists:boolean=false;
  size: any;
  //StgID: number;
  itemList = [];

  PhoneNumberEXistErrorMessage:string = '';
  _viewModel: any = {};
  showHide:boolean=false;

  constructor(public service: DriverService, private route: ActivatedRoute, private router: Router,public http:HttpClient) { }

   async ngOnInit() {
     var ID = this.route.snapshot.paramMap.get("id");
     if (ID != null)
     {
       this.showHide=true;
        var listresponse = await this.service.GetDriverByID(ID);
             
        if(listresponse.statusCode == 0)
          this._viewModel = listresponse.data;

        this._viewModel.title = "Edit Driver";
        
     }
     else
        this._viewModel.title = "Add Driver";

      var response = await this.service.GetVechileList();
      if(response.statusCode == 0)
        this.VechileList = response.data;

      var response = await this.service.GetGreenshopList();
        if(response.statusCode == 0)
          this.GreenShopList = response.data;
  }

  async ValidatePhoneNumber(phoneNumber:string){
    var response = await this.service.CheckPhoneNumber(phoneNumber);
    if(response.data == true){
       this.PhoneNumberEXistErrorMessage ="Phone Number already exists";
       this._viewModel.Phone = "";
       this.isNumberExists=true;
    }
    else{
      this.PhoneNumberEXistErrorMessage = '';
      this.isNumberExists=false;

    }

  }

  public uploadImage(event) {
    if (event.target.files.length == 0) {
      console.log("No file selected!");
      return;
    }

    //let file: File = event.target.files[0];
  this.imageToUpload = event.target.files[0];
  this._viewModel.image = this.imageToUpload.name;
  var mimeType = event.target.files[0].type;
  if (mimeType.match(/image\/*/) == null) {    
      this.picType = false;
      this.rowClass = false;  
      return;
    }

    this.picType = true;
    this.rowClass = true;

    this.size = event.target.files[0].size;
    this.size = this.size / 1000 /1000;
    if(this.size > 2)
    {
      this.picSize = false;
      this.rowClass = false;
      return;
    }
    this.picSize = true;
    this.rowClass = true;

    let file: File = event.target.files[0];
    this.imageToUpload = event.target.files[0];
    this._viewModel.image = this.imageToUpload.name;

    var reader = new FileReader();   
    reader.readAsDataURL(event.target.files[0]); 
    reader.onload = (_event) => { 
      this. _viewModel.fileImage  = "FileName"; 
      this. _viewModel.fileName  = reader.result; 

    }
}
//Download Licence code
DownloadFile(id:number):void
  {
    var response = this.service.Getfile(id);
    console.log(response);
    // console.log("Your value"+id);
    // this.getFile("http://localhost:64331/api/GetLicenceFile?id="+id)

    // .subscribe(fileData => 
    //   {
    //   let b:any = new Blob([fileData], { type: 'application/png, Image/gif,Image/png' });
    //   var url= window.URL.createObjectURL(b);
    //     window.open(url);
    //   }
    // );
  }
 
  // public getFile(path: string):Observable<any>{
  //   let options = new RequestOptions({responseType: ResponseContentType.Blob});
  //   return this.http.get(path,options)
  //       .map((response: Response) => <Blob>response.blob())  ;
  



public uploadFile (event) {
  if (event.target.files.length == 0) {
    return;
  } 

  let file: File = event.target.files[0];
  this.fileToUpload = event.target.files[0];
  this._viewModel.document = this.fileToUpload.name;
  var mimeType = event.target.files[0].type;
  // if (mimeType.match(/image\/*/) == null) {      
  //     return;
  //   }
    var reader = new FileReader();   
    reader.readAsDataURL(event.target.files[0]); 
    reader.onload = (_event) => { 
      this. _viewModel.LicenceFile  = reader.result; 
      this. _viewModel.Filelicence  = "LicenceFile"; 
    }

   // this.fileToUpload =<File>files[0];
  //  this._viewModel.document = this.fileToUpload.name;
  }



async onSubmit(){ 
  //console.log(this.StgID);
 // this._viewModel.ID = this.StgID;

 this.file.push(this.imageToUpload);// = this.imageToUpload;
 this.file.push(this.fileToUpload);
// this.itemList.push(this.imageToUpload); 
// this.itemList.push(this.fileToUpload);  

   var formResponse = await  this.service.SaveDriver( this.file,this._viewModel);
   if(formResponse.statusCode == 0)
   this.router.navigate(["/pages/driver/list/"]);
  
} 

// public getFile1(path: string):Observable<Blob>{
//  // let options = new RequestOptions({responseType: ResponseContentType.Blob,});
//  let headers = new Headers({'Authorization': this.getBearerToken()});
//       let options = new RequestOptions({
//           headers: headers, 
//           responseType: ResponseContentType.Blob
//         });
  
//   return this.http.get(path, options)
//           .map((response: Response) => <Blob>response.blob());             
          
// }
public getImage(imageUrl: string): Observable<Blob> {
  return this.http.get(imageUrl, { responseType: 'blob' });
}
// DownloadFile1(path:string){
//    console.log(path) 
//   this.getImage(path)
//     .subscribe(fileData => FileSaver.saveAs(fileData, "favicon.ico"));
// }



}
