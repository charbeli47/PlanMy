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
		public IEnumerable<WordPressPCL.Models.ItemCategory> cats;

		public List<WordPressPCL.Models.ItemCategory> categories=new List<WordPressPCL.Models.ItemCategory>();
		List<todoobj> specifiedobj = new List<todoobj>();

		public Planning()
        {
            InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
			planningView.TranslationX = 0;
			guestsView.TranslationX = Bounds.Width;
			budgetView.TranslationX = Bounds.Width * 2;
			checklistbut.Clicked += (object sender, EventArgs e) =>
			{
				checklistbut.Image = "bchecklist.png";
				guestbut.Image = "guestlist.png";
				budgetbut.Image = "budget.png";
				suppliersbut.Image = "suppliers.png";
				ordersbut.Image = "orders.png";
				planningView.IsVisible = true;
				guestsView.IsVisible = false;
				budgetView.IsVisible = false;
				favoriteView.IsVisible = false;
			};
			guestbut.Clicked += (object sender, EventArgs e) =>
			{
				checklistbut.Image = "checklist.png";
				guestbut.Image = "bguestslist.png";
				budgetbut.Image = "budget.png";
				suppliersbut.Image = "suppliers.png";
				ordersbut.Image = "orders.png";

				//guestsView = new guests();
				planningView.IsVisible = false;
				guestsView.IsVisible = true;
				budgetView.IsVisible = false;
				favoriteView.IsVisible = false;
				//await Navigation.PushAsync(new guests());
			};
			budgetbut.Clicked += (object sender, EventArgs e) =>
			{
				checklistbut.Image = "checklist.png";
				guestbut.Image = "guestlist.png";
				budgetbut.Image = "bbudget.png";
				suppliersbut.Image = "suppliers.png";
				ordersbut.Image = "orders.png";
				planningView.IsVisible = false;
				guestsView.IsVisible = false;
				budgetView.IsVisible = true;
				favoriteView.IsVisible = false;
			};
			suppliersbut.Clicked += async (s, e) => {
				checklistbut.Image = "checklist.png";
				guestbut.Image = "guestlist.png";
				budgetbut.Image = "budget.png";
				suppliersbut.Image = "bluesuppliers.png";
				ordersbut.Image = "orders.png";
				planningView.IsVisible = false;
				guestsView.IsVisible = false;
				budgetView.IsVisible = false;
				favoriteView.IsVisible = true;
			};
		

			// addtablebut.Clicked += (object sender, EventArgs e) =>
			//{
			//  popupaddtable.IsVisible = true;
			//};

			//addguestbut.Clicked += (object sender, EventArgs e) =>
			//{
			//  popupguest.IsVisible = true;
			//};
			//closepopuptable.Clicked += (object sender, EventArgs e) =>
			//{
			//  popupaddtable.IsVisible = false;
			//};
			//closepopupguest.Clicked += (object sender, EventArgs e) =>
			//{
			//  popupguest.IsVisible = false;
			//};
			allguest.Clicked += (object sender, EventArgs e) =>
			{
				allguest.Image = "ballguest.png";
				seatchart.Image = "seatingchart.png";
				allguestc.IsVisible = true;
				seatcharc.IsVisible = false;
			};
			seatchart.Clicked += (object sender, EventArgs e) =>
			{
				allguest.Image = "allguest.png";
				seatchart.Image = "bseatingchart.png";
				allguestc.IsVisible = false;
				seatcharc.IsVisible = true;
			};

			newtask.Clicked += async (object sender, EventArgs e) =>
			{
				await Navigation.PushModalAsync(new newtask(false, null));
			};
			addbudget.Clicked += async (object sender, EventArgs e) =>
			{
                var newexpense = new NewExpense(false, null);
                newexpense.OperationCompleted += Newexpense_OperationCompleted;
                await Navigation.PushModalAsync(newexpense);
			};



			// get tasks///
			gettasks();

			// get expenses in budget///
			
			getexpenses();

			getguests();

			allbut.Clicked += (object sender, EventArgs e) =>
            {
                //guestsView = new guests();
                checkList.IsVisible = true;
                todostack.IsVisible = false;
                donestack.IsVisible = false;

                //await Navigation.PushAsync(new guests());
            };

            todobut.Clicked += async (s, e) => {

                checkList.IsVisible = false;
                todostack.IsVisible = true;
                donestack.IsVisible = false;
            };
            donebut.Clicked += (object sender, EventArgs e) =>
            {
                checkList.IsVisible = false;
                todostack.IsVisible = false;
                donestack.IsVisible = true;

            };

        
            
        }
        

        private void Newexpense_OperationCompleted(object sender, EventArgs e)
        {
            getexpenses();
        }






        ///functions for task //
        public async void gettasks()
		{
            donestack.Children.Clear();
            todostack.Children.Clear();
            checkList.Children.Clear();
            WordpressService service = new WordpressService();
			cats = await service.GetItemCategoriesAsync();
			categories = cats.ToList();
			Connect con = new Connect();
			//await con.DownloadData("https://www.planmy.me/maizonpub-api/todolist.php", "action=get&todo_user=169");
			//var json = wc.DownloadString();
			string todostring = await con.DownloadData("https://www.planmy.me/maizonpub-api/todolist.php", "action=get&todo_user=169");
			List<todoobj> listoftodo = JsonConvert.DeserializeObject<List<todoobj>>(todostring);
			int numberttasks = listoftodo.Count;
			numbertotaltasks.Text = numberttasks.ToString();
			int donetaskscount = 0;
			foreach(todoobj o in listoftodo)
			{
				if (o.todo_read.ToString()=="1")
				{
					donetaskscount++;
				}
			}
			numbercompletedtasks.Text = donetaskscount.ToString();

			float division = donetaskscount / numberttasks;
			progress.Progress = division;

				IDictionary<todoobj, string> dictmonthtodo = new Dictionary<todoobj, string>();
			foreach (todoobj obj in listoftodo)
			{
				//int toid = Int32.Parse(obj.todo_id);
				DateTime dateTodo = DateTime.Parse(obj.todo_date);
				string monthName = dateTodo.ToString("MMM", CultureInfo.InvariantCulture);
				string year = dateTodo.Year.ToString();
				dictmonthtodo.Add(obj, monthName + " " + year);

			}

			foreach (var valuee in dictmonthtodo.Values.Distinct())
			{
				StackLayout donemonth=new StackLayout();
				 specifiedobj = dictmonthtodo.Where(item => item.Value == valuee).Select(item => item.Key).ToList();
				if (specifiedobj.Any(p => p.todo_read.ToString().Equals("1")))
				{
					 donemonth= createmonthstack(valuee);
				}
				foreach (todoobj o in specifiedobj)
				{
					StackLayout doneroww;
					string categoryo = "no category";
					int indexofequivalentcat = categories.FindIndex(a => a.Id.ToString() == o.todo_category.ToString());
					categoryo = categories.ElementAt(indexofequivalentcat).Name;
					if (o.todo_read.ToString() == "1")
					{
						if (o.is_priority.ToString() == "1")
						{
							doneroww = priorityrowdone(o.todo_title, categoryo);
						}
						else
						{
							doneroww = donerow(o.todo_title, categoryo);
						}
						doneroww.GestureRecognizers.Add(new TapGestureRecognizer
						{
							Command = new Command(() => {
                                var checlist = new checklist(o, categoryo);
                                checlist.OperationCompleted += Checlist_OperationCompleted;
                                Navigation.PushModalAsync(checlist);

                            } ),
						});
						donemonth.Children.Add(doneroww);
						donemonth.Children.Add(seperatorbetweenmonths());
					}
				}

				donestack.Children.Add(donemonth);


				//// for not done stack///
				///
				StackLayout notdonemonth = new StackLayout();
				
				if (specifiedobj.Any(p => p.todo_read.ToString().Equals("0")))
				{
					notdonemonth = createmonthstack(valuee);
				}


				foreach (todoobj o in specifiedobj)
				{
					StackLayout notdoneroww;
					string categoryo = "no category";
					int indexofequivalentcat = categories.FindIndex(a => a.Id.ToString() == o.todo_category.ToString());
					categoryo = categories.ElementAt(indexofequivalentcat).Name;
					if (o.todo_read.ToString() == "0")
					{
						if (o.is_priority.ToString() == "1")
						{
							notdoneroww = priorityrownotdone(o.todo_title, categoryo);
						}
						else
						{
							notdoneroww = notdonerow(o.todo_title, categoryo);
						}
						notdoneroww.GestureRecognizers.Add(new TapGestureRecognizer
						{
							Command = new Command(() => {
                                var chl = new checklist(o, categoryo);
                                chl.OperationCompleted += Checlist_OperationCompleted;
                                Navigation.PushModalAsync(chl);
                            }),
						});
						notdonemonth.Children.Add(notdoneroww);
						notdonemonth.Children.Add(seperatorbetweenmonths());
					}
				}

				todostack.Children.Add(notdonemonth);



				StackLayout month = createmonthstack(valuee);
			
				foreach (todoobj o in specifiedobj)
				{
				//forloop to fill the all stack without conditions///
					StackLayout row;

					string categoryo = "no category";
					int indexofequivalentcat = categories.FindIndex(a => a.Id.ToString() == o.todo_category.ToString());
					categoryo = categories.ElementAt(indexofequivalentcat).Name;

					if (o.todo_read.ToString() == "1")
					{
						if (o.is_priority.ToString() == "1")
						{
							row = priorityrowdone(o.todo_title, categoryo);
						}
						else
						{
							row = donerow(o.todo_title, categoryo);
						}
					}
					else 
					{
						if (o.is_priority.ToString() == "1")
						{
							row = priorityrownotdone(o.todo_title, categoryo);
						}
						else
						{
							row = notdonerow(o.todo_title, categoryo);
						}
					}

					//row = notdonerow(o.todo_title, categoryo);

					row.GestureRecognizers.Add(new TapGestureRecognizer
					{
						Command = new Command(() => {
                            var chl = new checklist(o, categoryo);
                                chl.OperationCompleted += Checlist_OperationCompleted;
                                Navigation.PushModalAsync(chl);
                        } ),
					});
					month.Children.Add(row);
					month.Children.Add(seperatorbetweenmonths());



				}
				checkList.Children.Add(month);
				

			}


		}

        private void Checlist_OperationCompleted(object sender, EventArgs e)
        {
            gettasks();
        }

        public StackLayout notdonerow(string titletxt, string descriptiontxt)
        {
            StackLayout stack = new StackLayout();
            stack.Orientation = StackOrientation.Horizontal;
            
            Image imgcheck = new Image();
            imgcheck.Source = "notchecked.png";
            imgcheck.Margin = new Thickness(10, 0, 0, 0);
            stack.Children.Add(imgcheck);

            StackLayout inverticalstack = new StackLayout();
            inverticalstack.Orientation = StackOrientation.Vertical;
            inverticalstack.Margin = new Thickness(15, 0, 0, 0);
            Label title = new Label();
            title.Text = titletxt;
            title.TextColor = Color.Black;
            title.FontAttributes = FontAttributes.Bold;
            title.FontSize = 14;
            inverticalstack.Children.Add(title);

            Label desc = new Label();
            desc.Text = descriptiontxt;
            desc.TextColor = Color.Gray;
            desc.FontAttributes = FontAttributes.Italic;
            desc.FontSize = 14;
            desc.Margin = new Thickness(0, -5, 0, 0);
            inverticalstack.Children.Add(desc);

            stack.Children.Add(inverticalstack);


            return stack;

        }
        public StackLayout donerow(string titletxt, string descriptiontxt)
        {
            StackLayout stack = new StackLayout();
            stack.Orientation = StackOrientation.Horizontal;
          
            Image imgcheck = new Image();
            imgcheck.Source = "checked.png";
            imgcheck.Margin = new Thickness(10, 0, 0, 0);
            stack.Children.Add(imgcheck);

            StackLayout inverticalstack = new StackLayout();
            inverticalstack.Orientation = StackOrientation.Vertical;
            inverticalstack.Margin = new Thickness(15, 0, 0, 0);
            Label title = new Label();
			title.Text = titletxt;
            title.TextColor = Color.Black;
            title.FontAttributes = FontAttributes.Bold;
            title.FontSize = 14;
            inverticalstack.Children.Add(title);

            Label desc = new Label();
            desc.Text = descriptiontxt;
            desc.TextColor = Color.Gray;
            desc.FontAttributes = FontAttributes.Italic;
            desc.FontSize = 14;
            desc.Margin = new Thickness(0, -5, 0, 0);
            inverticalstack.Children.Add(desc);

            stack.Children.Add(inverticalstack);


            return stack;

        }
        public StackLayout seperatorbetweenmonths()
        {
            StackLayout stack = new StackLayout();
            stack.Orientation = StackOrientation.Horizontal;
            stack.HeightRequest = 1;
            stack.BackgroundColor = Color.Black;
            return stack;

        }
		public StackLayout createmonthstack(string monthyear)
		{
			StackLayout stack = new StackLayout();
			stack.Orientation = StackOrientation.Vertical;
			//stack.Margin = new Thickness(15, 0, 15, 0);
			Label title = new Label();
			title.Text = "BY " + monthyear;
			title.TextColor = Color.Black;
			title.FontAttributes = FontAttributes.Bold;
			title.FontSize = 16;
			title.Margin = new Thickness(10, 0, 0, 0);
			stack.Children.Add(title);


			return stack;

		}
		public StackLayout priorityrownotdone(string titletxt, string descriptiontxt)
		{
			StackLayout stack = new StackLayout();
			stack.Orientation = StackOrientation.Horizontal;


			Image imgcheck = new Image();
			imgcheck.Source = "notchecked.png";
			stack.Children.Add(imgcheck);
			imgcheck.Margin = new Thickness(10, 0, 0, 0);

			StackLayout inverticalstack = new StackLayout();
			inverticalstack.Orientation = StackOrientation.Vertical;
			inverticalstack.Margin = new Thickness(15, 0, 0, 0);
			Label title = new Label();
			title.Text = titletxt;
			title.TextColor = Color.Black;
			title.FontAttributes = FontAttributes.Bold;
			title.FontSize = 14;
			inverticalstack.Children.Add(title);

			StackLayout inhorizontalstack = new StackLayout();
			inhorizontalstack.Orientation = StackOrientation.Horizontal;

			inverticalstack.Children.Add(inhorizontalstack);

			Label priority = new Label();
			priority.Text = "Priority";
			priority.TextColor = Color.DarkRed;
			priority.FontAttributes = FontAttributes.Italic;
			priority.Margin = new Thickness(0, -5, 0, 0);
			priority.FontSize = 14;
			//desc.Margin = new Thickness(0, -5, 0, 0);

			inhorizontalstack.Children.Add(priority);

			Label desc = new Label();
			desc.Text = descriptiontxt;
			desc.TextColor = Color.Gray;
			desc.FontAttributes = FontAttributes.Italic;
			desc.FontSize = 14;
			desc.Margin = new Thickness(0, -5, 0, 0);
			inhorizontalstack.Children.Add(desc);

			inverticalstack.HorizontalOptions = LayoutOptions.FillAndExpand;

			stack.Children.Add(inverticalstack);


			Image imgpriority = new Image();
			imgpriority.Source = "priority.png";
			imgpriority.HorizontalOptions = LayoutOptions.End;
			imgpriority.Margin = new Thickness(0, 0, 10, 0);

			stack.Children.Add(imgpriority);



			return stack;

		}
		public StackLayout priorityrowdone(string titletxt, string descriptiontxt)
		{
			StackLayout stack = new StackLayout();
			stack.Orientation = StackOrientation.Horizontal;


			Image imgcheck = new Image();
			imgcheck.Source = "checked.png";
			imgcheck.Margin = new Thickness(10, 0, 0, 0);
			stack.Children.Add(imgcheck);

			StackLayout inverticalstack = new StackLayout();
			inverticalstack.Orientation = StackOrientation.Vertical;
			inverticalstack.Margin = new Thickness(15, 0, 0, 0);
			Label title = new Label();
			title.Text = titletxt;
			title.TextColor = Color.Black;
			title.FontAttributes = FontAttributes.Bold;
			title.FontSize = 14;
			inverticalstack.Children.Add(title);

			StackLayout inhorizontalstack = new StackLayout();
			inhorizontalstack.Orientation = StackOrientation.Horizontal;

			inverticalstack.Children.Add(inhorizontalstack);

			Label priority = new Label();
			priority.Text = "Priority";
			priority.TextColor = Color.DarkRed;
			priority.FontAttributes = FontAttributes.Italic;
			priority.Margin = new Thickness(0, -5, 0, 0);
			priority.FontSize = 14;
			//desc.Margin = new Thickness(0, -5, 0, 0);

			inhorizontalstack.Children.Add(priority);

			Label desc = new Label();
			desc.Text = descriptiontxt;
			desc.TextColor = Color.Gray;
			desc.FontAttributes = FontAttributes.Italic;
			desc.FontSize = 14;
			desc.Margin = new Thickness(0, -5, 0, 0);
			inhorizontalstack.Children.Add(desc);

			inverticalstack.HorizontalOptions = LayoutOptions.FillAndExpand;

			stack.Children.Add(inverticalstack);


			Image imgpriority = new Image();
			imgpriority.Source = "priority.png";
			imgpriority.HorizontalOptions = LayoutOptions.End;
			imgpriority.Margin = new Thickness(0, 0, 10, 0);
			stack.Children.Add(imgpriority);



			return stack;

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
			string todostring = await con.DownloadData("http://planmy.me/maizonpub-api/budget_category.php", "action=get&category_user_id=169");
			List<expense> listofcats = JsonConvert.DeserializeObject<List<expense>>(todostring);

			foreach(expense e in listofcats)
			{
				StackLayout row = createtablerow(e.category_name);
                row.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    

                    Command = new Command(() => {
                        var catexpense = new Categorieexpense(e);
                        catexpense.OperationCompleted += Catexpense_OperationCompleted;
                        Navigation.PushModalAsync(catexpense);
                    } ),
				});
				//budgetstack.Children.Add(row);
				string todostringg = await con.DownloadData("https://planmy.me/maizonpub-api/budget.php", "action=get&category_id="+e.category_id+"&user_id=169");
				List<expenseforcat> listofexpenses = JsonConvert.DeserializeObject<List<expenseforcat>>(todostringg);

				foreach(expenseforcat ec in listofexpenses)
				{

					StackLayout rowexpense = createsupplierrowintable(ec.budget_list_name.ToString(),ec.budget_list_estimate_cost.ToString(),ec.budget_list_paid_cost.ToString());
						rowexpense.GestureRecognizers.Add(new TapGestureRecognizer
					{
                            
                    Command = new Command(() => {
                        var newexpense = new NewExpense(true, ec);
                        newexpense.OperationCompleted += Newexpense_OperationCompleted;
                        Navigation.PushModalAsync(newexpense);
                    }),
					});

					row.Children.Add(rowexpense);
					estimatedtotalamount = estimatedtotalamount + float.Parse(ec.budget_list_estimate_cost);
					actualtotalamount= actualtotalamount + float.Parse(ec.budget_list_actual_cost);
					paidtotalamount= paidtotalamount + float.Parse(ec.budget_list_paid_cost);
				}
				budgetstack.Children.Add(row);
				budgetstack.Children.Add(createseperatorbetweentables());
			}

			estimatedlabel.Text ="$"+" "+estimatedtotalamount.ToString();
			actuallabel.Text = "$" + " " + actualtotalamount.ToString();
			paidlabel.Text = "$" + " " + paidtotalamount.ToString();
			remaining = actualtotalamount - paidtotalamount;
			remaininglabel.Text = "Remaining $" + " " + remaining.ToString();
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



		public StackLayout createseperatorbetweentables()
		{

			StackLayout line = new StackLayout();
			line.Orientation = StackOrientation.Horizontal;
			line.HeightRequest = 5;
			line.BackgroundColor = Color.LightGray;
			line.HorizontalOptions = LayoutOptions.Fill;
			return line;

		}
		//fin functions for budget///
		public async void getguests()
		{
			Connect con = new Connect();
			string todostring = await con.DownloadData("https://planmy.me/maizonpub-api/guestlist.php", "action=get&userid=169");
			List<guest> listofguests = JsonConvert.DeserializeObject<List<guest>>(todostring);

			foreach(guest g in listofguests)
			{
				string status = "";
				if(g.RSVP=="Not Invited" || g.RSVP == "Declined")
				{
					status = "notattending.png";
				}
				if (g.RSVP == "No Response")
				{
					status = "pending2.png";
				}
				if (g.RSVP == "Accepted")
				{
					status = "attending.png";
				}

				StackLayout grow = createguestrow(g.guest_name,status);
				gueststack.Children.Add(grow);
			}
			
			
		}

		public StackLayout createguestrow(string guestname, string status)
		{
			StackLayout vlayout = new StackLayout();
			vlayout.Orientation = StackOrientation.Vertical;
			vlayout.IsVisible = true;

			StackLayout line = new StackLayout();
			line.Orientation = StackOrientation.Horizontal;
			line.HeightRequest = 1;
			line.BackgroundColor = Color.LightGray;
			line.HorizontalOptions = LayoutOptions.Fill;
			vlayout.Children.Add(line);

			StackLayout rowlayout = new StackLayout();
			rowlayout.Orientation = StackOrientation.Horizontal;
			rowlayout.Margin = new Thickness(15, 0, 15, 0);

			Label nameg = new Label();
			nameg.Text = guestname;
			nameg.FontSize = 16;
			nameg.TextColor = Color.Black;
			nameg.HorizontalOptions = LayoutOptions.StartAndExpand;
			rowlayout.Children.Add(nameg);

			Image img = new Image();
			img.Source = status;
			img.HorizontalOptions = LayoutOptions.End;
			img.VerticalOptions = LayoutOptions.Center;
			rowlayout.Children.Add(img);

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