import { Component } from '@angular/core';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.scss']
})
export class MainComponent {

  menu = [
    {
      name: 'Главная страница',
      url: 'publications'
    },
    {
      name: 'Личный кабинет',
      url: 'accounts/login'
    },
    {
      name: 'Администрирование',
      url: 'admin'
    },
    {
      name: 'Помощь',
      url: 'help'
    },
    {
      name: 'Поиск',
      url: 'find'
    }
  ]
}
