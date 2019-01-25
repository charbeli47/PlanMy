using MvvmHelpers;
using PlanMy.Helpers;
using PlanMy.Library;
using PlanMy.Models;
using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
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
                UserCookie cookie = new UserCookie();
                if(!string.IsNullOrEmpty(usr))
                    cookie = Newtonsoft.Json.JsonConvert.DeserializeObject<UserCookie>(usr);
                string msg = OutGoingText;
                var message = new Message
                {
                    Text = OutGoingText,
                    IsIncoming = false,
                    MessageDateTime = DateTime.Now,
                    SenderImg = cookie.configUsr.event_img
                };


                Messages.Add(message);

                //twilioMessenger?.SendMessage(message.Text);

                OutGoingText = string.Empty;
                
                string json = await con.DownloadData("https://www.planmy.me/maizonpub-api/chat.php", "action=insert&sender_id=" + cookie.user.id + "&receiver_id=" + vendor.UserId + "&message=" + msg);

                await con.DownloadData("https://planmy.me/maizonpub-api/add_device.php", "action=sendpush&body=" + msg + "&title=New message from User on PlanMy app&userid=" + vendor.UserId);
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
            UserCookie cookie = new UserCookie();
            if (!string.IsNullOrEmpty(usr))
            {
                cookie = Newtonsoft.Json.JsonConvert.DeserializeObject<UserCookie>(usr);
                string json = await con.DownloadData("https://www.planmy.me/maizonpub-api/chat.php", "action=get&my_id=" + cookie.user.id + "&partner_id=" + vendor.UserId);
                var items = Newtonsoft.Json.JsonConvert.DeserializeObject<List<WeddexChat>>(json);

                foreach (var item in items)
                {
                    if (item.type == "Incoming")
                        Messages.Add(new Message { Text = item.message, IsIncoming = true, MessageDateTime = DateTime.Parse(item.dateInsert), SenderImg = vendor.Thumb });
                    else
                        Messages.Add(new Message { Text = item.message, IsIncoming = false, MessageDateTime = DateTime.Parse(item.dateInsert), SenderImg = cookie.configUsr.event_img });
                }
                Xamarin.Forms.Device.StartTimer(new TimeSpan(0, 0, 1), () =>
                {
                    GetNewChats(con, vendor, cookie);

                    return true;
                });
            }

        }

        private async void GetNewChats(Connect con, VendorItem vendor, UserCookie cookie)
        {
            string postData = "action=getnew&my_id=" + cookie.user.id + "&partner_id=" + vendor.UserId;
            string jdata = await con.DownloadData("https://www.planmy.me/maizonpub-api/chat.php", postData);
            var datas = Newtonsoft.Json.JsonConvert.DeserializeObject<List<WeddexChat>>(jdata);

            foreach (var item in datas)
            {
                if (item.type == "Incoming")
                    Messages.Add(new Message { Text = item.message, IsIncoming = true, MessageDateTime = DateTime.Parse(item.dateInsert), SenderImg = vendor.Thumb });
                else
                    Messages.Add(new Message { Text = item.message, IsIncoming = false, MessageDateTime = DateTime.Parse(item.dateInsert), SenderImg = cookie.configUsr.event_img });
            }
        }

    }
}
