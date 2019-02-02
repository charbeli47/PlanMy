using PlanMy.Library;
using PlanMy.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlanMy
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class newtask : ContentPage
	{
		public IEnumerable<VendorCategory> cats;
        public event EventHandler<EventArgs> OperationCompleted;
        List<string> catnames = new List<string>();
		List<int> catids = new List<int>();
		public newtask (bool isedit,CheckList task)
		{
			InitializeComponent ();
			NavigationPage.SetHasNavigationBar(this, false);
			Loadcats(isedit,task);
            if (isedit == true && task != null)
            {
                priorityPicker.IsVisible = true;
                prioritylabel.IsVisible = true;
                Pagetitle.Text = "EDIT TASK";
                titleoftask.Text = task.Title;
                detailstask.Text = task.Description;
                if (!task.IsPriority)
				{
					priorityPicker.SelectedIndex = 1;
				}
				else
				{
					priorityPicker.SelectedIndex = 0;
				}
            }

		
		
			


			backarrow.Clicked += async (object sender, EventArgs e) =>
			{
				Navigation.PopModalAsync();
			};

				Savechanges.Clicked += async (object sender, EventArgs e) =>
			{
				if (isedit == true && task != null)
				{
					edittask(task);	


				}
				else
				{
					addtask();
				}


			};

		}



		public async void Loadcats(bool isedit,CheckList task)
		{
            WebClient client = new WebClient();
            //var vendors = service.GetItemCategoriesAsync();
            //cats = await service.GetItemCategoriesAsync();
            //VendorsListView.ItemsSource = vendors;
            var resp = client.DownloadString(Statics.apiLink + "Categories");
            var cats = Newtonsoft.Json.JsonConvert.DeserializeObject<List<VendorCategory>>(resp);
			catPicker.ItemsSource = cats;
			var catepicker = catPicker.ItemsSource;
			int selectedindex = 0;
			int i = 0;
			if (isedit == true)
			{
				
				foreach(VendorCategory item in catepicker)
				{
					if (item.Id.ToString() == task.VendorCategoryId.ToString())
					{
						selectedindex = i;
					}
					i++;
				}
				catPicker.SelectedIndex = selectedindex;
			}

        }

        //function to add task//
        public async void addtask()
		{
            var usr = await GetUser();
            VendorCategory catt = (VendorCategory)catPicker.SelectedItem;
            Connect con = new Connect();
            CheckList list = new CheckList { Description = detailstask.Text, Status = CheckListStatus.ToDo, VendorCategoryId = catt.Id, Timing = Datepickertask.Date, Title = titleoftask.Text, User = usr };
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(list);
            await con.PostToServer(Statics.apiLink + "CheckLists", json);
            OperationCompleted?.Invoke(this, EventArgs.Empty);
            Navigation.PopModalAsync();
        }
        public async Task<Users> GetUser()
        {
            Connect con = new Connect();
            var usr = await con.GetData("User");
            Users cookie = new Users();
            if (!string.IsNullOrEmpty(usr))
            {
                cookie = Newtonsoft.Json.JsonConvert.DeserializeObject<Users>(usr);
            }
            return cookie;
        }
        //function to edit task//
        public async void edittask(CheckList task)
		{
			VendorCategory catt = (VendorCategory)catPicker.SelectedItem;
            task.VendorCategoryId = catt.Id;
            task.Title = titleoftask.Text;
            task.Description = detailstask.Text;
            task.Timing = Datepickertask.Date;
            Connect con = new Connect();
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(task);
            con.PutToServer(Statics.apiLink + "CheckLists/"+task.Id, json);
            OperationCompleted?.Invoke(this, EventArgs.Empty);
            await Navigation.PopModalAsync();
		}
	}
}