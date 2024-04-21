import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './authGard/auth-guard.guard';

const routes: Routes = [
  { path: 'authorize', 
    loadChildren: () => import('./components/authorize/authorize.module').then(m => m.AuthorizeModule) 
  },
  {
    path: 'admin',
    loadChildren: () => import('./components/admin/admin.module').then(m => m.AdminModule),
    canActivate: [AuthGuard],
    data: { roles: ['Admin'] },
  },
  { path: 'user', 
    loadChildren: () => import('./components/user/user.module').then(m => m.UserModule),
    canActivate: [AuthGuard],
    data: { roles: ['User'] },
  },
  { path: '', redirectTo: '/authorize/login', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
