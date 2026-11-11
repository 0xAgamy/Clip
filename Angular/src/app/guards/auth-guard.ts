import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import {UserServices} from '../user/UserServices/user-services.service'
export const authGuard: CanActivateFn = (route, state) => {

  const _UserService= inject(UserServices)
  const _router= inject(Router)
  const token = _UserService.getJWT();
  if (token) return true;

  _router.navigateByUrl("/")
  return false
};
