import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { User } from 'src/app/model/User';
import { AdminService } from 'src/app/services/admin.service';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent implements OnInit {
  users : User[] = [];
  searchText : string ="";
  isLoading : boolean = false;
  constructor(private adminService: AdminService,private toaster: ToastrService) {
   
    
  }
  ngOnInit(): void {
    this.isLoading = true;
     this.adminService.getAllUsers().subscribe((response:any)=>{
     
     if(response.succeeded){
     this.users = response.payload.users;
     this.isLoading = false;
     }else{
      this.users = [];
      this.toaster.error(response.message, 'System Error. Please contact administrator',{timeOut: 2000,extendedTimeOut: 0});
      this.isLoading = false;
     }
     },
     error => {
      this.users = [];
      this.toaster.error(error.error.message, 'System Error. Please contact administrator',{timeOut: 2000,extendedTimeOut: 0});
      this.isLoading = false;
    });
  }
}
