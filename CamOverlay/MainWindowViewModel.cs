using Avalonia.Media;
using Avalonia.Media.Imaging;
using CamOverlay.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CamOverlay
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private Bitmap? _currentFrame;
        private IBrush _guideBoxColor = Brushes.Lime;
        private string _statusText = "Starting...";

        private readonly CameraService _cameraService;

        public Bitmap? CurrentFrame
        {
            get => _currentFrame;
            set { _currentFrame = value; OnPropertyChanged(); }
        }

        public string StatusText
        {
            get => _statusText;
            set { _statusText = value; OnPropertyChanged(); }
        }

        public MainWindowViewModel()
        {
            _cameraService = new CameraService();
            _cameraService.FrameReady += (bmp) =>
            {
                CurrentFrame = bmp;
            };
            _cameraService.StatusChanged += msg => StatusText = msg;
            _cameraService.StartAsync();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}