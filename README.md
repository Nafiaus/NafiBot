# NafiBot
This is a bot I'm creating for my twitch chat in C#, C++, and JavaScript. I'll be updating this as I add more to it.

Currently to run it you need to go live on youtube, next open cmd and go to server.js root dir and run "node server.js", afterwards go to the website http://localhost:3000 and click the buttons "Authorize", "Get Active Chat", "Start Tracking Chat" in that order, then run the C# application Nafibot.exe and you are set up!

How it works:
  After starting all the applications will connect to their targets and start listening. If a message is sent on Twitch, the camera controller will parse the message and look for cam commands, and nafibot will look for nafibot commands and they both will execute commands if found. If the message is sent on youtube, a node server will parse the message and see if any cam commands are sent and if so then it will forward the message to twitch chat through nafibot. The camera will pick up the message then execute the command.

---------------------------------------------------

FUTURE IDEAS

I want to add a logging method, so I can add a message history function so people can grab how many messages they have sent, or have a leaderboard.

IDEAS ADDED INSTEAD OF A LOGGING METHOD
- Chat GPT
- Merged 3 useless function into 1 useless function
- Enabled YouTube chat the ablility to use camera controls (aka learned javascript & node.js) One second... I LEARNED A NEW LANGUAGE INSTEAD OF ADDING LOGGING METHOD

---------------------------------------------------

CHAT COMMANDS

---------------------------------------------------

yo ::  $(touser) Yo Deez Nuts in your mouth, Got Em' Kappa

start :: Daddy chill... ❤️

!nafibot [thinker||sarcastic||rude] :: Ask Nafibot anything (using Chat GPT 4 Turbo) [secondary command is not needed]

!mystatus [mod||sub||turbo] :: Will return your status

!getmycolor :: $(touser) your chat color is $(chat color)

!discord :: THE DISCORD LINK! ---> https://discord.gg/zCFh23dXvW <--- click to join! Make sure to react "Thumbs Up" in the Rules!

!yt :: Subscribe to My YouTube! https://m.youtube.com/channel/UCvDQj2yZ-yHTFheHlSMQang

!time :: [CURRENT TIME IN CENTRAL STANDARD TIME]

!claim :: $(touser)  Has Claimed Deez Nuts \U0001f95c Got Em'

!activate ::  $(touser) Has Activated Deez Nuts \U0001f95c Got Em'
