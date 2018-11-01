using PlanMy.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordPressPCL.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlanMy.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class VendorsFilter : ContentPage
	{
        public VendorsFilter (int catid)
		{
			InitializeComponent ();
            LoadSwitches(catid);
        }
        public async void LoadSwitches(int catid)
        {
            try
            {

                WordpressService ws = new WordpressService();
                List<int> type = new List<int>(), city = new List<int>(), setting = new List<int>(), cateringservicesInt = new List<int>(), typeoffurnitureInt = new List<int>(), clienteleInt = new List<int>(), clothingInt = new List<int>(), beautyservicesInt = new List<int>(), typeofmusiciansInt = new List<int>(), itemlocationInt = new List<int>(), typeofserviceInt = new List<int>(), capacityInt = new List<int>(), honeymoonexperienceInt = new List<int>();
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
    }
    
}