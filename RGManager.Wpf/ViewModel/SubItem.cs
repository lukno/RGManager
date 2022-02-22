using System.Windows.Controls;

namespace RGManager.Wpf.ViewModel
{
    public class SubItem
    {
        public SubItem(string name, UserControl screen = null)
        {
            Name = name;
            Screen = screen;
        }

        public string Name { get; }
        public UserControl Screen { get; }
    }
}