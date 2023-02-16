import { AfterViewInit, Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { lastValueFrom, Subscription } from 'rxjs';
import { Authorization } from 'src/app/core/models/Authorization';
import { Publication, PublicationRequest } from 'src/app/core/models/Publication';
import { HttpRequestService } from 'src/app/core/services/HttpRequestService';
import { AppStoreProvider } from 'src/app/core/store/store';
import Quill from 'quill';
import * as _ from 'lodash';

@Component({
  selector: 'app-article',
  templateUrl: './article.component.html',
  styleUrls: ['./article.component.scss']
})
export class ArticleComponent implements OnInit, OnDestroy, AfterViewInit {

  subscribtions: Subscription[] = [];
  post: Publication = {
    Title: '',
    Content: '',
    ImgURL: '',
    IsPublished: false
  };
  text: string;
  mode: Mode;
  auth: Authorization | null;
  quill: Quill;

  maxTagCounter = 4;
  tags: string[] = [];
  cats: string[] = [];
  
  constructor(
    private http: HttpRequestService,
    private store: AppStoreProvider,
    private activeRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.activeRoute.params.subscribe(params => {
      const postId = params.id;

      if (postId) {
        const postSubscribtion = this.http.getPostById(params.id).subscribe(post => {
          if (post) {
            this.post = post;
            this.subscribtions.push(postSubscribtion);
            const delta = this.quill.clipboard.convert(this.post.Content as any)
            this.quill.setContents(delta);
          }
        })
        this.mode = Mode.Edit;
      } else {
        this.mode = Mode.Create;
      }
    })

    const authSubscribtion = this.store.getAuth().subscribe(auth => this.auth = auth);
    this.subscribtions.push(authSubscribtion);
  }

  ngOnDestroy(): void {
    this.subscribtions.forEach(element => element.unsubscribe());
  }

  save() {
    switch (this.mode) {
      case Mode.Edit:
        this.post.Content = this.quill.root.innerHTML;
        lastValueFrom(this.http.postUpdatePublication(this.post.Id!, this.post));
        break;
      case Mode.Create:
        const post = {
          Title: this.post.Title,
          Content: this.quill.root.innerHTML,
          ImgURL: this.post.ImgURL,
          Categories: this.cats.map(c => {
            return {
              Name: c
            }
          }),
          Tags: this.tags.map(c => {
            return {
              Name: c
            }
          }),
        } as PublicationRequest;
        lastValueFrom(this.http.createPublication(post));
        break;
    }
  }

  ngAfterViewInit(): void {
    var toolbar= [
      ['bold', 'italic', 'underline', 'strike'],
      ['blockquote', 'code-block'],
      [{ 'color': [] }, { 'background': [] }],
      [{ 'font': [] }],
      [{ 'align': [] }],
      ['clean'],
      ['image']
    ];

    var formats = [
      'background',
      'bold',
      'color',
      'font',
      'code',
      'italic',
      'link',
      'size',
      'strike',
      'script',
      'underline',
      'blockquote',
      'header',
      'indent',
      'list',
      'align',
      'direction',
      'code-block',
      'formula',
      'image'
    ];

    this.quill = new Quill('#editor-container', {
      modules: {
        toolbar: {
            container: toolbar,
            handlers: {
                image: this.imageHandler
            }
        }
      },
      placeholder: 'Текст статьи...',
      theme: 'snow',
      formats: formats
    })
  }

  imageHandler() {
    var el = document.getElementById('editor-container');
    var scrollPosition = el?.scrollTop ?? 0;

    const tooltip = (this.quill as any).theme.tooltip;
    const originalSave = tooltip.save;
    const originalHide = tooltip.hide;
  
    tooltip.save = () => {
      const range = this.quill.getSelection(true);
      const value = tooltip.textbox.value;
      if (value) {
        this.quill.insertEmbed(range.index, 'image', value, Quill.sources.USER);
      }
    };
    tooltip.hide = function () {
      tooltip.save = originalSave;
      tooltip.hide = originalHide;
      tooltip.hide();
    };
    tooltip.edit('image');
    tooltip.textbox.placeholder = 'Ссылка на изображение';
    tooltip.root.style.left = '0px';
    tooltip.root.style.top = scrollPosition + 'px';
    tooltip.textbox.value = ''
  }

  addElement(item: string, items: string[], input: HTMLInputElement) {
    if(_.some(items, i => i === item) === false && !!item && items.length < this.maxTagCounter) {
      items.push(item);
      input.value = '';
    }
  }
  deleteElement(item: string, items: string[]) {
    _.remove(items, i => i === item);
  }
}

enum Mode {
  Edit,
  Create
}
