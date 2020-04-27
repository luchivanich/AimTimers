using System;
using AimTimers.Droid.Renderers;
using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(TimePicker), typeof(CustomTimePickerRenderer))]
namespace AimTimers.Droid.Renderers
{
    class CustomTimePickerRenderer : TimePickerRenderer
    {
        private TimePickerDialog dialog = null;

        public CustomTimePickerRenderer(Context context)
            : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.TimePicker> e)
        {
            base.OnElementChanged(e);
            this.SetNativeControl(new Android.Widget.EditText(Context));
            this.Control.KeyListener = null;
            if (Control != null)
            {
                Control.SetHintTextColor(Android.Graphics.Color.White);
                Control.SetSingleLine(true);
                //Control.SetTypeface(null, TypefaceStyle.Bold);
                //Control.Gravity = GravityFlags.Center;

                // Remove borders
                GradientDrawable gd = new GradientDrawable();
                gd.SetStroke(0, Android.Graphics.Color.LightGray);
                Control.Background = gd;

                Control.Click += Control_Click;
                Control.FocusChange += Control_FocusChange;

                var format = Element.Format;
                if (string.IsNullOrWhiteSpace(format))
                {
                    format = @"hh\:mm";
                }

                var time = Element?.Time ?? default;
                Control.Text = time.ToString(format);
            }
        }

        void Control_Click(object sender, EventArgs e)
        {
            ShowTimePicker();
        }

        void Control_FocusChange(object sender, Android.Views.View.FocusChangeEventArgs e)
        {
            if (e.HasFocus)
                ShowTimePicker();
        }

        private void ShowTimePicker()
        {
            if (dialog == null)
            {
                dialog = new TimePickerDialog(Context, 3, OnTimeSet, Element.Time.Hours, Element.Time.Minutes, true);
            }
            dialog.Show();
        }

        private void OnTimeSet(object sender, TimePickerDialog.TimeSetEventArgs e)
        {
            var time = new TimeSpan(e.HourOfDay, e.Minute, 0);
            this.Element.SetValue(TimePicker.TimeProperty, time);

            this.Control.Text = time.ToString(@"hh\:mm");
        }
    }
}