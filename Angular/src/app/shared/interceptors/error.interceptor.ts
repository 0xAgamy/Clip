import { HttpInterceptorFn } from '@angular/common/http';
import { catchError, throwError } from 'rxjs';

export const errorInterceptor: HttpInterceptorFn = (req, next) => 
  next(req).pipe(
      catchError((err) => {

        console.error("[API]", req.method, req.url, err.status, err.message);
        if (err.status === 401) {
      
        }
        return throwError(() => err);   
      })
    );
      
