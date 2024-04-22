import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AdminService {

  constructor(private http: HttpClient) { }

  getAllUsers(){
   const url = `${environment.baseUrl}api/Admin/users`;
   return this.http.get(url);
  }

  getUserById(userId:string){
   const url = `${environment.baseUrl}api/Admin/users/${userId}`;
   return this.http.get(url);
  }
  
}
