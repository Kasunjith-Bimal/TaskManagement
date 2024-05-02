import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Task } from 'src/app/model/Task';
import { TaskService } from 'src/app/services/task.service';

@Component({
  selector: 'app-task-list-item',
  templateUrl: './task-list-item.component.html',
  styleUrls: ['./task-list-item.component.css']
})
export class TaskListItemComponent {
  @Output() deleteTaskEvent = new EventEmitter<number>();
  @Output() editTaskCheckBoxEvent = new EventEmitter<Task>();
  @Output() editInLineEvent = new EventEmitter<number>();
  @Input() taskdetail?: Task;
  constructor(private taskService: TaskService,private toastr: ToastrService,private router:Router) {
  }

  deleteTask(taskId : number){
    this.deleteTaskEvent.emit(taskId);
  }

  editTask(taskId: number){
    this.router.navigate(['user/tasks/'+taskId+'/edit']);
  }

  editInLine(taskId: number){
   this.editInLineEvent.emit(taskId);
  }

  onCheckboxChange(event : any,task: Task){
    this.editTaskCheckBoxEvent.emit(task);
  }

  
}


