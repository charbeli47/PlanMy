using Newtonsoft.Json;
using PlanMy.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlanMy.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewExpense : ContentPage
	{
        public event EventHandler<EventArgs> OperationCompleted;
        public NewExpense (bool isedit,expenseforcat ec)
		{
			InitializeComponent ();

			//get cats///
			getcatsexpenses(isedit, ec);


			// when w eare editting///
			if (isedit == true && ec != null)
			{
				Deleteexpenselabel.IsVisible = true;
				Deleteexpenselabel.Clicked += async (object sender, EventArgs e) =>
				{
					deleteexpense(ec);
                    OperationCompleted?.Invoke(this, EventArgs.Empty);
                    await Navigation.PopModalAsync();
				};

				addnewcat.IsVisible = false;
				expenddescription.Text = ec.budget_list_name;
				expendpaidcost.Text = ec.budget_list_paid_cost;
				expendactualcost.Text = ec.budget_list_actual_cost;
				expendestimatedcost.Text = ec.budget_list_estimate_cost;
			}
		


			NavigationPage.SetHasNavigationBar(this, false);
			backarrow.Clicked += async (object sender, EventArgs e) =>
			{
                await Navigation.PopModalAsync();
			};
		

			//

			Savechanges.Clicked += async (object sender, EventArgs e) =>
			{
				if (isedit == true && ec != null)
				{
					updateexpense(ec);
				}
				else
				{
					addnewexpense();
				}
					

			};
			addnewcat.Clicked += async (object sender, EventArgs e) =>
			{
				catPicker.IsVisible = false;
				addnewcat.IsVisible = false;
				choosefromcat.IsVisible = true;
				expendnewname.IsVisible = true;
				expensecontent.IsVisible = false;
				butaddcat.IsVisible = true;

				butaddcat.Clicked += async (object senderr, EventArgs ee) =>
				{

					addnewexpensecat();
				};

			};
			choosefromcat.Clicked += async (object sender, EventArgs e) =>
			{
				catPicker.IsVisible = true;
				addnewcat.IsVisible = true;
				choosefromcat.IsVisible =false;
				expendnewname.IsVisible = false;
				expensecontent.IsVisible =true;
				butaddcat.IsVisible = true;

			};

		}
		public async void addnewexpense()
		{
			expense expensecat = (expense)catPicker.SelectedItem;
            var usr = await GetUser();
			using (var cl = new HttpClient())
			{
				var formcontent = new FormUrlEncodedContent(new[]
				{
			new KeyValuePair<string,string>("category_id",expensecat.category_id.ToString()),
			new KeyValuePair<string, string>("user_id",usr.user.id.ToString()),
				new KeyValuePair<string,string>("name",expenddescription.Text),
			new KeyValuePair<string, string>("estimate_cost",expendestimatedcost.Text),
			new KeyValuePair<string,string>("actual_cost",expendactualcost.Text),
			new KeyValuePair<string, string>("paid_cost",expendpaidcost.Text),
			
		});

				var request = await cl.PostAsync("https://planmy.me/maizonpub-api/budget.php?action=insert", formcontent);
				request.EnsureSuccessStatusCode();
				var response = await request.Content.ReadAsStringAsync();
                OperationCompleted?.Invoke(this, EventArgs.Empty);
                await Navigation.PopModalAsync();

			}
		}
		
        public async Task<UserCookie> GetUser()
        {
            Connect con = new Connect();
            var usr = await con.GetData("User");
            UserCookie cookie = new UserCookie();
            if (!string.IsNullOrEmpty(usr))
            {
                cookie = Newtonsoft.Json.JsonConvert.DeserializeObject<UserCookie>(usr);
            }
            return cookie;
        }
		public async void getcatsexpenses(bool isedit,expenseforcat ec)
		{
			Connect con = new Connect();
            var usr = await GetUser();
            if (usr.user != null)
            {
                string todostring = await con.DownloadData("http://planmy.me/maizonpub-api/budget_category.php", "action=get&category_user_id=" + usr.user.id);
                List<expense> listofcats = JsonConvert.DeserializeObject<List<expense>>(todostring);
                catPicker.ItemsSource = listofcats;

                int selectedindex = 0;
                int i = 0;
                if (isedit == true)
                {

                    foreach (expense item in listofcats)
                    {
                        if (item.category_id.ToString() == ec.budget_list_category_id.ToString())
                        {
                            selectedindex = i;
                        }
                        i++;
                    }
                    catPicker.SelectedIndex = selectedindex;
                    catPicker.IsEnabled = false;
                }
            }
		}

		public async void updateexpense(expenseforcat expense)
		{

			using (var cl = new HttpClient())
			{
				var formcontent = new FormUrlEncodedContent(new[]
				{
			new KeyValuePair<string,string>("list_id",expense.budget_list_id),
			new KeyValuePair<string, string>("name",expenddescription.Text),
				new KeyValuePair<string,string>("estimate_cost",expendestimatedcost.Text),
			new KeyValuePair<string, string>("actual_cost",expendactualcost.Text),
			new KeyValuePair<string,string>("paid_cost",expendpaidcost.Text),


		});

				var request = await cl.PostAsync("https://planmy.me/maizonpub-api/budget.php?action=update", formcontent);
				request.EnsureSuccessStatusCode();
				var response = await request.Content.ReadAsStringAsync();
                OperationCompleted?.Invoke(this, EventArgs.Empty);
                await Navigation.PopModalAsync();
			}
		}

		public async void deleteexpense(expenseforcat expense)
		{

			using (var cl = new HttpClient())
			{
				var formcontent = new FormUrlEncodedContent(new[]
				{
			new KeyValuePair<string,string>("list_id",expense.budget_list_id)
		});

				var request = await cl.PostAsync("https://planmy.me/maizonpub-api/budget.php?action=delete", formcontent);
				request.EnsureSuccessStatusCode();
				var response = await request.Content.ReadAsStringAsync();
                OperationCompleted?.Invoke(this, EventArgs.Empty);
                await Navigation.PopModalAsync();
			}
		}


		public async void addnewexpensecat()
		{
			expense expensecat = (expense)catPicker.SelectedItem;
            var usr = await GetUser();
			using (var cl = new HttpClient())
			{
				var formcontent = new FormUrlEncodedContent(new[]
				{
			new KeyValuePair<string,string>("category_user_id",usr.user.id.ToString()),
			new KeyValuePair<string, string>("category_name",expendnewname.Text),
			

		});

				var request = await cl.PostAsync("https://planmy.me/maizonpub-api/budget_category.php?action=insert", formcontent);
				request.EnsureSuccessStatusCode();
				var response = await request.Content.ReadAsStringAsync();
                OperationCompleted?.Invoke(this, EventArgs.Empty);
                await Navigation.PopModalAsync();

			}
		}
		
	}

	
}