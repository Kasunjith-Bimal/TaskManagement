import { Component, OnInit } from '@angular/core';
import { AuthorizeService } from 'src/app/services/authorize.service';
import { UserService } from 'src/app/services/user.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { LoginUser } from 'src/app/model/LoginUser';
import { User } from 'src/app/model/User';


@Component({
  selector: 'app-user-detail',
  templateUrl: './user-detail.component.html',
  styleUrls: ['./user-detail.component.css']
})
export class UserDetailComponent implements OnInit {
  constructor(private authorizeService: AuthorizeService,private userService : UserService,private toaster: ToastrService,private router: Router) {
  }
  currentLogUser: LoginUser | null = null;
  user : User | null = null;
  private currentLogUserSubscription: Subscription = new Subscription();
  role: string ="";
  isLoading : boolean = false;
  ngOnDestroy(): void {
    this.currentLogUserSubscription.unsubscribe();
  }
  ngOnInit(): void {
    this.isLoading = true;
    this.currentLogUserSubscription = this.authorizeService.getLoggedInUser().subscribe((user) => {
      this.currentLogUser = user;
      //console.log(this.currentLogUser);
      if(this.currentLogUser){
        this.role = this.authorizeService.getRoleusingToken();
        this.userService.getUserById(this.currentLogUser.Id).subscribe(
          (response: any) => {
            if(response.succeeded){
             this.user = response.payload.user;
             this.isLoading = false;
            }else{
              this.isLoading = false;
              this.toaster.error(response.message, 'System Error. Please contact administrator',{timeOut: 3000,extendedTimeOut: 0});
            }
           
          },
          error => {
            this.isLoading = false;
            this.toaster.error(error.error.message, 'System Error. Please contact administrator',{timeOut: 3000,extendedTimeOut: 0});
          }
          );
      }
      
    });
  }

  onEdit(id : string){
    this.router.navigate(['/employee/'+id+'/edit'])
  }
}
