using Android.App;
using Android.Content;
using Android.Database;
using Android.OS;
using Android.Provider;
using Android.Widget;
using System;
namespace NewRelish
{
    [Activity(Label = "CalenderActivity")]
    public class CalenderActivity : ListActivity
    {
        [Obsolete]
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.CalenderList);

            // List Calendars
            var calendarsUri = CalendarContract.Calendars.ContentUri;

            string[] calendarsProjection = {
               CalendarContract.Calendars.InterfaceConsts.Id,
               CalendarContract.Calendars.InterfaceConsts.CalendarDisplayName,
               CalendarContract.Calendars.InterfaceConsts.AccountName
            };

            var loader = new CursorLoader(this, calendarsUri, calendarsProjection, null, null, null);
            var cursor = (ICursor)loader.LoadInBackground();

            string[] sourceColumns = {
                CalendarContract.Calendars.InterfaceConsts.CalendarDisplayName,
                CalendarContract.Calendars.InterfaceConsts.AccountName};

            int[] targetResources = {
                Resource.Id.calDisplayName,
                Resource.Id.calAccountName};

            SimpleCursorAdapter adapter = new SimpleCursorAdapter(this, Resource.Layout.CalListItem,
                cursor, sourceColumns, targetResources);

            ListAdapter = adapter;

            ListView.ItemClick += (sender, e) =>
            {
                int i = (e as Android.Widget.AdapterView.ItemClickEventArgs).Position;

                cursor.MoveToPosition(i);
                int calId = cursor.GetInt(cursor.GetColumnIndex(calendarsProjection[0]));

                var showEvents = new Intent(this, typeof(EventListActivity));
                showEvents.PutExtra("calId", calId);
                StartActivity(showEvents);
            };

        }
    }
}