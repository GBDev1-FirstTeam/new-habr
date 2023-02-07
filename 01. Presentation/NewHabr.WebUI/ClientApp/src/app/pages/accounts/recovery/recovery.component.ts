import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { lastValueFrom } from 'rxjs';
import { Authorization } from 'src/app/core/models/Authorization';
import { HttpRequestService } from 'src/app/core/services/HttpRequestService';
import { AppStoreProvider } from 'src/app/core/store/store';

@Component({
  selector: 'app-recovery',
  templateUrl: './recovery.component.html',
  styleUrls: ['./recovery.component.scss']
})
export class RecoveryComponent implements OnInit {

  step: number = 1;

  // step 1
  login: string;
  question: string;
  transationId: string;
  // step 2
  answer: string;
  // step 3
  password: string;
  repeatPassword: string;

  incorrectPasswords: boolean = false;
  auth: Authorization;

  constructor(private store: AppStoreProvider, private router: Router, private http: HttpRequestService) { }
  
  ngOnInit(): void {
    this.store.getAuth().subscribe(auth => {
      if (auth) {
        this.auth = auth;
        this.step = 3;
      }
    })
  }

  getQuestion() {
    if (!!this.login) {
      const loginSubscribtion = this.http.postRecoveryLogin({
        Login: this.login
      }).subscribe(response => {
        this.question = response.Question;
        this.transationId = response.TransactionId;

        this.step = 2;
        loginSubscribtion.unsubscribe();
      })
    }
  }

  answerQuestion() {
    if (!!this.answer) {
      this.store.recovery({
        Answer: this.answer,
        TransactionId: this.transationId
      })
    }
  }

  changePassword() {
    if (!!this.password && !!this.repeatPassword) {
      if (this.password !== this.repeatPassword) {
        this.incorrectPasswords = true;
        return;
      }

      lastValueFrom(this.http.postRecoveryChangePassword({
        UserId: this.auth.User.Id,
        Password: this.password
      }))

      this.step = 1;
      this.router.navigate(['accounts', this.auth.User.Id]);
    }
  }
}
