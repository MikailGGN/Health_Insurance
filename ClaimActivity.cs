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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NewRelish
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar")]
    public class ClaimActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {
        IList<PlanProvider> hmoproviders;
        // GridView gridview;
        static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
        static readonly string ApplicationName = "Google Sheets API Quickstart";
        static readonly string spreadsheetId = "11BJ1je9-3vqusvdK2qa-IK-ay8BkCJcyuJYDp28z_j4";
        static readonly string sheet = "Provider";


        GoogleCredential credential;
        private int progressStatus = 0, progressStatus1 = 100;
        GridView gv;
        private Button BtnNext, BtnPrev;
        // private readonly Paginator p = new Paginator();
        // private const int Totalpages = Paginator.Total_NUM_Items / Paginator.Items_Per_Pages;
        // private int CurrentPage = 0;
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_claim);
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            TextView Bartext = FindViewById<TextView>(Resource.Id.textView_homeclm);
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


            //  this.initialize();
            // gv.Adapter = new ArrayAdapter(this, Resource.Layout.gridview1, p.GenratePage(CurrentPage));
            // gv.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, p.GeneratePage(CurrentPage));

            await Task.Run(() => LoadListItem());

            gv.ItemClick += (sender, e) =>
            {

                // Toast.MakeText(Application.Context,services[ e.Position].Name.ToString(), ToastLength.Short).Show();
                var p = new Intent(this, typeof(PlansActivity));
                p.PutExtra("ProvidersName", hmoproviders[e.Position].Provider.ToString());
                p.PutExtra("TheProvider", "HMO Provider");
                StartActivity(p);

            };
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
                    gv = FindViewById<GridView>(Resource.Id.gv);
                    Op_plansLoad();

                    gv.Adapter = new GridAdapter2(this, hmoproviders);

                });
            }).Wait();

            progress.Visibility = ViewStates.Invisible;

        }

        /*  private void initialize() {

              //REFERENCE VIEWS
              gv = FindViewById<GridView>(Resource.Id.gv);
              BtnNext = FindViewById<Button>(Resource.Id.BtnNext);
              BtnPrev = FindViewById<Button>(Resource.Id.BtnPrev);

              BtnPrev.Enabled = false;

              //BUTTON CLICKS
              BtnNext.Click += BtnNext_Click;
              BtnPrev.Click += BtnPrev_Click;
          }

          void BtnPrev_Click(object sender, System.EventArgs e)
          {
              CurrentPage -= 1;
              gv.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, p.GeneratePage(CurrentPage));
              toggleButtons();

          }
          void BtnNext_Click(object sender, System.EventArgs e)
          {

              CurrentPage += 1;
              gv.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, p.GeneratePage(CurrentPage));
              toggleButtons();
          }

          private void toggleButtons()
          {
              if (CurrentPage == Totalpages)
              {
                  BtnNext.Enabled = false;
                  BtnPrev.Enabled = true;
              }
              else
                  if (CurrentPage == 0)
              {
                  BtnPrev.Enabled = false;
                  BtnNext.Enabled = true;
              }
              else
                      if (CurrentPage >= 1 && CurrentPage <= 5)
              {
                  BtnNext.Enabled = true;
                  BtnPrev.Enabled = true;
              }

          }*/


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
        public void Op_plansLoad()/**/
        {


            hmoproviders = new List<PlanProvider>();/**/

            SheetsService service;


            credential = GoogleCredential.FromStream(Assets.Open("client_secret.json")).CreateScoped(Scopes);

            service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
            var range = $"{sheet}!A2:C";
            SpreadsheetsResource.ValuesResource.GetRequest request =
                   service.Spreadsheets.Values.Get(spreadsheetId, range);
            ValueRange response = request.Execute();
            IList<IList<Object>> values = response.Values;

            if (values != null && values.Count > 0)
            {

                try
                {

                    hmoproviders = (from IList<Object> row in values
                                    select new PlanProvider()
                                    {
                                        ProviderId = Convert.ToInt32(row[0]),
                                        Provider = row[1].ToString(),
                                        ProviderImage = row[2].ToString()

                                    }).ToList();


                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }


        }

    }

    class Paginator
    {
        public const int Total_NUM_Items = 52;
        public const int Items_Per_Pages = 10;
        public const int Items_Remaining = Total_NUM_Items % Items_Per_Pages;
        public const int Last_Page = Total_NUM_Items / Items_Per_Pages;

        public ArrayList GeneratePage(int CurrentPage)
        {
            int StartItem = CurrentPage * Items_Per_Pages + 1;
            const int NoData = Items_Per_Pages;
            ArrayList PageData = new ArrayList();

            if (CurrentPage == Last_Page && Items_Remaining > 0)
            {
                for (int i = StartItem; i < StartItem + Items_Remaining; i++)
                {

                    PageData.Add("Airtel,2");
                    PageData.Add("AxA Mansard,45");
                    PageData.Add("ISALU,2");
                    PageData.Add("Reliance,45");
                    PageData.Add("Teo,2");
                    PageData.Add("Avon,45");
                    PageData.Add("Swift,2");

                }
            }
            else
            {
                for (int i = StartItem; i < StartItem + NoData; i++)
                {
                    PageData.Add("Teo,2");
                    PageData.Add("Avon,45");
                    PageData.Add("Swift,2");
                    PageData.Add("Airtel,2");
                    PageData.Add("AxA Mansard,45");
                    PageData.Add("ISALU,2");
                    PageData.Add("Reliance,45");

                }
            }

            return PageData;
        }

    }
}

