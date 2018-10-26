using PlanMy.Views;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
		public checklist(todoobj task)
		{
			InitializeComponent();
			// for testing purposes ///
			// fill the data///

			title.Text = task.todo_title;
			category.Text = task.todo_category.ToString();
			description.Text = task.todo_details;
			DateTime dateTodo = DateTime.Parse(task.todo_date);
			string monthName = dateTodo.ToString("MMM", CultureInfo.InvariantCulture);
			string year = dateTodo.Year.ToString();

			date.Text = "By" + " " + monthName + " " + year;


			//action for delete task///
			deletetask.Clicked += async (object sender, EventArgs e) =>
			{
				using (var cl = new HttpClient())
				{
					var formcontent = new FormUrlEncodedContent(new[]
					{
			new KeyValuePair<string,string>("todo_id",task.todo_id),
			
		});


					var request = await cl.PostAsync("https://www.planmy.me/maizonpub-api/todolist.php?action=delete", formcontent);

					request.EnsureSuccessStatusCode();

					var response = await request.Content.ReadAsStringAsync();

					//jsonResponselogin res = JsonConvert.DeserializeObject<jsonResponselogin>(response);

					//lbl1.Text = res.code + " " + res.status + " " + res.message;

				}
			};

			///action for edit task///
			///
		
			edittask.Clicked += async (object sender, EventArgs e) =>
			{

				Navigation.PushAsync(new newtask(true, task));
			};

			pendingbut.Clicked += async (object sender, EventArgs e) =>
			{

				edittaskstatus(task,"1");
			};


			searchsuppliers.GestureRecognizers.Add(
	new TapGestureRecognizer()
	{
		Command = new Command(() => { Navigation.PushModalAsync(new Vendors());})
	});
			for(int i = 0; i < 6; i++)
			{
				if (i == 0) {
					StackLayout firststack = createsupplierverticalstack("Elie Saab", "champagne.png");
					firststack.Margin = new Thickness(15, 0, 0, 0);
					Suppliersstack.Children.Add(firststack);
						}
				else
				{
					Suppliersstack.Children.Add(createsupplierverticalstack("Elie Saab", "champagne.png"));
				}
			}
			

		}
		public StackLayout createsupplierverticalstack(string nametxt,string imgurl)
		{
			StackLayout stack = new StackLayout();
			stack.Orientation = StackOrientation.Vertical;
			Image img = new Image();
			img.Source = imgurl;
			Label name = new Label();
			name.Text = nametxt;
			stack.Children.Add(img);
			stack.Children.Add(name);
			return stack;
		}

		// function for edit task status///
		public async void edittaskstatus(todoobj task, string statusval)
		{

			using (var cl = new HttpClient())
			{
				var formcontent = new FormUrlEncodedContent(new[]
				{
			new KeyValuePair<string,string>("todo_id",task.todo_id),
			new KeyValuePair<string, string>("todo_title",task.todo_title),
				new KeyValuePair<string,string>("todo_details",task.todo_details),
			new KeyValuePair<string, string>("todo_date",task.todo_date),
			new KeyValuePair<string,string>("todo_read",statusval),
			new KeyValuePair<string, string>("todo_category",task.todo_category.ToString()),
			new KeyValuePair<string, string>("is_priority",task.is_priority.ToString())
		});


				var request = await cl.PostAsync("https://www.planmy.me/maizonpub-api/todolist.php?action=update", formcontent);

				request.EnsureSuccessStatusCode();

				var response = await request.Content.ReadAsStringAsync();

				Navigation.PushModalAsync(new Planning());

			}
		}

	}


}