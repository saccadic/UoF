var WebSocket = require('ws');
var ws = new WebSocket('ws://localhost:8080');

ws.on('open', function open(data) {
  ws.send('connection');
  console.log(data);
});
var c=0;
ws.on('message', function(data, flags) {
    console.log(c +"::"+ data.length);
    c++;
});
var t=5;
setInterval(function() {
    ws.send('img');
    console.log("send");
  

}, t);