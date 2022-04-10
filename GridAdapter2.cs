using Android.App;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;

namespace NewRelish
{
    class GridAdapter2 : BaseAdapter<PlanProvider>
    {

        ClaimActivity c;
        IList<PlanProvider> hmoproviders;



        public GridAdapter2(ClaimActivity c, IList<PlanProvider> hmoproviders)
        {
            this.c = c;
            this.hmoproviders = hmoproviders;
        }

        public override PlanProvider this[int position] => hmoproviders[position];

        public override int Count => hmoproviders.Count;

        public override long GetItemId(int position)
        {
            return position;
        }

        /*  public override View GetView1(int position, View convertView, ViewGroup parent)
        {
              if (inflater == null)
             {
              inflater = (LayoutInflater)c.GetSystemService(Context.LayoutInflaterService);
           }

            if (convertView == null)
            {
                convertView = inflater.Inflate(Resource.Layout.gv_lyt, parent, false);
            }

           // View view;

           // if (convertView == null)
           // {
           //     view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.gv_lyt, parent, false);
           // }
          //  else
          //  {
           //     view = convertView;
           // }

            TextView Txt1 = convertView.FindViewById<TextView>(Resource.Id.textView_rl1);
            TextView Txt2 = convertView.FindViewById<TextView>(Resource.Id.textView_rl2);
            TextView Txt3 = convertView.FindViewById<TextView>(Resource.Id.textView_rl3);


            //BIND

            Txt1.Text = Convert.ToString( hmoplans[position].PlanId);
            Txt2.Text = hmoplans[position].PlanName;
            Txt3.Text = hmoplans[position].PlanProvider;
           


            return convertView;


            ////
            ///
           

        }
      */
        public override View GetView(int position, View convertView, ViewGroup parent)
        {

            View view;


            ///  view = c.LayoutInflater.Inflate(Resource.Layout.gv_lyt, null);

            // for (int i = 0; i < position; i++)
            if (convertView == null)
            {
                view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.gv_lyt2, parent, false);




                // }
            }
            else
            {
                view = convertView;

            }
            ImageView img = view.FindViewById<ImageView>(Resource.Id.imageView1);

            var myDrawableName = hmoproviders[position].ProviderImage;
            var myPackageName = Application.Context.PackageName;
            var resourceType = "drawable";

            var resourceId = Application.Context.Resources.GetIdentifier(myDrawableName, resourceType, myPackageName);

            img.SetImageResource((int)resourceId);
            view.FindViewById<TextView>(Resource.Id.textView_rl1).Text = hmoproviders[position].ProviderId.ToString();
            view.FindViewById<TextView>(Resource.Id.textView_rl2).Text = hmoproviders[position].Provider.ToString();


            if (position % 2 == 0)
            {
                view.SetBackgroundResource(Resource.Drawable.bg_rasberry_banner);
            }
            else
            {
                view.SetBackgroundResource(Resource.Drawable.bg_choco_banner);
            }
            return view;
        }
    }
}




