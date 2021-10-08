import { isDevMode } from "@angular/core";

export class CommonFunction {

  public static GetCurrentURLOrigin(): string {
 

   //var baseUrl = "https://mywaste.azurewebsites.net/api/";

    //console.log("Development Mode => " + isDevMode());
    //console.log("Current ULR => " + window.location.origin);
    //console.log("Current ULR Index of Local => " + window.location.origin.indexOf("localhost:4200"));

    // if (!isDevMode() || window.location.origin.indexOf("localhost:4200") < 0) {

    //var baseUrl = 'https://mywaste.azurewebsites.net/api/';

     var baseUrl = 'http://localhost:64331/api/';

    if (isDevMode()) {
     baseUrl = 'http://localhost:64331/api/';
    //baseUrl = 'https://mywaste.azurewebsites.net/api/';

    }
    return baseUrl;
  }
}
