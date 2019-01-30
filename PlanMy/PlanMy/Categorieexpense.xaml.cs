using PlanMy.Library;
using PlanMy.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlanMy
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Categorieexpense : ContentPage
	{
        public event EventHandler<EventArgs> OperationCompleted;
        public Categorieexpense (BudgetCategory category)
		{
			InitializeComponent ();
			expendcatname.Text = category.Title;
			Deleteexpenselabel.Clicked += async (object sender, EventArgs e) =>
			{
				deleteexpensecat(category);
			};

			savechanges.Clicked += async (object sender, EventArgs e) =>
			{
				updateexpensecat(category);
			};

			backarrow.Clicked += async (object sender, EventArgs e) =>
			{
                OperationCompleted?.Invoke(this, EventArgs.Empty);
                await Navigation.PopModalAsync();
			};
		}

		public async void deleteexpensecat(BudgetCategory category)
		{
            Connect con = new Connect();
            con.DeleteFromServer(Statics.apiLink + "BudgetCategories/" + category.Id);
            OperationCompleted?.Invoke(this, EventArgs.Empty);
            await Navigation.PopModalAsync();
		}
		public async void updateexpensecat(BudgetCategory category)
		{
            category.Title = expendcatname.Text;
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(category);
            Connect con = new Connect();
            con.PutToServer(Statics.apiLink, json);
            OperationCompleted?.Invoke(this, EventArgs.Empty);
            await Navigation.PopModalAsync();
		}

	}
}