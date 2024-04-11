import { Component, OnInit } from '@angular/core';
import { IonHeader, IonToolbar, IonInput, IonIcon, IonItem } from "@ionic/angular/standalone";

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
  standalone: true,
  imports: [IonIcon, IonInput, IonToolbar, IonHeader, IonItem]
})
export class HeaderComponent  implements OnInit {

  constructor() { }

  ngOnInit() {}

}
