namespace Planum.Ui.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string Greeting => "Welcome to Avalonia!";

        public CanvasViewModel Canvas { get; }

        public MainWindowViewModel()
        {
            Canvas = new CanvasViewModel();
        }
    }
}