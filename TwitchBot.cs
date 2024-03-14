using System.Net.Sockets;

namespace Nafi.Twitch
{
    // Twitch bot object class
    public class TwitchBot : IDisposable
    {
        bool exit = false;
        string channel;
        string nickname;

        TcpClient tcpClient;
        StreamReader inStream;
        StreamWriter outStream;

        Thread pingThread;

        public TwitchBot(string oauth_token, string channel, string nickname)
        {
            this.channel = channel;
            this.nickname = nickname;

            tcpClient = new TcpClient("irc.twitch.tv", 6667);
            inStream = new StreamReader(tcpClient.GetStream());
            outStream = new StreamWriter(tcpClient.GetStream());

            outStream.WriteLine("CAP REQ :twitch.tv/command twitch.tv/tags twitch.tv/membership");
            outStream.WriteLine($"PASS {oauth_token}");
            outStream.WriteLine($"NICK {nickname}");
            outStream.WriteLine($"JOIN #{channel}");
            outStream.Flush();

            pingThread = new Thread(() => pingTwitch());
            pingThread.Start();
        }

        // Read a message coming in from Twitch chat and parse it.
        public TwitchChatMessage? Read()
        {
            // Get Raw Message from chat
            string? chatRaw;
            while (true)
            {
                if (exit) return null;

                chatRaw = inStream.ReadLine();

                if (chatRaw == null)
                {
                    Thread.Sleep(100);
                    continue;
                }

                if (!chatRaw.Contains("PRIVMSG")) continue;
                break;
            }

            // Parse Raw Message into words by the ';' character
            string[] flags = chatRaw.Split(';');

            // Get Sender Color
            int colorStartIndex = flags[3].IndexOf("=") + 1;
            string chatColorHex = flags[3].Substring(colorStartIndex);
            string chatColor;
            switch (chatColorHex)
            {
                case "#FF0000":
                    chatColor = "Red";
                    break;
                case "#0000FF":
                    chatColor = "Blue";
                    break;
                case "#008000":
                    chatColor = "Green";
                    break;
                case "#B22222":
                    chatColor = "FireBrick";
                    break;
                case "#FF7F50":
                    chatColor = "Coral";
                    break;
                case "#DAA520":
                    chatColor = "Golden Rod";
                    break;
                case "#8A2BE2":
                    chatColor = "Blue Violet";
                    break;
                case "#D2691E":
                    chatColor = "Chocolate";
                    break;
                case "#5F9EA0":
                    chatColor = "Cadet Blue";
                    break;
                case "#9ACD32":
                    chatColor = "Yellow Green";
                    break;
                case "#2E8B57":
                    chatColor = "Sea Green";
                    break;
                case "#1E90FF":
                    chatColor = "Dodger Blue";
                    break;
                case "#00FF7F":
                    chatColor = "Spring Green";
                    break;
                case "#FF4500":
                    chatColor = "Orange Red";
                    break;
                case "#FF69B4":
                    chatColor = "Hot Pink";
                    break;
                default:
                    chatColor = "Error";
                    break;
            }

            // Get Subscriber Status
            int subcriberStartIndex = flags[12].IndexOf("=") + 1;
            string chatSubscriber;
            string subscriberBoolean = flags[12].Substring(subcriberStartIndex);
            if (subscriberBoolean == "0")
            {
                chatSubscriber = "False";
            }
            else if (subscriberBoolean == "1")
            {
                chatSubscriber = "True";
            }
            else
            {
                chatSubscriber = "Error";
            }

            // Get Mod Status
            int modStartIndex = flags[9].IndexOf("=") + 1;
            string chatMod;
            string modBoolean = flags[9].Substring(modStartIndex);
            if (modBoolean == "0")
            {
                chatMod = "False";
            }
            else if (modBoolean == "1")
            {
                chatMod = "True";
            }
            else
            {
                chatMod = "Error";
            }

            // Get Turbo Status
            int turboStartIndex = flags[14].IndexOf("=") + 1;
            string chatTurbo;
            string turboBoolean = flags[14].Substring(turboStartIndex);
            if (turboBoolean == "0")
            {
                chatTurbo = "False";
            }
            else if (turboBoolean == "1")
            {
                chatTurbo = "True";
            }
            else
            {
                chatTurbo = "Error";
            }

            // Get Returning Chatter Status
            int returningChatterStartIndex = flags[10].IndexOf("=") + 1;
            string chatReturningChatter;
            string returningChatterBoolean = flags[10].Substring(returningChatterStartIndex);
            if (returningChatterBoolean == "0")
            {
                chatReturningChatter = "False";
            }
            else if (returningChatterBoolean == "1")
            {
                chatReturningChatter = "True";
            }
            else
            {
                chatReturningChatter = "Error";
            }

            // Get First Message Status
            int firstMessageStartIndex = flags[6].IndexOf("=") + 1;
            string chatFirstMessage;
            string firstMessageBoolean = flags[6].Substring(firstMessageStartIndex);
            if (firstMessageBoolean == "0")
            {
                chatFirstMessage = "False";
            }
            else if (firstMessageBoolean == "1")
            {
                chatFirstMessage = "True";
            }
            else
            {
                chatFirstMessage = "Error";
            }

            // Get Sender Message ID
            int msgIdStartIndex = flags[8].IndexOf("=") + 1;
            string chatMsgId = flags[8].Substring(msgIdStartIndex);

            // Get Sender Name
            int nameStartIndex = flags[4].IndexOf("=") + 1;
            string chatSender = flags[4].Substring(nameStartIndex);

            // Get User ID
            int uidStartIndex = flags[15].IndexOf("=") + 1;
            string chatUID = flags[15].Substring(uidStartIndex);

            // Get Sender Message
            int messageStartIndex = flags[16].IndexOf(":") + 1;
            int messageOffset = (channel.Length * 4) + 6;
            string chatMessage = flags[16].Substring(messageOffset + messageOffset);

            // Get Sender Type
            int userTypeStartIndex = flags[16].IndexOf("=") + 1;
            int userTypeEndIndex = flags[16].IndexOf(":") - 1;
            int userTypeLength = userTypeEndIndex - userTypeStartIndex;
            string chatUserTypeRaw = flags[16].Substring(userTypeStartIndex, userTypeLength);
            string chatUserType;
            switch (chatUserTypeRaw)
            {
                case "":
                    chatUserType = "Normal User";
                    break;
                case "admin":
                    chatUserType = "Twitch Administrator";
                    break;
                case "global_mod":
                    chatUserType = "Global Moderator";
                    break;
                case "staff":
                    chatUserType = "Twitch Employee";
                    break;
                default:
                    chatUserType = "Error";
                    break;
            }

            return new TwitchChatMessage(chatRaw, chatUserType, chatColor, chatMsgId, chatMod, chatSubscriber, chatTurbo, chatUID, chatReturningChatter, chatFirstMessage, chatSender, chatMessage);
        }

        // Write a message to Twitch chat using bot
        public bool Write(string message)
        {
            return TcpWrite($":{nickname}!{nickname}@{nickname}.tmi.twitch.tv PRIVMSG #{channel} :{message}");
        }

        // Read a line from incoming from Twitch IRC server
        string? TcpRead()
        {
            return inStream.ReadLine();
        }

        // Write a line out to Twitch IRC server
        bool TcpWrite(string message)
        {
            try
            {
                outStream.WriteLine(message);
                outStream.Flush();
                return true;
            }
            catch { return false; }
        }

        // Ping Twitch IRC server every once in a while to remain connected
        void pingTwitch()
        {
            while (true)
            {
                if (exit) return;

                var success = TcpWrite("PING irc.twitch.tv");

                if (!success)
                {
                    Thread.Sleep(100);
                    continue;
                }
                for (int i = 0; i < 300; i++)
                {
                    if (exit) return;
                    Thread.Sleep(1000);
                    i++;
                }
            }
        }

        // Memory cleaning function
        public void Dispose()
        {
            exit = true;
            pingThread.Join();
            inStream.Dispose();
            outStream.Dispose();
            tcpClient.Dispose();
        }

        // Message logging for easy changing of logging location
        void LogMessage(bool isLogging, string message)
        {
            if (isLogging == true)
            {
                Console.WriteLine(message);
            }
        }
    }

    // Twitch chat message object class
    public class TwitchChatMessage
    {
        public readonly string Raw;
        public readonly string UserType;
        public readonly string Color;
        public readonly string MessageId;
        public readonly string IsModerator;
        public readonly string IsSubscriber;
        public readonly string IsTurbo;
        public readonly string UserId;
        public readonly string IsReturningChatter;
        public readonly string IsFirstMessage;
        public readonly string Sender;
        public readonly string Message;

        public TwitchChatMessage(string raw, string userType, string color, string msgId, string mod, string subscriber, string turbo, string uid, string returningChatter, string firstMessage, string sender, string message)
        {
            this.Raw = raw;
            this.UserType = userType;
            this.Color = color;
            this.MessageId = msgId;
            this.IsModerator = mod;
            this.IsSubscriber = subscriber;
            this.IsTurbo = turbo;
            this.UserId = uid;
            this.IsReturningChatter = returningChatter;
            this.IsFirstMessage = firstMessage;
            this.Sender = sender;
            this.Message = message;
        }
    }
}
