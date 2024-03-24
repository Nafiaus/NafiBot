const express = require('express');
const path = require('path');
const youtubeService = require('./youtubeService.js');
const server = express();

server.get('/', (req, res) =>

res.sendFile(path.join(__dirname + '/index.html'))
);

server.listen(3000, function() {
	console.log('Server is Ready');
});

server.get('/authorize', (request, response) => {
	console.log('/auth');
	youtubeService.getCode(response);
});

server.get('/callback', (req, response) => {
	const { code } = req.query;
	
	youtubeService.getTokensWithCode(code);
	response.redirect('/');
});

server.get('/find-active-chat', (req, res) => {
	youtubeService.findActiveChat();
	res.redirect('/');
});

server.get('/start-tracking-chat', (req, res) => {
	youtubeService.startTrackingChat();
	res.redirect('/');
});
1
server.get('/stop-tracking-chat', (req, res) => {
	youtubeService.stopTrackingChat();
	res.redirect('/');
});

server.get('/insert-message', (req, res) => {
	youtubeService.insertMecssage("Hello World");
	res.redirect('/');
});
