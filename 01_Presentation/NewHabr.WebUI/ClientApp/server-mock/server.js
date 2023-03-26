var ServerMock = require("mock-http-server");
var _ = require('lodash');

var server = new ServerMock({
    host: "localhost",
    port: 9000
});

server.on({
    method: 'GET',
    path: '/api/publications',
    reply: {
        status:  200,
        headers: { "content-type": "application/json" },
        body:    JSON.stringify([
            {
                Id: '2164687c-fb03-4308-b3ba-7dcf62a2abd5',
                Title: 'Lorem Ipsum',
                Content: '<p><strong>Sed ut perspiciatis</strong> unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo. Nemo enim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sed quia consequuntur magni dolores eos qui ratione voluptatem sequi nesciunt. Neque porro quisquam est, qui dolorem ipsum quia dolor sit amet,</p><pre class="ql-syntax" spellcheck="false"> consectetur, adipisci velit, sed quia non numqua m eius modi tempora incidunt ut labore et dolore magnam aliquam quaerat voluptatem. Ut enim ad minima veniam, quis nostrum exercitationem ullam corporis suscipit laboriosam, nisi ut aliquid ex ea commodi consequatur?</pre><p> Quis autem vel eum iure<span style="background-color: rgb(0, 138, 0);"> r</span><span style="color: rgb(255, 255, 0); background-color: rgb(0, 138, 0);">eprehende</span><span style="background-color: rgb(0, 138, 0);">rit q</span>ui in ea voluptate velit esse quam nihi<span style="color: rgb(230, 0, 0);">l molestiae con</span>sequatur, vel illum qui dolorem eum fugiat quo voluptas nulla pariatur</p>',
                UserId: '3458687c-fb03-4308-b3ba-7dcf62a2abc7',
                UserLogin: 'big_dick25',
                CreatedAt: 1674755729,
                ModifyAt: 1674755729,
                PublishedAt: 1674755729,
                ImgURL: 'https://avatarko.ru/img/kartinka/33/multfilm_lyagushka_32117.jpg',
                IsPublished: true,
                LikesCount: 164,
                IsLiked: true,
            },
            {
                Id: '1454687c-fb03-4308-b3ba-7dcf62a2abd5',
                Title: 'Target Milk',
                Content: 'But I must explain to you how all this mistaken idea of denouncing pleasure and praising pain was born and I will give you a complete account of the system, and expound the actual teachings of the great explorer of the truth, the master-builder of human happiness. No one rejects, dislikes, or avoids pleasure itself, because it is pleasure, but because those who do not know how to pursue pleasure rationally encounter consequences that are extremely painful. Nor again is there anyone who loves or pursues or desires to obtain pain of itself, because it is pain, but because occasionally circumstances occur in which toil and pain can procure him some great pleasure. To take a trivial example, which of us ever undertakes laborious physical exercise, except to obtain some advantage from it? But who has any right to find fault with a man who chooses to enjoy a pleasure that has no annoying consequences, or one who avoids a pain that produces no resultant pleasure?',
                UserId: '5678687c-fb03-4308-b3ba-7dcf62a2abc7',
                UserLogin: 'rybak74',
                CreatedAt: 1674755739,
                ModifyAt: 1674755739,
                PublishedAt: 1674755739,
                ImgURL: 'https://mirpozitiva.ru/wp-content/uploads/2019/11/1472042660_10.jpg',
                IsPublished: true,
                LikesCount: 15,
                IsLiked: false,
            },
        ])
    }
});

server.on({
    method: 'GET',
    path: '/api/publications/2164687c-fb03-4308-b3ba-7dcf62a2abd5',
    reply: {
        status:  200,
        headers: { "content-type": "application/json" },
        body:    JSON.stringify({
            Id: '2164687c-fb03-4308-b3ba-7dcf62a2abd5',
            Title: 'Lorem Ipsum',
            Content: '<p><strong>Sed ut perspiciatis</strong> unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo. Nemo enim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sed quia consequuntur magni dolores eos qui ratione voluptatem sequi nesciunt. Neque porro quisquam est, qui dolorem ipsum quia dolor sit amet,</p><pre class="ql-syntax" spellcheck="false"> consectetur, adipisci velit, sed quia non numqua m eius modi tempora incidunt ut labore et dolore magnam aliquam quaerat voluptatem. Ut enim ad minima veniam, quis nostrum exercitationem ullam corporis suscipit laboriosam, nisi ut aliquid ex ea commodi consequatur?</pre><p> Quis autem vel eum iure<span style="background-color: rgb(0, 138, 0);"> r</span><span style="color: rgb(255, 255, 0); background-color: rgb(0, 138, 0);">eprehende</span><span style="background-color: rgb(0, 138, 0);">rit q</span>ui in ea voluptate velit esse quam nihi<span style="color: rgb(230, 0, 0);">l molestiae con</span>sequatur, vel illum qui dolorem eum fugiat quo voluptas nulla pariatur</p>',
            UserId: '3458687c-fb03-4308-b3ba-7dcf62a2abc7',
            UserLogin: 'big_dick25',
            CreatedAt: 1674755729,
            ModifyAt: 1674755729,
            PublishedAt: 1674755729,
            ImgURL: 'https://avatarko.ru/img/kartinka/33/multfilm_lyagushka_32117.jpg',
            IsPublished: true,
            LikesCount: 35,
            IsLiked: true,
        })
    }
});

server.on({
    method: 'GET',
    path: '/api/publications/1454687c-fb03-4308-b3ba-7dcf62a2abd5',
    reply: {
        status:  200,
        headers: { "content-type": "application/json" },
        body:    JSON.stringify({
            Id: '1454687c-fb03-4308-b3ba-7dcf62a2abd5',
            Title: 'Target Milk',
            Content: 'But I must explain to you how all this mistaken idea of denouncing pleasure and praising pain was born and I will give you a complete account of the system, and expound the actual teachings of the great explorer of the truth, the master-builder of human happiness. No one rejects, dislikes, or avoids pleasure itself, because it is pleasure, but because those who do not know how to pursue pleasure rationally encounter consequences that are extremely painful. Nor again is there anyone who loves or pursues or desires to obtain pain of itself, because it is pain, but because occasionally circumstances occur in which toil and pain can procure him some great pleasure. To take a trivial example, which of us ever undertakes laborious physical exercise, except to obtain some advantage from it? But who has any right to find fault with a man who chooses to enjoy a pleasure that has no annoying consequences, or one who avoids a pain that produces no resultant pleasure?',
            UserId: '5678687c-fb03-4308-b3ba-7dcf62a2abc7',
            UserLogin: 'rybak74',
            CreatedAt: 1674755739,
            ModifyAt: 1674755739,
            PublishedAt: 1674755739,
            ImgURL: 'https://mirpozitiva.ru/wp-content/uploads/2019/11/1472042660_10.jpg',
            IsPublished: true,
            LikesCount: 35,
            IsLiked: true,
        })
    }
});

server.on({
    method: 'GET',
    path: '/api/publications/9994687c-fb03-4308-b3ba-7dcf62a2abd5',
    reply: {
        status:  200,
        headers: { "content-type": "application/json" },
        body:    JSON.stringify({
            Id: '9994687c-fb03-4308-b3ba-7dcf62a2abd5',
            Title: 'Lorem Ipsum 2',
            Content: 'Tro-lo-lo unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo. Nemo enim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sed quia consequuntur magni dolores eos qui ratione voluptatem sequi nesciunt. Neque porro quisquam est, qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit, sed quia non numquam eius modi tempora incidunt ut labore et dolore magnam aliquam quaerat voluptatem. Ut enim ad minima veniam, quis nostrum exercitationem ullam corporis suscipit laboriosam, nisi ut aliquid ex ea commodi consequatur? Quis autem vel eum iure reprehenderit qui in ea voluptate velit esse quam nihil molestiae consequatur, vel illum qui dolorem eum fugiat quo voluptas nulla pariatur',
            UserId: '3458687c-fb03-4308-b3ba-7dcf62a2abc7',
            UserLogin: 'big_dick25',
            CreatedAt: 1679955729,
            ModifyAt: 1679955729,
            ImgURL: 'https://kartinkof.club/uploads/posts/2022-09/1662227267_1-kartinkof-club-p-novie-i-krasivie-kartinki-lyubie-1.jpg',
            IsPublished: false,
            LikesCount: 35,
            IsLiked: true,
        })
    }
});

server.on({
    method: 'GET',
    path: '/api/users/3458687c-fb03-4308-b3ba-7dcf62a2abc7',
    reply: {
        status:  200,
        headers: { "content-type": "application/json" },
        body:    JSON.stringify({
            Id: '3458687c-fb03-4308-b3ba-7dcf62a2abc7',
            Login: 'big_dick25',
            FirstName: 'Илон',
            LastName: 'Маск',
            Patronymic: 'Алексеевич',
            Role: 'admin',
            Age: 45,
            Description: 'Миллиардер, мэтросексуал, филантроп',
            LikesCount: 35,
            IsLiked: true,
        })
    }
});

server.on({
    method: 'GET',
    path: '/api/users/5678687c-fb03-4308-b3ba-7dcf62a2abc7',
    reply: {
        status:  200,
        headers: { "content-type": "application/json" },
        body:    JSON.stringify({
            Id: '5678687c-fb03-4308-b3ba-7dcf62a2abc7',
            Login: 'rybak74',
            FirstName: 'Владимир',
            LastName: 'Потанин',
            Patronymic: 'Олегович',
            Role: 'user',
            Age: 62,
            Description: 'Бизнесмен, предприниматель',
            LikesCount: 35,
            IsLiked: true,
        })
    }
});

server.on({
    method: 'GET',
    path: '/api/comments/1454687c-fb03-4308-b3ba-7dcf62a2abd5',
    reply: {
        status:  200,
        headers: { "content-type": "application/json" },
        body:    JSON.stringify([
            {
                Id: '0004687c-fb03-4308-b3ba-7dcf62a2abd5',
                UserId: '3458687c-fb03-4308-b3ba-7dcf62a2abc7',
                UserLogin: 'big_dick25',
                ArticleId: '2164687c-fb03-4308-b3ba-7dcf62a2abd5',
                Text: 'О, боже, что я только что прочитал? Это же немыслимая дичь!',
                CreatedAt: 1674788739,
                LikesCount: 35,
                IsLiked: true,
            },
            {
                Id: '0014687c-fb03-4308-b3ba-7dcf62a2abd5',
                UserId: '5678687c-fb03-4308-b3ba-7dcf62a2abc7',
                UserLogin: 'rybak74',
                ArticleId: '2164687c-fb03-4308-b3ba-7dcf62a2abd5',
                Text: 'Сам ты дичь!',
                CreatedAt: 1674955739,
                LikesCount: 35,
                IsLiked: true,
            },
            {
                Id: '0024687c-fb03-4308-b3ba-7dcf62a2abd5',
                UserId: '3458687c-fb03-4308-b3ba-7dcf62a2abc7',
                UserLogin: 'big_dick25',
                ArticleId: '2164687c-fb03-4308-b3ba-7dcf62a2abd5',
                Text: 'Больше не буду писать комменты!!!',
                CreatedAt: 1679755739,
                LikesCount: 35,
                IsLiked: true,
            }
        ])
    }
});

server.on({
    method: 'GET',
    path: '/api/comments/2164687c-fb03-4308-b3ba-7dcf62a2abd5',
    reply: {
        status:  200,
        headers: { "content-type": "application/json" },
        body:    JSON.stringify([
            {
                Id: '0214687c-fb03-4308-b3ba-7dcf62a2abd5',
                UserId: '5678687c-fb03-4308-b3ba-7dcf62a2abc7',
                UserLogin: 'rybak74',
                ArticleId: '1454687c-fb03-4308-b3ba-7dcf62a2abd5',
                Text: 'Капитан Джек Воробей!',
                CreatedAt: 1674788739,
                LikesCount: 35,
                IsLiked: true,
            },
            {
                Id: '0204687c-fb03-4308-b3ba-7dcf62a2abd5',
                UserId: '3458687c-fb03-4308-b3ba-7dcf62a2abc7',
                UserLogin: 'big_dick25',
                ArticleId: '1454687c-fb03-4308-b3ba-7dcf62a2abd5',
                Text: 'Я пришел чтобы отомстить на твоей странице!',
                CreatedAt: 1674788900,
                LikesCount: 35,
                IsLiked: false,
            },
            {
                Id: '0224687c-fb03-4308-b3ba-7dcf62a2abd5',
                UserId: '5678687c-fb03-4308-b3ba-7dcf62a2abc7',
                UserLogin: 'rybak74',
                ArticleId: '1454687c-fb03-4308-b3ba-7dcf62a2abd5',
                Text: 'Я тоже больше не буду писать комменты!!!',
                CreatedAt: 1674789739,
                LikesCount: 35,
                IsLiked: false,
            },
            {
                Id: '0264687c-fb03-4308-b3ba-7dcf62a2abd5',
                UserId: '3458687c-fb03-4308-b3ba-7dcf62a2abc7',
                UserLogin: 'big_dick25',
                ArticleId: '1454687c-fb03-4308-b3ba-7dcf62a2abd5',
                Text: 'Никто и не просит!',
                CreatedAt: 1674798900,
                LikesCount: 35,
                IsLiked: true,
            },
        ])
    }
});

server.on({
    method: 'POST',
    path: '/api/login',
    filter: function(request) {
        return _.isEqual(request.body, {
            Login: 'admin',
            Password: 'admin'
        })
    },
    reply: {
        status:  200,
        headers: { "content-type": "application/json" },
        body:    JSON.stringify({
            Token: 'eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWUsImlhdCI6MTUxNjIzOTAyMn0.VFb0qJ1LRg_4ujbZoRMXnVkUgiuKq5KxWqNdbKq_G9Vvz-S1zZa9LPxtHWKa64zDl2ofkT8F6jBt_K4riU-fPg',
            RefreshToken: 'eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWUsImlhdCI6MTUxNjIzOTAyMn0.-DprLrW2OyqiAFiuWs14WO2TWp2EHtaX7a63dqrklk-xrjaZMrcPhpX4hkZw803SQx5HpGc-7VYBX8l82XlMZg',
            User: {
                Id: '3458687c-fb03-4308-b3ba-7dcf62a2abc7',
                Login: 'big_dick25',
                FirstName: 'Илон',
                LastName: 'Маск',
                Patronymic: 'Алексеевич',
                Role: 'admin',
                Age: 45,
                Description: 'Миллиардер, мэтросексуал, филантроп',
                LikesCount: 35,
                IsLiked: true,
            }
        })
    }
});

server.on({
    method: 'POST',
    path: '/api/login',
    filter: function(request) {
        return _.isEqual(request.body, {
            Login: 'user',
            Password: 'user'
        })
    },
    reply: {
        status:  200,
        headers: { "content-type": "application/json" },
        body:    JSON.stringify({
            Token: 'eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkRhbmlsIiwiYWRtaW4iOnRydWUsImlhdCI6MTUxNjIzOTAyMn0.TI9Co9XS1Md9Ov5Xq3hh2fLGmsWytLoFrvc7LzqMZ9pYnyhkpQ6PryJ-ImUAALC3kb2osa_7C5j_LSYVZYviXw',
            RefreshToken: 'eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkRhbmlsIiwiYWRtaW4iOnRydWUsImlhdCI6MTUxNjIzOTAyMn0.fK2ZYt85hw4ScpieADgGsKeW2brg9sb-KJUFF2o7Yq_RAzg3GEmnDBvCyfClXCcNE1xuX040S1IcAZbhX-7P6A',
            User: {
                Id: '5678687c-fb03-4308-b3ba-7dcf62a2abc7',
                Login: 'rybak74',
                FirstName: 'Владимир',
                LastName: 'Потанин',
                Patronymic: 'Олегович',
                Role: 'user',
                Age: 62,
                Description: 'Бизнесмен, предприниматель',
                LikesCount: 35,
                IsLiked: true,
            }
        })
    }
});

server.on({
    method: 'GET',
    path: '/api/users/3458687c-fb03-4308-b3ba-7dcf62a2abc7/publications',
    reply: {
        status:  200,
        headers: { "content-type": "application/json" },
        body:    JSON.stringify([
            {
                Id: '2164687c-fb03-4308-b3ba-7dcf62a2abd5',
                Title: 'Lorem Ipsum',
                Content: '<p><strong>Sed ut perspiciatis</strong> unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo. Nemo enim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sed quia consequuntur magni dolores eos qui ratione voluptatem sequi nesciunt. Neque porro quisquam est, qui dolorem ipsum quia dolor sit amet,</p><pre class="ql-syntax" spellcheck="false"> consectetur, adipisci velit, sed quia non numqua m eius modi tempora incidunt ut labore et dolore magnam aliquam quaerat voluptatem. Ut enim ad minima veniam, quis nostrum exercitationem ullam corporis suscipit laboriosam, nisi ut aliquid ex ea commodi consequatur?</pre><p> Quis autem vel eum iure<span style="background-color: rgb(0, 138, 0);"> r</span><span style="color: rgb(255, 255, 0); background-color: rgb(0, 138, 0);">eprehende</span><span style="background-color: rgb(0, 138, 0);">rit q</span>ui in ea voluptate velit esse quam nihi<span style="color: rgb(230, 0, 0);">l molestiae con</span>sequatur, vel illum qui dolorem eum fugiat quo voluptas nulla pariatur</p>',
                UserId: '3458687c-fb03-4308-b3ba-7dcf62a2abc7',
                UserLogin: 'big_dick25',
                CreatedAt: 1674755729,
                ModifyAt: 1674755729,
                PublishedAt: 1674755729,
                ImgURL: 'https://avatarko.ru/img/kartinka/33/multfilm_lyagushka_32117.jpg',
                IsPublished: true,
                LikesCount: 35,
                IsLiked: true,
            },
            {
                Id: '9994687c-fb03-4308-b3ba-7dcf62a2abd5',
                Title: 'Lorem Ipsum 2',
                Content: 'Tro-lo-lo unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo. Nemo enim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sed quia consequuntur magni dolores eos qui ratione voluptatem sequi nesciunt. Neque porro quisquam est, qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit, sed quia non numquam eius modi tempora incidunt ut labore et dolore magnam aliquam quaerat voluptatem. Ut enim ad minima veniam, quis nostrum exercitationem ullam corporis suscipit laboriosam, nisi ut aliquid ex ea commodi consequatur? Quis autem vel eum iure reprehenderit qui in ea voluptate velit esse quam nihil molestiae consequatur, vel illum qui dolorem eum fugiat quo voluptas nulla pariatur',
                UserId: '3458687c-fb03-4308-b3ba-7dcf62a2abc7',
                UserLogin: 'big_dick25',
                CreatedAt: 1679955729,
                ModifyAt: 1679955729,
                ImgURL: 'https://kartinkof.club/uploads/posts/2022-09/1662227267_1-kartinkof-club-p-novie-i-krasivie-kartinki-lyubie-1.jpg',
                IsPublished: false,
                LikesCount: 35,
                IsLiked: true,
            }
        ])
    }
});

server.on({
    method: 'GET',
    path: '/api/users/5678687c-fb03-4308-b3ba-7dcf62a2abc7/publications',
    reply: {
        status:  200,
        headers: { "content-type": "application/json" },
        body:    JSON.stringify([
            {
                Id: '1454687c-fb03-4308-b3ba-7dcf62a2abd5',
                Title: 'Target Milk',
                Content: 'But I must explain to you how all this mistaken idea of denouncing pleasure and praising pain was born and I will give you a complete account of the system, and expound the actual teachings of the great explorer of the truth, the master-builder of human happiness. No one rejects, dislikes, or avoids pleasure itself, because it is pleasure, but because those who do not know how to pursue pleasure rationally encounter consequences that are extremely painful. Nor again is there anyone who loves or pursues or desires to obtain pain of itself, because it is pain, but because occasionally circumstances occur in which toil and pain can procure him some great pleasure. To take a trivial example, which of us ever undertakes laborious physical exercise, except to obtain some advantage from it? But who has any right to find fault with a man who chooses to enjoy a pleasure that has no annoying consequences, or one who avoids a pain that produces no resultant pleasure?',
                UserId: '5678687c-fb03-4308-b3ba-7dcf62a2abc7',
                UserLogin: 'rybak74',
                CreatedAt: 1674755739,
                ModifyAt: 1674755739,
                PublishedAt: 1674755739,
                ImgURL: 'https://mirpozitiva.ru/wp-content/uploads/2019/11/1472042660_10.jpg',
                IsPublished: true,
                LikesCount: 35,
                IsLiked: true,
            }
        ])
    }
});

server.on({
    method: 'POST',
    path: '/api/comments/add',
    filter: function(request) {
        console.log(request)
        return true;
    },
    reply: {
        status:  200,
        headers: { "content-type": "application/json" }
    }
});

server.on({
    method: 'POST',
    path: '/api/publications/add',
    filter: function(request) {
        console.log(request)
        return true;
    },
    reply: {
        status:  200,
        headers: { "content-type": "application/json" }
    }
});

server.on({
    method: 'POST',
    path: '/api/publications/like',
    filter: function(request) {
        console.log(request)
        return true;
    },
    reply: {
        status:  200,
        headers: { "content-type": "application/json" }
    }
});

server.on({
    method: 'POST',
    path: '/api/users/like',
    filter: function(request) {
        console.log(request)
        return true;
    },
    reply: {
        status:  200,
        headers: { "content-type": "application/json" }
    }
});

server.on({
    method: 'POST',
    path: '/api/comments/like',
    filter: function(request) {
        console.log(request)
        return true;
    },
    reply: {
        status:  200,
        headers: { "content-type": "application/json" }
    }
});

server.on({
    method: 'POST',
    path: '/api/register',
    filter: function(request) {
        console.log(request)
        return true;
    },
    reply: {
        status:  200,
        headers: { "content-type": "application/json" },
        body:    JSON.stringify({
            Token: 'kyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkRhbmlsIiwiYWRtaW4iOnRydWUsImlhdCI6MTUxNjIzOTAyMn0.TI9Co9XS1Md9Ov5Xq3hh2fLGmsWytLoFrvc7LzqMZ9pYnyhkpQ6PryJ-ImUAALC3kb2osa_7C5j_LSYVZYviXw',
            RefreshToken: 'kyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkRhbmlsIiwiYWRtaW4iOnRydWUsImlhdCI6MTUxNjIzOTAyMn0.fK2ZYt85hw4ScpieADgGsKeW2brg9sb-KJUFF2o7Yq_RAzg3GEmnDBvCyfClXCcNE1xuX040S1IcAZbhX-7P6A',
            User: {
                Id: '8878687c-fb03-4308-b3ba-7dcf62a2abc7',
                Login: 'shabanchik95',
                FirstName: 'Данил',
                LastName: 'Шабанов',
                Patronymic: 'Валерьевич',
                Role: 'user',
                Age: 27,
                Description: 'Программист-гитарист',
                LikesCount: 0,
                IsLiked: false,
            }
        })
    }
});

server.on({
    method: 'POST',
    path: '/api/recovery/login',
    filter: function(request) {
        console.log(request)
        return true;
    },
    reply: {
        status:  200,
        headers: { "content-type": "application/json" },
        body:    JSON.stringify({
            Question: 'Два кольца, два конца, посередине гвоздик. Что это?',
            TransactionId: '0078687c-fb03-4308-b3ba-7dcf62a2abc7'
        })
    }
});

server.on({
    method: 'POST',
    path: '/api/recovery/answer',
    filter: function(request) {
        return _.isEqual(request.body, {
            Answer: 'Ножницы',
            TransactionId: '0078687c-fb03-4308-b3ba-7dcf62a2abc7'
        })
    },
    reply: {
        status:  200,
        headers: { "content-type": "application/json" },
        body:    JSON.stringify({
            Token: 'kyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkRhbmlsIiwiYWRtaW4iOnRydWUsImlhdCI6MTUxNjIzOTAyMn0.TI9Co9XS1Md9Ov5Xq3hh2fLGmsWytLoFrvc7LzqMZ9pYnyhkpQ6PryJ-ImUAALC3kb2osa_7C5j_LSYVZYviXw',
            RefreshToken: 'kyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkRhbmlsIiwiYWRtaW4iOnRydWUsImlhdCI6MTUxNjIzOTAyMn0.fK2ZYt85hw4ScpieADgGsKeW2brg9sb-KJUFF2o7Yq_RAzg3GEmnDBvCyfClXCcNE1xuX040S1IcAZbhX-7P6A',
            User: {
                Id: '8878687c-fb03-4308-b3ba-7dcf62a2abc7',
                Login: 'shabanchik95',
                FirstName: 'Данил',
                LastName: 'Шабанов',
                Patronymic: 'Валерьевич',
                Role: 'user',
                Age: 27,
                Description: 'Программист-гитарист',
                LikesCount: 0,
                IsLiked: false,
            }
        })
    }
});

server.on({
    method: 'POST',
    path: '/api/recovery/password',
    filter: function(request) {
        console.log(request)
        return true;
    },
    reply: {
        status:  200,
        headers: { "content-type": "application/json" }
    }
});

server.start(() => {
    console.log('Server succesfully started')
});
