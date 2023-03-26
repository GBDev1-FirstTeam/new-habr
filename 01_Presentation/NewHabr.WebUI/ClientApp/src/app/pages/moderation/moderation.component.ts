import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-moderation',
  templateUrl: './moderation.component.html',
  styleUrls: ['./moderation.component.scss']
})
export class ModerationComponent implements OnInit {

  menu = [
    {
      name: 'Данные',
      url: 'creation'
    },
    {
      name: 'Статьи',
      url: 'approval'
    },
    {
      name: 'Пользователи',
      url: 'regulation'
    }
  ]

  activePath: string;

  constructor(private router: Router) { }

  ngOnInit(): void {
    this.activePath = this.menu.find(item => this.router.url.includes(item.url))!.name;
  }

  changePath = (name: string) => this.activePath = name;
}
