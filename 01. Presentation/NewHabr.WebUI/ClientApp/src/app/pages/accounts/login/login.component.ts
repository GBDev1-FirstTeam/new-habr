import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Authorization } from 'src/app/core/models/Authorization';
import { AppStoreProvider } from 'src/app/core/store/store';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  login: string;
  password: string;
  auth: Authorization;

  constructor(private store: AppStoreProvider, private router: Router) { }
  
  ngOnInit(): void {
    this.store.getAuth().subscribe(auth => {
      if (auth) {
        this.auth = auth;
        this.router.navigate(['accounts', auth.User.Id, 'articles']);
      }
    })
  }

  authenticate() {
    if (!!this.login && !!this.password) {
      this.store.authentication(this.login, this.password);
    }
  }
}
