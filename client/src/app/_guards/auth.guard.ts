import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';

export const authGuard: CanActivateFn = (route, state) => {
  const accountService = inject(AccountService);
  const toastr = inject(ToastrService);

  //if user is logged in the auth reutrns true => it can navigate
  if(accountService.currentUser()){
    return true
  }
  else{
    toastr.error('This shall not pass!');
    return false;
  }
};
