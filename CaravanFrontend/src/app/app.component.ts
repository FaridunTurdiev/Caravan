import { Component } from '@angular/core';
import { IonApp, IonRouterOutlet } from '@ionic/angular/standalone';
import { NavigationMenuComponent } from './shared/components/navigation-menu/navigation-menu.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: 'app.component.html',
  standalone: true,
  imports: [IonApp, IonRouterOutlet, NavigationMenuComponent],
})
export class AppComponent {
  constructor(private router: Router) {}

  isLogin(){
    return this.router.url==='/log-in-page';
  }
  isSignup(){
    return this.router.url==='/sign-up-form';
  }

  isResetPassword(){
    return this.router.url.includes('/reset-password');
  }
}
