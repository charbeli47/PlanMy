using PlanMy.Library;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlanMy.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class VendorsFilter : ContentPage, INotifyPropertyChanged
    {
        public event EventHandler<EventArgs> OperationCompleted;
        public VendorsFilter (int catid, ref List<VendorTypeValue> types)
		{
			InitializeComponent ();
            IsLoading = true;
            BindingContext = this;
            LoadSwitches(catid, types);
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
        public void LoadSwitches(int catid, List<VendorTypeValue> types)
        {
           try
            {
                IsLoading = true;
                WebClient wc = new WebClient();
                string resp = wc.DownloadString(Statics.apiLink + "VendorTypes?CategoryId"+catid);
                var vendortypes = Newtonsoft.Json.JsonConvert.DeserializeObject<List<VendorType>>(resp);
                foreach(var row in vendortypes)
                {
                    AddTitle(row.Title);
                    AddSwitches(row.VendorTypeValues, types);
                }
            }
            catch(Exception ex)
            {

            }
            IsLoading = false;
        }
        private void AddTitle(string v)
        {
            try
            {
                Label title = new Label();
                title.Text = v;
                title.FontSize = 20;
                title.FontAttributes = FontAttributes.Bold;
                title.HorizontalTextAlignment = TextAlignment.Start;
                title.Margin = new Thickness(10, 10, 0, 0);
                FiltersList.Children.Add(title);
            }
            catch(Exception ex)
            {

            }
        }
        void AddSwitches(List<VendorTypeValue> types, List<VendorTypeValue> par)
        {
            try
            {
                foreach (var row in types)
                {
                    StackLayout layout = new StackLayout();
                    layout.Orientation = StackOrientation.Horizontal;
                    Label label = new Label();
                    label.Text = System.Net.WebUtility.HtmlDecode(row.Title);
                    Switch button = new Switch();
                    if (par.Contains(row))
                        button.IsToggled = true;
                    button.Toggled += (s, e) =>
                    {
                        try
                        {
                            if (e.Value == true)
                            {
                                par.Add(row);
                            }
                            else
                            {
                                if (par.Contains(row))
                                    par.Remove(row);
                            }
                        }
                        catch(Exception ex)
                        {

                        }
                    };
                    layout.Children.Add(button);
                    layout.Children.Add(label);
                    layout.Margin = new Thickness(10, 10, 0, 0);
                    FiltersList.Children.Add(layout);

                }
            }
            catch(Exception ex)
            {

            }
        }
        
        private async void filterBtn_Clicked(object sender, EventArgs e)
        {
            OperationCompleted?.Invoke(this, EventArgs.Empty);
            await Navigation.PopModalAsync();
        }
    }
    
}