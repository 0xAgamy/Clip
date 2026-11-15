import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { UserServices } from '../../user/UserServices/user-services.service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const _UserService= inject(UserServices)
  const token = _UserService.getJWT();
  if (!token) return next(req);

  return next(
    req.clone({ setHeaders: { Authorization: `Bearer ${token}` } })
  );
};
