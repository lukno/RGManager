using MaterialDesignThemes.Wpf;
using System.Collections.Generic;
using System.Windows.Controls;

namespace RGManager.Wpf.ViewModel
{
    public class ItemMenu
    {
        public ItemMenu(string header, List<SubItem> subItems, PackIconKind icon)
        {
            Header = header;
            SubItems = subItems;
            Icon = icon;
        }

        public string Header { get; }
        public List<SubItem> SubItems { get; }
        public PackIconKind Icon { get; }
        public UserControl Screen { get; }
    }
}
