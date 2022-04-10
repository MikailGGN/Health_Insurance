using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.Linq;
//using Bundle = Android.OS.Bundle;


using System.Threading;
using System.Threading.Tasks;

namespace NewRelish
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar")]
    class PlansActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {


        IList<HealthPlan> hmoplans;
        GridView gridview;
        static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
        static readonly string ApplicationName = "Google Sheets API Quickstart";
        static readonly string spreadsheetId = "11BJ1je9-3vqusvdK2qa-IK-ay8BkCJcyuJYDp28z_j4";
        static readonly string sheet = "Plan";


        GoogleCredential credential;
        private int progressStatus = 0, progressStatus1 = 100;
        private Dialog popupDialog;
        string x, y;
        protected override async void OnCreate(Bundle savedInstanceState)
        {


            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_plans);

            x = Intent.GetStringExtra("ProvidersName") ?? null;
            y = Intent.GetStringExtra("TheProvider") ?? null;

            Toast.MakeText(this,
           "ProvidersName is" + x + "and TheProvider " + y, ToastLength.Long).Show();
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            TextView Bartext = FindViewById<TextView>(Resource.Id.textView_homepln);
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

            textView1.Text = "*** Plan";

            var userimg = header.FindViewById<ImageView>(Resource.Id.imageView);

            userimg.Click += delegate
            {
                StartActivity(new Android.Content.Intent(this, typeof(ReferalActivity)));
            };

            var customplan = FindViewById<Button>(Resource.Id.button_pl);
            customplan.Click += delegate
            {
                StartActivity(new Android.Content.Intent(this, typeof(CustPlan_Activity)));
            };

            await Task.Run(() => LoadListItem());

            gridview.ItemClick += (sender, e) =>
            {

                // Toast.MakeText(Application.Context,services[ e.Position].Name.ToString(), ToastLength.Short).Show();
                var p = new Intent(this, typeof(PaymentActivity));
                p.PutExtra("ServiceName", hmoplans[e.Position].PlanName.ToString());
                p.PutExtra("TheService", "HMO Plan");
                StartActivity(p);

            };


        }

        void LoadListItem()
        {


            ProgressBar progress = FindViewById<ProgressBar>(Resource.Id.circularProgressbar);
            Task.Run(async () =>
            {


                progress.Visibility = ViewStates.Visible;
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
                    gridview = FindViewById<GridView>(Resource.Id.gridView1);
                    Op_plansLoad();
                    gridview.Adapter = new GridAdapter(this, hmoplans);

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
            Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
                .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            int id = item.ItemId;

            if (id == Resource.Id.nav_subscription)
            {
                StartActivity(new Android.Content.Intent(this, typeof(SubscriptionActivity)));

            }
            else if (id == Resource.Id.nav_referal)
            {
                StartActivity(new Android.Content.Intent(this, typeof(PrescriptionActivity)));
            }
            else if (id == Resource.Id.nav_claim)
            {
                StartActivity(new Android.Content.Intent(this, typeof(ClaimActivity)));
            }
            else if (id == Resource.Id.nav_manage)
            {
                StartActivity(new Android.Content.Intent(this, typeof(PlansActivity)));
            }
            else if (id == Resource.Id.nav_remit)
            {
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

        public void Op_plansLoad()/**/
        {


            hmoplans = new List<HealthPlan>();/**/

            SheetsService service;


            credential = GoogleCredential.FromStream(Assets.Open("client_secret.json")).CreateScoped(Scopes);

            service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
            var range = $"{sheet}!A2:D";
            SpreadsheetsResource.ValuesResource.GetRequest request =
                   service.Spreadsheets.Values.Get(spreadsheetId, range);
            ValueRange response = request.Execute();
            IList<IList<Object>> values = response.Values;

            if (values != null && values.Count > 0)
            {

                try
                {
                    if (string.IsNullOrEmpty(x))
                    {
                        hmoplans = (from IList<Object> row in values
                                    select new HealthPlan()
                                    {
                                        PlanId = Convert.ToInt32(row[0]),
                                        PlanName = row[1].ToString(),
                                        PlanProvider = row[2].ToString()

                                    }).ToList();
                    }
                    else
                    {

                        hmoplans = (from IList<Object> row in values
                                    where row[2].ToString() == x
                                    select new HealthPlan()
                                    {
                                        PlanId = Convert.ToInt32(row[0]),
                                        PlanName = row[1].ToString(),
                                        PlanProvider = row[2].ToString()

                                    }).ToList();
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }


        }
    }
}