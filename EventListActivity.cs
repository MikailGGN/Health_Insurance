﻿using Android.App;
using Android.Content;
using Android.Database;
using Android.OS;
using Android.Provider;
using Android.Views;
using Android.Widget;
using Java.Util;
using System;

namespace NewRelish
{
    [Activity(Label = "EventListActivity")]
    public class EventListActivity : ListActivity
    {
        int _calId;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here

            SetContentView(Resource.Layout.EventList);

            _calId = Intent.GetIntExtra("calId", -1);

            ListEvents();

            InitAddEvent();
        }

        void ListEvents()
        {
            var eventsUri = CalendarContract.Events.ContentUri;

            string[] eventsProjection = {
                CalendarContract.Events.InterfaceConsts.Id,
                CalendarContract.Events.InterfaceConsts.Title,
                CalendarContract.Events.InterfaceConsts.Dtstart
             };

            var loader = new CursorLoader(this, eventsUri, eventsProjection,
                               String.Format("calendar_id={0}", _calId), null, "dtstart ASC");
            var cursor = (ICursor)loader.LoadInBackground();

            string[] sourceColumns = {
                CalendarContract.Events.InterfaceConsts.Title,
                CalendarContract.Events.InterfaceConsts.Dtstart
            };

            int[] targetResources = {
                Resource.Id.eventTitle,
                Resource.Id.eventStartDate
            };

            var adapter = new SimpleCursorAdapter(this, Resource.Layout.EventListItem,
                 cursor, sourceColumns, targetResources);

            adapter.ViewBinder = new ViewBinder();

            ListAdapter = adapter;

            //            ListView.ItemClick += (sender, e) => { 
            //                int i = (e as ItemEventArgs).Position;
            //                
            //                cursor.MoveToPosition(i);
            //                int eventId = cursor.GetInt (cursor.GetColumnIndex (eventsProjection [0]));
            //                var uri = ContentUris.WithAppendedId(CalendarContract.Events.ContentUri, eventId);
            //                var intent = new Intent(Intent.ActionView, uri);
            //                StartActivity(intent);              
            //            };
        }

        void InitAddEvent()
        {
            var addSampleEvent = FindViewById<Button>(Resource.Id.addSampleEvent);

            addSampleEvent.Click += (sender, e) =>
            {
                // Create Event code
                ContentValues eventValues = new ContentValues();
                eventValues.Put(CalendarContract.Events.InterfaceConsts.CalendarId, _calId);
                eventValues.Put(CalendarContract.Events.InterfaceConsts.Title, "Test Event from M4A");
                eventValues.Put(CalendarContract.Events.InterfaceConsts.Description, "This is an event created from Mono for Android");
                eventValues.Put(CalendarContract.Events.InterfaceConsts.Dtstart, GetDateTimeMS(2011, 12, 15, 10, 0));
                eventValues.Put(CalendarContract.Events.InterfaceConsts.Dtend, GetDateTimeMS(2011, 12, 15, 11, 0));

                // GitHub issue #9 : Event start and end times need timezone support.
                // https://github.com/xamarin/monodroid-samples/issues/9
                eventValues.Put(CalendarContract.Events.InterfaceConsts.EventTimezone, "UTC");
                eventValues.Put(CalendarContract.Events.InterfaceConsts.EventEndTimezone, "UTC");

                var uri = ContentResolver.Insert(CalendarContract.Events.ContentUri, eventValues);
                Console.WriteLine("Uri for new event: {0}", uri);
            };
        }

        class ViewBinder : Java.Lang.Object, SimpleCursorAdapter.IViewBinder
        {
            public bool SetViewValue(View view, Android.Database.ICursor cursor, int columnIndex)
            {
                if (columnIndex == 2)
                {
                    long ms = cursor.GetLong(columnIndex);

                    DateTime date =
                        new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(ms).ToLocalTime();

                    TextView textView = (TextView)view;
                    textView.Text = date.ToLongDateString();

                    return true;
                }
                return false;
            }
        }

        long GetDateTimeMS(int yr, int month, int day, int hr, int min)
        {
            Calendar c = Calendar.GetInstance(Java.Util.TimeZone.Default);

            c.Set(Java.Util.CalendarField.DayOfMonth, 15);
            c.Set(Java.Util.CalendarField.HourOfDay, hr);
            c.Set(Java.Util.CalendarField.Minute, min);
            c.Set(Java.Util.CalendarField.Month, Calendar.December);
            c.Set(Java.Util.CalendarField.Year, 2011);

            return c.TimeInMillis;
        }
    }

}
