using MvvmHelpers;
using PlanMy.Helpers;
using PlanMy.Library;
using PlanMy.Models;
using Plugin.Geolocator;
using SendBird;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PlanMy.ViewModels
{
    public class MainChatViewModel : BaseViewModel
    {

        public ObservableRangeCollection<Message> Messages { get; }

        string outgoingText = string.Empty;

        public string OutGoingText
        {
            get { return outgoingText; }
            set { SetProperty(ref outgoingText, value); }
        }

        public ICommand SendCommand { get; set; }


        public ICommand LocationCommand { get; set; }

        public MainChatViewModel(VendorItem vendor)
        {
            // Initialize with default values
            //twilioMessenger = DependencyService.Get<ITwilioMessenger>();
            Messages = new ObservableRangeCollection<Message>();
            LoadChat(vendor);


            SendCommand = new Command(async() =>
            {
                Connect con = new Connect();
                var usr = await con.GetData("User");
                Users cookie = new Users();

                if (!string.IsNullOrEmpty(usr))
                {
                    cookie = Newtonsoft.Json.JsonConvert.DeserializeObject<Users>(usr);
                    string resp = await con.DownloadData(Statics.apiLink + "ChatChannels", "UserId=" + cookie.Id + "&VendorId=" + vendor.UserId);
                    var result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ChatChannel>>(resp);
                    if (result.Count > 0)
                    {
                        var row = result.OrderByDescending(x => x.Id).FirstOrDefault();
                        GroupChannel.GetChannel(row.ChannelUrl, (GroupChannel groupChannel, SendBirdException e) =>
                        {
                            if (e != null)
                            {
                                // Error.
                                return;
                            }
                            groupChannel.InviteWithUserId(vendor.UserId, (SendBirdException sbe) => { });
                            groupChannel.StartTyping();
                            string msg = OutGoingText;
                            var message = new Message
                            {
                                Text = OutGoingText,
                                IsIncoming = false,
                                MessageDateTime = DateTime.Now,
                                SenderImg = Statics.MediaLink + cookie.Image
                            };


                            Messages.Add(message);

                            //twilioMessenger?.SendMessage(message.Text);
                            groupChannel.SendUserMessage(OutGoingText, (UserMessage userMessage, SendBirdException se) =>
                            {
                                if (se != null)
                                {
                                    // Error.
                                    return;
                                }
                            });
                            OutGoingText = string.Empty;
                            groupChannel.EndTyping();

                        });
                    }
                }

            });

            LocationCommand = new Command(async () =>
            {
                try
                {
                    var local = await CrossGeolocator.Current.GetPositionAsync(new TimeSpan(10000));
                    var map = $"https://maps.googleapis.com/maps/api/staticmap?center={local.Latitude.ToString(CultureInfo.InvariantCulture)},{local.Longitude.ToString(CultureInfo.InvariantCulture)}&zoom=17&size=400x400&maptype=street&markers=color:red%7Clabel:%7C{local.Latitude.ToString(CultureInfo.InvariantCulture)},{local.Longitude.ToString(CultureInfo.InvariantCulture)}&key=";

                    var message = new Message
                    {
                        Text = "I am here",
                        AttachementUrl = map,
                        IsIncoming = false,
                        MessageDateTime = DateTime.Now
                    };

                    Messages.Add(message);
                    //twilioMessenger?.SendMessage("attach:" + message.AttachementUrl);

                }
                catch (Exception ex)
                {

                }
            });


        }
        async void LoadChat(VendorItem vendor)
        {
            
            Connect con = new Connect();
            var usr = await con.GetData("User");
            Users cookie = new Users();
            
            if (!string.IsNullOrEmpty(usr))
            {

                cookie = Newtonsoft.Json.JsonConvert.DeserializeObject<Users>(usr);
                List<string> userIds = new List<string> { cookie.Id, vendor.UserId };
                
                SendBirdClient.Connect(cookie.Id, async(User user, SendBirdException ev) =>
                {
                    if (ev != null)
                    {
                        // Error
                        return;
                    }
                    
                    SendBirdClient.ChannelHandler ch = new SendBirdClient.ChannelHandler();
                    //UserListQuery list = SendBirdClient.CreateUserListQuery(userIds);
                    
                    GroupChannel.CreateChannelWithUserIds(userIds, true, async (GroupChannel groupChannel, SendBirdException e) => {
                        if (e != null)
                        {
                            // Error.
                            return;
                        }
                        groupChannel.InviteWithUserId(vendor.UserId, (SendBirdException sbe) => { });
                        ChatChannel obj = new ChatChannel { ChannelUrl = groupChannel.Url, UserId = cookie.Id, VendorId = vendor.UserId };
                        string json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                        await con.PostToServer(Statics.apiLink + "ChatChannels", json);
                        
                        PreviousMessageListQuery mPrevMessageListQuery = groupChannel.CreatePreviousMessageListQuery();
                        mPrevMessageListQuery.Load(30, true, (List<BaseMessage> messages, SendBirdException evx) => {
                            if (evx != null)
                            {
                                // Error.
                                return;
                            }
                            foreach (var message in messages)
                            {
                                UserMessage userMsg = (UserMessage)message;
                                var sender = userMsg.Sender;
                                if (sender.UserId != cookie.Id)
                                    Messages.Add(new Message { Text = userMsg.Message, IsIncoming = true, MessageDateTime = ConvertToDate(userMsg.CreatedAt), SenderImg = Statics.MediaLink + vendor.Thumb });
                                else
                                    Messages.Add(new Message { Text = userMsg.Message, IsIncoming = true, MessageDateTime = ConvertToDate(userMsg.CreatedAt), SenderImg = Statics.MediaLink + cookie.Image });
                            }

                        });
                    });

                    ch.OnMessageReceived = (BaseChannel baseChannel, BaseMessage baseMessage) => {
                        UserMessage userMsg = (UserMessage)baseMessage;
                        Messages.Add(new Message { Text = userMsg.Message, IsIncoming = true, MessageDateTime = ConvertToDate(userMsg.CreatedAt), SenderImg = Statics.MediaLink + vendor.Thumb });
                    };
                    ch.OnChannelChanged = (BaseChannel BaseChannel) => {
                        GroupChannel groupChannel = (GroupChannel)BaseChannel;
                    };
                    SendBirdClient.AddChannelHandler(Statics.ChannelHandler, ch);
                    SendBirdClient.UpdateCurrentUserInfo(cookie.FirstName + " " + cookie.LastName, user.ProfileUrl, (SendBirdException e1) =>
                    {
                        if (e1 != null)
                        {
                            // Error
                            return;
                        }
                    });
                    //if (SendBirdClient.GetPendingPushToken() == null) return;

                    // For Android
                    SendBirdClient.RegisterFCMPushTokenForCurrentUser(SendBirdClient.GetPendingPushToken(), (SendBirdClient.PushTokenRegistrationStatus status, SendBirdException e1) =>
                    {
                        if (e1 != null)
                        {
                            // Error.
                            return;
                        }

                        if (status == SendBirdClient.PushTokenRegistrationStatus.PENDING)
                        {
                            // Try registration after connection is established.
                        }
                    });

                    // For iOS
                    SendBirdClient.RegisterAPNSPushTokenForCurrentUser(SendBirdClient.GetPendingPushToken(), (SendBirdClient.PushTokenRegistrationStatus status, SendBirdException e1) =>
                    {
                        if (e1 != null)
                        {
                            // Error.
                            return;
                        }

                        if (status == SendBirdClient.PushTokenRegistrationStatus.PENDING)
                        {
                            // Try registration after connection is established.
                        }
                    });
                    
                });
                
                
            }
        }
        DateTime ConvertToDate(long unixDate)
        {
            DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime date = start.AddMilliseconds(unixDate).ToLocalTime();
            return date;
        }
        //private async void GetNewChats(Connect con, VendorItem vendor, Users cookie)
        //{
        //    string postData = "action=getnew&my_id=" + cookie.Id + "&partner_id=" + vendor.UserId;
        //    string jdata = await con.DownloadData("https://www.planmy.me/maizonpub-api/chat.php", postData);
        //    var datas = Newtonsoft.Json.JsonConvert.DeserializeObject<List<WeddexChat>>(jdata);

        //    foreach (var item in datas)
        //    {
        //        if (item.type == "Incoming")
        //            Messages.Add(new Message { Text = item.message, IsIncoming = true, MessageDateTime = DateTime.Parse(item.dateInsert), SenderImg = vendor.Thumb });
        //        else
        //            Messages.Add(new Message { Text = item.message, IsIncoming = false, MessageDateTime = DateTime.Parse(item.dateInsert), SenderImg = Statics.MediaLink + cookie.Image });
        //    }
        //}

    }
}
