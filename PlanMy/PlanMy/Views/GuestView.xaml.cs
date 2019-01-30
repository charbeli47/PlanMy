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
	public partial class GuestView : ContentView
	{
        List<GuestList> listofguests = new List<GuestList>();
        public GuestView ()
		{
			InitializeComponent ();
            addtablebut.Clicked += async (object sender, EventArgs e) =>
            {
                var nt = new newtable(false, null);
                nt.OperationCompleted += (s, ev) => {
                    gettables();
                };

                await Navigation.PushModalAsync(nt);
            };

            addguestbut.Clicked += async (object sender, EventArgs e) =>
            {
                var ng = new newguest(false, null);
                ng.OperationCompleted += (s, ev) => {
                    getguests();
                };
                await Navigation.PushModalAsync(ng);
            };
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
                gueststack.IsVisible = true;
                seatstack.IsVisible = false;
            };
            seatchart.Clicked += (object sender, EventArgs e) =>
            {
                allguest.Image = "allguest.png";
                seatchart.Image = "bseatingchart.png";
                allguestc.IsVisible = false;
                seatcharc.IsVisible = true;
                gueststack.IsVisible = false;
                seatstack.IsVisible = true;
            };
            getguests();
            // on picker rsp change//
            RspPicker.SelectedIndexChanged += this.myPickerSelectedIndexChanged;
            gettables();
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
            gueststack.Children.Clear();
            var usr = await GetUser();
            Connect con = new Connect();
            if (usr != null)
            {
                string todostring = await con.DownloadData(Statics.apiLink+ "GuestLists", "UserId=" + usr.Id);
                listofguests = JsonConvert.DeserializeObject<List<GuestList>>(todostring);

                foreach (GuestList g in listofguests)
                {

                    string status = "";
                    if (g.GuestStatus == GuestStatus.Not_Invited || g.GuestStatus == GuestStatus.Declined)
                    {
                        status = "notattending.png";
                    }
                    if (g.GuestStatus == GuestStatus.No_Response)
                    {
                        status = "pending2.png";
                    }
                    if (g.GuestStatus == GuestStatus.Accepted)
                    {
                        status = "attending.png";
                    }

                    StackLayout grow = createguestrow(g.FullName, status);
                    grow.GestureRecognizers.Add(new TapGestureRecognizer
                    {
                        Command = new Command(async () =>
                        {
                            var ng = new newguest(true, g);
                            ng.OperationCompleted += (s, e) => { getguests(); };
                            await Navigation.PushModalAsync(ng);
                        }),
                    });
                    gueststack.Children.Add(grow);

                }
            }

        }
        public void myPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            gueststack.Children.Clear();
            int statusguest = RspPicker.SelectedIndex;
            foreach (GuestList gt in listofguests)
            {
                if ((int)gt.GuestStatus == statusguest)

                {
                    string status = "";
                    if (gt.GuestStatus == GuestStatus.Not_Invited || gt.GuestStatus == GuestStatus.Declined)
                    {
                        status = "notattending.png";
                    }
                    if (gt.GuestStatus == GuestStatus.No_Response)
                    {
                        status = "pending2.png";
                    }
                    if (gt.GuestStatus == GuestStatus.Accepted)
                    {
                        status = "attending.png";
                    }
                    StackLayout rowg = createguestrow(gt.FullName, status);
                    rowg.GestureRecognizers.Add(new TapGestureRecognizer
                    {
                        Command = new Command(async () => {
                            var ng = new newguest(true, gt);
                            ng.OperationCompleted += (s, ev) => {
                                getguests();
                            };
                            await Navigation.PushModalAsync(ng);
                        }),
                    });
                    gueststack.Children.Add(rowg);
                }
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


        public async void gettables()
        {
            seatstack.Children.Clear();
            Connect con = new Connect();
            var usr = await GetUser();
            string todostring = await con.DownloadData(Statics.apiLink+ "GuestListTables", "UserId=" + usr.Id);
            List<GuestListTables> listoftables = JsonConvert.DeserializeObject<List<GuestListTables>>(todostring);

            foreach (GuestListTables t in listoftables)
            {

                string rowstring = await con.DownloadData(Statics.apiLink+"GuestsList", "UserId=" + usr.Id + "&tableId=" + t.Id);
                List<GuestList> listofguesttable = JsonConvert.DeserializeObject<List<GuestList>>(rowstring);
                StackLayout tablerow = createtableguestrow(t, t.Title, listofguesttable.Count.ToString());
                foreach (GuestList gt in listofguesttable)
                {
                    string status = "";
                    if (gt.GuestStatus == GuestStatus.Not_Invited || gt.GuestStatus == GuestStatus.Declined)
                    {
                        status = "notattending.png";
                    }
                    if (gt.GuestStatus == GuestStatus.No_Response)
                    {
                        status = "pending2.png";
                    }
                    if (gt.GuestStatus == GuestStatus.Accepted)
                    {
                        status = "attending.png";
                    }

                    StackLayout guest = createguestrowintable(gt.FullName, status);
                    tablerow.Children.Add(guest);
                }

                seatstack.Children.Add(tablerow);
                seatstack.Children.Add(createseperatorbetweentables());
            }
        }
        public StackLayout createtableguestrow(GuestListTables t, string namet, string numberguests)
        {
            StackLayout tablelayout = new StackLayout();
            tablelayout.Orientation = StackOrientation.Vertical;

            StackLayout rowlayout = new StackLayout();
            rowlayout.Orientation = StackOrientation.Horizontal;
            rowlayout.Margin = new Thickness(15, 0, 15, 0);

            Button plusimg = new Button();
            plusimg.Image = "plus.png";
            plusimg.BackgroundColor = Color.Transparent;
            plusimg.HorizontalOptions = LayoutOptions.Start;
            plusimg.Margin = new Thickness(0, 0, 25, 0);
            plusimg.VerticalOptions = LayoutOptions.Center;
            plusimg.Clicked += (s, e) => {
                var addGuestTable = new AddGuestToTable(t);
                addGuestTable.OperationCompleted += AddGuestTable_OperationCompleted; ;
                Navigation.PushModalAsync(addGuestTable);
            };
            rowlayout.Children.Add(plusimg);

            StackLayout vlayout = new StackLayout();
            vlayout.Orientation = StackOrientation.Vertical;

            Label nametable = new Label();
            nametable.Text = namet;
            nametable.FontSize = 18;
            nametable.TextColor = Color.Black;
            nametable.GestureRecognizers
.Add(new TapGestureRecognizer
{
    Command = new Command(() => {
        var newtable = new newtable(true, t);
        newtable.OperationCompleted += (s, ev) => { gettables(); };
        Navigation.PushModalAsync(newtable);

    }),
}); Label numberg = new Label();
            numberg.Text = numberguests + " guests";
            numberg.FontSize = 12;
            numberg.FontAttributes = FontAttributes.Italic;
            numberg.TextColor = Color.Black;
            numberg.Margin = new Thickness(0, -5, 0, 0);

            vlayout.Children.Add(nametable);
            vlayout.Children.Add(numberg);
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

        private void AddGuestTable_OperationCompleted(object sender, EventArgs e)
        {
            gettables();
        }

        public StackLayout createguestrowintable(string guestname, string status)
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

            StackLayout rowlayout = new StackLayout();
            rowlayout.Orientation = StackOrientation.Horizontal;
            rowlayout.Margin = new Thickness(15, 0, 15, 0);

            Image plusimg = new Image();
            plusimg.Source = "guesttable.png";
            plusimg.HorizontalOptions = LayoutOptions.Start;
            plusimg.Margin = new Thickness(0, 0, 25, 0);
            plusimg.VerticalOptions = LayoutOptions.Center;
            rowlayout.Children.Add(plusimg);



            Label nameg = new Label();
            nameg.Text = guestname;
            nameg.FontSize = 16;
            nameg.TextColor = Color.Black;
            nameg.HorizontalOptions = LayoutOptions.FillAndExpand;
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
    }
}