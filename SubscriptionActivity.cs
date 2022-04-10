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

namespace NewRelish
{
    /// <summary>
    /// [Activity(Label = "SubscriptionActivity")]
    /// </summary>

    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar")]
    public class SubscriptionActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_subscriptions);

            // Create your application 
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            TextView Bartext = FindViewById<TextView>(Resource.Id.textView_homesub);
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

            //buttons
            var subhcp = FindViewById<Button>(Resource.Id.button_sub_hcp);

            subhcp.Click += delegate
            {
                StartActivity(new Android.Content.Intent(this, typeof(ClaimActivity)));
            };

            var subpy = FindViewById<Button>(Resource.Id.button_sub_py);

            subpy.Click += delegate
            {
                StartActivity(new Android.Content.Intent(this, typeof(PaymentActivity)));
            };
            //app label
            //var applabel = FindViewById<TextView>(Resource.Id.application_name);
            //   applabel.Click += delegate {
            //     StartActivity(new Android.Content.Intent(this, typeof(MainActivity)));
            // };
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

    }
}