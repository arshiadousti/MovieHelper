using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace MovieHelperBot
{
    public partial class MainForm : Form
    {
        private ITelegramBotClient bot;
        private static string Token;
        private Thread botThread;
        MovieHelperBotEntities context = new MovieHelperBotEntities();
        ReplyKeyboardMarkup mainReplayKeyboard;
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Token = tokenTextBox.Text;
            botThread = new Thread(new ThreadStart(Run));
            botThread.Start();

            mainReplayKeyboard = new ReplyKeyboardMarkup();
            KeyboardButton[] row1=
            {
                new KeyboardButton("افزودن لیست جدید"),("لیست های من")
            };
            mainReplayKeyboard.Keyboard = new KeyboardButton[][]
            {
                row1
            };
            

        }

        private void Run()
        {
            bot = new TelegramBotClient(Token);

            this.Invoke(new Action(() =>
            {
                Status.Text = "Online";
                Status.ForeColor = Color.Green;
            }));


            int offset = 0;
            // اینجا به طور پیشفرض این حرف رو دادم و بعدا چک میکنم که اگر کال بک کوئری با این حرف شروع شده باشه
            // یعنی میخواد که به لیستش اضافه کنه 
            var addListCommand = "A";

            while (true)
            {
                Update[] updates = bot.GetUpdatesAsync(offset).Result;

                foreach (var item in updates)
                {
                    offset = item.Id + 1;

                    if (item.CallbackQuery != null)
                    {

                        StringBuilder stringBuilder = new StringBuilder();
                        string callBackText = item.CallbackQuery.Data.ToString();
                        string[] callback = callBackText.Split('-');
                        var callBackUser = item.CallbackQuery.From.Id;
                        var user = GetUser(item.CallbackQuery.From.Username);

                        //به عنوان مثال اگر اون حرف اول کال بک کوئری با "ال" شروع میشد یعنی مشاهده لیست
                        if (callback[0] == "L")
                        {
                            int listID = Convert.ToInt32(callback[1].ToString());
                            List<UsersList> usersLists = context.UsersList.Where(x => x.ListId == listID)
                                .ToList();

                            if (usersLists.Count > 0)
                            {
                                stringBuilder = new StringBuilder();
                                stringBuilder.AppendLine("فیلم های موجود در لیست شما : ");
                                stringBuilder.AppendLine("********************************");
                                foreach (var movieName in usersLists)
                                {
                                    stringBuilder.AppendLine(movieName.MovieName);
                                }

                                bot.SendTextMessageAsync(callBackUser, stringBuilder.ToString());
                            }
                            else
                            {
                                stringBuilder = new StringBuilder();
                                stringBuilder.AppendLine("لیست مورد نظر خالی است");
                                bot.SendTextMessageAsync(callBackUser, stringBuilder.ToString());
                            }
                        }

                        //و اینجا برای اضافه به لیست
                        else if(callback[0] == "A*")
                        {
                            if (user != null)
                            {


                                UsersList usersList = new UsersList()
                                {
                                    UserId = user.Id,
                                    ListId = int.Parse(callback[2].ToString()),
                                    MovieName = callback[1]
                                };
                                context.UsersList.Add(usersList);
                                context.SaveChanges();
                                stringBuilder.Append("فیلم مورد نظر با موفقیت در لیست علاقه مندی شما اضافه شد.");
                                bot.SendTextMessageAsync(callBackUser, stringBuilder.ToString());
                            }
                            else
                            {
                                user = new Users()
                                {
                                    ChatId = item.CallbackQuery.Message.From.Id,
                                    Username = item.CallbackQuery.From.Username,
                                };
                                
                                context.Users.Add(user);
                                Lists lists = new Lists()
                                {
                                    UserId = user.Id,
                                    Name = "علاقه مندی ها"
                                };
                                
                                context.Lists.Add(lists);

                                UsersList usersList = new UsersList()
                                {
                                    ListId = lists.Id,
                                    UserId = user.Id,
                                    MovieName = callback[1]
                                };
                                context.UsersList.Add(usersList);
                                context.SaveChanges();
                                stringBuilder.Append("فیلم مورد نظر با موفقیت در لیست علاقه مندی شما اضافه شد.");
                                bot.SendTextMessageAsync(callBackUser, stringBuilder.ToString());
                            }
                        }

                        else
                        {
                            UsersList usersList = new UsersList()
                            {
                                ListId = int.Parse(callback[1]),
                                MovieName = callback[0],
                                UserId = user.Id
                            };

                            context.UsersList.Add(usersList);
                            context.SaveChanges();
                            stringBuilder = new StringBuilder();
                            stringBuilder.AppendLine("فیلم مورد نظر با موفقیت به لیست شما اضافه شد.");
                            bot.SendTextMessageAsync(callBackUser, stringBuilder.ToString());
                        }
                    }

                    if (item.Message == null)
                        continue;



                    //----------------------
                    
                    var text = item.Message.Text.ToLower();
                    var chatId = item.Message.Chat.Id;
                    var from = item.Message.From.Username;
                    var messageId = item.Message.MessageId;

                    StringBuilder sb = new StringBuilder();
                    switch (text)
                    {
                        case "/start":
                            {
                                sb.AppendLine("سلام" + from + "عزیز.");
                                sb.AppendLine("میتونی اسم فیلم یا سریال مورد نظرت رو سرچ کنی تا اطلاعات کاملی بهت بدم");
                                sb.AppendLine("همچنین یک لیست میتونی از فیلم هایی که میخوای درست کنی!");
                                sb.AppendLine("************************************");
                                sb.AppendLine("برای جستجو کافیست تنها اسم فیلم یا سریال رو بنویسید : مثال :" + "Inception");
                                sb.AppendLine("حمایت رایگان از ما : " + "");
                                sb.AppendLine("*************************************");
                                sb.AppendLine("یک نکته مهم اینکه در نوشتن اسم فیلم یا سریال مورد نظر خود دقت کنید به عنوان مثال");
                                sb.AppendLine("فیلم old boy با فیلم olbboy فرق داره! پس در نوشتن نقطه فاصله حروف اضافه دقت کنید.");

                                bot.SendTextMessageAsync(chatId, sb.ToString(),ParseMode.Default,false,false,0,mainReplayKeyboard);
                                break;
                            }
                        case "لیست های من":
                            {


                                var user = context.Users.FirstOrDefault(x => x.Username == from);
                                if (user != null)
                                {
 

                                    

                                    List<Lists> myLists = new List<Lists>();
                                    myLists = context.Lists.Where(x => x.UserId == user.Id).ToList();
                                    sb = new StringBuilder();
                                    sb.AppendLine("لیست های موجود شما عبارت اند از : ");
                                    var myListKeyBoardLine = new InlineKeyboardButton[myLists.Count()][];
                                    var myListKeyBoardButton = new InlineKeyboardButton[1];
                                    int j = 0;
                                    for (int i = 0; i < myLists.Count(); i++)
                                    {

                                        myListKeyBoardButton[0] = new InlineKeyboardButton
                                        {
                                            Text = myLists[j].Name,
                                            CallbackData = "L" + "-" + myLists[j].Id.ToString()
                                        };

                                        j++;
                                        myListKeyBoardLine[i] = myListKeyBoardButton;
                                        myListKeyBoardButton = new InlineKeyboardButton[1];
                                    }


                                    InlineKeyboardMarkup inlineMyListKeyboard = new InlineKeyboardMarkup(myListKeyBoardLine);
                                    bot.SendTextMessageAsync(chatId, sb.ToString(), ParseMode.Default, false, false, 0, inlineMyListKeyboard);

                                   

                                }
                                else
                                {
                                    sb = new StringBuilder();
                                    sb.AppendLine("شما هنوز هیچ لیستی ندارید !");

                                }
                                break;
                            }
                        case "افزودن لیست جدید":
                            {
                                addListCommand = "*A";
                                sb = new StringBuilder();
                                sb.AppendLine("لطفا نام لیست خود را وارد کنید.");
                                bot.SendTextMessageAsync(chatId, sb.ToString());
                                break;
                            }
                        default:
                            {
                                /*
                                  اما نکته ای که اینجا هست وجود 
                                 A و *A
                                هستش.
                                من برای اینکه بفهمم کاربر این متنی که ارسال کرده اسم فیلم هست یا اسم لیست 
                                از این روش استفاده کردم. 
                                اگر
                                addListCommand = A 
                                باشه یعنی اسم فیلم هست در غیر اینصورت اسم لیست
                                 */
                                if (addListCommand == "A")
                                {
                                    GetMovie(chatId, messageId, text,from);
                                }
                                else
                                {
                                    var user = context.Users.FirstOrDefault(x => x.Username == from);
                                    if (user == null)
                                    {

                                        user = new Users()
                                        {
                                            ChatId = chatId,
                                            Username = from,
                                        };
                                        context.Users.Add(user);
                                    }

                                    Lists costumList = new Lists()
                                    {
                                        Name = text,
                                        UserId = user.Id
                                    };
                                    context.Lists.Add(costumList);
                                    context.SaveChanges();
                                    sb = new StringBuilder();
                                    sb.AppendLine("لیست شما با موفقیت ذخیره شد");
                                    addListCommand = "A";
                                    bot.SendTextMessageAsync(chatId, sb.ToString());
                                }
                                break;
                            }
                    }

                    this.Invoke(new Action(() =>
                    {
                        dgReport.Rows.Add(chatId, from, text.ToString(), item.Message.Date.ToString());
                    }));
                }



            }
        }


        private void GetMovie(long chatid, int messageid, string text , string username)
        {
            StringBuilder sbMovie = new StringBuilder();
            StringBuilder sb = new StringBuilder();
            var webClient = new WebClient();
            Movie movie = new Movie();
            string json = "";

            //--------------------------------------
            sbMovie.Append("http://www.omdbapi.com/?t=");
            sbMovie.Append(text);
            sbMovie.Append("&apikey=2c971d80&");
            //-----------------------------------------

            json = webClient.DownloadString(sbMovie.ToString());

            JObject googleResult = JObject.Parse(json);

            movie = JsonConvert.DeserializeObject<Movie>(googleResult.ToString());

            sb = new StringBuilder();

            if (movie.Title != null && movie.Poster != "N/A")
            {
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(
                        new Uri(movie.Poster), @"F:\MovieBot\Images\image2.jpg");
                }


                sb.AppendLine("نام فیلم : " + movie.Title);
                sb.AppendLine("سال تولید : " + movie.Year.Replace('â', ' ').Replace('€', '-'));
                sb.AppendLine("درجه سنی : " + movie.Rated);
                sb.AppendLine("مدت زمان : " + movie.RunTime);
                sb.AppendLine("ژانر : " + movie.Genre);
                sb.AppendLine("کارگردان : " + movie.Director);
                sb.AppendLine("بازیگران : " + movie.Actors);
                sb.AppendLine("خلاصه داستان : " + movie.Plot);
                sb.AppendLine("زبان : " + movie.Language);
                sb.AppendLine("محصول : " + movie.Country);
                sb.AppendLine("جوایز : " + movie.Awards);
                sb.AppendLine("IMDB : " + movie.imdbRating);
                sb.AppendLine("MetaScore : " + movie.Metascore);

                FileStream stream = System.IO.File.Open(@"F:\MovieBot\Images\image2.jpg", FileMode.Open);
                InputOnlineFile image = new InputOnlineFile(stream, "movie.jpg");

                if (!IsUserExists(username))
                {
                    var inlineKeyboard = new InlineKeyboardMarkup(new[]
                 {
                    
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("افزودن به لیست علاقه مندی ها","A*"+"-"+movie.Title+"-"+1)
                    },
                    
                }) ;

                    bot.SendPhotoAsync(chatid, image, sb.ToString(), ParseMode.Default, false, messageid, inlineKeyboard);
                }
                else
                {
                    var user = GetUser(username);
                    List<Lists> getUserLists = context.Lists.Where(x => x.UserId == user.Id).ToList();
                    var addToListLine = new InlineKeyboardButton[getUserLists.Count()][];
                    var addToListButton = new InlineKeyboardButton[1];
                    for(int i=0; i<getUserLists.Count(); i++)
                    {
                        addToListButton[0] = new InlineKeyboardButton
                        {
                            Text = getUserLists[i].Name,
                            CallbackData = movie.Title + "-" + getUserLists[i].Id
                        };

                        addToListLine[i] = addToListButton;
                        addToListButton = new InlineKeyboardButton[1];
                    }

                    InlineKeyboardMarkup inlineAddToListKeyboard = new InlineKeyboardMarkup(addToListLine);
                    bot.SendPhotoAsync(chatid, image, sb.ToString(), ParseMode.Default, false, messageid, inlineAddToListKeyboard);


                }

            }
            else
            {
                sb.AppendLine("متاسفانه فیلم مورد یافت نشد");
                sb.AppendLine("میتونید نگارش فیلم رو چک کنید و دوباره امتحان کنید");
                bot.SendTextMessageAsync(chatid, sb.ToString(), Telegram.Bot.Types.Enums.ParseMode.Default, false, false, messageid);
            }
        }

        private MovieHelperBot.Users GetUser(string username)
        {
            var user = context.Users.FirstOrDefault(x => x.Username == username);
            return user;
        }

        private bool IsUserExists(string username)
        {
            var user = GetUser(username);
            if (user != null)
            {
                return true;
            }

            return false;
        }

        
    }
}
