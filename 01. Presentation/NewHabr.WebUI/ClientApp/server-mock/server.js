var ServerMock = require("mock-http-server");

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
                Content: 'Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo. Nemo enim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sed quia consequuntur magni dolores eos qui ratione voluptatem sequi nesciunt. Neque porro quisquam est, qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit, sed quia non numquam eius modi tempora incidunt ut labore et dolore magnam aliquam quaerat voluptatem. Ut enim ad minima veniam, quis nostrum exercitationem ullam corporis suscipit laboriosam, nisi ut aliquid ex ea commodi consequatur? Quis autem vel eum iure reprehenderit qui in ea voluptate velit esse quam nihil molestiae consequatur, vel illum qui dolorem eum fugiat quo voluptas nulla pariatur',
                UserId: '3458687c-fb03-4308-b3ba-7dcf62a2abc7',
                CreatedAt: 1674755729,
                ModifyAt: 1674755729,
                PublishedAt: 1674755729,
            },
            {
                Id: '1454687c-fb03-4308-b3ba-7dcf62a2abd5',
                Title: 'Target Milk',
                Content: 'But I must explain to you how all this mistaken idea of denouncing pleasure and praising pain was born and I will give you a complete account of the system, and expound the actual teachings of the great explorer of the truth, the master-builder of human happiness. No one rejects, dislikes, or avoids pleasure itself, because it is pleasure, but because those who do not know how to pursue pleasure rationally encounter consequences that are extremely painful. Nor again is there anyone who loves or pursues or desires to obtain pain of itself, because it is pain, but because occasionally circumstances occur in which toil and pain can procure him some great pleasure. To take a trivial example, which of us ever undertakes laborious physical exercise, except to obtain some advantage from it? But who has any right to find fault with a man who chooses to enjoy a pleasure that has no annoying consequences, or one who avoids a pain that produces no resultant pleasure?',
                UserId: '5678687c-fb03-4308-b3ba-7dcf62a2abc7',
                CreatedAt: 1674755739,
                ModifyAt: 1674755739,
                PublishedAt: 1674755739,
            },
        ])
    }
});

server.start(() => {
    console.log('Server succesfully started')
});
