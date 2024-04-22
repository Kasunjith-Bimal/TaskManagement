import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { LoginUser } from 'src/app/model/LoginUser';
import { Task } from 'src/app/model/Task';
import { AuthorizeService } from 'src/app/services/authorize.service';
import { InteractionService } from 'src/app/services/interaction.service';
import { TaskService } from 'src/app/services/task.service';
import { UserService } from 'src/app/services/user.service';


@Component({
  selector: 'app-task-list',
  templateUrl: './task-list.component.html',
  styleUrls: ['./task-list.component.css']
})
export class TaskListComponent implements OnInit {
  searchText : string = "";
  tasks : Task[] = [];
  isLoading : boolean = false;
  currentLogUser: LoginUser | null = null;
  role: string ="";
  showConfirmation : boolean = false;
  deleteId : number = 0;
 constructor(private taskService: TaskService,private toastr: ToastrService,private interactionService: InteractionService,private userService : UserService,private authorizeService : AuthorizeService) {
 }
 ngOnInit(): void {
  this.isLoading = true;
   this.authorizeService.getLoggedInUser().subscribe((user)=>{
    this.currentLogUser = user;
    if(this.currentLogUser){
      this.role = this.authorizeService.getRoleusingToken();
      this.userService.getUserTasksByUserId(this.currentLogUser.Id).subscribe((response:any) => {
        console.log("response",response);
        if(response.succeeded){
         this.tasks = response.payload.tasks;
         this.interactionService.sendTaskCount( this.tasks.length);
         this.isLoading = false;
        }else{
         this.tasks = [];
         this.interactionService.sendTaskCount(0);
         this.toastr.error(response.message, 'System Error. Please contact administrator',{timeOut: 2000,extendedTimeOut: 0});
         this.isLoading = false;
        }
   
     },
     error => {
       this.tasks = [];
       this.interactionService.sendTaskCount(0);
       this.toastr.error(error.error.message, 'System Error. Please contact administrator',{timeOut: 2000,extendedTimeOut: 0});
       this.isLoading = false;
     }
     );
    }
  }); 
}

deleteTaskEventClick(taskId : number){
  this.deleteId = taskId;
  this.showConfirmation = true;
}

onConfirmDelete(){
  this.showConfirmation = false;
  this.taskService.deleteTask(this.deleteId).subscribe(
        (response: any) => {
          console.log(response);
          if(response.succeeded){
           this.tasks = this.tasks.filter(obj => obj.id !== this.deleteId);
            this.interactionService.sendTaskCount(this.tasks.length);
            this.toastr.success('Task deleted successfully', 'Success');
          }else{
            this.toastr.error(response.message, 'System Error. Please contact administrator',{timeOut: 2000,extendedTimeOut: 0});
          }
        
        },
        error => {
          this.toastr.error(error.error.message, 'System Error. Please contact administrator',{timeOut: 2000,extendedTimeOut: 0});
        }
  );
}

onCancelDelete(){
  this.showConfirmation = false;
}

addedTaskEventClick(task : Task){
  this.tasks.unshift(task);
  this.interactionService.sendTaskCount(this.tasks.length);
}

editTaskCheckBoxEventChange(task : Task){
  this.taskService.updateTask(task).subscribe(
  (response: any) => {
    console.log(response);
    if(response.succeeded){
      this.toastr.success('Task updated successfully', 'Success');
   

    }else{
      this.toastr.error(response.message, 'System Error. Please contact administrator',{timeOut: 2000,extendedTimeOut: 0});
    }
  
  },
  error => {
    this.toastr.error(error.error.message, 'System Error. Please contact administrator',{timeOut: 2000,extendedTimeOut: 0});
  }
);
}

}
