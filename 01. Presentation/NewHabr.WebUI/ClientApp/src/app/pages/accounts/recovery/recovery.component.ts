import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { lastValueFrom, Subscription } from 'rxjs';
import { RecoveryResponse } from 'src/app/core/models/Recovery';
import { SecureQuestion } from 'src/app/core/models/SecureQuestion';
import { HttpRequestService } from 'src/app/core/services/HttpRequestService';

@Component({
  selector: 'app-recovery',
  templateUrl: './recovery.component.html',
  styleUrls: ['./recovery.component.scss']
})
export class RecoveryComponent implements OnInit, OnDestroy {

  subscribtions: Subscription[] = [];
  step: number = 1;

  // step 1
  login: string;
  // step 2
  activeQuestion: SecureQuestion;
  questionsList: Array<SecureQuestion>;
  answer: string;
  recoveryResponse: RecoveryResponse;
  // step 3
  password: string;
  repeatPassword: string;

  incorrectPasswords: boolean = false;
  succesfulChangePassword: boolean;

  constructor(private router: Router, private http: HttpRequestService) { }
  
  ngOnInit(): void {
    const questionsSubscribtion = this.http.getAllQuestions().subscribe(questions => {
      if (questions) {
        this.questionsList = questions;
        this.activeQuestion = this.questionsList[0];
      }
    })

    this.subscribtions.push(questionsSubscribtion);
  }

  ngOnDestroy(): void {
    this.subscribtions.forEach(element => element.unsubscribe());
  }

  getQuestion() {
    if (!!this.login) {
      this.step = 2;
    }
  }

  answerQuestion() {
    if (!!this.answer) {
      const recoveryRequestSubscribtion = this.http.requestRecovery({
        UserName: this.login,
        SecureQuestionId: this.activeQuestion.Id,
        Answer: this.answer
      }).subscribe(response => {
        if (response) {
          this.recoveryResponse = response;
          this.step = 3;
          recoveryRequestSubscribtion.unsubscribe();
        }
      })
    }
  }

  changePassword() {
    if (!!this.password && !!this.repeatPassword) {
      if (this.password !== this.repeatPassword) {
        this.incorrectPasswords = true;
        return;
      }

      lastValueFrom(this.http.requestResetPassword({
        Token: this.recoveryResponse.Token,
        UserName: this.recoveryResponse.User.UserName,
        NewPassword: this.password
      })).then(() => {
        this.succesfulChangePassword = true;
        setTimeout(() => this.router.navigate(['accounts', 'login']), 1000);
      })
    }
  }
}
