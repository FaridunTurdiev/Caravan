import { Component } from '@angular/core';
import { IonHeader, IonToolbar, IonTitle, IonContent, IonInput, IonIcon, IonButton } from '@ionic/angular/standalone';
import { HeaderComponent } from '../header/header.component';

@Component({
  selector: 'app-home',
  templateUrl: 'home.page.html',
  styleUrls: ['home.page.scss'],
  standalone: true,
  imports: [IonButton, IonIcon, IonInput, IonHeader, IonToolbar, IonTitle, IonContent, IonInput, HeaderComponent],
})
export class HomePage {
  constructor() {}
}
