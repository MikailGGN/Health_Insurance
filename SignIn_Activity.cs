using Android.App;
using Android.Gms.Tasks;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using Firebase.Auth;

namespace NewRelish
{
    [Activity(Label = "SignIn_Activity")]
    public class SignIn_Activity : AppCompatActivity, IOnCompleteListener

    {

        FirebaseAuth auth;

        public void OnComplete(Task task)
        {
            if (task.IsComplete)
            {
                Toast.MakeText(this, "Sign In Successful !", ToastLength.Short).Show();
                Finish();
            }
            else
            {
                Toast.MakeText(this, "Sing In field !", ToastLength.Short).Show();
                Finish();
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SignIn);
            auth = FirebaseAuth.Instance;
            // Create your application here
            var edtEmail = FindViewById<EditText>(Resource.Id.edtEmail);
            var edtPassword = FindViewById<EditText>(Resource.Id.edtPassword);
            var btnSignIn = FindViewById<Button>(Resource.Id.btnSingIn);
            btnSignIn.Click += delegate
            {
                auth.CreateUserWithEmailAndPassword(edtEmail.Text, edtPassword.Text)
                .AddOnCompleteListener(this);
            };
        }
    }
}