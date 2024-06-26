import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Register } from 'src/app/model/Register';
import { AuthorizeService } from 'src/app/services/authorize.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  registerForm!: FormGroup;
  registerEmployee : Register  = {
    email :'',
    fullName :'',
  }

  constructor(private formBuilder: FormBuilder,private authorizeService: AuthorizeService,private toastr : ToastrService,private router: Router) {


    this.registerForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      fullName: ['', Validators.required],
    });
  }

  ngOnInit() {
    
    this.registerForm.setValue({
      email: this.registerEmployee.email,
      fullName: this.registerEmployee.fullName,
    });
  }

  onSubmit() {
    if (this.registerForm.invalid) {
      return;
    }else{
      let registerValue = this.registerForm.value;
   

     let newEmployee: Register  = {
        email :registerValue.email,
        fullName :registerValue.fullName,
      }

      this.authorizeService.register(newEmployee).subscribe(
        (response: any) => {
          if(response.succeeded){
            this.toastr.success('User registed successfully', 'Success');
            setTimeout(() => {

              if(registerValue.role == 1){
                this.router.navigate(['admin/users']);
              }else{
                this.router.navigate(['user/tasks']); 
              }
            }, 500);
          }else{
            this.toastr.error(response.message, 'System Error. Please contact administrator',{timeOut: 3000,extendedTimeOut: 0});
          }
         
        },
        error => {
          this.toastr.error(error.error.message, 'System Error. Please contact administrator',{timeOut: 3000,extendedTimeOut: 0});
        }
      );
    }

    //console.log(this.registerForm.value); // Implement registration logic here
  }
}
