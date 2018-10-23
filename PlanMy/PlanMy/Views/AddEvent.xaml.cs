using PlanMy.Library;
using System;
using System.Collections.Generic;
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
	public partial class AddEvent : ContentPage
	{
        protected Stream stream = Stream.Null;
        public AddEvent()
		{
			InitializeComponent();
			//EventTypePicker.SelectedIndex = 0;
			
		}

        private void closeBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private async void createEventBtn_Clicked(object sender, EventArgs e)
        {
            Connect con = new Connect();
            var usr = await con.GetData("User");
            UserCookie cookie = Newtonsoft.Json.JsonConvert.DeserializeObject<UserCookie>(usr);
            string json = "{'action':'updateevent','userid':"+cookie.user.id+",'eventname':'"+ eventname.Text+ "','eventdate':'"+eventDate.Date+"'}";
            string filename = Guid.NewGuid().ToString()+".jpg";
            
            bool uploaded = await Upload(stream, filename, json);
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            var pickPictureButton = (Button)sender;
            stream = await DependencyService.Get<IPicturePicker>().GetImageStreamAsync();

            if (stream != null)
            {
                pickImg.Source = ImageSource.FromStream(() => stream);
                pickImg.IsVisible = true;

                TapGestureRecognizer recognizer = new TapGestureRecognizer();
                recognizer.Tapped += (sender2, args) =>
                {
                    //(MainPage as ContentPage).Content = this.Content;
                    pickPictureButton.IsEnabled = true;
                };
                pickImg.GestureRecognizers.Add(recognizer);
                //(MainPage as ContentPage).Content = image;
            }
            else
            {
                pickPictureButton.IsEnabled = true;
            }
        }
        
        public async Task<bool> Upload(Stream stream, string filename, string json)
        {
            byte[] bitmapData;
            using (BinaryReader br = new BinaryReader(stream))
            {
                bitmapData = br.ReadBytes((int)stream.Length);
            }
            var fileContent = new ByteArrayContent(bitmapData);

            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream");
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "eventimg",
                FileName = filename
            };

            string boundary = "---8393774hhy37373773";
            MultipartFormDataContent multipartContent = new MultipartFormDataContent(boundary);
            multipartContent.Add(fileContent);
            var body = new StringContent(json, Encoding.UTF8, "application/json");
            multipartContent.Add(body);

            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.PostAsync("https://www.planmy.me/maizonpub-api/users.php", multipartContent);
            response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();

                return true;
            }
            return false;
        }
    }
}