import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { lastValueFrom } from 'rxjs';
import { HttpRequestService } from 'src/app/core/services/HttpRequestService';

@Component({
  selector: 'app-creation',
  templateUrl: './creation.component.html',
  styleUrls: ['./creation.component.scss']
})
export class CreationComponent implements OnInit {

  succesfulTag: boolean;
  succesfulCat: boolean;
  succesfulQue: boolean;

  constructor(private http: HttpRequestService) { }

  ngOnInit(): void { }

  @ViewChild('tag') tag: ElementRef<HTMLInputElement>;
  @ViewChild('cat') cat: ElementRef<HTMLInputElement>;
  @ViewChild('question') question: ElementRef<HTMLInputElement>;

  addTag(name: string) {
    if (!!name) {
      lastValueFrom(this.http.addTag({Name: name})).then(() => {
        this.succesfulTag = true;
        this.tag.nativeElement.value = '';
      })
    }
  }
  addCategory(name: string) {
    if (!!name) {
      lastValueFrom(this.http.addCategory({Name: name})).then(() => {
        this.succesfulCat = true;
        this.cat.nativeElement.value = '';
      })
    }
  }
  addQuestion(name: string) {
    if (!!name) {
      lastValueFrom(this.http.addQuestion({Question: name})).then(() => {
        this.succesfulQue = true;
        this.question.nativeElement.value = '';
      })
    }
  }
}
