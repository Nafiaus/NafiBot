# NafiBot
This is a bot I'm creating for my twitch chat in C#, C++

How it works:
  After starting all the applications will connect to their targets and start listening. If a message is sent on Twitch, the camera controller will parse the message and look for cam commands, and nafibot will look for nafibot commands and they both will execute commands if found.

---------------------------------------------------

FUTURE IDEAS

I want to add a logging method, so I can add a message history function so people can grab how many messages they have sent, or have a leaderboard.

IDEAS ADDED INSTEAD OF A LOGGING METHOD
- Chat GPT
- Merged 3 useless function into 1 useless function
- Enabled YouTube chat the ablility to use camera controls (aka learned javascript & node.js) One second... I LEARNED A NEW LANGUAGE INSTEAD OF ADDING LOGGING METHOD
- Added a python script that will grab command line arguments from c# app and turn the gpt text into a voice to be played aloud to stream. Yes I learned another new language instead of logging...
- Removed python and javascript because I hate it.
- Added the ability to use my keyboard to Twitch

---------------------------------------------------

CAMERA COMMANDS

---------------------------------------------------

!laser :: shoots a laser for 5 seconds

!camreset :: resets the camera to normal

!left :: moves camera left 5°

!right :: moves camera right 5°

!down :: moves camera down 5°

!up :: moves camera up 5°

!white :: turns the camera light white

!red :: turns the camera light red

!yellow :: turns the camera light yellow

!green :: turns the camera light green

!blue :: turns the camera light blue

!cyan :: turns the camera light cyan

!magenta :: turns the camera light magenta

!lightoff :: turns off the camera light

!lighton :: turns on the camera light

!brightnessup :: turns up the brightness by 10

!brightnessdown :: turns down the brightness by 10

!flashbang :: "flashbangs" me.

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
