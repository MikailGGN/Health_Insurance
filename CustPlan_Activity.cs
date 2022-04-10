using Android.App;
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
using System.IO;
using System.Linq;

namespace NewRelish
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar")]
    public class CustPlan_Activity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {
        IList<PlanProvider> hmoproviders;
        IList<HealthServiceDetail> hsdetail;
        IList<Serviceoptions> Serviceoptions;

        ListView mList;

        static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
        static readonly string ApplicationName = "Google Sheets API Quickstart";
        static readonly string spreadsheetId = "11BJ1je9-3vqusvdK2qa-IK-ay8BkCJcyuJYDp28z_j4";
        static readonly string sheet = "Provider";

        GoogleCredential credential;
        private int progressStatus = 0, progressStatus1 = 100;
        private Dialog popupDialog;
        Spinner sphp;

        CheckBox chkGroup;
        static readonly string[] gScopes = { SheetsService.Scope.Spreadsheets };
        static readonly string gApplicationName = "Google Sheets API Quickstart";

        GoogleCredential gcredential;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_cust_plan);
            // Create your application here

            Op_Load();
            sphp = FindViewById<Spinner>(Resource.Id.sp_custplan);

            ArrayAdapter ada = new ArrayAdapter<string>(this, Resource.Layout.spinitem,
                hmoproviders.Select(r => r.Provider.ToString()).ToArray());

            ada.SetDropDownViewResource(Resource.Layout.support_simple_spinner_dropdown_item);

            sphp.Adapter = ada;
            sphp.ItemSelected += sphp_ItemSelected;

            //

            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            TextView Bartext = FindViewById<TextView>(Resource.Id.textView_homepay);
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

            Serviceoptions = new List<Serviceoptions>();
            // addlist = FindViewById<ListView>(Resource.Id.listv);
            // addList.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1,Serviceoptions .);

            mList = FindViewById<ListView>(Resource.Id.listView_cp);

            HealthService_Data();
            mList.Adapter = new ServiceDetailAdapter(this, hsdetail);
            mList.SetItemChecked(-1, true);
            mList.ChoiceMode = ChoiceMode.Multiple; // 2

            //mList.ItemClick += OnListItemClick;

            var btn_cl = FindViewById<Button>(Resource.Id.button_Cal);
            btn_cl.Click += delegate
            {

                // var sparseArray = FindViewById<ListView>(Resource.Id.listView_cp).CheckedItemPositions;

                //  Toast.MakeText(this,
                //    sparseArray.KeyAt(i) + "=" + sparseArray.ValueAt(i) + ",", ToastLength.Long).Show();




            };


        }
        //void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        //{
        //    Toast.MakeText(this,
        //                "05", ToastLength.Long).Show();
        //    var listView = sender as ListView;

        //var t = hsdetail[e.Position];
        //    Android.Widget.Toast.MakeText(this, t.ServiceItemCost, Android.Widget.ToastLength.Short).Show();
        //}


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

        void sphp_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        void Op_Load()/**/
        {


            hmoproviders = new List<PlanProvider>();/**/

            SheetsService service;


            credential = GoogleCredential.FromStream(Assets.Open("client_secret.json")).CreateScoped(Scopes);

            service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
            var range = $"{sheet}!A2:B";
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
                                        // PlanProvider = row[2].ToString()

                                    }).ToList();


                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }


        }

        public void HealthService_Data()/**/
        {
            String gspreadsheetId = "11BJ1je9-3vqusvdK2qa-IK-ay8BkCJcyuJYDp28z_j4";
            String gsheet = "ServiceDetails";

            hsdetail = new List<HealthServiceDetail>();/**/

            SheetsService gservice;


            gcredential = GoogleCredential.FromStream(Assets.Open("client_secret.json")).CreateScoped(gScopes);

            gservice = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = gcredential,
                ApplicationName = gApplicationName,
            });
            var range = $"{gsheet}!A2:C";
            SpreadsheetsResource.ValuesResource.GetRequest request =
                   gservice.Spreadsheets.Values.Get(gspreadsheetId, range);
            ValueRange response = request.Execute();
            IList<IList<Object>> values = response.Values;

            if (values != null && values.Count > 0)
            {

                try
                {

                    hsdetail = (from IList<Object> row in values
                                    //  where row != null
                                select new HealthServiceDetail()
                                {
                                    ServiceDetail = row[0].ToString() ?? "",
                                    ServiceDetailID = row[1].ToString() ?? "",
                                    ServiceItemCost = row[2].ToString() ?? ""

                                }).ToList();


                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }


        }

    }

    class ServiceDetailAdapter : BaseAdapter<HealthServiceDetail>
    {
        IList<HealthServiceDetail> HmoserviceDetail;
        CustPlan_Activity c;
        private IList<HealthServiceDetail> hsdetail;

        public ServiceDetailAdapter(CustPlan_Activity c, IList<HealthServiceDetail> hsdetail)
        {
            this.c = c;
            this.HmoserviceDetail = hsdetail;
        }
        public override HealthServiceDetail this[int position] => HmoserviceDetail[position];

        public override int Count => HmoserviceDetail.Count;


        public override long GetItemId(int position)
        {
            return position;
        }



        public override View GetView(int position, View convertView, ViewGroup parent)
        {

            View view;



            if (convertView == null)
            {
                view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.servicelist_row, parent, false);

            }
            else
            {
                view = convertView;
            }

            view.FindViewById<TextView>(Resource.Id.nameTextView).Text = HmoserviceDetail[position].ServiceDetail.ToString();
            view.FindViewById<TextView>(Resource.Id.costTextView).Text = HmoserviceDetail[position].ServiceItemCost.ToString();



            CheckBox chkCaptain = view.FindViewById<CheckBox>(Resource.Id.checkBoxCost);
            //   chkCaptain.Tag = HmoserviceDetail[position].ToString();
            chkCaptain.Tag = HmoserviceDetail[position].ServiceDetail + "<--->  Cost =" + HmoserviceDetail[position].ServiceItemCost;
            // chkCaptain.Text = HmoserviceDetail[position].ServiceItemCost;

            chkCaptain.SetOnCheckedChangeListener(null);
            chkCaptain.Checked = HmoserviceDetail[position].IsChecked;
            chkCaptain.SetOnCheckedChangeListener(new CheckedChangeListener(this.c, this.HmoserviceDetail));
            ((ListView)parent).ChoiceMode = ChoiceMode.Multiple;

            ((ListView)parent).SetItemChecked(position, true);

            // ...

            if (position % 2 == 0)
            {
                view.SetBackgroundResource(Resource.Drawable.bg_grey_banner);
            }


            return view;
        }

        //  void OnCheckedChanged(CompoundButton buttonView, bool isChecked)
        //  {
        //    int position = (int)buttonView.Tag;
        //     HmoserviceDetail[position].IsChecked = isChecked;
        // }
    }

    class CheckedChangeListener : Java.Lang.Object, CompoundButton.IOnCheckedChangeListener
    {
        private Activity activity;
        private IList<HealthServiceDetail> HmoserviceDetail;
        //  IList<HealthServiceDetail> HmoserviceDetail;
        private static List<Serviceoptions> Serviceoptions;
        ListView addlist;
        ArrayAdapter adapter;
        public static List<Serviceoptions> Instance
        {
            get
            {
                if (Serviceoptions == null)
                {
                    Serviceoptions = new List<Serviceoptions>();
                }
                return Serviceoptions;
            }
            set
            {
                Serviceoptions = value;
            }
        }
        public CheckedChangeListener(Activity activity, IList<HealthServiceDetail> hsdetail2)
        {
            this.activity = activity;
            this.HmoserviceDetail = hsdetail2;
            Serviceoptions = new List<Serviceoptions>();


        }

        public void OnCheckedChanged(CompoundButton buttonView, bool isChecked)
        {


            if (isChecked)
            {
                // Serviceoptions = new List<Serviceoptions>();
                // int position = (int)buttonView.Tag; HmoserviceDetail[position].IsChecked = isChecked;
                string slist = (string)buttonView.Tag;
                string[] slistitem = slist.Split("<--->  Cost =");

                Serviceoptions.Add(new Serviceoptions()
                {
                    ServiceDetail = slistitem[0],
                    ServiceItemCost = slistitem[1],
                });


                addlist = this.activity.FindViewById<ListView>(Resource.Id.listView_added);

                adapter = new ArrayAdapter(this.activity, Android.Resource.Layout.SimpleListItemMultipleChoice, Serviceoptions.ToList());
                addlist.Adapter = adapter;

                var Hcosted = this.activity.FindViewById<Button>(Resource.Id.button_Cal);
                string[] c = Hcosted.Text.Split(" NGN");
                Double hc = Convert.ToDouble(c[0]);
                hc = hc + Convert.ToDouble(slistitem[1]);
                Hcosted.Text = string.Format(hc.ToString(), "0,000") + " NGN";
            }



        }



    }
    public static class ServiceoptionsData
    {
        public static List<Serviceoptions> Serviceoptions { get; private set; }

        static ServiceoptionsData()
        {
            var temp = new List<Serviceoptions>();
            //  string x, y;
            //    Addoptions(temp);//,x,y);

            Serviceoptions = temp.OrderBy(i => i.ServiceDetail).ToList();
        }
        private static void Addoptions(List<Serviceoptions> serviceoptions, string a, string b)
        {
            serviceoptions.Add(new Serviceoptions()
            {
                ServiceDetail = a,
                ServiceItemCost = b,

            });

        }
    }
}