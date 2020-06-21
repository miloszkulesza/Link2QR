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
using Google.Android.Material.Snackbar;
using Link2QR.Services;
using Plugin.CurrentActivity;

namespace Link2QR.Droid
{
    public class SnackbarService : ISnackbarService
    {
        public void ShowSnackbar(string message)
        {
            Activity activity = CrossCurrentActivity.Current.Activity;
            View view = activity.FindViewById(Android.Resource.Id.Content);
            Snackbar.Make(view, message, Snackbar.LengthLong).Show();
        }
    }
}