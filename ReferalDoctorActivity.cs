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
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NewRelish
{

    [Activity(Theme = "@style/AppTheme.NoActionBar")]
    public class ReferalDoctorActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {
        TextView timeDisplay, _dateDisplay;
        Button timeSelectButton, _dateSelectButton;
        /* IList<Subscriber> subscriber;
         GridView gridview;
         static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
         static readonly string ApplicationName = "Google Sheets API Quickstart";
         static readonly string spreadsheetId = "11BJ1je9-3vqusvdK2qa-IK-ay8BkCJcyuJYDp28z_j4";
         static readonly string sheet = "Subscriber";

         GoogleCredential credential;*/
        private int progressStatus = 0, progressStatus1 = 100;
        IList<HealthStates> states;
        IList<HealthLgas> lgas;
        private List<string> Mlist;
        Spinner sp, spp;
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_referal_doctor);
            Load_State();
            Load_Lga();
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


            timeDisplay = FindViewById<TextView>(Resource.Id.time_display3);
            timeSelectButton = FindViewById<Button>(Resource.Id.select_button3);
            timeSelectButton.Click += TimeSelectOnClick;

            _dateDisplay = FindViewById<TextView>(Resource.Id.date_display3);
            _dateSelectButton = FindViewById<Button>(Resource.Id.date_select_button3);
            _dateSelectButton.Click += DateSelect_OnClick;

            sp = FindViewById<Spinner>(Resource.Id.spdc1);

            ArrayAdapter adapter = new ArrayAdapter(this, Resource.Layout.spinitem, Mlist);

            adapter.SetDropDownViewResource(Resource.Layout.support_simple_spinner_dropdown_item);
            sp.Adapter = adapter;

            sp.ItemSelected += sp_ItemSelected;


            spp = FindViewById<Spinner>(Resource.Id.spdcL);


            var LgaList = lgas.Select(r => r.Lga.ToString())
                         .ToArray();
            ArrayAdapter adptr = new ArrayAdapter(this, Resource.Layout.spinitem, LgaList);

            adptr.SetDropDownViewResource(Resource.Layout.support_simple_spinner_dropdown_item);


            spp.Adapter = adptr;
            spp.ItemSelected += spp_ItemSelected;

            //await Task.Run(() => LoadListItem());


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

        void TimeSelectOnClick(object sender, EventArgs eventArgs)
        {
            TimePickerFragment frag = TimePickerFragment.NewInstance(
                delegate (DateTime time)
                {
                    timeDisplay.Text = time.ToShortTimeString();
                });

            frag.Show(FragmentManager, TimePickerFragment.TAG);
        }

        void DateSelect_OnClick(object sender, EventArgs eventArgs)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                _dateDisplay.Text = time.ToLongDateString();
            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }
        void sp_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            try
            {

                //  services.Where(i => i.State == Mlist[e.Position].ToString()).ToList();
                //  myList.Adapter = new ServiceListAdapter(this, services.Where(i => i.State == Mlist[e.Position].ToString()).ToList());

                //Local govt

                var Lga_List = lgas.Where(r => r.State == Mlist[e.Position].ToString()).Select(r => r.Lga.ToString()).ToArray();
                ArrayAdapter adptr = new ArrayAdapter(this, Resource.Layout.spinitem, Lga_List);
                adptr.SetDropDownViewResource(Resource.Layout.support_simple_spinner_dropdown_item);
                spp.Adapter = adptr;


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        void spp_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            try
            {
                // if (services != null || services.Count > 0)
                //{

                //  myList.Adapter = new ServiceListAdapter(this, services.Where(i => i.Lga == Mlist[e.Position].ToString()).ToList());
                // }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        void Load_State()
        {
            Mlist = new List<string>();


            Mlist.Add("Select State");
            Mlist.Add("Borno");
            Mlist.Add("FCT");
            Mlist.Add("Akwa Ibom");
            Mlist.Add("Ebonyi");
            Mlist.Add("Abia");
            Mlist.Add("Ogun");
            Mlist.Add("Cross River");
            Mlist.Add("Imo");
            Mlist.Add("Rivers");
            Mlist.Add("Kogi");
            Mlist.Add("Ekiti");
            Mlist.Add("Oyo");
            Mlist.Add("Niger");
            Mlist.Add("Benue");
            Mlist.Add("Lagos");
            Mlist.Add("Anambra");
            Mlist.Add("Kano");
            Mlist.Add("Gombe");
            Mlist.Add("Edo");
            Mlist.Add("Ondo");
            Mlist.Add("Nasarawa");
            Mlist.Add("Kebbi");
            Mlist.Add("Bauchi");
            Mlist.Add("Enugu");
            Mlist.Add("Delta");
            Mlist.Add("Zamfara");
            Mlist.Add("Taraba");
            Mlist.Add("Kwara");
            Mlist.Add("Osun");
            Mlist.Add("Jigawa");
            Mlist.Add("Plateau");
            Mlist.Add("Katsina");
            Mlist.Add("Yobe");
            Mlist.Add("Sokoto");
            Mlist.Add("Kaduna");
            Mlist.Add("Bayelsa");
            Mlist.Add("Adamawa");
        }
        void Load_Lga()
        {

            lgas = new List<HealthLgas>();
            // Add parts to the list.
            lgas.Add(new HealthLgas() { State = "Select State", Lga = "Select Local Govt." });
            lgas.Add(new HealthLgas() { State = "Borno", Lga = "Abadam" });
            lgas.Add(new HealthLgas() { State = "FCT", Lga = "Abaji" });
            lgas.Add(new HealthLgas() { State = "Akwa Ibom", Lga = "Abak" });
            lgas.Add(new HealthLgas() { State = "Ebonyi", Lga = "Abakaliki" });
            lgas.Add(new HealthLgas() { State = "Abia", Lga = "Aba North" });
            lgas.Add(new HealthLgas() { State = "Abia", Lga = "Aba South" });
            lgas.Add(new HealthLgas() { State = "Ogun", Lga = "Abeokuta North" });
            lgas.Add(new HealthLgas() { State = "Ogun", Lga = "Abeokuta South" });
            lgas.Add(new HealthLgas() { State = "Cross River", Lga = "Abi" });
            lgas.Add(new HealthLgas() { State = "Imo", Lga = "Aboh Mbaise" });
            lgas.Add(new HealthLgas() { State = "Rivers", Lga = "Abua/Odual" });
            lgas.Add(new HealthLgas() { State = "Kogi", Lga = "Adavi" });
            lgas.Add(new HealthLgas() { State = "Ekiti", Lga = "Ado Ekiti" });
            lgas.Add(new HealthLgas() { State = "Ogun", Lga = "Ado-Odo/Ota" });
            lgas.Add(new HealthLgas() { State = "Oyo", Lga = "Afijio" });
            lgas.Add(new HealthLgas() { State = "Ebonyi", Lga = "Afikpo North" });
            lgas.Add(new HealthLgas() { State = "Ebonyi", Lga = "Afikpo South" });
            lgas.Add(new HealthLgas() { State = "Niger", Lga = "Agaie" });
            lgas.Add(new HealthLgas() { State = "Benue", Lga = "Agatu" });
            lgas.Add(new HealthLgas() { State = "Niger", Lga = "Agwara" });
            lgas.Add(new HealthLgas() { State = "Lagos", Lga = "Agege" });
            lgas.Add(new HealthLgas() { State = "Anambra", Lga = "Aguata" });
            lgas.Add(new HealthLgas() { State = "Imo", Lga = "Ahiazu Mbaise" });
            lgas.Add(new HealthLgas() { State = "Rivers", Lga = "Ahoada East" });
            lgas.Add(new HealthLgas() { State = "Rivers", Lga = "Ahoada West" });
            lgas.Add(new HealthLgas() { State = "Kogi", Lga = "Ajaokuta" });
            lgas.Add(new HealthLgas() { State = "Lagos", Lga = "Ajeromi-Ifelodun" });
            lgas.Add(new HealthLgas() { State = "Kano", Lga = "Ajingi" });
            lgas.Add(new HealthLgas() { State = "Cross River", Lga = "Akamkpa" });
            lgas.Add(new HealthLgas() { State = "Oyo", Lga = "Akinyele" });
            lgas.Add(new HealthLgas() { State = "Gombe", Lga = "Akko" });
            lgas.Add(new HealthLgas() { State = "Edo", Lga = "Akoko-Edo" });
            lgas.Add(new HealthLgas() { State = "Ondo", Lga = "Akoko North-East" });
            lgas.Add(new HealthLgas() { State = "Ondo", Lga = "Akoko North-West" });
            lgas.Add(new HealthLgas() { State = "Ondo", Lga = "Akoko South-West" });
            lgas.Add(new HealthLgas() { State = "Ondo", Lga = "Akoko South-East" });
            lgas.Add(new HealthLgas() { State = "Cross River", Lga = "Akpabuyo" });
            lgas.Add(new HealthLgas() { State = "Rivers", Lga = "Akuku-Toru" });
            lgas.Add(new HealthLgas() { State = "Ondo", Lga = "Akure North" });
            lgas.Add(new HealthLgas() { State = "Ondo", Lga = "Akure South" });
            lgas.Add(new HealthLgas() { State = "Nasarawa", Lga = "Akwa Ibomnga" });
            lgas.Add(new HealthLgas() { State = "Kano", Lga = "Albasu" });
            lgas.Add(new HealthLgas() { State = "Kebbi", Lga = "Aleiro" });
            lgas.Add(new HealthLgas() { State = "Lagos", Lga = "Alimosho" });
            lgas.Add(new HealthLgas() { State = "Bauchi", Lga = "Alkaleri" });
            lgas.Add(new HealthLgas() { State = "Lagos", Lga = "Amuwo-Odofin" });
            lgas.Add(new HealthLgas() { State = "Anambra", Lga = "Anambra East" });
            lgas.Add(new HealthLgas() { State = "Anambra", Lga = "Anambra West" });
            lgas.Add(new HealthLgas() { State = "Anambra", Lga = "Anaocha" });
            lgas.Add(new HealthLgas() { State = "Rivers", Lga = "Andoni" });
            lgas.Add(new HealthLgas() { State = "Enugu", Lga = "Aninri" });
            lgas.Add(new HealthLgas() { State = "Delta", Lga = "Aniocha North" });
            lgas.Add(new HealthLgas() { State = "Delta", Lga = "Aniocha South" });
            lgas.Add(new HealthLgas() { State = "Zamfara", Lga = "Anka" });
            lgas.Add(new HealthLgas() { State = "Kogi", Lga = "Ankpa" });
            lgas.Add(new HealthLgas() { State = "Benue", Lga = "Apa" });
            lgas.Add(new HealthLgas() { State = "Lagos", Lga = "Apapa" });
            lgas.Add(new HealthLgas() { State = "Benue", Lga = "Ado" });
            lgas.Add(new HealthLgas() { State = "Taraba", Lga = "Ardo Kola" });
            lgas.Add(new HealthLgas() { State = "Kebbi", Lga = "Arewa Dandi" });
            lgas.Add(new HealthLgas() { State = "Kebbi", Lga = "Argungu" });
            lgas.Add(new HealthLgas() { State = "Abia", Lga = "Arochukwu" });
            lgas.Add(new HealthLgas() { State = "Kwara", Lga = "Asa" });
            lgas.Add(new HealthLgas() { State = "Rivers", Lga = "Asari-Toru" });
            lgas.Add(new HealthLgas() { State = "Borno", Lga = "Askira/Uba" });
            lgas.Add(new HealthLgas() { State = "Osun", Lga = "Atakunmosa East" });
            lgas.Add(new HealthLgas() { State = "Osun", Lga = "Atakunmosa West" });
            lgas.Add(new HealthLgas() { State = "Oyo", Lga = "Atiba" });
            lgas.Add(new HealthLgas() { State = "Oyo", Lga = "Atisbo" });
            lgas.Add(new HealthLgas() { State = "Kebbi", Lga = "Augie" });
            lgas.Add(new HealthLgas() { State = "Jigawa", Lga = "Auyo" });
            lgas.Add(new HealthLgas() { State = "Nasarawa", Lga = "Awe" });
            lgas.Add(new HealthLgas() { State = "Enugu", Lga = "Awgu" });
            lgas.Add(new HealthLgas() { State = "Anambra", Lga = "Awka North" });
            lgas.Add(new HealthLgas() { State = "Anambra", Lga = "Awka South" });
            lgas.Add(new HealthLgas() { State = "Anambra", Lga = "Ayamelum" });
            lgas.Add(new HealthLgas() { State = "Osun", Lga = "Aiyedaade" });
            lgas.Add(new HealthLgas() { State = "Osun", Lga = "Aiyedire" });
            lgas.Add(new HealthLgas() { State = "Jigawa", Lga = "Babura" });
            lgas.Add(new HealthLgas() { State = "Lagos", Lga = "Badagry" });
            lgas.Add(new HealthLgas() { State = "Kebbi", Lga = "Bagudo" });
            lgas.Add(new HealthLgas() { State = "Kano", Lga = "Bagwai" });
            lgas.Add(new HealthLgas() { State = "Cross River", Lga = "Bakassi" });
            lgas.Add(new HealthLgas() { State = "Plateau", Lga = "Bokkos" });
            lgas.Add(new HealthLgas() { State = "Katsina", Lga = "Bakori" });
            lgas.Add(new HealthLgas() { State = "Zamfara", Lga = "Bakura" });
            lgas.Add(new HealthLgas() { State = "Gombe", Lga = "Balanga" });
            lgas.Add(new HealthLgas() { State = "Taraba", Lga = "Bali" });
            lgas.Add(new HealthLgas() { State = "Borno", Lga = "Bama" });
            lgas.Add(new HealthLgas() { State = "Yobe", Lga = "Bade" });
            lgas.Add(new HealthLgas() { State = "Plateau", Lga = "Barkin Ladi" });
            lgas.Add(new HealthLgas() { State = "Kwara", Lga = "Baruten" });
            lgas.Add(new HealthLgas() { State = "Kogi", Lga = "Bassa" });
            lgas.Add(new HealthLgas() { State = "Plateau", Lga = "Bassa" });
            lgas.Add(new HealthLgas() { State = "Katsina", Lga = "Batagarawa" });
            lgas.Add(new HealthLgas() { State = "Katsina", Lga = "Batsari" });
            lgas.Add(new HealthLgas() { State = "Bauchi", Lga = "Bauchi" });
            lgas.Add(new HealthLgas() { State = "Katsina", Lga = "Baure" });
            lgas.Add(new HealthLgas() { State = "Borno", Lga = "Bayo" });
            lgas.Add(new HealthLgas() { State = "Kano", Lga = "Bebeji" });
            lgas.Add(new HealthLgas() { State = "Cross River", Lga = "Bekwarra" });
            lgas.Add(new HealthLgas() { State = "Abia", Lga = "Bende" });
            lgas.Add(new HealthLgas() { State = "Cross River", Lga = "Biase" });
            lgas.Add(new HealthLgas() { State = "Kano", Lga = "Bichi" });
            lgas.Add(new HealthLgas() { State = "Niger", Lga = "Bida" });
            lgas.Add(new HealthLgas() { State = "Gombe", Lga = "Billiri" });
            lgas.Add(new HealthLgas() { State = "Katsina", Lga = "Bindawa" });
            lgas.Add(new HealthLgas() { State = "Sokoto", Lga = "Binji" });
            lgas.Add(new HealthLgas() { State = "Jigawa", Lga = "Biriniwa" });
            lgas.Add(new HealthLgas() { State = "Kaduna", Lga = "Birnin Gwari" });
            lgas.Add(new HealthLgas() { State = "Kebbi", Lga = "Birnin Kebbi" });
            lgas.Add(new HealthLgas() { State = "Jigawa", Lga = "Birnin Kudu" });
            lgas.Add(new HealthLgas() { State = "Zamfara", Lga = "Birnin Magaji/Kiyaw" });
            lgas.Add(new HealthLgas() { State = "Borno", Lga = "Biu" });
            lgas.Add(new HealthLgas() { State = "Sokoto", Lga = "Bodinga" });
            lgas.Add(new HealthLgas() { State = "Bauchi", Lga = "Bogoro" });
            lgas.Add(new HealthLgas() { State = "Cross River", Lga = "Boki" });
            lgas.Add(new HealthLgas() { State = "Osun", Lga = "Boluwaduro" });
            lgas.Add(new HealthLgas() { State = "Delta", Lga = "Bomadi" });
            lgas.Add(new HealthLgas() { State = "Rivers", Lga = "Bonny" });
            lgas.Add(new HealthLgas() { State = "Niger", Lga = "Borgu" });
            lgas.Add(new HealthLgas() { State = "Osun", Lga = "Boripe" });
            lgas.Add(new HealthLgas() { State = "Yobe", Lga = "Bursari" });
            lgas.Add(new HealthLgas() { State = "Niger", Lga = "Bosso" });
            lgas.Add(new HealthLgas() { State = "Bayelsa", Lga = "Brass" });
            lgas.Add(new HealthLgas() { State = "Jigawa", Lga = "Buji" });
            lgas.Add(new HealthLgas() { State = "Zamfara", Lga = "Bukkuyum" });
            lgas.Add(new HealthLgas() { State = "Benue", Lga = "Buruku" });
            lgas.Add(new HealthLgas() { State = "Zamfara", Lga = "Bungudu" });
            lgas.Add(new HealthLgas() { State = "Kano", Lga = "Bunkure" });
            lgas.Add(new HealthLgas() { State = "Kebbi", Lga = "Bunza" });
            lgas.Add(new HealthLgas() { State = "Delta", Lga = "Burutu" });
            lgas.Add(new HealthLgas() { State = "FCT", Lga = "Bwari" });
            lgas.Add(new HealthLgas() { State = "Cross River", Lga = "Calabar Municipal" });
            lgas.Add(new HealthLgas() { State = "Cross River", Lga = "Calabar South" });
            lgas.Add(new HealthLgas() { State = "Niger", Lga = "Chanchaga" });
            lgas.Add(new HealthLgas() { State = "Katsina", Lga = "Charanchi" });
            lgas.Add(new HealthLgas() { State = "Borno", Lga = "Chibok" });
            lgas.Add(new HealthLgas() { State = "Kaduna", Lga = "Chikun" });
            lgas.Add(new HealthLgas() { State = "Kano", Lga = "Dala" });
            lgas.Add(new HealthLgas() { State = "Yobe", Lga = "Damaturu" });
            lgas.Add(new HealthLgas() { State = "Bauchi", Lga = "Damban" });
            lgas.Add(new HealthLgas() { State = "Kano", Lga = "Dambatta" });
            lgas.Add(new HealthLgas() { State = "Borno", Lga = "Damboa" });
            lgas.Add(new HealthLgas() { State = "Kebbi", Lga = "Dandi" });
            lgas.Add(new HealthLgas() { State = "Katsina", Lga = "Dandume" });
            lgas.Add(new HealthLgas() { State = "Sokoto", Lga = "Dange Shuni" });
            lgas.Add(new HealthLgas() { State = "Katsina", Lga = "Danja" });
            lgas.Add(new HealthLgas() { State = "Katsina", Lga = "Dan Musa" });
            lgas.Add(new HealthLgas() { State = "Bauchi", Lga = "Darazo" });
            lgas.Add(new HealthLgas() { State = "Bauchi", Lga = "Dass" });
            lgas.Add(new HealthLgas() { State = "Katsina", Lga = "Daura" });
            lgas.Add(new HealthLgas() { State = "Kano", Lga = "Dawakin Kudu" });
            lgas.Add(new HealthLgas() { State = "Kano", Lga = "Dawakin Tofa" });
            lgas.Add(new HealthLgas() { State = "Rivers", Lga = "Degema" });
            lgas.Add(new HealthLgas() { State = "Kogi", Lga = "Dekina" });
            lgas.Add(new HealthLgas() { State = "Adamawa", Lga = "Demsa" });
            lgas.Add(new HealthLgas() { State = "Borno", Lga = "Dikwa" });
            lgas.Add(new HealthLgas() { State = "Kano", Lga = "Doguwa" });
            lgas.Add(new HealthLgas() { State = "Nasarawa", Lga = "Doma" });
            lgas.Add(new HealthLgas() { State = "Taraba", Lga = "Donga" });
            lgas.Add(new HealthLgas() { State = "Gombe", Lga = "Dukku" });
            lgas.Add(new HealthLgas() { State = "Anambra", Lga = "Dunukofia" });
            lgas.Add(new HealthLgas() { State = "Jigawa", Lga = "Dutse" });
            lgas.Add(new HealthLgas() { State = "Katsina", Lga = "Dutsi" });
            lgas.Add(new HealthLgas() { State = "Katsina", Lga = "Dutsin Ma" });
            lgas.Add(new HealthLgas() { State = "Akwa Ibom", Lga = "Eastern Obolo" });
            lgas.Add(new HealthLgas() { State = "Ebonyi", Lga = "Ebonyi" });
            lgas.Add(new HealthLgas() { State = "Niger", Lga = "Edati" });
            lgas.Add(new HealthLgas() { State = "Osun", Lga = "Ede North" });
            lgas.Add(new HealthLgas() { State = "Osun", Lga = "Ede South" });
            lgas.Add(new HealthLgas() { State = "Kwara", Lga = "Edu" });
            lgas.Add(new HealthLgas() { State = "Osun", Lga = "Ife Central" });
            lgas.Add(new HealthLgas() { State = "Osun", Lga = "Ife East" });
            lgas.Add(new HealthLgas() { State = "Osun", Lga = "Ife North" });
            lgas.Add(new HealthLgas() { State = "Osun", Lga = "Ife South" });
            lgas.Add(new HealthLgas() { State = "Ekiti", Lga = "Efon" });
            lgas.Add(new HealthLgas() { State = "Ogun", Lga = "Egbado North" });
            lgas.Add(new HealthLgas() { State = "Ogun", Lga = "Egbado South" });
            lgas.Add(new HealthLgas() { State = "Oyo", Lga = "Egbeda" });
            lgas.Add(new HealthLgas() { State = "Osun", Lga = "Egbedore" });
            lgas.Add(new HealthLgas() { State = "Edo", Lga = "Egor" });
            lgas.Add(new HealthLgas() { State = "Imo", Lga = "Ehime Mbano" });
            lgas.Add(new HealthLgas() { State = "Osun", Lga = "Ejigbo" });
            lgas.Add(new HealthLgas() { State = "Bayelsa", Lga = "Ekeremor" });
            lgas.Add(new HealthLgas() { State = "Akwa Ibom", Lga = "Eket" });
            lgas.Add(new HealthLgas() { State = "Kwara", Lga = "Ekiti" });
            lgas.Add(new HealthLgas() { State = "Ekiti", Lga = "Ekiti East" });
            lgas.Add(new HealthLgas() { State = "Ekiti", Lga = "Ekiti South-West" });
            lgas.Add(new HealthLgas() { State = "Ekiti", Lga = "Ekiti West" });
            lgas.Add(new HealthLgas() { State = "Anambra", Lga = "Ekwusigo" });
            lgas.Add(new HealthLgas() { State = "Rivers", Lga = "Eleme" });
            lgas.Add(new HealthLgas() { State = "Rivers", Lga = "Emuoha" });
            lgas.Add(new HealthLgas() { State = "Ekiti", Lga = "Emure" });
            lgas.Add(new HealthLgas() { State = "Enugu", Lga = "Enugu East" });
            lgas.Add(new HealthLgas() { State = "Enugu", Lga = "Enugu North" });
            lgas.Add(new HealthLgas() { State = "Enugu", Lga = "Enugu South" });
            lgas.Add(new HealthLgas() { State = "Lagos", Lga = "Epe" });
            lgas.Add(new HealthLgas() { State = "Edo", Lga = "Esan Central" });
            lgas.Add(new HealthLgas() { State = "Edo", Lga = "Esan North-East" });
            lgas.Add(new HealthLgas() { State = "Edo", Lga = "Esan South-East" });
            lgas.Add(new HealthLgas() { State = "Edo", Lga = "Esan West" });
            lgas.Add(new HealthLgas() { State = "Ondo", Lga = "Ese Odo" });
            lgas.Add(new HealthLgas() { State = "Akwa Ibom", Lga = "Esit Eket" });
            lgas.Add(new HealthLgas() { State = "Akwa Ibom", Lga = "Essien Udim" });
            lgas.Add(new HealthLgas() { State = "Rivers", Lga = "Etche" });
            lgas.Add(new HealthLgas() { State = "Delta", Lga = "Ethiope East" });
            lgas.Add(new HealthLgas() { State = "Delta", Lga = "Ethiope West" });
            lgas.Add(new HealthLgas() { State = "Akwa Ibom", Lga = "Etim Ekpo" });
            lgas.Add(new HealthLgas() { State = "Akwa Ibom", Lga = "Etinan" });
            lgas.Add(new HealthLgas() { State = "Lagos", Lga = "Eti Osa" });
            lgas.Add(new HealthLgas() { State = "Edo", Lga = "Etsako Central" });
            lgas.Add(new HealthLgas() { State = "Edo", Lga = "Etsako East" });
            lgas.Add(new HealthLgas() { State = "Edo", Lga = "Etsako West" });
            lgas.Add(new HealthLgas() { State = "Cross River", Lga = "Etung" });
            lgas.Add(new HealthLgas() { State = "Ogun", Lga = "Ewekoro" });
            lgas.Add(new HealthLgas() { State = "Enugu", Lga = "Ezeagu" });
            lgas.Add(new HealthLgas() { State = "Imo", Lga = "Ezinihitte" });
            lgas.Add(new HealthLgas() { State = "Ebonyi", Lga = "Ezza North" });
            lgas.Add(new HealthLgas() { State = "Ebonyi", Lga = "Ezza South" });
            lgas.Add(new HealthLgas() { State = "Kano", Lga = "Fagge" });
            lgas.Add(new HealthLgas() { State = "Kebbi", Lga = "Fakai" });
            lgas.Add(new HealthLgas() { State = "Katsina", Lga = "Faskari" });
            lgas.Add(new HealthLgas() { State = "Yobe", Lga = "Fika" });
            lgas.Add(new HealthLgas() { State = "Adamawa", Lga = "Fufure" });
            lgas.Add(new HealthLgas() { State = "Gombe", Lga = "Funakaye" });
            lgas.Add(new HealthLgas() { State = "Yobe", Lga = "Fune" });
            lgas.Add(new HealthLgas() { State = "Katsina", Lga = "Funtua" });
            lgas.Add(new HealthLgas() { State = "Kano", Lga = "Gabasawa" });
            lgas.Add(new HealthLgas() { State = "Sokoto", Lga = "Gada" });
            lgas.Add(new HealthLgas() { State = "Jigawa", Lga = "Gagarawa" });
            lgas.Add(new HealthLgas() { State = "Bauchi", Lga = "Gamawa" });
            lgas.Add(new HealthLgas() { State = "Bauchi", Lga = "Ganjuwa" });
            lgas.Add(new HealthLgas() { State = "Adamawa", Lga = "Ganye" });
            lgas.Add(new HealthLgas() { State = "Jigawa", Lga = "Garki" });
            lgas.Add(new HealthLgas() { State = "Kano", Lga = "Garko" });
            lgas.Add(new HealthLgas() { State = "Kano", Lga = "Garun Mallam" });
            lgas.Add(new HealthLgas() { State = "Taraba", Lga = "Gashaka" });
            lgas.Add(new HealthLgas() { State = "Taraba", Lga = "Gassol" });
            lgas.Add(new HealthLgas() { State = "Kano", Lga = "Gaya" });
            lgas.Add(new HealthLgas() { State = "Adamawa", Lga = "Gayuk" });
            lgas.Add(new HealthLgas() { State = "Kano", Lga = "Gezawa" });
            lgas.Add(new HealthLgas() { State = "Niger", Lga = "Gbako" });
            lgas.Add(new HealthLgas() { State = "Benue", Lga = "Gboko" });
            lgas.Add(new HealthLgas() { State = "Ekiti", Lga = "Gbonyin" });
            lgas.Add(new HealthLgas() { State = "Yobe", Lga = "Geidam" });
            lgas.Add(new HealthLgas() { State = "Bauchi", Lga = "Giade" });
            lgas.Add(new HealthLgas() { State = "Kaduna", Lga = "Giwa" });
            lgas.Add(new HealthLgas() { State = "Rivers", Lga = "Gokana" });
            lgas.Add(new HealthLgas() { State = "Gombe", Lga = "Gombe" });
            lgas.Add(new HealthLgas() { State = "Adamawa", Lga = "Gombi" });
            lgas.Add(new HealthLgas() { State = "Sokoto", Lga = "Goronyo" });
            lgas.Add(new HealthLgas() { State = "Adamawa", Lga = "Grie" });
            lgas.Add(new HealthLgas() { State = "Borno", Lga = "Gubio" });
            lgas.Add(new HealthLgas() { State = "Sokoto", Lga = "Gudu" });
            lgas.Add(new HealthLgas() { State = "Yobe", Lga = "Gujba" });
            lgas.Add(new HealthLgas() { State = "Yobe", Lga = "Gulani" });
            lgas.Add(new HealthLgas() { State = "Benue", Lga = "Guma" });
            lgas.Add(new HealthLgas() { State = "Jigawa", Lga = "Gumel" });
            lgas.Add(new HealthLgas() { State = "Zamfara", Lga = "Gummi" });
            lgas.Add(new HealthLgas() { State = "Niger", Lga = "Gurara" });
            lgas.Add(new HealthLgas() { State = "Jigawa", Lga = "Guri" });
            lgas.Add(new HealthLgas() { State = "Zamfara", Lga = "Gusau" });
            lgas.Add(new HealthLgas() { State = "Borno", Lga = "Guzamala" });
            lgas.Add(new HealthLgas() { State = "Sokoto", Lga = "Gwadabawa" });
            lgas.Add(new HealthLgas() { State = "FCT", Lga = "Gwagwalada" });
            lgas.Add(new HealthLgas() { State = "Kano", Lga = "Gwale" });
            lgas.Add(new HealthLgas() { State = "Kebbi", Lga = "Gwandu" });
            lgas.Add(new HealthLgas() { State = "Jigawa", Lga = "Gwaram" });
            lgas.Add(new HealthLgas() { State = "Kano", Lga = "Gwarzo" });
            lgas.Add(new HealthLgas() { State = "Benue", Lga = "Gwer East" });
            lgas.Add(new HealthLgas() { State = "Benue", Lga = "Gwer West" });
            lgas.Add(new HealthLgas() { State = "Jigawa", Lga = "Gwiwa" });
            lgas.Add(new HealthLgas() { State = "Borno", Lga = "Gwoza" });
            lgas.Add(new HealthLgas() { State = "Jigawa", Lga = "Hadejia" });
            lgas.Add(new HealthLgas() { State = "Borno", Lga = "Hawul" });
            lgas.Add(new HealthLgas() { State = "Adamawa", Lga = "Hong" });
            lgas.Add(new HealthLgas() { State = "Oyo", Lga = "Ibadan North" });
            lgas.Add(new HealthLgas() { State = "Oyo", Lga = "Ibadan North-East" });
            lgas.Add(new HealthLgas() { State = "Oyo", Lga = "Ibadan North-West" });
            lgas.Add(new HealthLgas() { State = "Oyo", Lga = "Ibadan South-East" });
            lgas.Add(new HealthLgas() { State = "Oyo", Lga = "Ibadan South-West" });
            lgas.Add(new HealthLgas() { State = "Kogi", Lga = "Ibaji" });
            lgas.Add(new HealthLgas() { State = "Oyo", Lga = "Ibarapa Central" });
            lgas.Add(new HealthLgas() { State = "Oyo", Lga = "Ibarapa East" });
            lgas.Add(new HealthLgas() { State = "Oyo", Lga = "Ibarapa North" });
            lgas.Add(new HealthLgas() { State = "Lagos", Lga = "Ibeju-Lekki" });
            lgas.Add(new HealthLgas() { State = "Akwa Ibom", Lga = "Ibeno" });
            lgas.Add(new HealthLgas() { State = "Akwa Ibom", Lga = "Ibesikpo Asutan" });
            lgas.Add(new HealthLgas() { State = "Taraba", Lga = "Ibi" });
            lgas.Add(new HealthLgas() { State = "Akwa Ibom", Lga = "Ibiono-Ibom" });
            lgas.Add(new HealthLgas() { State = "Kogi", Lga = "Idah" });
            lgas.Add(new HealthLgas() { State = "Ondo", Lga = "Idanre" });
            lgas.Add(new HealthLgas() { State = "Imo", Lga = "Ideato North" });
            lgas.Add(new HealthLgas() { State = "Imo", Lga = "Ideato South" });
            lgas.Add(new HealthLgas() { State = "Anambra", Lga = "Idemili North" });
            lgas.Add(new HealthLgas() { State = "Anambra", Lga = "Idemili South" });
            lgas.Add(new HealthLgas() { State = "Oyo", Lga = "Ido" });
            lgas.Add(new HealthLgas() { State = "Ekiti", Lga = "Ido Osi" });
            lgas.Add(new HealthLgas() { State = "Lagos", Lga = "Ifako-Ijaiye" });
            lgas.Add(new HealthLgas() { State = "Osun", Lga = "Ifedayo" });
            lgas.Add(new HealthLgas() { State = "Ondo", Lga = "Ifedore" });
            lgas.Add(new HealthLgas() { State = "Kwara", Lga = "Ifelodun" });
            lgas.Add(new HealthLgas() { State = "Osun", Lga = "Ifelodun" });
            lgas.Add(new HealthLgas() { State = "Ogun", Lga = "Ifo" });
            lgas.Add(new HealthLgas() { State = "Kaduna", Lga = "Igabi" });
            lgas.Add(new HealthLgas() { State = "Kogi", Lga = "Igalamela Odolu" });
            lgas.Add(new HealthLgas() { State = "Enugu", Lga = "Igbo Etiti" });
            lgas.Add(new HealthLgas() { State = "Enugu", Lga = "Igbo Eze North" });
            lgas.Add(new HealthLgas() { State = "Enugu", Lga = "Igbo Eze South" });
            lgas.Add(new HealthLgas() { State = "Edo", Lga = "Igueben" });
            lgas.Add(new HealthLgas() { State = "Anambra", Lga = "Ihiala" });
            lgas.Add(new HealthLgas() { State = "Imo", Lga = "Ihitte/Uboma" });
            lgas.Add(new HealthLgas() { State = "Ondo", Lga = "Ilaje" });
            lgas.Add(new HealthLgas() { State = "Ogun", Lga = "Ijebu East" });
            lgas.Add(new HealthLgas() { State = "Ogun", Lga = "Ijebu North" });
            lgas.Add(new HealthLgas() { State = "Ogun", Lga = "Ijebu North East" });
            lgas.Add(new HealthLgas() { State = "Ogun", Lga = "Ijebu Ode" });
            lgas.Add(new HealthLgas() { State = "Ekiti", Lga = "Ijero" });
            lgas.Add(new HealthLgas() { State = "Kogi", Lga = "Ijumu" });
            lgas.Add(new HealthLgas() { State = "Akwa Ibom", Lga = "Ika" });
            lgas.Add(new HealthLgas() { State = "Delta", Lga = "Ika North East" });
            lgas.Add(new HealthLgas() { State = "Kaduna", Lga = "Ikara" });
            lgas.Add(new HealthLgas() { State = "Delta", Lga = "Ika South" });
            lgas.Add(new HealthLgas() { State = "Imo", Lga = "Ikeduru" });
            lgas.Add(new HealthLgas() { State = "Lagos", Lga = "Ikeja" });
            lgas.Add(new HealthLgas() { State = "Ogun", Lga = "Ikenne" });
            lgas.Add(new HealthLgas() { State = "Ekiti", Lga = "Ikere" });
            lgas.Add(new HealthLgas() { State = "Ekiti", Lga = "Ikole" });
            lgas.Add(new HealthLgas() { State = "Cross River", Lga = "Ikom" });
            lgas.Add(new HealthLgas() { State = "Akwa Ibom", Lga = "Ikono" });
            lgas.Add(new HealthLgas() { State = "Lagos", Lga = "Ikorodu" });
            lgas.Add(new HealthLgas() { State = "Akwa Ibom", Lga = "Ikot Abasi" });
            lgas.Add(new HealthLgas() { State = "Akwa Ibom", Lga = "Ikot Ekpene" });
            lgas.Add(new HealthLgas() { State = "Edo", Lga = "Ikpoba Okha" });
            lgas.Add(new HealthLgas() { State = "Rivers", Lga = "Ikwerre" });
            lgas.Add(new HealthLgas() { State = "Ebonyi", Lga = "Ikwo" });
            lgas.Add(new HealthLgas() { State = "Abia", Lga = "Ikwuano" });
            lgas.Add(new HealthLgas() { State = "Osun", Lga = "Ila" });
            lgas.Add(new HealthLgas() { State = "Ekiti", Lga = "Ilejemeje" });
            lgas.Add(new HealthLgas() { State = "Ondo", Lga = "Ile Oluji/Okeigbo" });
            lgas.Add(new HealthLgas() { State = "Osun", Lga = "Ilesa East" });
            lgas.Add(new HealthLgas() { State = "Osun", Lga = "Ilesa West" });
            lgas.Add(new HealthLgas() { State = "Sokoto", Lga = "Illela" });
            lgas.Add(new HealthLgas() { State = "Kwara", Lga = "Ilorin East" });
            lgas.Add(new HealthLgas() { State = "Kwara", Lga = "Ilorin South" });
            lgas.Add(new HealthLgas() { State = "Kwara", Lga = "Ilorin West" });
            lgas.Add(new HealthLgas() { State = "Ogun", Lga = "Imeko Afon" });
            lgas.Add(new HealthLgas() { State = "Katsina", Lga = "Ingawa" });
            lgas.Add(new HealthLgas() { State = "Akwa Ibom", Lga = "Ini" });
            lgas.Add(new HealthLgas() { State = "Ogun", Lga = "Ipokia" });
            lgas.Add(new HealthLgas() { State = "Ondo", Lga = "Irele" });
            lgas.Add(new HealthLgas() { State = "Oyo", Lga = "Irepo" });
            lgas.Add(new HealthLgas() { State = "Kwara", Lga = "Irepodun" });
            lgas.Add(new HealthLgas() { State = "Osun", Lga = "Irepodun" });
            lgas.Add(new HealthLgas() { State = "Ekiti", Lga = "Irepodun/Ifelodun" });
            lgas.Add(new HealthLgas() { State = "Osun", Lga = "Irewole" });
            lgas.Add(new HealthLgas() { State = "Sokoto", Lga = "Isa" });
            lgas.Add(new HealthLgas() { State = "Ekiti", Lga = "Ise/Orun" });
            lgas.Add(new HealthLgas() { State = "Oyo", Lga = "Iseyin" });
            lgas.Add(new HealthLgas() { State = "Ebonyi", Lga = "Ishielu" });
            lgas.Add(new HealthLgas() { State = "Imo", Lga = "Isiala Mbano" });
            lgas.Add(new HealthLgas() { State = "Abia", Lga = "Isiala Ngwa North" });
            lgas.Add(new HealthLgas() { State = "Abia", Lga = "Isiala Ngwa South" });
            lgas.Add(new HealthLgas() { State = "Kwara", Lga = "Isin" });
            lgas.Add(new HealthLgas() { State = "Enugu", Lga = "Isi Uzo" });
            lgas.Add(new HealthLgas() { State = "Osun", Lga = "Isokan" });
            lgas.Add(new HealthLgas() { State = "Delta", Lga = "Isoko North" });
            lgas.Add(new HealthLgas() { State = "Delta", Lga = "Isoko South" });
            lgas.Add(new HealthLgas() { State = "Imo", Lga = "Isu" });
            lgas.Add(new HealthLgas() { State = "Abia", Lga = "Isuikwuato" });
            lgas.Add(new HealthLgas() { State = "Bauchi", Lga = "Itas/Gadau" });
            lgas.Add(new HealthLgas() { State = "Oyo", Lga = "Itesiwaju" });
            lgas.Add(new HealthLgas() { State = "Akwa Ibom", Lga = "Itu" });
            lgas.Add(new HealthLgas() { State = "Ebonyi", Lga = "Ivo" });
            lgas.Add(new HealthLgas() { State = "Oyo", Lga = "Iwajowa" });
            lgas.Add(new HealthLgas() { State = "Osun", Lga = "Iwo" });
            lgas.Add(new HealthLgas() { State = "Ebonyi", Lga = "Izzi" });
            lgas.Add(new HealthLgas() { State = "Kaduna", Lga = "Jaba" });
            lgas.Add(new HealthLgas() { State = "Adamawa", Lga = "Jada" });
            lgas.Add(new HealthLgas() { State = "Jigawa", Lga = "Jahun" });
            lgas.Add(new HealthLgas() { State = "Yobe", Lga = "Jakusko" });
            lgas.Add(new HealthLgas() { State = "Taraba", Lga = "Jalingo" });
            lgas.Add(new HealthLgas() { State = "Bauchi", Lga = "Jama'are" });
            lgas.Add(new HealthLgas() { State = "Kebbi", Lga = "Jega" });
            lgas.Add(new HealthLgas() { State = "Kaduna", Lga = "Jema'a" });
            lgas.Add(new HealthLgas() { State = "Borno", Lga = "Jere" });
            lgas.Add(new HealthLgas() { State = "Katsina", Lga = "Jibia" });
            lgas.Add(new HealthLgas() { State = "Plateau", Lga = "Jos East" });
            lgas.Add(new HealthLgas() { State = "Plateau", Lga = "Jos North" });
            lgas.Add(new HealthLgas() { State = "Plateau", Lga = "Jos South" });
            lgas.Add(new HealthLgas() { State = "Kogi", Lga = "Kabba/Bunu" });
            lgas.Add(new HealthLgas() { State = "Kano", Lga = "Kabo" });
            lgas.Add(new HealthLgas() { State = "Kaduna", Lga = "Kachia" });
            lgas.Add(new HealthLgas() { State = "Kaduna", Lga = "Kaduna North" });
            lgas.Add(new HealthLgas() { State = "Kaduna", Lga = "Kaduna South" });
            lgas.Add(new HealthLgas() { State = "Jigawa", Lga = "Kafin Hausa" });
            lgas.Add(new HealthLgas() { State = "Katsina", Lga = "Kafur" });
            lgas.Add(new HealthLgas() { State = "Borno", Lga = "Kaga" });
            lgas.Add(new HealthLgas() { State = "Kaduna", Lga = "Kagarko" });
            lgas.Add(new HealthLgas() { State = "Kwara", Lga = "Kaiama" });
            lgas.Add(new HealthLgas() { State = "Katsina", Lga = "Kaita" });
            lgas.Add(new HealthLgas() { State = "Oyo", Lga = "Kajola" });
            lgas.Add(new HealthLgas() { State = "Kaduna", Lga = "Kajuru" });
            lgas.Add(new HealthLgas() { State = "Borno", Lga = "Kala/Balge" });
            lgas.Add(new HealthLgas() { State = "Kebbi", Lga = "Kalgo" });
            lgas.Add(new HealthLgas() { State = "Gombe", Lga = "Kaltungo" });
            lgas.Add(new HealthLgas() { State = "Plateau", Lga = "Kanam" });
            lgas.Add(new HealthLgas() { State = "Katsina", Lga = "Kankara" });
            lgas.Add(new HealthLgas() { State = "Plateau", Lga = "Kanke" });
            lgas.Add(new HealthLgas() { State = "Katsina", Lga = "Kankia" });
            lgas.Add(new HealthLgas() { State = "Kano", Lga = "Kano Municipal" });
            lgas.Add(new HealthLgas() { State = "Yobe", Lga = "Karasuwa" });
            lgas.Add(new HealthLgas() { State = "Kano", Lga = "Karaye" });
            lgas.Add(new HealthLgas() { State = "Taraba", Lga = "Karim Lamido" });
            lgas.Add(new HealthLgas() { State = "Nasarawa", Lga = "Karu" });
            lgas.Add(new HealthLgas() { State = "Bauchi", Lga = "Katagum" });
            lgas.Add(new HealthLgas() { State = "Niger", Lga = "Katcha" });
            lgas.Add(new HealthLgas() { State = "Katsina", Lga = "Katsina" });
            lgas.Add(new HealthLgas() { State = "Benue", Lga = "Katsina-Ala" });
            lgas.Add(new HealthLgas() { State = "Kaduna", Lga = "Kaura" });
            lgas.Add(new HealthLgas() { State = "Zamfara", Lga = "Kaura Namoda" });
            lgas.Add(new HealthLgas() { State = "Kaduna", Lga = "Kauru" });
            lgas.Add(new HealthLgas() { State = "Jigawa", Lga = "Kazaure" });
            lgas.Add(new HealthLgas() { State = "Nasarawa", Lga = "Keana" });
            lgas.Add(new HealthLgas() { State = "Sokoto", Lga = "Kebbe" });
            lgas.Add(new HealthLgas() { State = "Nasarawa", Lga = "Keffi" });
            lgas.Add(new HealthLgas() { State = "Rivers", Lga = "Khana" });
            lgas.Add(new HealthLgas() { State = "Kano", Lga = "Kibiya" });
            lgas.Add(new HealthLgas() { State = "Bauchi", Lga = "Kirfi" });
            lgas.Add(new HealthLgas() { State = "Jigawa", Lga = "Kiri Kasama" });
            lgas.Add(new HealthLgas() { State = "Kano", Lga = "Kiru" });
            lgas.Add(new HealthLgas() { State = "Jigawa", Lga = "Kiyawa" });
            lgas.Add(new HealthLgas() { State = "Kogi", Lga = "Kogi" });
            lgas.Add(new HealthLgas() { State = "Kebbi", Lga = "Koko/Besse" });
            lgas.Add(new HealthLgas() { State = "Nasarawa", Lga = "Kokona" });
            lgas.Add(new HealthLgas() { State = "Bayelsa", Lga = "Kolokuma/Opokuma" });
            lgas.Add(new HealthLgas() { State = "Borno", Lga = "Konduga" });
            lgas.Add(new HealthLgas() { State = "Benue", Lga = "Konshisha" });
            lgas.Add(new HealthLgas() { State = "Niger", Lga = "Kontagora" });
            lgas.Add(new HealthLgas() { State = "Lagos", Lga = "Kosofe" });
            lgas.Add(new HealthLgas() { State = "Jigawa", Lga = "Kaugama" });
            lgas.Add(new HealthLgas() { State = "Kaduna", Lga = "Kubau" });
            lgas.Add(new HealthLgas() { State = "Kaduna", Lga = "Kudan" });
            lgas.Add(new HealthLgas() { State = "FCT", Lga = "Kuje" });
            lgas.Add(new HealthLgas() { State = "Borno", Lga = "Kukawa" });
            lgas.Add(new HealthLgas() { State = "Kano", Lga = "Kumbotso" });
            lgas.Add(new HealthLgas() { State = "Taraba", Lga = "Kumi" });
            lgas.Add(new HealthLgas() { State = "Kano", Lga = "Kunchi" });
            lgas.Add(new HealthLgas() { State = "Kano", Lga = "Kura" });
            lgas.Add(new HealthLgas() { State = "Katsina", Lga = "Kurfi" });
            lgas.Add(new HealthLgas() { State = "Katsina", Lga = "Kusada" });
            lgas.Add(new HealthLgas() { State = "FCT", Lga = "Kwali" });
            lgas.Add(new HealthLgas() { State = "Benue", Lga = "Kwande" });
            lgas.Add(new HealthLgas() { State = "Gombe", Lga = "Kwami" });
            lgas.Add(new HealthLgas() { State = "Sokoto", Lga = "Kware" });
            lgas.Add(new HealthLgas() { State = "Borno", Lga = "Kwaya Kusar" });
            lgas.Add(new HealthLgas() { State = "Nasarawa", Lga = "Lafia" });
            lgas.Add(new HealthLgas() { State = "Oyo", Lga = "Lagelu" });
            lgas.Add(new HealthLgas() { State = "Lagos", Lga = "Lagos Island" });
            lgas.Add(new HealthLgas() { State = "Lagos", Lga = "Lagos Mainland" });
            lgas.Add(new HealthLgas() { State = "Plateau", Lga = "Langtang South" });
            lgas.Add(new HealthLgas() { State = "Plateau", Lga = "Langtang North" });
            lgas.Add(new HealthLgas() { State = "Niger", Lga = "Lapai" });
            lgas.Add(new HealthLgas() { State = "Adamawa", Lga = "Lamurde" });
            lgas.Add(new HealthLgas() { State = "Taraba", Lga = "Lau" });
            lgas.Add(new HealthLgas() { State = "Niger", Lga = "Lavun" });
            lgas.Add(new HealthLgas() { State = "Kaduna", Lga = "Lere" });
            lgas.Add(new HealthLgas() { State = "Benue", Lga = "Logo" });
            lgas.Add(new HealthLgas() { State = "Kogi", Lga = "Lokoja" });
            lgas.Add(new HealthLgas() { State = "Yobe", Lga = "Machina" });
            lgas.Add(new HealthLgas() { State = "Adamawa", Lga = "Madagali" });
            lgas.Add(new HealthLgas() { State = "Kano", Lga = "Madobi" });
            lgas.Add(new HealthLgas() { State = "Borno", Lga = "Mafa" });
            lgas.Add(new HealthLgas() { State = "Niger", Lga = "Magama" });
            lgas.Add(new HealthLgas() { State = "Borno", Lga = "Magumeri" });
            lgas.Add(new HealthLgas() { State = "Katsina", Lga = "Mai'Adua" });
            lgas.Add(new HealthLgas() { State = "Borno", Lga = "Maiduguri" });
            lgas.Add(new HealthLgas() { State = "Jigawa", Lga = "Maigatari" });
            lgas.Add(new HealthLgas() { State = "Adamawa", Lga = "Maiha" });
            lgas.Add(new HealthLgas() { State = "Kebbi", Lga = "Maiyama" });
            lgas.Add(new HealthLgas() { State = "Kaduna", Lga = "Makarfi" });
            lgas.Add(new HealthLgas() { State = "Kano", Lga = "Makoda" });
            lgas.Add(new HealthLgas() { State = "Jigawa", Lga = "Malam Madori" });
            lgas.Add(new HealthLgas() { State = "Katsina", Lga = "Malumfashi" });
            lgas.Add(new HealthLgas() { State = "Plateau", Lga = "Mangu" });
            lgas.Add(new HealthLgas() { State = "Katsina", Lga = "Mani" });
            lgas.Add(new HealthLgas() { State = "Zamfara", Lga = "Maradun" });
            lgas.Add(new HealthLgas() { State = "Niger", Lga = "Mariga" });
            lgas.Add(new HealthLgas() { State = "Benue", Lga = "Makurdi" });
            lgas.Add(new HealthLgas() { State = "Borno", Lga = "Marte" });
            lgas.Add(new HealthLgas() { State = "Zamfara", Lga = "Maru" });
            lgas.Add(new HealthLgas() { State = "Niger", Lga = "Mashegu" });
            lgas.Add(new HealthLgas() { State = "Katsina", Lga = "Mashi" });
            lgas.Add(new HealthLgas() { State = "Katsina", Lga = "Matazu" });
            lgas.Add(new HealthLgas() { State = "Adamawa", Lga = "Mayo Belwa" });
            lgas.Add(new HealthLgas() { State = "Imo", Lga = "Mbaitoli" });
            lgas.Add(new HealthLgas() { State = "Akwa Ibom", Lga = "Mbo" });
            lgas.Add(new HealthLgas() { State = "Adamawa", Lga = "Michika" });
            lgas.Add(new HealthLgas() { State = "Jigawa", Lga = "Miga" });
            lgas.Add(new HealthLgas() { State = "Plateau", Lga = "Mikang" });
            lgas.Add(new HealthLgas() { State = "Kano", Lga = "Minjibir" });
            lgas.Add(new HealthLgas() { State = "Bauchi", Lga = "Misau" });
            lgas.Add(new HealthLgas() { State = "Ekiti", Lga = "Moba" });
            lgas.Add(new HealthLgas() { State = "Borno", Lga = "Mobbar" });
            lgas.Add(new HealthLgas() { State = "Adamawa", Lga = "Mubi North" });
            lgas.Add(new HealthLgas() { State = "Adamawa", Lga = "Mubi South" });
            lgas.Add(new HealthLgas() { State = "Niger", Lga = "Mokwa" });
            lgas.Add(new HealthLgas() { State = "Borno", Lga = "Monguno" });
            lgas.Add(new HealthLgas() { State = "Kogi", Lga = "Mopa Muro" });
            lgas.Add(new HealthLgas() { State = "Kwara", Lga = "Moro" });
            lgas.Add(new HealthLgas() { State = "Niger", Lga = "Moya" });
            lgas.Add(new HealthLgas() { State = "Akwa Ibom", Lga = "Mkpat-Enin" });
            lgas.Add(new HealthLgas() { State = "FCT", Lga = "Municipal Area Council" });
            lgas.Add(new HealthLgas() { State = "Katsina", Lga = "Musawa" });
            lgas.Add(new HealthLgas() { State = "Lagos", Lga = "Mushin" });
            lgas.Add(new HealthLgas() { State = "Gombe", Lga = "Nafada" });
            lgas.Add(new HealthLgas() { State = "Yobe", Lga = "Nangere" });
            lgas.Add(new HealthLgas() { State = "Kano", Lga = "Nasarawa" });
            lgas.Add(new HealthLgas() { State = "Nasarawa", Lga = "Nasarawa" });
            lgas.Add(new HealthLgas() { State = "Nasarawa", Lga = "Nasarawa Egon" });
            lgas.Add(new HealthLgas() { State = "Delta", Lga = "Ndokwa East" });
            lgas.Add(new HealthLgas() { State = "Delta", Lga = "Ndokwa West" });
            lgas.Add(new HealthLgas() { State = "Bayelsa", Lga = "Nembe" });
            lgas.Add(new HealthLgas() { State = "Borno", Lga = "Ngala" });
            lgas.Add(new HealthLgas() { State = "Borno", Lga = "Nganzai" });
            lgas.Add(new HealthLgas() { State = "Kebbi", Lga = "Ngaski" });
            lgas.Add(new HealthLgas() { State = "Imo", Lga = "Ngor Okpala" });
            lgas.Add(new HealthLgas() { State = "Yobe", Lga = "Nguru" });
            lgas.Add(new HealthLgas() { State = "Bauchi", Lga = "Ningi" });
            lgas.Add(new HealthLgas() { State = "Imo", Lga = "Njaba" });
            lgas.Add(new HealthLgas() { State = "Anambra", Lga = "Njikoka" });
            lgas.Add(new HealthLgas() { State = "Enugu", Lga = "Nkanu East" });
            lgas.Add(new HealthLgas() { State = "Enugu", Lga = "Nkanu West" });
            lgas.Add(new HealthLgas() { State = "Imo", Lga = "Nkwerre" });
            lgas.Add(new HealthLgas() { State = "Anambra", Lga = "Nnewi North" });
            lgas.Add(new HealthLgas() { State = "Anambra", Lga = "Nnewi South" });
            lgas.Add(new HealthLgas() { State = "Akwa Ibom", Lga = "Nsit-Atai" });
            lgas.Add(new HealthLgas() { State = "Akwa Ibom", Lga = "Nsit-Ibom" });
            lgas.Add(new HealthLgas() { State = "Akwa Ibom", Lga = "Nsit-Ubium" });
            lgas.Add(new HealthLgas() { State = "Enugu", Lga = "Nsukka" });
            lgas.Add(new HealthLgas() { State = "Adamawa", Lga = "Numan" });
            lgas.Add(new HealthLgas() { State = "Imo", Lga = "Nwangele" });
            lgas.Add(new HealthLgas() { State = "Ogun", Lga = "Obafemi Owode" });
            lgas.Add(new HealthLgas() { State = "Cross River", Lga = "Obanliku" });
            lgas.Add(new HealthLgas() { State = "Benue", Lga = "Obi" });
            lgas.Add(new HealthLgas() { State = "Nasarawa", Lga = "Obi" });
            lgas.Add(new HealthLgas() { State = "Abia", Lga = "Obi Ngwa" });
            lgas.Add(new HealthLgas() { State = "Rivers", Lga = "Obio/Akpor" });
            lgas.Add(new HealthLgas() { State = "Osun", Lga = "Obokun" });
            lgas.Add(new HealthLgas() { State = "Akwa Ibom", Lga = "Obot Akara" });
            lgas.Add(new HealthLgas() { State = "Imo", Lga = "Obowo" });
            lgas.Add(new HealthLgas() { State = "Cross River", Lga = "Obubra" });
            lgas.Add(new HealthLgas() { State = "Cross River", Lga = "Obudu" });
            lgas.Add(new HealthLgas() { State = "Ogun", Lga = "Odeda" });
            lgas.Add(new HealthLgas() { State = "Ondo", Lga = "Odigbo" });
            lgas.Add(new HealthLgas() { State = "Ogun", Lga = "Odogbolu" });
            lgas.Add(new HealthLgas() { State = "Osun", Lga = "Odo Otin" });
            lgas.Add(new HealthLgas() { State = "Cross River", Lga = "Odukpani" });
            lgas.Add(new HealthLgas() { State = "Kwara", Lga = "Offa" });
            lgas.Add(new HealthLgas() { State = "Kogi", Lga = "Ofu" });
            lgas.Add(new HealthLgas() { State = "Rivers", Lga = "Ogba/Egbema/Ndoni" });
            lgas.Add(new HealthLgas() { State = "Benue", Lga = "Ogbadibo" });
            lgas.Add(new HealthLgas() { State = "Anambra", Lga = "Ogbaru" });
            lgas.Add(new HealthLgas() { State = "Bayelsa", Lga = "Ogbia" });
            lgas.Add(new HealthLgas() { State = "Oyo", Lga = "Ogbomosho North" });
            lgas.Add(new HealthLgas() { State = "Oyo", Lga = "Ogbomosho South" });
            lgas.Add(new HealthLgas() { State = "Rivers", Lga = "Ogu/Bolo" });
            lgas.Add(new HealthLgas() { State = "Cross River", Lga = "Ogoja" });
            lgas.Add(new HealthLgas() { State = "Oyo", Lga = "Ogo Oluwa" });
            lgas.Add(new HealthLgas() { State = "Kogi", Lga = "Ogori/Magongo" });
            lgas.Add(new HealthLgas() { State = "Ogun", Lga = "Ogun Waterside" });
            lgas.Add(new HealthLgas() { State = "Imo", Lga = "Oguta" });
            lgas.Add(new HealthLgas() { State = "Abia", Lga = "Ohafia" });
            lgas.Add(new HealthLgas() { State = "Imo", Lga = "Ohaji/Egbema" });
            lgas.Add(new HealthLgas() { State = "Ebonyi", Lga = "Ohaozara" });
            lgas.Add(new HealthLgas() { State = "Ebonyi", Lga = "Ohaukwu" });
            lgas.Add(new HealthLgas() { State = "Benue", Lga = "Ohimini" });
            lgas.Add(new HealthLgas() { State = "Edo", Lga = "Orhionmwon" });
            lgas.Add(new HealthLgas() { State = "Enugu", Lga = "Oji River" });
            lgas.Add(new HealthLgas() { State = "Lagos", Lga = "Ojo" });
            lgas.Add(new HealthLgas() { State = "Benue", Lga = "Oju" });
            lgas.Add(new HealthLgas() { State = "Kogi", Lga = "Okehi" });
            lgas.Add(new HealthLgas() { State = "Kogi", Lga = "Okene" });
            lgas.Add(new HealthLgas() { State = "Kwara", Lga = "Oke Ero" });
            lgas.Add(new HealthLgas() { State = "Imo", Lga = "Okigwe" });
            lgas.Add(new HealthLgas() { State = "Ondo", Lga = "Okitipupa" });
            lgas.Add(new HealthLgas() { State = "Akwa Ibom", Lga = "Okobo" });
            lgas.Add(new HealthLgas() { State = "Delta", Lga = "Okpe" });
            lgas.Add(new HealthLgas() { State = "Rivers", Lga = "Okrika" });
            lgas.Add(new HealthLgas() { State = "Kogi", Lga = "Olamaboro" });
            lgas.Add(new HealthLgas() { State = "Osun", Lga = "Ola Oluwa" });
            lgas.Add(new HealthLgas() { State = "Osun", Lga = "Olorunda" });
            lgas.Add(new HealthLgas() { State = "Oyo", Lga = "Olorunsogo" });
            lgas.Add(new HealthLgas() { State = "Oyo", Lga = "Oluyole" });
            lgas.Add(new HealthLgas() { State = "Kogi", Lga = "Omala" });
            lgas.Add(new HealthLgas() { State = "Rivers", Lga = "Omuma" });
            lgas.Add(new HealthLgas() { State = "Oyo", Lga = "Ona Ara" });
            lgas.Add(new HealthLgas() { State = "Ondo", Lga = "Ondo East" });
            lgas.Add(new HealthLgas() { State = "Ondo", Lga = "Ondo West" });
            lgas.Add(new HealthLgas() { State = "Ebonyi", Lga = "Onicha" });
            lgas.Add(new HealthLgas() { State = "Anambra", Lga = "Onitsha North" });
            lgas.Add(new HealthLgas() { State = "Anambra", Lga = "Onitsha South" });
            lgas.Add(new HealthLgas() { State = "Akwa Ibom", Lga = "Onna" });
            lgas.Add(new HealthLgas() { State = "Benue", Lga = "Okpokwu" });
            lgas.Add(new HealthLgas() { State = "Rivers", Lga = "Opobo/Nkoro" });
            lgas.Add(new HealthLgas() { State = "Edo", Lga = "Oredo" });
            lgas.Add(new HealthLgas() { State = "Oyo", Lga = "Orelope" });
            lgas.Add(new HealthLgas() { State = "Osun", Lga = "Oriade" });
            lgas.Add(new HealthLgas() { State = "Oyo", Lga = "Ori Ire" });
            lgas.Add(new HealthLgas() { State = "Imo", Lga = "Orlu" });
            lgas.Add(new HealthLgas() { State = "Osun", Lga = "Orolu" });
            lgas.Add(new HealthLgas() { State = "Akwa Ibom", Lga = "Oron" });
            lgas.Add(new HealthLgas() { State = "Imo", Lga = "Orsu" });
            lgas.Add(new HealthLgas() { State = "Imo", Lga = "Oru East" });
            lgas.Add(new HealthLgas() { State = "Akwa Ibom", Lga = "Oruk Anam" });
            lgas.Add(new HealthLgas() { State = "Anambra", Lga = "Orumba North" });
            lgas.Add(new HealthLgas() { State = "Anambra", Lga = "Orumba South" });
            lgas.Add(new HealthLgas() { State = "Imo", Lga = "Oru West" });
            lgas.Add(new HealthLgas() { State = "Ondo", Lga = "Ose" });
            lgas.Add(new HealthLgas() { State = "Delta", Lga = "Oshimili North" });
            lgas.Add(new HealthLgas() { State = "Delta", Lga = "Oshimili South" });
            lgas.Add(new HealthLgas() { State = "Lagos", Lga = "Oshodi-Isolo" });
            lgas.Add(new HealthLgas() { State = "Abia", Lga = "Osisioma" });
            lgas.Add(new HealthLgas() { State = "Osun", Lga = "Osogbo" });
            lgas.Add(new HealthLgas() { State = "Benue", Lga = "Oturkpo" });
            lgas.Add(new HealthLgas() { State = "Edo", Lga = "Ovia North-East" });
            lgas.Add(new HealthLgas() { State = "Edo", Lga = "Ovia South-West" });
            lgas.Add(new HealthLgas() { State = "Edo", Lga = "Owan East" });
            lgas.Add(new HealthLgas() { State = "Edo", Lga = "Owan West" });
            lgas.Add(new HealthLgas() { State = "Imo", Lga = "Owerri Municipal" });
            lgas.Add(new HealthLgas() { State = "Imo", Lga = "Owerri North" });
            lgas.Add(new HealthLgas() { State = "Imo", Lga = "Owerri West" });
            lgas.Add(new HealthLgas() { State = "Ondo", Lga = "Owo" });
            lgas.Add(new HealthLgas() { State = "Ekiti", Lga = "Oye" });
            lgas.Add(new HealthLgas() { State = "Anambra", Lga = "Oyi" });
            lgas.Add(new HealthLgas() { State = "Rivers", Lga = "Oyigbo" });
            lgas.Add(new HealthLgas() { State = "Oyo", Lga = "Oyo" });
            lgas.Add(new HealthLgas() { State = "Oyo", Lga = "Oyo East" });
            lgas.Add(new HealthLgas() { State = "Kwara", Lga = "Oyun" });
            lgas.Add(new HealthLgas() { State = "Niger", Lga = "Paikoro" });
            lgas.Add(new HealthLgas() { State = "Plateau", Lga = "Pankshin" });
            lgas.Add(new HealthLgas() { State = "Delta", Lga = "Patani" });
            lgas.Add(new HealthLgas() { State = "Kwara", Lga = "Pategi" });
            lgas.Add(new HealthLgas() { State = "Rivers", Lga = "Port Harcourt" });
            lgas.Add(new HealthLgas() { State = "Yobe", Lga = "Potiskum" });
            lgas.Add(new HealthLgas() { State = "Plateau", Lga = "Qua'an Pan" });
            lgas.Add(new HealthLgas() { State = "Sokoto", Lga = "Rabah" });
            lgas.Add(new HealthLgas() { State = "Niger", Lga = "Rafi" });
            lgas.Add(new HealthLgas() { State = "Kano", Lga = "Rano" });
            lgas.Add(new HealthLgas() { State = "Ogun", Lga = "Remo North" });
            lgas.Add(new HealthLgas() { State = "Niger", Lga = "Rijau" });
            lgas.Add(new HealthLgas() { State = "Katsina", Lga = "Rimi" });
            lgas.Add(new HealthLgas() { State = "Kano", Lga = "Rimin Gado" });
            lgas.Add(new HealthLgas() { State = "Jigawa", Lga = "Ringim" });
            lgas.Add(new HealthLgas() { State = "Plateau", Lga = "Riyom" });
            lgas.Add(new HealthLgas() { State = "Kano", Lga = "Rogo" });
            lgas.Add(new HealthLgas() { State = "Jigawa", Lga = "Roni" });
            lgas.Add(new HealthLgas() { State = "Sokoto", Lga = "Sabon Birni" });
            lgas.Add(new HealthLgas() { State = "Kaduna", Lga = "Sabon Gari" });
            lgas.Add(new HealthLgas() { State = "Katsina", Lga = "Sabuwa" });
            lgas.Add(new HealthLgas() { State = "Katsina", Lga = "Safana" });
            lgas.Add(new HealthLgas() { State = "Bayelsa", Lga = "Sagbama" });
            lgas.Add(new HealthLgas() { State = "Kebbi", Lga = "Sakaba" });
            lgas.Add(new HealthLgas() { State = "Oyo", Lga = "Saki East" });
            lgas.Add(new HealthLgas() { State = "Oyo", Lga = "Saki West" });
            lgas.Add(new HealthLgas() { State = "Katsina", Lga = "Sandamu" });
            lgas.Add(new HealthLgas() { State = "Kaduna", Lga = "Sanga" });
            lgas.Add(new HealthLgas() { State = "Delta", Lga = "Sapele" });
            lgas.Add(new HealthLgas() { State = "Taraba", Lga = "Sardauna" });
            lgas.Add(new HealthLgas() { State = "Ogun", Lga = "Shagamu" });
            lgas.Add(new HealthLgas() { State = "Sokoto", Lga = "Shagari" });
            lgas.Add(new HealthLgas() { State = "Kebbi", Lga = "Shanga" });
            lgas.Add(new HealthLgas() { State = "Borno", Lga = "Shani" });
            lgas.Add(new HealthLgas() { State = "Kano", Lga = "Shanono" });
            lgas.Add(new HealthLgas() { State = "Adamawa", Lga = "Shelleng" });
            lgas.Add(new HealthLgas() { State = "Plateau", Lga = "Shendam" });
            lgas.Add(new HealthLgas() { State = "Zamfara", Lga = "Shinkafi" });
            lgas.Add(new HealthLgas() { State = "Bauchi", Lga = "Shira" });
            lgas.Add(new HealthLgas() { State = "Niger", Lga = "Shiroro" });
            lgas.Add(new HealthLgas() { State = "Gombe", Lga = "Shongom" });
            lgas.Add(new HealthLgas() { State = "Lagos", Lga = "Shomolu" });
            lgas.Add(new HealthLgas() { State = "Sokoto", Lga = "Silame" });
            lgas.Add(new HealthLgas() { State = "Kaduna", Lga = "Soba" });
            lgas.Add(new HealthLgas() { State = "Sokoto", Lga = "Sokoto North" });
            lgas.Add(new HealthLgas() { State = "Sokoto", Lga = "Sokoto South" });
            lgas.Add(new HealthLgas() { State = "Adamawa", Lga = "Song" });
            lgas.Add(new HealthLgas() { State = "Bayelsa", Lga = "Southern Ijaw" });
            lgas.Add(new HealthLgas() { State = "Niger", Lga = "Suleja" });
            lgas.Add(new HealthLgas() { State = "Jigawa", Lga = "Sule Tankarkar" });
            lgas.Add(new HealthLgas() { State = "Kano", Lga = "Sumaila" });
            lgas.Add(new HealthLgas() { State = "Kebbi", Lga = "Suru" });
            lgas.Add(new HealthLgas() { State = "Oyo", Lga = "Surulere" });
            lgas.Add(new HealthLgas() { State = "Lagos", Lga = "Surulere" });
            lgas.Add(new HealthLgas() { State = "Niger", Lga = "Tafa" });
            lgas.Add(new HealthLgas() { State = "Bauchi", Lga = "Tafawa Balewa" });
            lgas.Add(new HealthLgas() { State = "Rivers", Lga = "Tai" });
            lgas.Add(new HealthLgas() { State = "Kano", Lga = "Takai" });
            lgas.Add(new HealthLgas() { State = "Taraba", Lga = "Takum" });
            lgas.Add(new HealthLgas() { State = "Zamfara", Lga = "Talata Mafara" });
            lgas.Add(new HealthLgas() { State = "Sokoto", Lga = "Tambuwal" });
            lgas.Add(new HealthLgas() { State = "Sokoto", Lga = "Tangaza" });
            lgas.Add(new HealthLgas() { State = "Kano", Lga = "Tarauni" });
            lgas.Add(new HealthLgas() { State = "Benue", Lga = "Tarka" });
            lgas.Add(new HealthLgas() { State = "Yobe", Lga = "Tarmuwa" });
            lgas.Add(new HealthLgas() { State = "Jigawa", Lga = "Taura" });
            lgas.Add(new HealthLgas() { State = "Adamawa", Lga = "Toungo" });
            lgas.Add(new HealthLgas() { State = "Kano", Lga = "Tofa" });
            lgas.Add(new HealthLgas() { State = "Bauchi", Lga = "Toro" });
            lgas.Add(new HealthLgas() { State = "Nasarawa", Lga = "Toto" });
            lgas.Add(new HealthLgas() { State = "Zamfara", Lga = "Chafe" });
            lgas.Add(new HealthLgas() { State = "Kano", Lga = "Tsanyawa" });
            lgas.Add(new HealthLgas() { State = "Kano", Lga = "Tudun Wada" });
            lgas.Add(new HealthLgas() { State = "Sokoto", Lga = "Tureta" });
            lgas.Add(new HealthLgas() { State = "Enugu", Lga = "Udenu" });
            lgas.Add(new HealthLgas() { State = "Enugu", Lga = "Udi" });
            lgas.Add(new HealthLgas() { State = "Delta", Lga = "Udu" });
            lgas.Add(new HealthLgas() { State = "Akwa Ibom", Lga = "Udung-Uko" });
            lgas.Add(new HealthLgas() { State = "Delta", Lga = "Ughelli North" });
            lgas.Add(new HealthLgas() { State = "Delta", Lga = "Ughelli South" });
            lgas.Add(new HealthLgas() { State = "Abia", Lga = "Ugwunagbo" });
            lgas.Add(new HealthLgas() { State = "Edo", Lga = "Uhunmwonde" });
            lgas.Add(new HealthLgas() { State = "Akwa Ibom", Lga = "Ukanafun" });
            lgas.Add(new HealthLgas() { State = "Benue", Lga = "Ukum" });
            lgas.Add(new HealthLgas() { State = "Abia", Lga = "Ukwa East" });
            lgas.Add(new HealthLgas() { State = "Abia", Lga = "Ukwa West" });
            lgas.Add(new HealthLgas() { State = "Delta", Lga = "Ukwuani" });
            lgas.Add(new HealthLgas() { State = "Abia", Lga = "Umuahia North" });
            lgas.Add(new HealthLgas() { State = "Abia", Lga = "Umuahia South" });
            lgas.Add(new HealthLgas() { State = "Abia", Lga = "Umu Nneochi" });
            lgas.Add(new HealthLgas() { State = "Kano", Lga = "Ungogo" });
            lgas.Add(new HealthLgas() { State = "Imo", Lga = "Unuimo" });
            lgas.Add(new HealthLgas() { State = "Akwa Ibom", Lga = "Uruan" });
            lgas.Add(new HealthLgas() { State = "Akwa Ibom", Lga = "Urue-Offong/Oruko" });
            lgas.Add(new HealthLgas() { State = "Benue", Lga = "Ushongo" });
            lgas.Add(new HealthLgas() { State = "Taraba", Lga = "Ussa" });
            lgas.Add(new HealthLgas() { State = "Delta", Lga = "Uvwie" });
            lgas.Add(new HealthLgas() { State = "Akwa Ibom", Lga = "Uyo" });
            lgas.Add(new HealthLgas() { State = "Enugu", Lga = "Uzo Uwani" });
            lgas.Add(new HealthLgas() { State = "Benue", Lga = "Vandeikya" });
            lgas.Add(new HealthLgas() { State = "Sokoto", Lga = "Wamako" });
            lgas.Add(new HealthLgas() { State = "Nasarawa", Lga = "Wamba" });
            lgas.Add(new HealthLgas() { State = "Kano", Lga = "Warawa" });
            lgas.Add(new HealthLgas() { State = "Bauchi", Lga = "Warji" });
            lgas.Add(new HealthLgas() { State = "Delta", Lga = "Warri North" });
            lgas.Add(new HealthLgas() { State = "Delta", Lga = "Warri South" });
            lgas.Add(new HealthLgas() { State = "Delta", Lga = "Warri South West" });
            lgas.Add(new HealthLgas() { State = "Kebbi", Lga = "Wasagu/Danko" });
            lgas.Add(new HealthLgas() { State = "Plateau", Lga = "Wase" });
            lgas.Add(new HealthLgas() { State = "Kano", Lga = "Wudil" });
            lgas.Add(new HealthLgas() { State = "Taraba", Lga = "Wukari" });
            lgas.Add(new HealthLgas() { State = "Sokoto", Lga = "Wurno" });
            lgas.Add(new HealthLgas() { State = "Niger", Lga = "Wushishi" });
            lgas.Add(new HealthLgas() { State = "Sokoto", Lga = "Yabo" });
            lgas.Add(new HealthLgas() { State = "Kogi", Lga = "Yagba East" });
            lgas.Add(new HealthLgas() { State = "Kogi", Lga = "Yagba West" });
            lgas.Add(new HealthLgas() { State = "Cross River", Lga = "Yakuur" });
            lgas.Add(new HealthLgas() { State = "Cross River", Lga = "Yala" });
            lgas.Add(new HealthLgas() { State = "Gombe", Lga = "Yamaltu/Deba" });
            lgas.Add(new HealthLgas() { State = "Jigawa", Lga = "Yankwashi" });
            lgas.Add(new HealthLgas() { State = "Kebbi", Lga = "Yauri" });
            lgas.Add(new HealthLgas() { State = "Bayelsa", Lga = "Yenagoa" });
            lgas.Add(new HealthLgas() { State = "Adamawa", Lga = "Yola North" });
            lgas.Add(new HealthLgas() { State = "Adamawa", Lga = "Yola South" });
            lgas.Add(new HealthLgas() { State = "Taraba", Lga = "Yorro" });
            lgas.Add(new HealthLgas() { State = "Yobe", Lga = "Yunusari" });
            lgas.Add(new HealthLgas() { State = "Yobe", Lga = "Yusufari" });
            lgas.Add(new HealthLgas() { State = "Bauchi", Lga = "Zaki" });
            lgas.Add(new HealthLgas() { State = "Katsina", Lga = "Zango" });
            lgas.Add(new HealthLgas() { State = "Kaduna", Lga = "Zangon Kataf" });
            lgas.Add(new HealthLgas() { State = "Kaduna", Lga = "Zaria" });
            lgas.Add(new HealthLgas() { State = "Taraba", Lga = "Zing" });
            lgas.Add(new HealthLgas() { State = "Zamfara", Lga = "Zurmi" });
            lgas.Add(new HealthLgas() { State = "Kebbi", Lga = "Zuru" });


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