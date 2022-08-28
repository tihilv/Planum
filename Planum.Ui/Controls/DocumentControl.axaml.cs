using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Planum.Ui.Controls;

public partial class DocumentControl : UserControl
{
    public DocumentControl()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}