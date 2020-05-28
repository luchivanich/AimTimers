using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace AimTimers.Utils
{
    public class ContextMenuSettings
    {
        public Dictionary<string, ICommand> Commands { get; set; } = new Dictionary<string, ICommand>();

        public Thickness Margin { get; set;}
    }
}
