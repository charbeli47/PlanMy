using Newtonsoft.Json;
using PlanMy.Library;
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
	public partial class CheckListView : ContentView
	{
        public List<VendorCategory> categories = new List<VendorCategory>();
        public IEnumerable<VendorCategory> cats;
        List<CheckList> specifiedobj = new List<CheckList>();
        public CheckListView()
		{
			InitializeComponent ();
            newtask.Clicked += async (object sender, EventArgs e) =>
            {
                var ntask = new newtask(false, null);
                ntask.OperationCompleted += (s, ev) => {
                    gettasks();
                };
                await Navigation.PushModalAsync(ntask);
            };
            // get tasks///
            gettasks();
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
        public async void gettasks()
        {
            var usr = await GetUser();
            donestack.Children.Clear();
            todostack.Children.Clear();
            checkList.Children.Clear();
            WebClient client = new WebClient();
            var resp = client.DownloadString(Statics.apiLink + "Categories");
            cats = Newtonsoft.Json.JsonConvert.DeserializeObject<List<VendorCategory>>(resp);
            categories = cats.ToList();
            Connect con = new Connect();
            //await con.DownloadData("https://www.planmy.me/maizonpub-api/todolist.php", "action=get&todo_user=169");
            //var json = wc.DownloadString();
            if (usr != null)
            {
                string todostring = await con.DownloadData(Statics.apiLink+"CheckLists", "UserId=" + usr.Id);
                List<CheckList> listoftodo = JsonConvert.DeserializeObject<List<CheckList>>(todostring);
                int numberttasks = listoftodo.Count;
                numbertotaltasks.Text = numberttasks.ToString();
                int donetaskscount = 0;
                foreach (CheckList o in listoftodo)
                {
                    if (o.Status == CheckListStatus.Done)
                    {
                        donetaskscount++;
                    }
                }
                numbercompletedtasks.Text = donetaskscount.ToString();
                float division = 0;
                if (numberttasks != 0)
                    division = donetaskscount / numberttasks;
                progress.Progress = division;

                IDictionary<CheckList, string> dictmonthtodo = new Dictionary<CheckList, string>();
                foreach (CheckList obj in listoftodo)
                {
                    //int toid = Int32.Parse(obj.todo_id);
                    DateTime dateTodo = obj.Timing;
                    string monthName = dateTodo.ToString("MMM", CultureInfo.InvariantCulture);
                    string year = dateTodo.Year.ToString();
                    dictmonthtodo.Add(obj, monthName + " " + year);

                }

                foreach (var valuee in dictmonthtodo.Values.Distinct())
                {
                    StackLayout donemonth = new StackLayout();
                    specifiedobj = dictmonthtodo.Where(item => item.Value == valuee).Select(item => item.Key).ToList();
                    if (specifiedobj.Any(p => p.Status.Equals(CheckListStatus.Done)))
                    {
                        donemonth = createmonthstack(valuee);
                    }
                    foreach (CheckList o in specifiedobj)
                    {
                        StackLayout doneroww;
                        string categoryo = "no category";
                        int indexofequivalentcat = categories.FindIndex(a => a.Id == o.VendorCategoryId);
                        categoryo = categories.ElementAt(indexofequivalentcat).Title;
                        if (o.Status == CheckListStatus.Done)
                        {
                            if (o.IsPriority)
                            {
                                doneroww = priorityrowdone(o.Title, categoryo);
                            }
                            else
                            {
                                doneroww = donerow(o.Title, categoryo);
                            }
                            doneroww.GestureRecognizers.Add(new TapGestureRecognizer
                            {
                                Command = new Command(() =>
                                {
                                    var checlist = new checklist(o, categoryo);
                                    checlist.OperationCompleted += Checlist_OperationCompleted;
                                    Navigation.PushModalAsync(checlist);

                                }),
                            });
                            donemonth.Children.Add(doneroww);
                            donemonth.Children.Add(seperatorbetweenmonths());
                        }
                    }

                    donestack.Children.Add(donemonth);


                    //// for not done stack///
                    ///
                    StackLayout notdonemonth = new StackLayout();

                    if (specifiedobj.Any(p => p.Status.Equals(CheckListStatus.ToDo)))
                    {
                        notdonemonth = createmonthstack(valuee);
                    }


                    foreach (CheckList o in specifiedobj)
                    {
                        StackLayout notdoneroww;
                        string categoryo = "no category";
                        int indexofequivalentcat = categories.FindIndex(a => a.Id == o.VendorCategoryId);
                        categoryo = categories.ElementAt(indexofequivalentcat).Title;
                        if (o.Status == CheckListStatus.ToDo)
                        {
                            if (o.IsPriority)
                            {
                                notdoneroww = priorityrownotdone(o.Title, categoryo);
                            }
                            else
                            {
                                notdoneroww = notdonerow(o.Title, categoryo);
                            }
                            notdoneroww.GestureRecognizers.Add(new TapGestureRecognizer
                            {
                                Command = new Command(() =>
                                {
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

                    foreach (CheckList o in specifiedobj)
                    {
                        //forloop to fill the all stack without conditions///
                        StackLayout row;

                        string categoryo = "no category";
                        int indexofequivalentcat = categories.FindIndex(a => a.Id == o.VendorCategoryId);
                        categoryo = categories.ElementAt(indexofequivalentcat).Title;

                        if (o.Status == CheckListStatus.Done)
                        {
                            if (o.IsPriority)
                            {
                                row = priorityrowdone(o.Title, categoryo);
                            }
                            else
                            {
                                row = donerow(o.Title, categoryo);
                            }
                        }
                        else
                        {
                            if (o.IsPriority)
                            {
                                row = priorityrownotdone(o.Title, categoryo);
                            }
                            else
                            {
                                row = notdonerow(o.Title, categoryo);
                            }
                        }

                        //row = notdonerow(o.todo_title, categoryo);

                        row.GestureRecognizers.Add(new TapGestureRecognizer
                        {
                            Command = new Command(() =>
                            {
                                var chl = new checklist(o, categoryo);
                                chl.OperationCompleted += Checlist_OperationCompleted;
                                Navigation.PushModalAsync(chl);
                            }),
                        });
                        month.Children.Add(row);
                        month.Children.Add(seperatorbetweenmonths());



                    }
                    checkList.Children.Add(month);


                }
            }

        }
    }
}