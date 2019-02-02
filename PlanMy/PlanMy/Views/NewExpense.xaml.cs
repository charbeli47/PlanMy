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
        public NewExpense (bool isedit,Budget ec)
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
				expenddescription.Text = ec.Description;
				expendpaidcost.Text = ec.PaidCost.ToString();
				expendactualcost.Text = ec.ActualCost.ToString();
				expendestimatedcost.Text = ec.EstimatedCost.ToString();
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
			BudgetCategory expensecat = (BudgetCategory)catPicker.SelectedItem;
            var usr = await GetUser();
            Connect con = new Connect();
            MultipartFormDataContent data = new MultipartFormDataContent();
            Budget budget = new Budget { BudgetCategoryId = expensecat.Id, ActualCost = float.Parse(expendactualcost.Text), Description = expenddescription.Text, EstimatedCost = float.Parse(expendestimatedcost.Text), PaidCost = float.Parse(expendpaidcost.Text), UserId = usr.Id };
            string json = JsonConvert.SerializeObject(budget);
            var budgetj = new StringContent(json);
            data.Add(budgetj, "budget");
            await con.PostToServer(Statics.apiLink + "Budgets", data);
            OperationCompleted?.Invoke(this, EventArgs.Empty);
            await Navigation.PopModalAsync();
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
		public async void getcatsexpenses(bool isedit,Budget ec)
		{
			Connect con = new Connect();
            var usr = await GetUser();
            if (usr != null)
            {
                string todostring = await con.DownloadData(Statics.apiLink+"BudgetCategories", "UserId=" + usr.Id);
                List<BudgetCategory> listofcats = JsonConvert.DeserializeObject<List<BudgetCategory>>(todostring);
                catPicker.ItemsSource = listofcats;

                int selectedindex = 0;
                int i = 0;
                if (isedit == true)
                {

                    foreach (BudgetCategory item in listofcats)
                    {
                        if (item.Id == ec.BudgetCategoryId)
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

		public async void updateexpense(Budget expense)
		{
            Connect con = new Connect();
            expense.Description = expenddescription.Text;
            expense.EstimatedCost = float.Parse(expendestimatedcost.Text);
            expense.ActualCost = float.Parse(expendactualcost.Text);
            expense.PaidCost = float.Parse(expendpaidcost.Text);
            string json = JsonConvert.SerializeObject(expense);
            con.PutToServer(Statics.apiLink + "Budgets/"+expense.Id, json);
            OperationCompleted?.Invoke(this, EventArgs.Empty);
            await Navigation.PopModalAsync();
		}

		public async void deleteexpense(Budget expense)
		{
            Connect con = new Connect();
            con.DeleteFromServer(Statics.apiLink + "Budgets/" + expense.Id);
            OperationCompleted?.Invoke(this, EventArgs.Empty);
            await Navigation.PopModalAsync();
		}


        public async void addnewexpensecat()
        {
            //BudgetCategory expensecat = (BudgetCategory)catPicker.SelectedItem;
            var usr = await GetUser();
            Connect con = new Connect();
            MultipartFormDataContent data = new MultipartFormDataContent();
            BudgetCategory cat = new BudgetCategory { Title = expendnewname.Text, UserId = usr.Id };
            string json = JsonConvert.SerializeObject(cat);
            var budgetCategory = new StringContent(json);
            data.Add(budgetCategory, "wishList");
            await con.PostToServer(Statics.apiLink + "BudgetCategories", data);
            OperationCompleted?.Invoke(this, EventArgs.Empty);
            await Navigation.PopModalAsync();
        }
		
	}
}