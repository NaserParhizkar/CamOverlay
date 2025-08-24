using Avalonia.Controls;

namespace CamOverlay
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();

        }

        //// Ensure initial measurement on window open
        //private void Window_Opened(object sender, System.EventArgs e)
        //{
        //    if (BorderMainImage.Bounds.Width > 0 && BorderMainImage.Bounds.Height > 0)
        //    {
        //        double overlaySize = Math.Min(BorderMainImage.Bounds.Width, BorderMainImage.Bounds.Height) * 0.1;
        //        _viewModel.OverlaySize = overlaySize;
        //    }
        //}
    }
}