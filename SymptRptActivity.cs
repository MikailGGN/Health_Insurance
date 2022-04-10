using Android.App;
using Android.OS;

namespace NewRelish
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar")]
    class SymptRptActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.content_rpt_syptm);
        }
    }
}