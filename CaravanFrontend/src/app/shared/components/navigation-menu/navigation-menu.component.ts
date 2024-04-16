import { Component, OnInit } from '@angular/core';
import { animate, style, transition, trigger } from '@angular/animations';
import { RouterLink } from '@angular/router';

const enterTransition = transition(':enter', [
  style({
    opacity: 0,
  }),
  animate('0.5s ease-in', style({ opacity: 1 })),
]);
const exitTransition = transition(':leave', [
  style({
    opacity: 1,
  }),
  animate('0.5s ease-out', style({ opacity: 0 })),
]);
const show = trigger('show', [enterTransition]);
const hide = trigger('hide', [exitTransition]);

@Component({
  selector: 'app-navigation-menu',
  templateUrl: './navigation-menu.component.html',
  styleUrls: ['./navigation-menu.component.scss'],
  standalone: true,
  imports: [ RouterLink]
})
export class NavigationMenuComponent  implements OnInit {
  navBars= false;
  constructor() { }

  ngOnInit() {}

  onMenuButtonClick(){
    this.navBars = !this.navBars;
  }

}
