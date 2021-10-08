import { Injectable, Injector } from '@angular/core';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { NbAuthJWTToken, NbAuthService } from './auth';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {

  private authService: NbAuthService;
  private tokenService: NbAuthJWTToken;

  constructor(private injector: Injector) {
  }

  // public getToken(): string {
  //     return localStorage.getItem('auth_app_token');
  // }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (request.url.toLocaleLowerCase().indexOf("login") > -1 || request.url.toLocaleLowerCase().indexOf("registrationform") > -1) return next.handle(request);
    this.authService = this.injector.get(NbAuthService); // get it here within intercept
    


    this.authService.isAuthenticated().subscribe((result) => {
      if (result) {
        this.authService.getToken().subscribe(result1 => {
          var token = result1.getValue();
          if (token) {
           
            request = request.clone({
               setHeaders: {
                 Authorization: `Bearer ` + token              
                
                
               }
              
              
            
            });
}
        })
      }
    });
    return next.handle(request);
  }

}
