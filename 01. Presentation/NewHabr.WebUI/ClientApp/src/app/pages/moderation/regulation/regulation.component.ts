import { Component, OnInit } from '@angular/core';
import { HttpRequestService } from 'src/app/core/services/HttpRequestService';

@Component({
  selector: 'app-regulation',
  templateUrl: './regulation.component.html',
  styleUrls: ['./regulation.component.scss']
})
export class RegulationComponent implements OnInit {

  constructor(private http: HttpRequestService) { }
  
  ngOnInit(): void { }
}
