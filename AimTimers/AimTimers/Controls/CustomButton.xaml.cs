using Xamarin.Forms;

namespace AimTimers.Controls
{
    public partial class CustomButton : ContentView
    {
        public static readonly BindableProperty IconCodeProperty = BindableProperty.Create(
            nameof(IconCode),
            typeof(string),
            typeof(CustomButton),
            propertyChanged: OnIconCodeChanged);

        static void OnIconCodeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue == null)
            {
                return;
            }

            var control = (CustomButton)bindable;
            control.IconCodeLabel.Text = newValue.ToString();
        }

        public string IconCode
        {
            get { return (string)GetValue(IconCodeProperty); }
            set { SetValue(IconCodeProperty, value); }
        }

        public static readonly BindableProperty TextProperty = BindableProperty.Create(
            nameof(Text),
            typeof(string),
            typeof(CustomButton),
            propertyChanged: OnTextChanged);

        static void OnTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CustomButton)bindable;
            control.TextLabel.Text = newValue.ToString();
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public CustomButton()
        {
            InitializeComponent();
        }
    }
}