import { Component, OnInit } from '@angular/core';
import { lastValueFrom } from 'rxjs';
import { HttpRequestService } from 'src/app/core/services/HttpRequestService';

@Component({
  selector: 'app-creation',
  templateUrl: './creation.component.html',
  styleUrls: ['./creation.component.scss']
})
export class CreationComponent implements OnInit {

  constructor(private http: HttpRequestService) { }

  ngOnInit(): void { }

  addTag(name: string) {
    if (!!name) {
      lastValueFrom(this.http.addTag({Name: name}))
    }
  }
  addCategory(name: string) {
    if (!!name) {
      lastValueFrom(this.http.addCategory({Name: name}))
    }
  }
  addQuestion(name: string) {
    if (!!name) {
      lastValueFrom(this.http.addQuestion({Question: name}))
    }
  }
}
