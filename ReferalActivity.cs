using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using System;
//using Bundle = Android.OS.Bundle;
using System.Threading;
using System.Threading.Tasks;

namespace NewRelish
{

    [Activity(Theme = "@style/AppTheme.NoActionBar")]
    public class ReferalActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {

        /* IList<Subscriber> subscriber;
        GridView gridview;
        static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
        static readonly string ApplicationName = "Google Sheets API Quickstart";
        static readonly string spreadsheetId = "11BJ1je9-3vqusvdK2qa-IK-ay8BkCJcyuJYDp28z_j4";
        static readonly string sheet = "Subscriber";

        GoogleCredential credential;*/

        TextView _dateDisplay;
        Button _dateSelectButton;

        private int progressStatus = 0, progressStatus1 = 100;
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_referal);

            // Create your application 
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            TextView Bartext = FindViewById<TextView>(Resource.Id.textView_homeref);
            Bartext.Text = "eltCare";
            Bartext.TextSize = 20;
            Bartext.TextAlignment = TextAlignment.TextStart;
            Bartext.Click += delegate
            {
                // Toast.MakeText(this,
                //"Button in ToolBar clicked",ToastLength.Long).Show();
                StartActivity(new Android.Content.Intent(this, typeof(MainActivity)));
            };


            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            drawer.AddDrawerListener(toggle);
            toggle.SyncState();

            NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);

            LinearLayout header = (LinearLayout)navigationView.GetHeaderView(0);
            var textView = header.FindViewById<TextView>(Resource.Id.textView);

            textView.Text = "Lastname, Firstname ";
            textView.Click += delegate
            {
                StartActivity(new Android.Content.Intent(this, typeof(ReferalActivity)));
            };

            var textView1 = header.FindViewById<TextView>(Resource.Id.textViewTitle);

            textView1.Text = "***";

            var userimg = header.FindViewById<ImageView>(Resource.Id.imageView);

            userimg.Click += delegate
            {
                StartActivity(new Android.Content.Intent(this, typeof(ReferalActivity)));
            };


            _dateDisplay = FindViewById<TextView>(Resource.Id.date_display);
            _dateSelectButton = FindViewById<Button>(Resource.Id.date_select_button);
            _dateSelectButton.Click += DateSelect_OnClick;


            // await Task.Run(() => LoadListItem());


        }
        void LoadListItem()
        {
            ProgressBar progress = FindViewById<ProgressBar>(Resource.Id.circularProgressbar);
            Task.Run(async () =>
            {

                //progress.Visibility = ViewStates.Visible;
                progress.Progress = 100;
                progress.SecondaryProgress = 100;
                new System.Threading.Thread(new ThreadStart(delegate
                {
                    while (progressStatus < 100)
                    {
                        progressStatus += 1;
                        progressStatus1 -= 1;
                        progress.Progress = progressStatus1;
                        System.Threading.Thread.Sleep(100);
                    }
                })).Start();

                // 

                // Console.WriteLine($"Are we on the UI thread? {Looper.MainLooper.Thread == Looper.MyLooper()?.Thread}");
                // Simulate a long running task
                await Task.Delay(TimeSpan.FromSeconds(10));

                RunOnUiThread(() =>
                {
                    //release below void to test mobile
                    // Op_subsLoad();
                    // load subscriber

                });
            }).Wait();

            progress.Visibility = ViewStates.Invisible;

        }
        public override void OnBackPressed()
        {
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            if (drawer.IsDrawerOpen(GravityCompat.Start))
            {
                drawer.CloseDrawer(GravityCompat.Start);
            }
            else
            {
                base.OnBackPressed();
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View)sender;
            //    Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
            //     .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();

            //Chat
            StartActivity(new Android.Content.Intent(this, typeof(ChatActivity)));
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            int id = item.ItemId;

            if (id == Resource.Id.nav_subscription)
            {
                //Subscription
                StartActivity(new Android.Content.Intent(this, typeof(SubscriptionActivity)));

            }
            else if (id == Resource.Id.nav_referal)
            {//Prescription
                StartActivity(new Android.Content.Intent(this, typeof(PrescriptionActivity)));
            }
            else if (id == Resource.Id.nav_claim)
            {//Provider
                StartActivity(new Android.Content.Intent(this, typeof(ClaimActivity)));
            }
            else if (id == Resource.Id.nav_manage)
            {//Plans
                StartActivity(new Android.Content.Intent(this, typeof(PlansActivity)));
            }
            else if (id == Resource.Id.nav_remit)
            {//Payments
                StartActivity(new Android.Content.Intent(this, typeof(PaymentActivity)));
            }
            else if (id == Resource.Id.nav_share)
            {

            }
            // else if (id == Resource.Id.nav_send)  { }


            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            drawer.CloseDrawer(GravityCompat.Start);
            return true;
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        void DateSelect_OnClick(object sender, EventArgs eventArgs)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                _dateDisplay.Text = time.ToLongDateString();
            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }
        //test with phone
        /*  public void Op_subsLoad()
          {
              string pnumber = null;
              TelephonyManager tm;

              tm = (TelephonyManager)Application.Context.GetSystemService(TelephonyService);

              // pnumber = tm.Line1Number;
              pnumber = "08023000001";
              subscriber = new List<Subscriber>();

              SheetsService service;


              credential = GoogleCredential.FromStream(Assets.Open("client_secret.json")).CreateScoped(Scopes);
              service = new SheetsService(new BaseClientService.Initializer()
              {
                  HttpClientInitializer = credential,
                  ApplicationName = ApplicationName,
              });
              var range = $"{sheet}!A2:G";
              SpreadsheetsResource.ValuesResource.GetRequest request =
                     service.Spreadsheets.Values.Get(spreadsheetId, range);
              ValueRange response = request.Execute();
              IList<IList<Object>> values = response.Values;

              if (values != null && values.Count > 0)
              {

                  try
                  {

                      subscriber = (IList<Subscriber>)(from IList<Object> row in values

                                    select new Subscriber()
                                    {
                                        Mobilenumber = row[0].ToString(),
                                        FirstName = row[1].ToString(),
                                        LastName = row[2].ToString(),
                                        Dob = row[3].ToString(),
                                        Sex = row[4].ToString(),
                                        Height = Convert.ToDecimal( row[5]),
                                         Weight = Convert.ToDecimal(row[6]),
                                         photo = row[7].ToString()
                                    }).ToList().Where(s =>
                                    {
                                        bool v = s.Mobilenumber == pnumber;
                                        return v;
                                    });


                  }
                  catch (Exception ex)
                  {
                      Console.WriteLine(ex.Message);
                  }
              }


          }*/
    }
}