import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.scss']
})
export class MainComponent implements OnInit {

  menu = [
    {
      name: 'Главная страница',
      url: '/publications'
    },
    {
      name: 'Личный кабинет',
      url: '/account'
    },
    {
      name: 'Администрирование',
      url: '/admin'
    },
    {
      name: 'Помощь',
      url: '/help'
    },
    {
      name: 'Поиск',
      url: '/find'
    }
  ]

  constructor() { }

  ngOnInit(): void { }
}
