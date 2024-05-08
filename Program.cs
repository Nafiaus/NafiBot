using Nafi.OpenAI;
using Nafi.Twitch;
using NAudio.Wave;
using System.Diagnostics.Contracts;
using System.Diagnostics.Tracing;
using WindowsInput;
using WindowsInput.Native;
class Program
{
    static string oauth = "OAUTH";
    static string channel = "nafiaus";
    static string nick = "Nafibot";
    static string gptResponsePointerFile = @".\voices\responsepointer.txt";
    static string debugFile = @".\data\debug_log.txt";
    static string dataFile = @".\data\flashbang.dat";

    static bool isFirstFlashbang = true;
    static bool isFirstSpawn = false;

    static int gptResponsePointer = 0;
    static int flashbangCounter = 0;

    static bool controlMe = true;

    static TwitchBot bot = new TwitchBot(oauth, channel, nick);
    static OpenAiService ai = new OpenAiService();
    static InputSimulator inSim = new InputSimulator();

    static void Main(string[] args)
    {
        log($"{nick} has started monitoring chat for {channel}.");
        if (!File.Exists(gptResponsePointerFile))
        {
            flashbangCounter = loadFlashbangCounter(dataFile);
            log($"Loading \"flashbangCounter\" from \"{dataFile}\"");
            using (FileStream fs = new FileStream(gptResponsePointerFile, FileMode.CreateNew))
            {
                log($"Creating File \"{gptResponsePointerFile}\".");
                using (StreamWriter w = new StreamWriter(fs))
                {
                    log($"Writing \"{gptResponsePointer}\" to \"{gptResponsePointerFile}\"");
                    w.Write(gptResponsePointer);
                }
            }
        }
        else
        {
            using (FileStream fs = new FileStream(gptResponsePointerFile, FileMode.Open, FileAccess.Read))
            {
                log($"Reading \"{gptResponsePointerFile}\"");
                using (StreamReader r = new StreamReader(fs))
                {
                    log($"Assigning \"gptResponsePointer\" from \"{gptResponsePointerFile}\"");
                    gptResponsePointer = r.Read();
                }
            }
        }
        
        while (true)
        {
            TwitchChatMessage? chatMessage = null;
            chatMessage = bot.Read();
            if (chatMessage != null)
            {
                log("Recieved Twitch Chat Message.");
                GetFirstTime(chatMessage);
                CheckCommand(chatMessage);
            }
        }
        static void CheckCommand(TwitchChatMessage chatMessage)
        {
            log($"Checking for Commands in \"{chatMessage.Message}\" from \"{chatMessage.Sender}\".");
            string msg = chatMessage.Message;
            string[] words = msg.Split(' ');
            switch (words[0])
            {
                case "!wzid":
                    WarzoneMobileID(chatMessage);
                    break;
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
                case "!mystatus": // requires user input "sub" "mod" "turbo"
                    GetStatus(chatMessage);
                    break;
                case "!getmycolor":
                    GetChatterColor(chatMessage);
                    break;
                case "!nafibot": // requires user input "why is the end of the shoelace called an aglet?"
                    Task askGpt = AskGPT(chatMessage);
                    break;
                case "!flashbang":
                    flashbangCounter += 1;
                    saveFlashbangCounter(dataFile, flashbangCounter);
                    if (isFirstFlashbang)
                    {
                        bot.Write("Every flashbang you do murders little puppies... Remember that... !flashbangcount");
                    }
                    isFirstFlashbang = false;
                    break;
                case "!flashbangcount":
                    if (flashbangCounter == 0)
                    {
                        bot.Write("Luckily no one has used flashbang yet so no little cute puppies have been RUTHLESSLY MURDERED BY FIRING SQAUD :) <3");
                    }
                    bot.Write($"Thanks to the flashbangs, {flashbangCounter} little cute puppies have been ruthlessly murdered... I hope you can live with yourself!");
                    break;
                case "!commands":
                    GiveCommands(chatMessage);
                    break;
                case "!controlme":
                    GiveControls(chatMessage);
                    break;
                case "!activateControlMe":
                    if (chatMessage.Sender.ToLower() == "nafiaus" || chatMessage.IsModerator == "true")
                    {
                        controlMe = true;
                    }
                    break;
                case "!deactivateControlMe":
                    if (chatMessage.Sender.ToLower() == "nafiaus" || chatMessage.IsModerator == "true")
                    {
                        controlMe = false;
                    }
                    break;
                default:
                    break;
            }
            if (controlMe)
            {
                switch (words[0].ToLower())
                {
                    case "!up":
                        inSim.Keyboard.KeyDown(VirtualKeyCode.VK_W);
                        Thread.Sleep(100);
                        inSim.Keyboard.KeyUp(VirtualKeyCode.VK_W);
                        break;
                    case "!down":
                        inSim.Keyboard.KeyDown(VirtualKeyCode.VK_S);
                        Thread.Sleep(100);
                        inSim.Keyboard.KeyUp(VirtualKeyCode.VK_S);
                        break;
                    case "!right":
                        inSim.Keyboard.KeyDown(VirtualKeyCode.VK_D);
                        Thread.Sleep(100);
                        inSim.Keyboard.KeyUp(VirtualKeyCode.VK_D);
                        break;
                    case "!left":
                        inSim.Keyboard.KeyDown(VirtualKeyCode.VK_A);
                        Thread.Sleep(100);
                        inSim.Keyboard.KeyUp(VirtualKeyCode.VK_A);
                        break;
                    case "!shift":
                        inSim.Keyboard.KeyDown(VirtualKeyCode.SHIFT);
                        Thread.Sleep(3000);
                        inSim.Keyboard.KeyUp(VirtualKeyCode.SHIFT);
                        break;
                    case "!control":
                        inSim.Keyboard.KeyDown(VirtualKeyCode.LCONTROL);
                        Thread.Sleep(3000);
                        inSim.Keyboard.KeyUp(VirtualKeyCode.LCONTROL);
                        break;
                    case "!spacebar":
                        inSim.Keyboard.KeyDown(VirtualKeyCode.SPACE);
                        Thread.Sleep(3000);
                        inSim.Keyboard.KeyUp(VirtualKeyCode.SPACE);
                        break;
                    case "!1":
                        inSim.Keyboard.KeyDown(VirtualKeyCode.VK_1);
                        Thread.Sleep(3000);
                        inSim.Keyboard.KeyUp(VirtualKeyCode.VK_1);
                        break;
                    case "!2":
                        inSim.Keyboard.KeyDown(VirtualKeyCode.VK_2);
                        Thread.Sleep(3000);
                        inSim.Keyboard.KeyUp(VirtualKeyCode.VK_2);
                        break;
                    case "!3":
                        inSim.Keyboard.KeyDown(VirtualKeyCode.VK_3);
                        Thread.Sleep(3000);
                        inSim.Keyboard.KeyUp(VirtualKeyCode.VK_3);
                        break;
                    case "!4":
                        inSim.Keyboard.KeyDown(VirtualKeyCode.VK_4);
                        Thread.Sleep(3000);
                        inSim.Keyboard.KeyUp(VirtualKeyCode.VK_4);
                        break;
                    case "!5":
                        inSim.Keyboard.KeyDown(VirtualKeyCode.VK_5);
                        Thread.Sleep(3000);
                        inSim.Keyboard.KeyUp(VirtualKeyCode.VK_5);
                        break;
                    case "!6":
                        inSim.Keyboard.KeyDown(VirtualKeyCode.VK_6);
                        Thread.Sleep(3000);
                        inSim.Keyboard.KeyUp(VirtualKeyCode.VK_6);
                        break;
                    case "!7":
                        inSim.Keyboard.KeyDown(VirtualKeyCode.VK_7);
                        Thread.Sleep(3000);
                        inSim.Keyboard.KeyUp(VirtualKeyCode.VK_7);
                        break;
                    case "!8":
                        inSim.Keyboard.KeyDown(VirtualKeyCode.VK_8);
                        Thread.Sleep(3000);
                        inSim.Keyboard.KeyUp(VirtualKeyCode.VK_8);
                        break;
                    case "!9":
                        inSim.Keyboard.KeyDown(VirtualKeyCode.VK_9);
                        Thread.Sleep(3000);
                        inSim.Keyboard.KeyUp(VirtualKeyCode.VK_9);
                        break;
                    case "!0":
                        inSim.Keyboard.KeyDown(VirtualKeyCode.VK_0);
                        Thread.Sleep(3000);
                        inSim.Keyboard.KeyUp(VirtualKeyCode.VK_0);
                        break;
                    case "!a":
                        inSim.Keyboard.KeyDown(VirtualKeyCode.VK_A);
                        Thread.Sleep(3000);
                        inSim.Keyboard.KeyUp(VirtualKeyCode.VK_A);
                        break;
                    case "!emote":
                        inSim.Keyboard.KeyDown(VirtualKeyCode.VK_B);
                        Thread.Sleep(100);
                        inSim.Keyboard.KeyUp(VirtualKeyCode.VK_B);
                        break;
                    case "!c":
                        inSim.Keyboard.KeyDown(VirtualKeyCode.VK_C);
                        Thread.Sleep(3000);
                        inSim.Keyboard.KeyUp(VirtualKeyCode.VK_C);
                        break;
                    case "!d":
                        inSim.Keyboard.KeyDown(VirtualKeyCode.VK_D);
                        Thread.Sleep(3000);
                        inSim.Keyboard.KeyUp(VirtualKeyCode.VK_D);
                        break;
                    case "!e":
                        inSim.Keyboard.KeyDown(VirtualKeyCode.VK_E);
                        Thread.Sleep(3000);
                        inSim.Keyboard.KeyUp(VirtualKeyCode.VK_E);
                        break;
                    case "!f":
                        inSim.Keyboard.KeyDown(VirtualKeyCode.VK_F);
                        Thread.Sleep(3000);
                        inSim.Keyboard.KeyUp(VirtualKeyCode.VK_F);
                        break;
                    case "!g":
                        inSim.Keyboard.KeyDown(VirtualKeyCode.VK_G);
                        Thread.Sleep(3000);
                        inSim.Keyboard.KeyUp(VirtualKeyCode.VK_G);
                        break;
                    case "!h":
                        inSim.Keyboard.KeyDown(VirtualKeyCode.VK_H);
                        Thread.Sleep(3000);
                        inSim.Keyboard.KeyUp(VirtualKeyCode.VK_H);
                        break;
                    case "!i":
                        inSim.Keyboard.KeyDown(VirtualKeyCode.VK_I);
                        Thread.Sleep(3000);
                        inSim.Keyboard.KeyUp(VirtualKeyCode.VK_I);
                        break;
                    case "!j":
                        inSim.Keyboard.KeyDown(VirtualKeyCode.VK_J);
                        Thread.Sleep(3000);
                        inSim.Keyboard.KeyUp(VirtualKeyCode.VK_J);
                        break;
                    case "!k":
                        inSim.Keyboard.KeyDown(VirtualKeyCode.VK_K);
                        Thread.Sleep(3000);
                        inSim.Keyboard.KeyUp(VirtualKeyCode.VK_K);
                        break;
                    case "!l":
                        inSim.Keyboard.KeyDown(VirtualKeyCode.VK_L);
                        Thread.Sleep(3000);
                        inSim.Keyboard.KeyUp(VirtualKeyCode.VK_L);
                        break;
                    case "!m":
                        inSim.Keyboard.KeyDown(VirtualKeyCode.VK_M);
                        Thread.Sleep(3000);
                        inSim.Keyboard.KeyUp(VirtualKeyCode.VK_M);
                        break;
                    case "!n":
                        inSim.Keyboard.KeyDown(VirtualKeyCode.VK_N);
                        Thread.Sleep(3000);
                        inSim.Keyboard.KeyUp(VirtualKeyCode.VK_N);
                        break;
                    case "!o":
                        inSim.Keyboard.KeyDown(VirtualKeyCode.VK_O);
                        Thread.Sleep(3000);
                        inSim.Keyboard.KeyUp(VirtualKeyCode.VK_O);
                        break;
                    case "!p":
                        inSim.Keyboard.KeyDown(VirtualKeyCode.VK_P);
                        Thread.Sleep(3000);
                        inSim.Keyboard.KeyUp(VirtualKeyCode.VK_P);
                        break;
                    case "!q":
                        inSim.Keyboard.KeyDown(VirtualKeyCode.VK_Q);
                        Thread.Sleep(3000);
                        inSim.Keyboard.KeyUp(VirtualKeyCode.VK_Q);
                        break;
                    case "!r":
                        inSim.Keyboard.KeyDown(VirtualKeyCode.VK_R);
                        Thread.Sleep(3000);
                        inSim.Keyboard.KeyUp(VirtualKeyCode.VK_R);
                        break;
                    case "!s":
                        inSim.Keyboard.KeyDown(VirtualKeyCode.VK_S);
                        Thread.Sleep(3000);
                        inSim.Keyboard.KeyUp(VirtualKeyCode.VK_S);
                        break;
                    case "!t":
                        inSim.Keyboard.KeyDown(VirtualKeyCode.VK_T);
                        Thread.Sleep(3000);
                        inSim.Keyboard.KeyUp(VirtualKeyCode.VK_T);
                        break;
                    case "!u":
                        inSim.Keyboard.KeyDown(VirtualKeyCode.VK_U);
                        Thread.Sleep(3000);
                        inSim.Keyboard.KeyUp(VirtualKeyCode.VK_U);
                        break;
                    case "!v":
                        inSim.Keyboard.KeyDown(VirtualKeyCode.VK_V);
                        Thread.Sleep(3000);
                        inSim.Keyboard.KeyUp(VirtualKeyCode.VK_V);
                        break;
                    case "!w":
                        inSim.Keyboard.KeyDown(VirtualKeyCode.VK_W);
                        Thread.Sleep(1000);
                        inSim.Keyboard.KeyUp(VirtualKeyCode.VK_W);
                        break;
                    case "!x":
                        inSim.Keyboard.KeyDown(VirtualKeyCode.VK_X);
                        Thread.Sleep(3000);
                        inSim.Keyboard.KeyUp(VirtualKeyCode.VK_X);
                        break;
                    case "!y":
                        inSim.Keyboard.KeyDown(VirtualKeyCode.VK_Y);
                        Thread.Sleep(3000);
                        inSim.Keyboard.KeyUp(VirtualKeyCode.VK_Y);
                        break;
                    case "!z":
                        inSim.Keyboard.KeyDown(VirtualKeyCode.VK_Z);
                        Thread.Sleep(3000);
                        inSim.Keyboard.KeyUp(VirtualKeyCode.VK_Z);
                        break;
                    case "!leftclick":
                        inSim.Mouse.LeftButtonDown();
                        Thread.Sleep(3000);
                        inSim.Mouse.LeftButtonUp();
                        break;
                    case "!rightclick":
                        inSim.Mouse.RightButtonDown();
                        Thread.Sleep(3000);
                        inSim.Mouse.RightButtonUp();
                        break;
                    default:
                        break;
                }
            }
        }

        static int loadFlashbangCounter(string filePath)
        {
            if (!File.Exists(filePath)) { return -1; }

            int count = 0;
            using (var f = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var r = new BinaryReader(f, System.Text.Encoding.UTF8, false))
                {
                    count = r.ReadInt32();
                }
            }
            return count;
        }

        static void saveFlashbangCounter(string filePath, int amt)
        {

            if (File.Exists(filePath)) { File.Delete(filePath); }

            using (var f = File.Open(filePath, FileMode.Create))
            {
                using (var w = new BinaryWriter(f, System.Text.Encoding.UTF8, false))
                {
                    w.Write(amt);
                }
            }
        }

        // Shares Warzone Mobile ID
        static void WarzoneMobileID(TwitchChatMessage chatMessage)
        {
            bot.Write("NAFIAUS#6328340");
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

        // Give Control me Commands
        static void GiveControls(TwitchChatMessage chatMessage)
        {
            string line = chatMessage.Sender + " you can move me up with \"!w\", down with \"!s\", left with \"!a\", or right with \"!d\". Shoot my gun with \"!leftclick\", aim with \"!rightclick\". Switch weapons[Fortnite] with \"!1-5\" for melee \"!f\"";
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

        static void GetStatus(TwitchChatMessage chatMessage)
        {
            string msg = chatMessage.Message;
            string[] words = msg.Split(' ');
            if (words.Length == 1)
            {
                bot.Write($"Sorry {chatMessage.Sender} you must include the status you want to check, such as \"sub\", \"mod\", \"turbo\".");
            }
            else if (words[1] == "turbo" || words[1] == "mod" || words[1] == "sub")
            {
                switch (words[1])
                {
                    case "turbo":
                        if (chatMessage.IsTurbo == "False")
                        {
                            bot.Write($"{chatMessage.Sender} you are not a Twitch Turbo User!");
                        }
                        else if (chatMessage.IsTurbo == "True")
                        {
                            bot.Write($"{chatMessage.Sender} you are a Twitch Turbo User!");
                        }
                        break;
                    case "mod":
                        if (chatMessage.IsModerator == "False")
                        {
                            bot.Write($"{chatMessage.Sender} you are not a Moderator! ROFL");
                        }
                        else if (chatMessage.IsModerator == "True")
                        {
                            bot.Write($"{chatMessage.Sender} you are a moderator! <3");
                        }
                        break;
                    case "sub":
                        if (chatMessage.IsSubscriber == "False")
                        {
                            bot.Write($"{chatMessage.Sender} you are not a subscriber! Sadge");
                        }
                        else if (chatMessage.IsSubscriber == "True")
                        {
                            bot.Write($"{chatMessage.Sender} you are an amazing subscriber! <3");
                        }
                        break;
                }
            }
            else
            {
                bot.Write($"Sorry {chatMessage.Sender} that format was invalid. Please use this command with \"sub\", \"mod\", or \"turbo\". ex. !mystatus turbo");
            }
        }

        static async Task AskGPT(TwitchChatMessage chatMessage)
        {
            string gptVoiceExtension = "wav";
            string assistantStyle = "default";
            string msg = chatMessage.Message;

            string[] words = msg.Split(' ');

            List<string> wordList = words.ToList();
            if (words[1] == "thinker" || words[1] == "sarcastic" || words[1] == "rude" || words[1] == "stupid" || words[1] == "default")
            {
                assistantStyle = words[1];
                wordList.RemoveAt(1);
            }
            wordList.RemoveAt(0);
            string prompt = string.Join(" ", wordList);

            string voiceFile = ".\\voices\\" + gptResponsePointer + "." + gptVoiceExtension;
            string chatFile = ".\\voices\\" + gptResponsePointer + ".txt";

            string response = await ai.getAiChatResponse(assistantStyle, prompt);
            bot.Write($"{response}");

            if (!File.Exists(chatFile))
            {
                using (FileStream fs = new FileStream(chatFile, FileMode.CreateNew))
                {
                    using (StreamWriter w = new StreamWriter(fs))
                    {
                        w.WriteLine(chatMessage.Sender);
                        w.WriteLine(chatMessage.Message);
                    }
                }
            }

            gptResponsePointer += 1;
            Console.WriteLine(gptResponsePointer);
            if (File.Exists(gptResponsePointerFile))
            {
                File.Delete(gptResponsePointerFile);
            }
            using (FileStream fs = new FileStream(gptResponsePointerFile, FileMode.CreateNew))
            {
                using (BinaryWriter w = new BinaryWriter(fs, System.Text.Encoding.UTF8, false))
                {
                    w.Write(gptResponsePointer);
                }
            }

            using (FileStream fs = new FileStream(gptResponsePointerFile, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader r = new BinaryReader(fs, System.Text.Encoding.UTF8, false))
                {
                    gptResponsePointer = r.ReadInt32();
                }
            }
        }
    }

    static void log(string text)
    {
        if (!File.Exists(debugFile))
        {
            using (FileStream fs = new FileStream(debugFile, FileMode.CreateNew))
            {
                using (StreamWriter w = new StreamWriter(fs))
                {
                    w.WriteLine("Created Debug File");
                }
            }
        } else if (File.Exists(debugFile))
        {
            using (FileStream fs = new FileStream(debugFile, FileMode.Append))
            {
                using (StreamWriter w = new StreamWriter(fs))
                {
                    w.WriteLine(text);
                    Console.WriteLine(text);
                }
            }
        }
    }
}
