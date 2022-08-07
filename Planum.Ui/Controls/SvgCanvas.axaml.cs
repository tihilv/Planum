using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Planum.Ui.ViewModels;

namespace Planum.Ui.Controls
{
    public partial class SvgCanvas : UserControl
    {
        private Point _currentPoint;
        private bool _moving;

        public SvgCanvas()
        {
            InitializeComponent();
        }

        private CanvasViewModel? ViewModel => (CanvasViewModel?)DataContext;
        
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        protected override void ArrangeCore(Rect finalRect)
        {
            base.ArrangeCore(finalRect);
            ViewModel?.Resize(finalRect.Width, finalRect.Height);
        }

        protected override void OnPointerMoved(PointerEventArgs e)
        {
            base.OnPointerMoved(e);
            var newPosition = e.GetPosition(this);
            if (_moving)
                ViewModel?.Move(_currentPoint - newPosition);
            else
                ViewModel?.SetCursor(newPosition);
            _currentPoint = newPosition;
        }

        protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
        {
            var delta = 1.1;
            base.OnPointerWheelChanged(e);
            ViewModel?.Zoom(e.GetPosition(this), e.Delta.Y > 0 ? delta : 1/delta);
        }

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);
            if (e.GetCurrentPoint(this).Properties.IsMiddleButtonPressed)
            {
                if (e.ClickCount == 2)
                    ViewModel?.ZoomToExtents();
                else
                    _moving = true;
            }
        }

        protected override void OnPointerReleased(PointerReleasedEventArgs e)
        {
            if (!e.GetCurrentPoint(this).Properties.IsMiddleButtonPressed)
                _moving = false;
        }
    }
}