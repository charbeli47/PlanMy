using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PCLStorage;
using PlanMy.Library;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
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
        public AddEvent(Users user)
		{
			InitializeComponent();
            if(user.Events!=null)
            {
                eventname.Text = user.Events.Title;
                eventlocation.Text = user.Events.Description;
                pickImg.Source = user.Events.Image;
                pickImg.IsVisible = false;
                eventDate.Date = user.Events.Date;
            }
            //EventTypePicker.SelectedIndex = 0;
            IsLoading = false;
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
            Users cookie = new Users();
            if (!string.IsNullOrEmpty(usr))
                cookie = Newtonsoft.Json.JsonConvert.DeserializeObject<Users>(usr);
            Events events = new Events { Date = eventDate.Date, Title = eventname.Text, Description = eventlocation.Text, UserId = cookie.Id, IsPrivate = false };
            //string data = "userid=" + cookie.id+ "&eventname=" + eventname.Text+ "&eventdate=" + eventDate.Date.ToString("MM/dd/yyyy")+ "&eventlocation="+eventlocation.Text;
            string filename = Guid.NewGuid().ToString()+".jpg";
            bool uploaded = await Upload(stream, filename, events);
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
        public async Task<bool> Upload(Stream stream, string filename, Events data)
        {
            var jsonToSend = JsonConvert.SerializeObject(data, Formatting.None, new IsoDateTimeConverter());
            var body = new StringContent(jsonToSend, Encoding.UTF8, "application/json");
            //byte[] bitmapData;
            //using (BinaryReader br = new BinaryReader(stream))
            //{
            //    bitmapData = br.ReadBytes((int)stream.Length);
            //}
            //var fileContent = new ByteArrayContent(bitmapData);
            IsLoading = true;
            MultipartFormDataContent multipartContent = new MultipartFormDataContent();
            multipartContent.Add(body);
            if (stream != null)
            {
                var fileContent = new StreamContent(stream);
                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "Image",
                    FileName = filename
                };
                multipartContent.Add(fileContent);
            }
            
            
            
            //var body = new StringContent(json);
            //multipartContent.Add(body);

            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.PostAsync(Statics.apiLink+"Events", multipartContent);
            response.EnsureSuccessStatusCode();
            IsLoading = false;
            if (response.IsSuccessStatusCode)
            {
                Connect con = new Connect();
                var usr = await con.GetData("User");
                Users cookie = new Users();
                if (!string.IsNullOrEmpty(usr))
                    cookie = Newtonsoft.Json.JsonConvert.DeserializeObject<Users>(usr);
                if (stream != null)
                    cookie.Events.Image = await DownLoadFile("http://test.planmy.me/Media/" + filename, filename);
                
                
                var resp = Newtonsoft.Json.JsonConvert.SerializeObject(cookie);
                await con.SaveData("User", resp);
                string content = await response.Content.ReadAsStringAsync();
                OperationCompleted?.Invoke(this, EventArgs.Empty);
                await Navigation.PopModalAsync();
                return true;
            }
            return false;
        }

        private async Task<string> DownLoadFile(string link, string filename)
        {
            IsLoading = true;
            WebClient downclient = new WebClient();
            IFolder rootFolder = FileSystem.Current.LocalStorage;
            IFolder folder = await rootFolder.CreateFolderAsync("EventImg", CreationCollisionOption.OpenIfExists);
            string path = folder.Path + "/" + filename;
            downclient.DownloadFile(link, path);
            IsLoading = false;
            return path;
        }
    }
}