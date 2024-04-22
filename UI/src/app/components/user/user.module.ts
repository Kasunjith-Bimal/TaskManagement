import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { UserDetailComponent } from './user-detail/user-detail.component';
import { TaskListComponent } from './task/task-list/task-list.component';
import { TaskListItemComponent } from './task/task-list/task-list-item/task-list-item.component';
import { TaskFormComponent } from './task/task-form/task-form.component';
import { TaskSearchPipe } from './pipe/task-search.pipe';
import { ConfirmationDialogComponent } from './confirmation-dialog/confirmation-dialog.component';



const routes: Routes = [
  { path: 'userDetail', component: UserDetailComponent },
  { path: 'tasks', component: TaskListComponent }
];

@NgModule({
  declarations: [
    UserDetailComponent,
    TaskListComponent,
    TaskListItemComponent,
    TaskFormComponent,
    TaskSearchPipe,
    ConfirmationDialogComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forChild(routes)
  ]
})
export class UserModule { }
