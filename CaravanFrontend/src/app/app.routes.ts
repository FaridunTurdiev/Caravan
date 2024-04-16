import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: 'home', loadComponent: () => import('./pages/home/home.page').then((m) => m.HomePage),},
  { path: '', redirectTo: 'home', pathMatch: 'full',},
  {
    path: 'messages',
    loadComponent: () => import('./pages/messages/messages.page').then( m => m.MessagesPage)
  },
  {
    path: 'add-product',
    loadComponent: () => import('./pages/add-product/add-product.page').then( m => m.AddProductPage)
  },
  {
    path: 'settings',
    loadComponent: () => import('./pages/settings/settings.page').then( m => m.SettingsPage)
  },
  {
    path: 'cart',
    loadComponent: () => import('./pages/cart/cart.page').then( m => m.CartPage)
  },
  {
    path: 'account',
    loadComponent: () => import('./pages/account/account.page').then( m => m.AccountPage)
  },
];
