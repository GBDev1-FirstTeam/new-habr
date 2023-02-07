import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Authorization } from 'src/app/core/models/Authorization';
import { AppStoreProvider } from 'src/app/core/store/store';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

  login: string;
  name: string;
  secondName: string;
  patronymic: string;
  age: number;
  about: string;
  password: string;
  repeatPassword: string;

  incorrectPasswords: boolean = false;

  auth: Authorization;

  constructor(private store: AppStoreProvider, private router: Router) { }
  
  ngOnInit(): void {
    this.store.getAuth().subscribe(auth => {
      if (auth) {
        this.auth = auth;
        this.router.navigate(['accounts', auth.User.Id]);
      }
    })
  }

  register() {
    if (!!this.login && !!this.name && !!this.secondName && !!this.patronymic && !!this.age && !!this.about && !!this.password && !!this.repeatPassword) {
      if (this.password !== this.repeatPassword) {
        this.incorrectPasswords = true;
        return;
      }
      this.store.register({
        Login: this.login,
        Password: this.password,
        FirstName: this.name,
        LastName: this.secondName,
        Patronymic: this.patronymic,
        Role: 'user',
        Age: this.age,
        Description: this.about
      });

      this.incorrectPasswords = false;
    }
  }
}
