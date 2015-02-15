var WebSocketServer = require('ws').Server
  , wss = new WebSocketServer({ port: 8080 });

wss.on('connection', function connection(ws) {
  
  console.log(ws.protocolVersion);
  
  ws.on('message', function incoming(message) {
    console.log('received: %s', message);
    ws.send("From Node.js:"+message);
  });

  ws.on('close', function(message) {
    console.log('close');
  });
  
  ws.send('Node.js');
});