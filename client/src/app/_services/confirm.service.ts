import { inject, Injectable } from '@angular/core';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { map } from 'rxjs';
import { ConfirmDialogComponent } from '../modal/confirm-dialog/confirm-dialog.component';

@Injectable({
  providedIn: 'root'
})
export class ConfirmService {
  bsModalRef? : BsModalRef;
  private modalService = inject(BsModalService);

  confirm(
    title = 'Confirmation',
    message = 'Leave page: Any change will be lost, are you sure?',
    btnOkText = 'Ok',
    btnCancelText = 'Cancel'
  ) {
    const config : ModalOptions = {
      initialState: {
        title, 
        message,
        btnOkText,
        btnCancelText
      }
    };
    this.bsModalRef = this.modalService.show(ConfirmDialogComponent,config);
    return this.bsModalRef.onHidden?.pipe(map(()=> {
      if(this.bsModalRef?.content)
      {
        return this.bsModalRef.content.result;
      }
      else return false;
    }))
  }
}