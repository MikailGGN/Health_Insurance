using Android.Views;
using Android.Widget;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using Object = Java.Lang.Object;

namespace NewRelish
{
    class CustomAdapter : BaseAdapter<HealthLgas>
    {
        Service_Activity c;
        IList<HealthLgas> lgas;
        // private LayoutInflater inflater;
        // IList<HealthServices> services;

        public CustomAdapter(Service_Activity c, IList<HealthLgas> lgas)
        {
            this.c = c;
            this.lgas = lgas;
        }

        public override HealthLgas this[int position] => lgas[position];

        public override int Count => lgas.Count;

        public override long GetItemId(int position)
        {
            return position;
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view;
            if (convertView == null)
            {
                view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.spmodel, parent, false);

            }
            else
            {
                view = convertView;
            }

            //TextView nameTxt = convertView.FindViewById<TextView>(Resource.Id.nameTxt);

            //BIND

            // nameTxt.Text = states[position].Name;
            view.FindViewById<TextView>(Resource.Id.nameTxt).Text = lgas[position].Lga.ToString();

            return view;
        }


    }

    class State
    {
        private string name;


        public State()
        {
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string states { get; internal set; }
    }
    class StatesCollection
    {


        // public static JavaList<State> GetStates()
        public static IList<State> GetStates()
        {
            IList<State> states = new List<State>();

            var asset = Android.App.Application.Context.Assets;

            String[] gScopes = { SheetsService.Scope.Spreadsheets };
            String gApplicationName = "Google Sheets API Quickstart";
            String gspreadsheetId = "11BJ1je9-3vqusvdK2qa-IK-ay8BkCJcyuJYDp28z_j4";
            String gsheet = "State";
            GoogleCredential gcredential;
            ///services = new List<HealthServices>();/**/

            SheetsService gservice;


            gcredential = GoogleCredential.FromStream(asset.Open("client_secret.json")).CreateScoped(gScopes);

            gservice = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = gcredential,
                ApplicationName = gApplicationName,
            });
            var range = $"{gsheet}!b2:b";
            SpreadsheetsResource.ValuesResource.GetRequest request =
                   gservice.Spreadsheets.Values.Get(gspreadsheetId, range);
            ValueRange response = request.Execute();
            //  IList<IList<Object>>
            var values = response.Values;
            // JavaList<Object> values = response.Values;


            if (values != null && values.Count > 0)
            {
                try
                {
                    states = (IList<State>)(from IList<Object> row in values
                                                //  where row != null
                                            select new State()
                                            {
                                                Name = row[0].ToString() ?? ""// ,

                                            }).ToList();

                    //   string[] va = values.Select(i => i.ToString()).ToArray();
                    //  states.Add( va);
                }
                //ADD States

                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }


            states.Count();


            return states;
        }
    }
}
