using PlanMy.Library;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordPressPCL.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlanMy.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class VendorsFilter : ContentPage, INotifyPropertyChanged
    {
        public event EventHandler<EventArgs> OperationCompleted;
        public VendorsFilter (int catid, ref List<int> type, ref List<int> city, ref List<int> setting, ref List<int> cateringservicesInt, ref List<int> typeoffurnitureInt, ref List<int> clienteleInt, ref List<int> clothingInt, ref List<int> beautyservicesInt, ref List<int> typeofmusiciansInt, ref List<int> itemlocationInt, ref List<int> typeofserviceInt, ref List<int> capacityInt, ref List<int> honeymoonexperienceInt)
		{
			InitializeComponent ();
            IsLoading = true;
            BindingContext = this;
            LoadSwitches(catid, type, city, setting, cateringservicesInt, typeoffurnitureInt, clienteleInt, clothingInt, beautyservicesInt, typeofmusiciansInt, itemlocationInt, typeofserviceInt, capacityInt, honeymoonexperienceInt);
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
        public async void LoadSwitches(int catid, List<int> type, List<int> city, List<int> setting, List<int> cateringservicesInt, List<int> typeoffurnitureInt, List<int> clienteleInt, List<int> clothingInt, List<int> beautyservicesInt, List<int> typeofmusiciansInt, List<int> itemlocationInt, List<int> typeofserviceInt, List<int> capacityInt, List<int> honeymoonexperienceInt)
        {
            try
            {
                IsLoading = true;
                WordpressService ws = new WordpressService();
                
                switch (catid)
                {

                    case 3:
                        var types = await ws.GetItemTypesAsync();
                        var cities = await ws.GetItemCitiesAsync();
                        var settings = await ws.GetItemSettingsAsync();
                        var capacities = await ws.GetCapacitiesAsync();
                        AddTitle("Type of venue");
                        AddSwitches(types, type);
                        AddTitle("City");
                        AddSwitches(cities, city);
                        AddTitle("Setting");
                        AddSwitches(settings, setting);
                        AddTitle("Capacity");
                        AddSwitches(capacities, capacityInt);
                        break;
                    case 44:
                        var cateringservices = await ws.GetItemCateringServicesAsync();
                        AddTitle("Catering Services");
                        AddSwitches(cateringservices, cateringservicesInt);
                        break;
                    case 60:
                        var typeoffurniture = await ws.GetItemTypeOfFurnituresAsync();
                        AddTitle("Type of furniture");
                        AddSwitches(typeoffurniture, typeoffurnitureInt);
                        break;
                    case 46:
                        var itemclientele = await ws.GetItemClientelesAsync();//Newtonsoft.Json.JsonConvert.DeserializeObject<List<ItemCategory>>(json);
                        AddTitle("Clientele");
                        AddSwitches(itemclientele, clienteleInt);
                        var itemclothing = await ws.GetItemClothingsAsync();
                        AddTitle("Clothing");
                        AddSwitches(itemclothing, clothingInt);
                        break;
                    case 43:
                        var itembeautyservices = await ws.GetItemBeautyServicesAsync();
                        AddTitle("Beauty Services");
                        AddSwitches(itembeautyservices, beautyservicesInt);
                        break;
                    case 59:
                        var itemtypeofmusicians = await ws.GetItemTypeOfMusiciansAsync();
                        AddTitle("Type of Entertainment");
                        AddSwitches(itemtypeofmusicians, typeofmusiciansInt);
                        break;
                    case 54:
                        var honeymoonexperience = await ws.GetHoneymoonExperiencesAsync();
                        AddTitle("Honeymoon Experience");
                        AddSwitches(honeymoonexperience, honeymoonexperienceInt);
                        break;
                    case 51:
                        var typeofservice = await ws.GetTypeOfServicesAsync();
                        AddTitle("Type of Service");
                        AddSwitches(typeofservice, typeofserviceInt);
                        break;
                }
                var itemlocation = await ws.GetItemLocationsAsync();
                AddTitle("Location");
                AddSwitches(itemlocation, itemlocationInt);
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
        private void AddSwitches(IEnumerable<ItemCategory> types, List<int> par)
        {
            try
            {
                foreach (var row in types)
                {
                    StackLayout layout = new StackLayout();
                    layout.Orientation = StackOrientation.Horizontal;
                    Label label = new Label();
                    label.Text = System.Net.WebUtility.HtmlDecode(row.Name);
                    Switch button = new Switch();
                    if (par.Contains(row.Id))
                        button.IsToggled = true;
                    button.Toggled += (s, e) =>
                    {
                        try
                        {
                            if (e.Value == true)
                            {
                                par.Add(row.Id);
                            }
                            else
                            {
                                if (par.Contains(row.Id))
                                    par.Remove(row.Id);
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