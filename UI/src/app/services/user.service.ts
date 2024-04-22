import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient) { }

  getUserById(userId:string){
    const url = `${environment.baseUrl}api/users/${userId}`;
    return this.http.get(url);
   }


   getUserTasksByUserId(userId:string){
    const url = `${environment.baseUrl}api/users/${userId}/tasks`;
    return this.http.get(url);
   }
}
