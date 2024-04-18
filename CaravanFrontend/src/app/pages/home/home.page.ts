import { Component } from '@angular/core';
import { IonHeader, IonToolbar, IonTitle, IonContent, IonInput, IonIcon, IonButton, IonGrid, IonRow, IonCol, IonCardContent, IonCardHeader, IonCard, IonCardTitle, IonCardSubtitle } from '@ionic/angular/standalone';
import { HeaderComponent } from '../../shared/components/header/header.component';
import { NavigationMenuComponent } from '../../shared/components/navigation-menu/navigation-menu.component';


@Component({
  selector: 'app-home',
  templateUrl: 'home.page.html',
  styleUrls: ['home.page.scss'],
  standalone: true,
  imports: [IonCardSubtitle, IonCardTitle, IonCard, 
    IonCardHeader, 
    IonCardContent, 
    IonCol, 
    IonRow, 
    IonGrid, 
    IonButton, 
    IonIcon, 
    IonInput, 
    IonHeader, 
    IonToolbar, 
    IonTitle, 
    IonContent, 
    IonInput, 
    HeaderComponent, 
    NavigationMenuComponent,
  ],
})
export class HomePage {
  

  constructor() {}

  
}
