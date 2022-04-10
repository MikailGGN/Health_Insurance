using Android.Views;
using Android.Widget;
using System.Collections.Generic;

namespace NewRelish
{
    class ServiceListAdapter : BaseAdapter<HealthServices>
    {
        IList<HealthServices> Hmoservices;
        Service_Activity c;
        public ServiceListAdapter(Service_Activity c, IList<HealthServices> services)
        {
            this.c = c;
            this.Hmoservices = services;
        }
        public override HealthServices this[int position] => Hmoservices[position];

        public override int Count => Hmoservices.Count;


        public override long GetItemId(int position)
        {
            return position;
        }

        /* public override View GetView1(int position, View convertView, ViewGroup parent)
         {
             var view = convertView;

             if (view == null)
             {
                 view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.servicerow, parent, false);

               //  var photo = view.FindViewById<ImageView>(Resource.Id.photoImageView);
                 var name = view.FindViewById<TextView>(Resource.Id.nameTextView);
               //  var state = view.FindViewById<TextView>(Resource.Id.stateTextView);
               //  var contact = view.FindViewById<TextView>(Resource.Id.contactTextView);
               //  var contactdest = view.FindViewById<TextView>(Resource.Id.contactdesTextView);
               //  var contactemail = view.FindViewById<TextView>(Resource.Id.contactemailTextView);
               //  var contactphone = view.FindViewById<TextView>(Resource.Id.contactphoneTextView);
                // var address = view.FindViewById<TextView>(Resource.Id.addressTextView);

                 //   view.Tag = new ViewHolder() { Photo = photo, Name = name, Department = department };
             }

             var holder = (ViewHolder)view.Tag;

            // holder.Photo.SetImageDrawable(ImageManager.Get(parent.Context, services[position].ImageUrl));
             holder.Name.Text = Hmoservices[position].Name;
         //    holder.State.Text = Hmoservices[position].State;
         //    holder.Address.Text = Hmoservices[position].Address;
          //   holder.Contact.Text = Hmoservices[position].Contact;
          //   holder.ContactDesignation.Text = Hmoservices[position].ContactDesignation;
          //   holder.ContactEmail.Text = Hmoservices[position].ContactEmail;
           //  holder.ContactTelephone.Text = Hmoservices[position].ContactTelephone;
            // holder.State.Text = services[position].State;


             return view;

         }*/

        public override View GetView(int position, View convertView, ViewGroup parent)
        {

            View view;


            ///  view = c.LayoutInflater.Inflate(Resource.Layout.gv_lyt, null);

            // for (int i = 0; i < position; i++)
            if (convertView == null)
            {
                view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.servicerow, parent, false);




                // }
            }
            else
            {
                view = convertView;
            }
            view.FindViewById<TextView>(Resource.Id.nameTextView).Text = Hmoservices[position].Name.ToString();
            //  view.FindViewById<TextView>(Resource.Id.textView_rl2).Text = hmoplans[position].PlanName.ToString();
            //  view.FindViewById<TextView>(Resource.Id.textView_rl3).Text = hmoplans[position].PlanProvider.ToString();

            if (position % 2 == 0)
            {
                view.SetBackgroundResource(Resource.Drawable.bg_grey_banner);
            }


            return view;
        }
    }
}