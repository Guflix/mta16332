using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;


namespace FinalApp
{
    [Activity(Label = "XamarinCamera")]
    public class Activity2 : Activity
    {
        ImageView imageView2;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Activity2);
            imageView2 = FindViewById<ImageView>(Resource.Id.imageView2);


            Bitmap image2 = (Bitmap)Intent.GetParcelableExtra("MyData");


            //Bitmap bitmap = (Bitmap)Intent.Extras.Get("data");
            imageView2.SetImageBitmap(image2);


        }
    }
}