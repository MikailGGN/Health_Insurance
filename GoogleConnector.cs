using Android.App;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.IO;

//using Android.Accounts;
namespace eltCare
{
    class GoogleConnector : Activity
    {

        public string ReturnAccount()
        {

            //  AccountManager manager = (AccountManager) GetSystemService(AccountService);
            //  Account[] list = manager.GetAccounts();

            String gmail = null;

            //   foreach (Account account in list)
            //   {
            //      if (account.Type.Equals("com.google"))
            //    {
            //        gmail = account.Name;
            //        break;
            //    }
            // }

            return gmail;
        }
        public void OperationPlans()
        {

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
            var PersonalID = ReturnAccount();
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
        /// <summary>
        /// 
        /// </summary>
        public void OperationGym()
        {
            string[] Scopes = { SheetsService.Scope.Spreadsheets };
            string ApplicationName = "Google Sheets API Quickstart";
            GoogleCredential credential;
            using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream)
                    .CreateScoped(Scopes);
            }

            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Define request parameters.
            String spreadsheetId = "1f2gqvcvXKXCV6muqNc6jc23dP01YrDGZD2ynHxn7Ztc";
            String range = "Subculture!B2:I";
            SpreadsheetsResource.ValuesResource.GetRequest request =
                    service.Spreadsheets.Values.Get(spreadsheetId, range);

            ValueRange response = request.Execute();
            IList<IList<Object>> values = response.Values;
            if (values != null && values.Count > 0)
            {
                //  Console.WriteLine("Name, Major");

                foreach (var row in values)
                {

                    // Print columns A and E, which correspond to indices 0 and 4.
                    // Console.WriteLine("{0}, {1}", row[0], row[4]);
                    try
                    {
                        //Output0Buffer.AddRow();



                        //Output0Buffer.Comment = Convert.ToString(row[8].ToString());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }





            }


        }
    }
}