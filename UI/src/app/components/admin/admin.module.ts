import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { UserListComponent } from './user-list/user-list.component';
import { UserListItemComponent } from './user-list/user-list-item/user-list-item.component';
import { UserSearchPipe } from './pipes/user-search.pipe';
import { AdminLoaderComponent } from './admin-loader/admin-loader.component';



const routes: Routes = [
  { path: 'users', component: UserListComponent },

];

@NgModule({
  declarations: [
   UserListComponent,
   UserListItemComponent,
   UserSearchPipe,
   AdminLoaderComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forChild(routes),
  ]
})
export class AdminModule { }
