using Newtonsoft.Json;
using PlanMy.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlanMy.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BudgetView : ContentView
	{
		public BudgetView ()
		{
			InitializeComponent ();
            addbudget.Clicked += async (object sender, EventArgs e) =>
            {
                var newexpense = new NewExpense(false, null);
                newexpense.OperationCompleted += Newexpense_OperationCompleted;
                await Navigation.PushModalAsync(newexpense);
            };
            getexpenses();
        }
        private void Newexpense_OperationCompleted(object sender, EventArgs e)
        {
            getexpenses();
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



        public StackLayout createtablerow(string name)
        {
            StackLayout tablelayout = new StackLayout();
            tablelayout.Orientation = StackOrientation.Vertical;

            StackLayout rowlayout = new StackLayout();
            rowlayout.Orientation = StackOrientation.Horizontal;
            rowlayout.Margin = new Thickness(15, 0, 15, 0);

            ///Image plusimg = new Image();
            //plusimg.Source = icon;
            //plusimg.HorizontalOptions = LayoutOptions.Start;
            //plusimg.Margin = new Thickness(0, 0, 25, 0);
            //plusimg.VerticalOptions = LayoutOptions.Center;
            //rowlayout.Children.Add(plusimg);

            StackLayout vlayout = new StackLayout();
            vlayout.Orientation = StackOrientation.Vertical;

            Label nametable = new Label();
            nametable.Text = name;
            nametable.FontSize = 18;
            nametable.TextColor = Color.Black;
            nametable.VerticalOptions = LayoutOptions.Center;
            nametable.HorizontalOptions = LayoutOptions.FillAndExpand;
            nametable.Margin = new Thickness(0, 10, 0, 0);
            nametable.FontAttributes = FontAttributes.Bold;


            vlayout.Children.Add(nametable);

            vlayout.HorizontalOptions = LayoutOptions.FillAndExpand;
            rowlayout.Children.Add(vlayout);


            Button img = new Button();
            img.Image = "downarrow.png";
            img.HorizontalOptions = LayoutOptions.End;
            img.VerticalOptions = LayoutOptions.Center;
            img.BackgroundColor = Color.Transparent;
            img.Clicked += (object sender, EventArgs e) =>
            {
                if (img.Image == "downarrow.png")
                {
                    img.Image = "uparrow.png";
                    foreach (View v in tablelayout.Children)
                    {
                        v.IsVisible = true;
                    }
                }
                else
                {
                    int i = 0;
                    img.Image = "downarrow.png";
                    foreach (View v in tablelayout.Children)
                    {
                        if (i == 0) { }
                        else
                        {
                            v.IsVisible = false;
                        }
                        i++;
                    }
                }
            };

            rowlayout.Children.Add(img);




            tablelayout.Children.Add(rowlayout);


            return tablelayout;

        }
        //// functions for budget///
        public async void getexpenses()
        {
            budgetstack.Children.Clear();

            float estimatedtotalamount = 0;
            float actualtotalamount = 0;
            float paidtotalamount = 0;
            float remaining = 0;

            Connect con = new Connect();
            var usr = await GetUser();
            if (usr != null)
            {
                string todostring = await con.DownloadData(Statics.apiLink+ "BudgetCategories","UserId =" + usr.Id);
                List<BudgetCategory> listofcats = JsonConvert.DeserializeObject<List<BudgetCategory>>(todostring);

                foreach (BudgetCategory e in listofcats)
                {
                    StackLayout row = createtablerow(e.Title);
                    row.GestureRecognizers.Add(new TapGestureRecognizer
                    {


                        Command = new Command(() =>
                        {
                            var catexpense = new Categorieexpense(e);
                            catexpense.OperationCompleted += Catexpense_OperationCompleted;
                            Navigation.PushModalAsync(catexpense);
                        }),
                    });
                    //budgetstack.Children.Add(row);
                    List<Budget> listofexpenses = e.Budgets;

                    foreach (Budget ec in listofexpenses)
                    {

                        StackLayout rowexpense = createsupplierrowintable(ec.Description.ToString(), ec.EstimatedCost.ToString(), ec.PaidCost.ToString());
                        rowexpense.GestureRecognizers.Add(new TapGestureRecognizer
                        {

                            Command = new Command(() =>
                            {
                                var newexpense = new NewExpense(true, ec);
                                newexpense.OperationCompleted += Newexpense_OperationCompleted;
                                Navigation.PushModalAsync(newexpense);
                            }),
                        });

                        row.Children.Add(rowexpense);
                        estimatedtotalamount = estimatedtotalamount + ec.EstimatedCost;
                        actualtotalamount = actualtotalamount + ec.ActualCost;
                        paidtotalamount = paidtotalamount + ec.PaidCost;
                    }
                    budgetstack.Children.Add(row);
                    budgetstack.Children.Add(createseperatorbetweentables());
                }
            }
                estimatedlabel.Text = "$" + " " + estimatedtotalamount.ToString();
                actuallabel.Text = "$" + " " + actualtotalamount.ToString();
                paidlabel.Text = "$" + " " + paidtotalamount.ToString();
                remaining = actualtotalamount - paidtotalamount;
                remaininglabel.Text = "Remaining $" + " " + remaining.ToString();
            
        }
        public StackLayout createseperatorbetweentables()
        {

            StackLayout line = new StackLayout();
            line.Orientation = StackOrientation.Horizontal;
            line.HeightRequest = 5;
            line.BackgroundColor = Color.LightGray;
            line.HorizontalOptions = LayoutOptions.Fill;
            return line;

        }
        private void Catexpense_OperationCompleted(object sender, EventArgs e)
        {
            getexpenses();
        }

        public StackLayout createsupplierrowintable(string venuname, string pricep, string paidprice)
        {
            StackLayout vlayout = new StackLayout();
            vlayout.Orientation = StackOrientation.Vertical;
            vlayout.IsVisible = false;

            StackLayout line = new StackLayout();
            line.Orientation = StackOrientation.Horizontal;
            line.HeightRequest = 1;
            line.BackgroundColor = Color.LightGray;
            line.HorizontalOptions = LayoutOptions.Fill;
            vlayout.Children.Add(line);

            Label nameg = new Label();
            nameg.Text = venuname;
            nameg.FontSize = 16;
            nameg.TextColor = Color.Black;
            nameg.FontAttributes = FontAttributes.Bold;
            nameg.HorizontalOptions = LayoutOptions.FillAndExpand;
            nameg.Margin = new Thickness(15, 0, 0, 0);
            vlayout.Children.Add(nameg);


            StackLayout rowlayout = new StackLayout();
            rowlayout.Orientation = StackOrientation.Horizontal;
            rowlayout.Margin = new Thickness(15, 0, 15, 0);

            Label cost = new Label();
            cost.Text = "Cost:$";
            cost.FontSize = 12;
            cost.TextColor = Color.Black;
            //cost.Margin = new Thickness(15, 0, 0, 0);
            rowlayout.Children.Add(cost);

            Label price = new Label();
            price.Text = pricep;
            price.FontSize = 12;
            price.TextColor = Color.Black;
            //cost.HorizontalOptions = LayoutOptions.FillAndExpand;
            rowlayout.Children.Add(price);

            Label estimated = new Label();
            estimated.Text = "(estimate)";
            estimated.FontSize = 12;
            estimated.TextColor = Color.Black;
            estimated.HorizontalOptions = LayoutOptions.FillAndExpand;
            rowlayout.Children.Add(estimated);

            Label paid = new Label();
            paid.Text = "Paid:$ ";
            paid.FontSize = 12;
            paid.TextColor = Color.Black;
            //cost.Margin = new Thickness(15, 0, 0, 0);
            rowlayout.Children.Add(paid);

            Label paidp = new Label();
            paidp.Text = paidprice;
            paidp.FontSize = 12;
            paidp.TextColor = Color.Black;
            //cost.Margin = new Thickness(15, 0, 0, 0);
            rowlayout.Children.Add(paidp);







            vlayout.Children.Add(rowlayout);

            StackLayout line2 = new StackLayout();
            line2.Orientation = StackOrientation.Horizontal;
            line2.HeightRequest = 1;
            line2.BackgroundColor = Color.LightGray;
            line2.HorizontalOptions = LayoutOptions.Fill;
            vlayout.Children.Add(line);
            return vlayout;
        }
    }
}