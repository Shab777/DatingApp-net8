import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { User } from '../_models/user';
import { map } from 'rxjs';
import { environment } from '../../environments/environment';
import { LikesService } from './likes.service';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private http = inject(HttpClient); // new method
  private likesService = inject(LikesService);
  baseUrl = environment.apiUrl;
  // store a current user information into an object and send this informatio to app compo via injecting this ang.service
  currentUser = signal<User | null>(null)
  

  //constructor(private http:HttpClient) { } old method

  login(model :any){
    return this.http.post<User>(this.baseUrl + 'account/login',model).pipe(
      map((response : User) =>{
        const user = response;        
        if(user)//if the user exists then store info into currentuser object
        {
          this.setCurrentUser(user);
        }
      })
    );
  }

  // write a method to register a new user
  register(model :any){
    return this.http.post<User>(this.baseUrl + 'account/register',model).pipe(
      map((response : User) =>{
        const user = response;        
        if(user)//if the user exists then send user to setCurrent Usermethod and get the current user dtls
        {   
            this.setCurrentUser(user);
        }
        return user;
      })
    );
  }

  //house keeping for set current user
  setCurrentUser(user : User){
      localStorage.setItem('user', JSON.stringify(user));
      this.currentUser.set(user);
      this.likesService.getLikeIds();       
  }
  //the above method returns a response i.e return a tokey key & u.n prop from api server. ang. service are singleton

  logout(){
    localStorage.removeItem('user');
    this.currentUser.set(null)
  }

}
