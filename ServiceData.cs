using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.Linq;



namespace NewRelish
{

    class ServiceData
    {

        IList<Services> hserve;
        static readonly string[] gScopes = { SheetsService.Scope.Spreadsheets };
        static readonly string gApplicationName = "Google Sheets API Quickstart";
        static readonly string gspreadsheetId = "1aNKfTAX6JWPsPUUof5S4-sRbwLCZyPr5TGxJYHZ7-Po";
        static readonly string gsheet = "Labeled";
        GoogleCredential gcredential;

        public static List<Services> GetServices { get; private set; }

        public ServiceData()
        {
            var temp = new List<Services>();

            Serve(temp);
            Serve(temp);
            Serve(temp);
            Serve(temp);
            Serve(temp);
            Serve(temp);
            Serve(temp);

            GetServices = temp.OrderBy(i => i.Name).ToList();
        }

        public void Serve(List<Services> services)
        {
            // Android.Content.Res.AssetManager

            var asset = Android.App.Application.Context.Assets;

            SheetsService gservice;


            gcredential = GoogleCredential.FromStream(asset.Open("client_secret.json")).CreateScoped(gScopes);

            gservice = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = gcredential,
                ApplicationName = gApplicationName,
            });
            var range = $"{gsheet}!C2:E";
            SpreadsheetsResource.ValuesResource.GetRequest request =
                   gservice.Spreadsheets.Values.Get(gspreadsheetId, range);
            ValueRange response = request.Execute();
            IList<IList<Object>> values = response.Values;

            if (values != null && values.Count > 0)
            {

                try
                {

                    hserve = (from IList<Object> row in values
                              select new Services()
                              {
                                  Name = row[0].ToString(),
                                  State = row[1].ToString(),
                                  Address = row[2].ToString(),
                                  Contact = row[3].ToString(),
                                  ContactDesignation = row[4].ToString(),
                                  ContactEmail = row[5].ToString(),
                                  ContactTelephone = row[6].ToString()

                              }).ToList();


                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

        }
    }
}