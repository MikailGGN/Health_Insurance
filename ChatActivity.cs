using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Xamarin.Database;
using System;
using System.Collections.Generic;
using static Android.Views.View;
using Object = Java.Lang.Object;

namespace NewRelish
{
    [Activity(Label = "ChatActivity", Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class ChatActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener, IValueEventListener, IOnClickListener
    {
        private FirebaseClient firebaseClient;
        private List<MessageContent> lstMessage = new List<MessageContent>();
        private ListView lstChat;
        private EditText edtChat;
        private FloatingActionButton fab;
        public static FirebaseApp app;
        FirebaseAuth auth;

        //      FirebaseAuth auth;
        //
        public int MyResultCode = 1;
        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
        }
        private void InitFirebaseAuth()
        {
            var options = new FirebaseOptions.Builder()
                .SetApplicationId("1:920083122070:android:018804ec92b4724a63bfb7")
                .SetApiKey("AIzaSyBVxdAlL9FORUH_psnhRikJRfmfDFZmcyw")
               .SetDatabaseUrl("https://newrelish-bc4be-default-rtdb.europe-west1.firebasedatabase.app")
               .Build();

            if (app == null)
                app = FirebaseApp.InitializeApp(this, options);
            auth = FirebaseAuth.GetInstance(app);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //  SetContentView(Resource.Layout.activity_chat);
            SetContentView(Resource.Layout.chatter);
            //Init Firebase
            //  InitFirebaseAuth();
            //  auth = FirebaseAuth.GetInstance(MainActivity.app);


            // Firebase.FirebaseApp.InitializeApp(this);
            firebaseClient = new FirebaseClient(GetString(Resource.String.firebase_database_url));

            FirebaseDatabase.Instance.GetReference("chats").AddValueEventListener(this);
            fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            edtChat = FindViewById<EditText>(Resource.Id.input);
            lstChat = FindViewById<ListView>(Resource.Id.list_of_messages);

            fab.Click += delegate { PostMessage(); };

            if (FirebaseAuth.Instance.CurrentUser == null)
            {
                //  StartActivityForResult(new Android.Content.Intent(this, typeof(SignInActivity)), MyResultCode);
            }
            else
            {
                Toast.MakeText(this, "Welcome" + FirebaseAuth.Instance.CurrentUser.Email, ToastLength.Short).Show();
                DisplayChatMessage();
            }


        }

        public bool OnNavigationItemSelected(IMenuItem menuItem)
        {
            throw new NotImplementedException();
        }

        private async void PostMessage()
        {
            var items = await firebaseClient.Child("chats").PostAsync(new MessageContent(FirebaseAuth.Instance.CurrentUser.Email, edtChat.Text));
            edtChat.Text = "";
        }
        public void OnCancelled(DatabaseError error)
        {

        }

        public void OnDataChange(DataSnapshot snapshot)
        {
            DisplayChatMessage();
        }

        private async void DisplayChatMessage()
        {
            lstMessage.Clear();
            var items = await firebaseClient.Child("chats")
                .OnceAsync<MessageContent>();
            foreach (var item in items)
                lstMessage.Add(item.Object);
            ListViewAdapter adapter = new ListViewAdapter(this, lstMessage);
            lstChat.Adapter = adapter;
        }

        public void OnClick(View v)
        {
            //  throw new NotImplementedException();

            if (v.Id == null) //.Id.dashboard_btn_change_pass)
                              // ChangePassword(input_new_password.Text);
                Snackbar.Make(v, "Replace with your own action", Snackbar.LengthLong)
                .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
            //   else if (v.Id == Resource.Id.dashboard_btn_logout)
            //     LogoutUser();

        }
    }


    internal class MessageContent
    {
        public string Email { get; set; }
        public string Message { get; set; }
        public string Time { get; set; }
        public MessageContent() { }
        public MessageContent(string Email, string Message)
        {
            this.Email = Email;
            this.Message = Message;
            Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }

    internal class ListViewAdapter : BaseAdapter
    {
        private ChatActivity chatActivity;
        private List<MessageContent> lstMessage;

        public ListViewAdapter(ChatActivity chatActivity, List<MessageContent> lstMessage)
        {
            this.chatActivity = chatActivity;
            this.lstMessage = lstMessage;
        }

        public override int Count
        {
            get { return lstMessage.Count; }
        }

        public override Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            LayoutInflater inflater = (LayoutInflater)chatActivity.BaseContext.GetSystemService(Context.LayoutInflaterService);
            View ItemView = inflater.Inflate(Resource.Layout.List_item, null);
            TextView message_user, message_time, message_content;
            message_user = ItemView.FindViewById<TextView>(Resource.Id.message_user);
            message_time = ItemView.FindViewById<TextView>(Resource.Id.message_time);
            message_content = ItemView.FindViewById<TextView>(Resource.Id.message_text);

            message_user.Text = lstMessage[position].Email;
            message_time.Text = lstMessage[position].Time;
            message_content.Text = lstMessage[position].Message;

            return ItemView;
        }
    }
}