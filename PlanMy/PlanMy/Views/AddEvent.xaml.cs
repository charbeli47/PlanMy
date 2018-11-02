using PlanMy.Library;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlanMy.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddEvent : ContentPage, INotifyPropertyChanged
    {
        protected Stream stream;
        public event EventHandler<EventArgs> OperationCompleted;
        public AddEvent(ConfigUser user)
		{
			InitializeComponent();
            if(user!=null)
            {
                eventname.Text = user.event_name;
                eventlocation.Text = user.event_location;
                pickImg.Source = user.event_img;
                pickImg.IsVisible = false;
                eventDate.Date = DateTime.Parse(user.event_date);
            }
            //EventTypePicker.SelectedIndex = 0;
            IsLoading = true;
            BindingContext = this;
        }

        private void closeBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
        private bool isLoading;
        public bool IsLoading
        {
            get
            {
                return this.isLoading;
            }

            set
            {
                this.isLoading = value;
                RaisePropertyChanged("IsLoading");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
        private async void createEventBtn_Clicked(object sender, EventArgs e)
        {
            Connect con = new Connect();
            var usr = await con.GetData("User");
            UserCookie cookie = Newtonsoft.Json.JsonConvert.DeserializeObject<UserCookie>(usr);
            string data = "action=updateevent&userid=" + cookie.user.id+ "&eventname=" + eventname.Text+ "&eventdate=" + eventDate.Date.ToString("MM/dd/yyyy")+ "&eventlocation="+eventlocation.Text;
            string filename = Guid.NewGuid().ToString()+".jpg";
            //var stream = GetStream(content);
            bool uploaded = await Upload(stream, filename, data);
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            var pickPictureButton = (Button)sender;
            stream = await DependencyService.Get<IPicturePicker>().GetImageStreamAsync();
            ImgLabel.Text = "Event Image: 1 Image added";
            /*if (stream != null)
            {
                //pickImg.Source = ImageSource.FromStream(() => stream);    
                
                //pickImg.IsVisible = true;

                /*TapGestureRecognizer recognizer = new TapGestureRecognizer();
                recognizer.Tapped += (sender2, args) =>
                {
                    //(MainPage as ContentPage).Content = this.Content;
                    pickPictureButton.IsEnabled = true;
                };
                pickImg.GestureRecognizers.Add(recognizer);*/
            //(MainPage as ContentPage).Content = image;
            /* }
             else
             {
                 pickPictureButton.IsEnabled = true;
             }
           /*  try
             {
                 var reader = new StreamReader(stream);
                 content = reader.ReadToEnd();
                 reader.Close();
             }
             catch (Exception ex)
             {

             }*/
        }
        static Stream GetStream(string content)
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            return stream;
        }
        public async Task<bool> Upload(Stream stream, string filename, string data)
        {
            //byte[] bitmapData;
            //using (BinaryReader br = new BinaryReader(stream))
            //{
            //    bitmapData = br.ReadBytes((int)stream.Length);
            //}
            //var fileContent = new ByteArrayContent(bitmapData);
            IsLoading = true;
            MultipartFormDataContent multipartContent = new MultipartFormDataContent();
            if (stream != null)
            {
                var fileContent = new StreamContent(stream);
                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "eventimg",
                    FileName = filename
                };
                multipartContent.Add(fileContent);
            }

            
            
            //var body = new StringContent(json);
            //multipartContent.Add(body);

            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.PostAsync("https://www.planmy.me/maizonpub-api/users.php?"+data, multipartContent);
            response.EnsureSuccessStatusCode();
            IsLoading = false;
            if (response.IsSuccessStatusCode)
            {
                Connect con = new Connect();
                var usr = await con.GetData("User");
                UserCookie cookie = Newtonsoft.Json.JsonConvert.DeserializeObject<UserCookie>(usr);
                var config = cookie.configUsr;
                config.event_date = eventDate.Date.ToString("MM/dd/yyyy");
                config.event_name = eventname.Text;
                config.event_location = eventlocation.Text;
                if (stream != null)
                    config.event_img = "https://planmy.me/wp-content/uploads/events/" + filename;
                cookie.configUsr = config;
                var resp = Newtonsoft.Json.JsonConvert.SerializeObject(cookie);
                await con.SaveData("User", resp);
                string content = await response.Content.ReadAsStringAsync();
                OperationCompleted?.Invoke(this, EventArgs.Empty);
                await Navigation.PopModalAsync();
                return true;
            }
            return false;
        }
    }
}