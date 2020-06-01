using AimTimers.Controls;
using AimTimers.Droid.Renderers;
using Android.Content;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ContextMenuButton), typeof(ContextMenuButtonRenderer))]
namespace AimTimers.Droid.Renderers
{
    public class ContextMenuButtonRenderer : VisualElementRenderer<Xamarin.Forms.View>
    {
        public ContextMenuButtonRenderer(Context context)
            : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.View> e)
        {
            base.OnElementChanged(e);
            if (Element is ContextMenuButton contextButton)
            {
                contextButton.GetCoordinates = GetCoordinatesNative;
            }
        }

        private (int x, int y) GetCoordinatesNative()
        {
            var density = Resources.DisplayMetrics.Density;

            var coords = new int[2];
            GetLocationOnScreen(coords);
            return ((int)(coords[0]/density), (int)(coords[1]/density));
        }
    }
}