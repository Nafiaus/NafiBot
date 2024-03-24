const { google } = require('googleapis');
const youtube = google.youtube('v3');
const OAuth2 = google.auth.OAuth2;
const util = require('util');
const fs = require('fs');
const tmi = require('tmi.js');

let liveChatId;
let nextPage;

const intervalTime = 5000;
let interval;
let lastMessageCount;
let currentMessagePointer = 0;
let outputStream;
let chatMessages = [];

const TOKEN_FILE = './tokens.json';

const channel = 'your channel name';  // CHANGE
const botUsername = 'your bots username';  // CHANGE
const twitchOAuth = 'your twitch oauth token';  // CHANGE
const clientId = 'your youtube client id';  // CHANGE
const clientSecret = 'your youtube secret';  // CHANGE

const redirectURI = 'http://localhost:3000/callback';

const writeFilePromise = util.promisify(fs.writeFile);
const readFilePromise = util.promisify(fs.readFile);

const client = new tmi.Client({
	connection: {
		secure: true,
		reconnect: true
	},
	identity: {
		username: botUsername,
		password: twitchOAuth
	},
	channels: [ channel ]
});

const save = async (path, str) => {
	await writeFilePromise(path, str);
};

const read = async path => {
	const fileContents = await readFilePromise(path);
	return JSON.parse(fileContents);
};

const scope = [
	'https://www.googleapis.com/auth/youtube.readonly',
	'https://www.googleapis.com/auth/youtube',
	'https://www.googleapis.com/auth/youtube.force-ssl'
];

const auth = new OAuth2(clientId, clientSecret, redirectURI);

const youtubeService = {};

youtubeService.getCode = response => {
	const authUrl = auth.generateAuthUrl({
		access_type: 'offline', scope
	});
	response.redirect(authUrl);
};

youtubeService.getTokensWithCode = async code => {
	const credentials = await auth.getToken(code);
	
	youtubeService.authorize(credentials);
};

youtubeService.authorize = ({ tokens }) => {
	console.log('Succesfully set credentials');
	console.log('tokens:', tokens);
	save(TOKEN_FILE, JSON.stringify(tokens));
};

auth.on('tokens', (tokens) => {
	if (tokens.refresh_token) {
		save(TOKEN_FILE, JSON.stringify(auth.tokens));
		console.log(tokens.refresh_token);
	}
	console.log(tokens.access_token);
});

const checkTokens = async () => {
	const tokens = await read(TOKEN_FILE);
	if (tokens) {
		auth.setCredentials(tokens);
		console.log('Tokens set');
	} else {
		console.log('No tokens set');
	}
};

checkTokens();
client.connect();

youtubeService.findActiveChat = async () => {
	const response = await youtube.liveBroadcasts.list({
		auth,
		part: 'snippet',
		mine: 'true'
	});
	console.log(response.data.items[0]);
	const latestChat = response.data.items[0];
	liveChatId = latestChat.snippet.liveChatId;
	console.log('Chat ID Found:', liveChatId);
};

const getChatMessages = async () => {
	const response = await youtube.liveChatMessages.list({
		auth,
		part: 'snippet',
		liveChatId,
		pageToken: nextPage
	});
	const { data } = response;
	const newMessages = data.items;
	
	let twitchMessage;
	
	lastMessageCount = chatMessages.length;
	
	chatMessages.push(...newMessages);
	nextPage = data.nextPageToken;
	
	let chatMessageDifference = chatMessages.length - lastMessageCount;
	
	if (lastMessageCount == chatMessages.length) {
		
	} else {
		for (let i = 0; i < chatMessageDifference; i++) {
			outputStream = chatMessages[currentMessagePointer].snippet.displayMessage;
			switch (outputStream.toLowerCase()) {
				case '!up':
					client.say(channel, '!up');
					break;
				case '!right':
					client.say(channel, '!right');
					break;
				case '!left':
					client.say(channel, '!left');
					break;
				case '!down':
					client.say(channel, '!down');
					break;
				case '!flashbang':
					client.say(channel, '!flashbang');
					break;
				case '!laser':
					client.say(channel, '!laser');
					break;
				case '!camreset':
					client.say(channel, '!camreset');
					break;
				case '!white':
					client.say(channel, '!white');
					break;
				case '!red':
					client.say(channel, '!red');
					break;
				case '!yellow':
					client.say(channel, '!yellow');
					break;
				case '!green':
					client.say(channel, '!green');
					break;
				case '!blue':
					client.say(channel, '!blue');
					break;
				case '!cyan':
					client.say(channel, '!cyan');
					break;
				case '!magenta':
					client.say(channel, '!magenta');
					break;
				case '!lightoff':
					client.say(channel, '!lightoff');
					break;
				case '!lighton':
					client.say(channel, '!lighton');
					break;
				case '!brightnessup':
					client.say(channel, '!brightnessup');
					break;
				case '!brightnessdown':
					client.say(channel, '!brightnessdown');
					break;
				default:
					// do nothing
			}
			console.log(outputStream);
			currentMessagePointer++;
		}
	}
	lastMessageCount = chatMessages.length;
};

youtubeService.startTrackingChat = () => {
	interval = setInterval(getChatMessages, intervalTime);
	console.log('Starting Message Tracking');
};

youtubeService.stopTrackingChat = () => {
	clearInterval(interval);
	console.log('Ending Message Tracking');
};


youtubeService.insertMessage = messageText => {
	youtube.liveChatMessages.insert(
	{
		auth,
		part: 'snippet',
		resource: {
			snippet: {
				type: 'textMessageEvent',
				liveChatId,
				textMessageDetails: {
					messageText
				}
			}
		}
	},
	() => {}
	);
}

module.exports = youtubeService;
// Nothing after this
