//using Java.Lang;
using Newtonsoft.Json;
using PlanMy.Library;
using PlanMy.ViewModels;
using PlanMy.Views;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlanMy.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Planning : ContentPage
	{
		
		

		public Planning()
        {
            
            InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
			checklistbut.Clicked += (object sender, EventArgs e) =>
			{
				checklistbut.Image = "bchecklist.png";
				guestbut.Image = "guestlist.png";
				budgetbut.Image = "budget.png";
				suppliersbut.Image = "suppliers.png";
				ordersbut.Image = "orders.png";
                carouselView.Position = 0;
			};
			guestbut.Clicked += (object sender, EventArgs e) =>
			{
				checklistbut.Image = "checklist.png";
				guestbut.Image = "bguestslist.png";
				budgetbut.Image = "budget.png";
				suppliersbut.Image = "suppliers.png";
				ordersbut.Image = "orders.png";
                carouselView.Position = 1;
            };
			budgetbut.Clicked += (object sender, EventArgs e) =>
			{
				checklistbut.Image = "checklist.png";
				guestbut.Image = "guestlist.png";
				budgetbut.Image = "bbudget.png";
				suppliersbut.Image = "suppliers.png";
				ordersbut.Image = "orders.png";
                carouselView.Position = 2;
            };
			suppliersbut.Clicked += async (s, e) => {
				checklistbut.Image = "checklist.png";
				guestbut.Image = "guestlist.png";
				budgetbut.Image = "budget.png";
				suppliersbut.Image = "bluesuppliers.png";
				ordersbut.Image = "orders.png";
                carouselView.Position = 3;
            };
            ordersbut.Clicked += async (s, e) => {
                checklistbut.Image = "checklist.png";
                guestbut.Image = "guestlist.png";
                budgetbut.Image = "budget.png";
                suppliersbut.Image = "suppliers.png";
                ordersbut.Image = "blueorders.png";
                carouselView.Position = 4;
            };

            LoadPage();


        }
        async void LoadPage()
        {

            var usr = await GetUser();
            if (usr!=null)
            {
                checklistbut.IsVisible = true;
                guestbut.IsVisible = true;
                budgetbut.IsVisible = true;
                suppliersbut.IsVisible = true;
                ordersbut.IsVisible = true;
                LoginStack.IsVisible = false;
                viewStack.IsVisible = true;
                List<Views> vs = new List<Views>();
                CheckListView checklist = new CheckListView();
                GuestView guests = new GuestView();
                BudgetView budget = new BudgetView();
                favoriteView favorite = new favoriteView();
                OrdersView orders = new OrdersView();
                vs.Add(new Views { content = checklist });
                vs.Add(new Views { content = guests });
                vs.Add(new Views { content = budget });
                vs.Add(new Views { content = favorite });
                vs.Add(new Views { content = orders });
                carouselView.ItemsSource = vs;
                
            }
            else
            {
                checklistbut.IsVisible = false;
                guestbut.IsVisible = false;
                budgetbut.IsVisible = false;
                suppliersbut.IsVisible = false;
                ordersbut.IsVisible = false;
                viewStack.IsVisible = false;
                LoginStack.IsVisible = true;
            }
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

        ///functions for task //


        private void carouselView_PositionSelected(object sender, CarouselView.FormsPlugin.Abstractions.PositionSelectedEventArgs e)
        {
            switch (e.NewValue)
            {
                case 0:
                    checklistbut.Image = "bchecklist.png";
                    guestbut.Image = "guestlist.png";
                    budgetbut.Image = "budget.png";
                    suppliersbut.Image = "suppliers.png";
                    ordersbut.Image = "orders.png";
                    break;
                case 1:
                    checklistbut.Image = "checklist.png";
                    guestbut.Image = "bguestslist.png";
                    budgetbut.Image = "budget.png";
                    suppliersbut.Image = "suppliers.png";
                    ordersbut.Image = "orders.png";
                    break;
                case 2:
                    checklistbut.Image = "checklist.png";
                    guestbut.Image = "guestlist.png";
                    budgetbut.Image = "bbudget.png";
                    suppliersbut.Image = "suppliers.png";
                    ordersbut.Image = "orders.png";
                    break;
                case 3:
                    checklistbut.Image = "checklist.png";
                    guestbut.Image = "guestlist.png";
                    budgetbut.Image = "budget.png";
                    suppliersbut.Image = "bluesuppliers.png";
                    ordersbut.Image = "orders.png";
                    break;
                case 4:
                    checklistbut.Image = "checklist.png";
                    guestbut.Image = "guestlist.png";
                    budgetbut.Image = "budget.png";
                    suppliersbut.Image = "suppliers.png";
                    ordersbut.Image = "blueorders.png";
                    break;
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            var login = new Login();
            login.OperationCompleted += (s, ev) => {
                LoadPage();
            };
            Navigation.PushModalAsync(new Login());
        }
    }



}