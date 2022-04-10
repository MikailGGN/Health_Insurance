using Android.App;
using Android.OS;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;

namespace NewRelish
{
    [Activity(Label = "ConnectorActivity")]
    public class ConnectorActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application 
            string[] Scopes = { SheetsService.Scope.Spreadsheets };
            string ApplicationName = "Google Sheets API Quickstart";
            string spreadsheetId = "1aNKfTAX6JWPsPUUof5S4-sRbwLCZyPr5TGxJYHZ7-Po";
            string sheet = "Labeled";
            GoogleCredential credential;

            SheetsService service;


            credential = GoogleCredential.FromStream(Assets.Open("client_secret.json")).CreateScoped(Scopes);

            service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
            var PersonalID = "null";// ReturnAccount();
            var dtnow = DateTime.Now.ToShortDateString();// 
            var range = $"{sheet}!B:F";
            var valueRange = new ValueRange();

            var oblist = new List<object>() { dtnow, PersonalID };
            valueRange.Values = new List<IList<object>> { oblist };

            var appendRequest =
                service.Spreadsheets.Values.Append(valueRange, spreadsheetId, range);
            appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
            // var appendReponse = appendRequest.Execute();



        }
    }
}