import { Component, OnInit } from '@angular/core';
import { IonHeader, IonToolbar, IonInput, IonIcon, IonItem, IonSearchbar } from "@ionic/angular/standalone";
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
  standalone: true,
  imports: [IonSearchbar, IonIcon, IonInput, IonToolbar, IonHeader, IonItem, RouterLink]
})
export class HeaderComponent  implements OnInit {
 
  logo:string = "/assets/header-logo.png";
  constructor() { }

  ngOnInit() {}

}
