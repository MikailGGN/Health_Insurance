using Android.Views;
using Android.Widget;
using System.Collections.Generic;

namespace NewRelish
{
    class GridAdapter : BaseAdapter<HealthPlan>
    {

        PlansActivity c;
        IList<HealthPlan> hmoplans;



        public GridAdapter(PlansActivity c, IList<HealthPlan> hmoplans)
        {
            this.c = c;
            this.hmoplans = hmoplans;
        }

        public override HealthPlan this[int position] => hmoplans[position];

        public override int Count => hmoplans.Count;

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
                view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.gv_lyt, parent, false);




                // }
            }
            else
            {
                view = convertView;
            }

            view.FindViewById<TextView>(Resource.Id.textView_rl1).Text = hmoplans[position].PlanId.ToString();
            view.FindViewById<TextView>(Resource.Id.textView_rl2).Text = hmoplans[position].PlanName.ToString();
            view.FindViewById<TextView>(Resource.Id.textView_rl3).Text = hmoplans[position].PlanProvider.ToString();


            if (position % 5 == 0)
            {
                view.SetBackgroundResource(Resource.Drawable.bg_blue_banner);
            }
            else if (position % 3 == 0)
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




