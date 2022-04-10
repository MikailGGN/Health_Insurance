using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Firebase;
using Firebase.Auth;
using System;
using System.Threading;
using static Android.App.ActionBar;

namespace NewRelish
{
    [Activity(Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {
        Button button_rs;
        //  private Button btnshowPopup;
        private Button btnPopupCancel;
        private Button btnPopOk;
        private Dialog popupDialog;

        private MediaRecorder _recorder;
        private MediaPlayer _player;
        private Button _start;
        private Button _stop;
        string path;
        public static FirebaseApp app;
        FirebaseAuth auth;

        private int progressStatus = 0, progressStatus1 = 100;

        public int MyResultCode = 1;
        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            // if (FirebaseAuth.Instance.CurrentUser == null)
            //   StartActivityForResult(new Android.Content.Intent(this, typeof(SignIn_Activity)), MyResultCode);


            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);


            //  toolbar.Click += delegate { StartActivity(new Android.Content.Intent(this, typeof(MainActivity))); };
            //Toolbar will now take on default actionbar characteristics

            TextView Bartext = FindViewById<TextView>(Resource.Id.textView_home);
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
            //Event report Syptoms

            button_rs = FindViewById<Button>(Resource.Id.button_rs);
            button_rs.Click += delegate
            {
                // SetContentView(Resource.Layout.content_rpt_syptm);
                // setWindowParams();

                popupDialog = new Dialog(this);
                popupDialog.SetContentView(Resource.Layout.content_rpt_syptm);

                popupDialog.Show();


                _start = popupDialog.FindViewById<Button>(Resource.Id.start);
                _stop = popupDialog.FindViewById<Button>(Resource.Id.stop);
                path = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/symptoms.3gpp";
                _start.Click += start_Click;
                _stop.Click += stop_Click;
            };

            // private MemoryStream inputStream;

            var btn_doc = FindViewById<Button>(Resource.Id.button_sdtr);
            btn_doc.Click += delegate
            {
                //doctor
                StartActivity(new Android.Content.Intent(this, typeof(CalenderActivity)));

                // StartActivity(new Android.Content.Intent(this, typeof(ScheduleDoctorActivity)));

            };
            var btn_pharm = FindViewById<Button>(Resource.Id.button_ph);
            btn_pharm.Click += delegate
            {
                // progressbar();
                Intent p = new Intent(this, typeof(Service_Activity));
                p.PutExtra("TheService", "Pharmacy");
                StartActivity(p);
            };
            var btn_hosptl = FindViewById<Button>(Resource.Id.button_hs);
            btn_hosptl.Click += delegate
            {
                //  progressbar();
                var p = new Intent(this, typeof(Service_Activity));
                p.PutExtra("TheService", "Hospital");
                StartActivity(p);
            };

            var btn_dr = FindViewById<Button>(Resource.Id.button_dr);
            btn_dr.Click += delegate
            {
                //  progressbar();
                var p = new Intent(this, typeof(Service_Activity));
                p.PutExtra("TheService", "Ambulance");
                StartActivity(p);
            };
            var btn_gy = FindViewById<Button>(Resource.Id.button_gy);
            btn_gy.Click += delegate
            {
                // progressbar();
                var p = new Intent(this, typeof(Service_Activity));
                p.PutExtra("TheService", "Gym");

                StartActivity(p);
            };

            ///circularProgressbar
            //    InitFirebaseAuth();
            // MenuItem item;

        }

        private void InitFirebaseAuth()
        {
            var options = new FirebaseOptions.Builder()
               .SetApplicationId("1:920083122070:android:018804ec92b4724a63bfb7")
               .SetApiKey("AIzaSyBVxdAlL9FORUH_psnhRikJRfmfDFZmcyw")
               .Build();

            if (app == null)
                app = FirebaseApp.InitializeApp(this, options);
            auth = FirebaseAuth.GetInstance(app);
        }
        private void start_Click(object sender, System.EventArgs e)
        {
            _stop.Enabled = !_stop.Enabled;
            _start.Enabled = !_start.Enabled;
            _recorder.SetAudioSource(AudioSource.Mic);
            _recorder.SetOutputFormat(OutputFormat.Default);
            _recorder.SetAudioEncoder(AudioEncoder.Default);
            _recorder.SetOutputFile(path);
            _recorder.Prepare();
            _recorder.Start();
        }
        private void stop_Click(object sender, System.EventArgs e)
        {
            _stop.Enabled = !_stop.Enabled;
            _recorder.Stop();
            _recorder.Reset();
            _player.SetDataSource(path);
            _player.Prepare();
            _player.Start();
        }
        protected override void OnResume()
        {
            base.OnResume();
            _recorder = new MediaRecorder();
            _player = new MediaPlayer();
            _player.Completion += (sender, e) =>
            {
                _player.Reset();
                _start.Enabled = !_start.Enabled;
            };
        }
        protected override void OnPause()
        {
            base.OnPause();
            _player.Release();
            _recorder.Release();
            _player.Dispose();
            _recorder.Dispose();
            _player = null;
            _recorder = null;
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
            // Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
            //  .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();

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
             //  progressbar();
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
                StartActivity(new Android.Content.Intent(this, typeof(SignInActivity)));
            }
            // else if (id == Resource.Id.nav_send)  { }

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            drawer.CloseDrawer(GravityCompat.Start);
            return true;

            // MenuItem item;
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public void setWindowParams()
        {
            Android.Views.WindowManagerLayoutParams wlp = Window.Attributes;
            wlp.DimAmount = 90;
            //  wlp.Flags = WindowManagerFlags.DimBehind | WindowManagerFlags.BlurBehind | WindowManagerFlags.NotTouchable | WindowManagerFlags.NotTouchModal | WindowManagerFlags.TouchableWhenWaking | WindowManagerFlags.KeepScreenOn | WindowManagerFlags.LayoutInScreen | WindowManagerFlags.LayoutNoLimits | WindowManagerFlags.Fullscreen;

            wlp.Flags = ((WindowManagerFlags)2038);

        }

        private void BtnshowPopup_Click(object sender, System.EventArgs e)
        {
            popupDialog = new Dialog(this);
            popupDialog.SetContentView(Resource.Layout.changesub);
            popupDialog.Window.SetSoftInputMode(SoftInput.AdjustResize);
            popupDialog.Show();

            // Some Time Layout width not fit with windows size  
            // but Below lines are not necessery  
            popupDialog.Window.SetLayout(LayoutParams.MatchParent, LayoutParams.WrapContent);
            popupDialog.Window.SetBackgroundDrawableResource(Android.Resource.Color.Transparent);

            // Access Popup layout fields like below  
            btnPopupCancel = popupDialog.FindViewById<Button>(Resource.Id.btnCancel);
            btnPopOk = popupDialog.FindViewById<Button>(Resource.Id.btnOk);

            // Events for that popup layout  
            btnPopupCancel.Click += BtnPopupCancel_Click;
            btnPopOk.Click += BtnPopOk_Click;

            // Some Additional Tips   
            // Set the dialog Title Property - popupDialog.Window.SetTitle("Alert Title");  
        }

        private void BtnPopOk_Click(object sender, System.EventArgs e)
        {
            popupDialog.Dismiss();
            popupDialog.Hide();
        }

        private void BtnPopupCancel_Click(object sender, System.EventArgs e)
        {
            popupDialog.Dismiss();
            popupDialog.Hide();
        }
        public void progressbar()
        {

            ProgressBar circularbar = FindViewById<ProgressBar>(Resource.Id.circularProgressbar);


            circularbar.Progress = 100;
            circularbar.SecondaryProgress = 100;
            new System.Threading.Thread(new ThreadStart(delegate
            {
                while (progressStatus < 100)
                {
                    progressStatus += 1;
                    progressStatus1 -= 1;
                    circularbar.Progress = progressStatus1;
                    System.Threading.Thread.Sleep(100);
                }
            })).Start();
        }

    }
}

