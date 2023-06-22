using System;

namespace LoginsAdmin.Presentation.ViewModels
{
    public class IconTouchedEventArgs : EventArgs
    {
        public string IconName { get; }

        public IconTouchedEventArgs(string iconName)
        {
            IconName = iconName;
        }
    }

}
