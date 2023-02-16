import { AfterViewInit, Component, HostListener, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import Quill from 'quill';
import { lastValueFrom, Subscription } from 'rxjs';
import { Authorization } from 'src/app/core/models/Authorization';
import { Publication } from 'src/app/core/models/Publication';
import { HttpRequestService } from 'src/app/core/services/HttpRequestService';
import { AppStoreProvider } from 'src/app/core/store/store';

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
        this.post.ModifyAt = +Date.now();
        this.post.Content = this.quill.root.innerHTML;
        lastValueFrom(this.http.postUpdatePublication(this.post.Id!, this.post));
        break;
      case Mode.Create:
        const post = {
          Title: this.post.Title,
          Content: this.quill.root.innerHTML,
          //UserId: this.auth?.User.Id,
          //UserLogin: "sjdnf",
          CreatedAt: +Date.now(),
          ModifyAt: +Date.now(),
          ImgURL: this.post.ImgURL,
          //IsPublished: false
        } as Publication
        lastValueFrom(this.http.postCreatePublication(post));
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
      ['image'] //add image here
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
    });
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
    // Called on hide and save.
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

  //@HostListener('paste', ['$event'])
  //private pasteFromClipboard(event: any): void {
  //  if (event.clipboardData?.files && event.clipboardData.files.length && event.clipboardData.files[0].type.search(/^image\//i) === 0) {
  //    event.preventDefault();
  //  }
  //}
}

enum Mode {
  Edit,
  Create
}
