using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Planum.Ui.Controls
{
    public partial class SvgCanvas : UserControl
    {
        public SvgCanvas()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}