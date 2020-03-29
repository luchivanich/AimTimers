using System;
using AimTimers.Droid.Renderers;
using Android.App;
using Android.Content;
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
                Control.Click += Control_Click;
                Control.FocusChange += Control_FocusChange;

                if (Element != null && !Element.Time.Equals(default(TimeSpan)))
                    Control.Text = Element.Time.ToString(@"hh\:mm");
                else
                    Control.Text = "00:00";
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