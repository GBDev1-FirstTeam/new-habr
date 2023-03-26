import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { Authorization } from 'src/app/core/models/Authorization';
import { SecureQuestion } from 'src/app/core/models/SecureQuestion';
import { HttpRequestService } from 'src/app/core/services/HttpRequestService';
import { AppStoreProvider } from 'src/app/core/store/store';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit, OnDestroy {

  subscribtions: Subscription[] = [];

  userName: string;
  questionsList: Array<SecureQuestion>;
  activeQuestion: SecureQuestion;
  answerQuestion: string;
  password: string;
  repeatPassword: string;
  incorrectPasswords: boolean = false;

  auth: Authorization;

  constructor(
    private http: HttpRequestService,
    private store: AppStoreProvider,
    private router: Router) { }
  
  ngOnInit(): void {
    const authSubscribtion = this.store.getAuth().subscribe(auth => {
      if (auth) {
        this.auth = auth;
        this.router.navigate(['accounts', auth.User.Id]);
      }
    })
    const questionsSubscribtion = this.http.getAllQuestions().subscribe(questions => {
      if (questions) {
        this.questionsList = questions;
        this.activeQuestion = this.questionsList[0];
      }
    })

    this.subscribtions.push(authSubscribtion);
    this.subscribtions.push(questionsSubscribtion);
  }

  ngOnDestroy(): void {
    this.subscribtions.forEach(element => element.unsubscribe());
  }

  register(question: SecureQuestion) {
    if (!!this.userName && !!this.answerQuestion && !!this.password && !!this.repeatPassword) {
      if (this.password !== this.repeatPassword) {
        this.incorrectPasswords = true;
        return;
      }
      this.store.register({
        UserName: this.userName,
        Password: this.password,
        SecurityQuestionId: question.Id,
        SecurityQuestionAnswer: this.answerQuestion
      })

      this.incorrectPasswords = false;
    }
  }
}
