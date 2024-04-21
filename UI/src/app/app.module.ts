import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { UserListComponent } from './components/admin/user-list/user-list.component';
import { UserListItemComponent } from './components/admin/user-list/user-list-item/user-list-item.component';
import { ChangePasswordComponent } from './components/authorize/change-password/change-password.component';
import { LoginComponent } from './components/authorize/login/login.component';
import { RegisterComponent } from './components/authorize/register/register.component';
import { UserDetailComponent } from './components/user/user-detail/user-detail.component';
import { ConfirmationDialogComponent } from './components/shared/confirmation-dialog/confirmation-dialog.component';
import { LoaderComponent } from './components/shared/loader/loader.component';
import { NavigationComponent } from './components/shared/navigation/navigation.component';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { ToastrModule } from 'ngx-toastr';
import { AuthInterceptor } from './interceptor/auth.interceptor';


@NgModule({
  declarations: [
    AppComponent,
    ConfirmationDialogComponent,
    LoaderComponent,
    NavigationComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    ToastrModule.forRoot({
      timeOut: 2000,
      positionClass: 'toast-bottom-right',
      preventDuplicates: false,
      closeButton: false,
    }),
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
