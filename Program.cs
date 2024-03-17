using Nafi.Twitch;

namespace NafiBot
{
    class Program
    {
        static string oauth = [OAUTH TOKEN HERE]; // CHANGE YOUR OAUTH TOKEN
        static string channel = [YOUR CHANNEL HERE]; // CHANGE YOUR CHANNEL NAME
        static string nick = [BOT NICKNAME HERE]; // CHANGE YOUR BOT NICKNAME

        static TwitchBot bot = new TwitchBot(oauth, channel, nick);

        static void Main(string[] args)
        {
            while (true)
            {
                TwitchChatMessage? chatMessage = null;
                chatMessage = bot.Read();
                if (chatMessage != null)
                {
                    GetFirstTime(chatMessage);
                    CheckCommand(chatMessage);
                }
            }

            static void CheckCommand(TwitchChatMessage chatMessage)
            {
                string msg = chatMessage.Message;
                switch (msg)
                {
                    case "yo":
                        DeezNutsYo(chatMessage);
                        break;
                    case "start":
                        DaddyChill(chatMessage);
                        break;
                    case "!discord":
                        PlugDiscord(chatMessage);
                        break;
                    case "!yt":
                        PlugYT(chatMessage);
                        break;
                    case "!time":
                        GetTime(chatMessage);
                        break;
                    case "!claim":
                        ClaimNuts(chatMessage);
                        break;
                    case "!activate":
                        ActivateNuts(chatMessage);
                        break;
                    case "!amimod":
                        GetModStatus(chatMessage);
                        break;
                    case "!amisub":
                        GetSubStatus(chatMessage);
                        break;
                    case "!amiturbo":
                        GetTurboStatus(chatMessage);
                        break;
                    case "!getmycolor":
                        GetChatterColor(chatMessage);
                        break;
                    case "!commands":
                        GiveCommands(chatMessage);
                        break;
                    default:
                        break;
                }
            }

            // Checks to see if chatter is first time chatter
            static void GetFirstTime(TwitchChatMessage chatMessage)
            {
                if (chatMessage.IsFirstMessage == "True")
                {
                    bot.Write($"Welcome {chatMessage.Sender} to the stream! I hope you enjoy your stay. You can get a list of commands by using \"!commands\"");
                }
            }

            // Give Commands
            static void GiveCommands(TwitchChatMessage chatMessage)
            {
                string line = chatMessage.Sender + " you can get a list of commands here: https://discord.gg/Z5ZbqW4h9u";
                bot.Write(line);
            }
            // Deez Nuts Yo
            static void DeezNutsYo(TwitchChatMessage chatMessage)
            {
                string line = chatMessage.Sender + " Yo Deez Nuts in your mouth, Got Em\' Kappa";
                bot.Write(line);
            }

            // Daddy Chill
            static void DaddyChill(TwitchChatMessage chatMessage)
            {
                string line = "Daddy Chill... <3";
                bot.Write(line);
            }

            // This function plugs my Discord
            static void PlugDiscord(TwitchChatMessage chatMessage)
            {
                string line = "THE DISCORD LINK! ---> https://discord.gg/zCFh23dXvW <--- click to join! Make sure to react \"Thumbs Up\" in the Rules!";
                bot.Write(line);
            }
            // This function is a Troll Function
            static void ClaimNuts(TwitchChatMessage chatMessage)
            {
                string line = chatMessage.Sender + " Has Claimed Deez Nuts \U0001f95c Got Em'";
                bot.Write(line);
            }

            // This function is a Troll Function
            static void ActivateNuts(TwitchChatMessage chatMessage)
            {
                string line = chatMessage.Sender + " Has Activated Deez Nuts \U0001f95c Got Em'";
                bot.Write(line);
            }

            // This function is for plugging my YouTube Channel
            static void PlugYT(TwitchChatMessage chatMessage)
            {
                string line = "Subscribe to My YouTube! https://m.youtube.com/channel/UCvDQj2yZ-yHTFheHlSMQang";
                bot.Write(line);
            }

            // This function grabs the current local time
            static void GetTime(TwitchChatMessage chatMessage)
            {
                DateTime now = DateTime.Now;
                bot.Write(now.ToString());
            }

            // This function grabs you color in chat
            static void GetChatterColor(TwitchChatMessage chatMessage)
            {
                string chatColor;
                switch (chatMessage.Color)
                {
                    case "Red":
                        chatColor = "#FF0000";
                        break;
                    case "Blue":
                        chatColor = "#0000FF";
                        break;
                    case "Green":
                        chatColor = "#008000";
                        break;
                    case "FireBrick":
                        chatColor = "#B22222";
                        break;
                    case "Coral":
                        chatColor = "#FF7F50";
                        break;
                    case "Golden Rod":
                        chatColor = "#DAA520";
                        break;
                    case "Blue Violet":
                        chatColor = "#8A2BE2";
                        break;
                    case "Chocolate":
                        chatColor = "#D2691E";
                        break;
                    case "Cadet Blue":
                        chatColor = "#5F9EA0";
                        break;
                    case "Yellow Green":
                        chatColor = "#9ACD32";
                        break;
                    case "Sea Green":
                        chatColor = "#2E8B57";
                        break;
                    case "Dodger Blue":
                        chatColor = "#1E90FF";
                        break;
                    case "Spring Green":
                        chatColor = "#00FF7F";
                        break;
                    case "Orange Red":
                        chatColor = "#FF4500";
                        break;
                    case "Hot Pink":
                        chatColor = "#FF69B4";
                        break;
                    default:
                        chatColor = "Error";
                        break;
                }
                bot.Write($"{chatMessage.Sender} your chat color is {chatMessage.Color} or {chatColor}");
            }

            // This function grabs the mod status of the sender
            static void GetModStatus(TwitchChatMessage chatMessage)
            {
                if (chatMessage.IsModerator == "False")
                {
                    bot.Write($"{chatMessage.Sender} you are not a Moderator! ROFL");
                }
                else if (chatMessage.IsModerator == "True")
                {
                    bot.Write($"{chatMessage.Sender} you are a moderator! <3");
                }
            }

            // This function grabs the mod status of the sender
            static void GetSubStatus(TwitchChatMessage chatMessage)
            {
                if (chatMessage.IsSubscriber == "False")
                {
                    bot.Write($"{chatMessage.Sender} you are not a subscriber! Sadge");
                }
                else if (chatMessage.IsSubscriber == "True")
                {
                    bot.Write($"{chatMessage.Sender} you are an amazing subscriber! <3");
                }
            }

            // This function grabs the mod status of the sender
            static void GetTurboStatus(TwitchChatMessage chatMessage)
            {
                if (chatMessage.IsTurbo == "False")
                {
                    bot.Write($"{chatMessage.Sender} you are not a Twitch Turbo User!");
                }
                else if (chatMessage.IsTurbo == "True")
                {
                    bot.Write($"{chatMessage.Sender} you are a Twitch Turbo User!");
                }
            }
        }
    }
}
