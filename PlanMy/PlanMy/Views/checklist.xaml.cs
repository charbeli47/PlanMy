using PlanMy.Library;
using PlanMy.Views;
using System;
using System.Collections.Generic;
using System.Globalization;
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
	public partial class checklist : ContentPage
	{
        public event EventHandler<EventArgs> OperationCompleted;
        public checklist(CheckList task,string catname)
		{
			InitializeComponent();
			// for testing purposes ///
			// fill the data///
			backarrow.Clicked += async (object sender, EventArgs e) =>
			{
                OperationCompleted?.Invoke(this, EventArgs.Empty);
                await Navigation.PopModalAsync();
			};
			NavigationPage.SetHasNavigationBar(this, false);
			title.Text = task.Title;
			category.Text = catname;
			description.Text = task.Description;
			DateTime dateTodo = task.Timing;
			string monthName = dateTodo.ToString("MMM", CultureInfo.InvariantCulture);
			string year = dateTodo.Year.ToString();

			date.Text = "By" + " " + monthName + " " + year;


			//action for delete task///
			deletetask.Clicked += async (object sender, EventArgs e) =>
			{
                Connect con = new Connect();
                con.DeleteFromServer(Statics.apiLink + "CheckLists/" + task.Id);
                OperationCompleted?.Invoke(this, EventArgs.Empty);
                await Navigation.PopModalAsync();
			};

			///action for edit task///
			///
		
			edittask.Clicked += async (object sender, EventArgs e) =>
			{

				await Navigation.PushModalAsync(new newtask(true, task));
			};

			pendingbut.Clicked += (object sender, EventArgs e) =>
			{

				edittaskstatus(task,CheckListStatus.Done);
			};


			searchsuppliers.GestureRecognizers.Add(
	new TapGestureRecognizer()
	{
		Command = new Command(() => { Navigation.PushModalAsync(new Vendors());})
	});
			
            

        }
        public async void LoadRecommendedSuppliers(CheckList task)
        {
            int cat = task.VendorCategoryId;
            Connect con = new Connect();
            string fresp = await con.DownloadData(Statics.apiLink + "VendorItems/Featured/" + cat, "");
            var featuredItems = Newtonsoft.Json.JsonConvert.DeserializeObject<List<VendorItem>>(fresp);
            for (int i = 0; i < featuredItems.Count; i++)
            {
                var item = featuredItems[i];
                if (i == 0)
                {
                    StackLayout firststack = createsupplierverticalstack(item, WebUtility.HtmlDecode(item.Title), Statics.MediaLink+item.Thumb);
                    firststack.Margin = new Thickness(15, 0, 0, 0);
                    Suppliersstack.Children.Add(firststack);
                }
                else
                {
                    Suppliersstack.Children.Add(createsupplierverticalstack(item, WebUtility.HtmlDecode(item.Title), Statics.MediaLink + item.Thumb));
                }
            }
        }
		public StackLayout createsupplierverticalstack(VendorItem item, string nametxt,string imgurl)
		{
			StackLayout stack = new StackLayout();
			stack.Orientation = StackOrientation.Vertical;
			Image img = new Image();
			img.Source = imgurl;
            TapGestureRecognizer recognizer = new TapGestureRecognizer();
            recognizer.Tapped += (sender2, args) =>
            {
                //(MainPage as ContentPage).Content = this.Content;
                Navigation.PushModalAsync(new selectedvendor(item.Title, item), true);
            };
            img.GestureRecognizers.Add(recognizer);
            Label name = new Label();
			name.Text = nametxt;
			stack.Children.Add(img);
			stack.Children.Add(name);
			return stack;
		}

		// function for edit task status///
		public async void edittaskstatus(CheckList task, CheckListStatus statusval)
		{
            Connect con = new Connect();
            task.Status = statusval;
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(task);
            con.PutToServer(Statics.apiLink + "CheckLists", json);
            OperationCompleted?.Invoke(this, EventArgs.Empty);
            await Navigation.PopModalAsync();
		}

	}


}