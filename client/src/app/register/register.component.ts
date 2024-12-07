import { Component, EventEmitter, inject, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
model : any ={}
private accountService = inject(AccountService);
//@Input() usersFromHomeComponent : any;
@Output() cancelRegister = new EventEmitter();

//add methods
register(){
  this.accountService.register(this.model).subscribe({
    next : response => {
      console.log(response);
      this.cancel();
      
    },
    error : error => console.log(error)
  })
}

cancel(){
  this.cancelRegister.emit(false);
}

}
