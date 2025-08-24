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
        //private double _imageWidth;
        //private double _imageHeight;
        //private double _overlaySize;
        //private double _scale = 0.6; // 60%, change to 0.7 for 70%

        //public event PropertyChangedEventHandler PropertyChanged;

        //public double ImageWidth
        //{
        //    get => _imageWidth;
        //    set
        //    {
        //        if (_imageWidth != value)
        //        {
        //            _imageWidth = value;
        //            OnPropertyChanged();
        //            UpdateOverlaySize();
        //        }
        //    }
        //}

        //public double ImageHeight
        //{
        //    get => _imageHeight;
        //    set
        //    {
        //        if (_imageHeight != value)
        //        {
        //            _imageHeight = value;
        //            OnPropertyChanged();
        //            UpdateOverlaySize();
        //        }
        //    }
        //}

        //public double OverlaySize
        //{
        //    get => _overlaySize;
        //    set
        //    {
        //        if (_overlaySize != value)
        //        {
        //            _overlaySize = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}

        //public double Scale
        //{
        //    get => _scale;
        //    set
        //    {
        //        if (_scale != value)
        //        {
        //            _scale = value;
        //            OnPropertyChanged();
        //            UpdateOverlaySize();
        //        }
        //    }
        //}

        //private void UpdateOverlaySize()
        //{
        //    var minDim = _imageWidth < _imageHeight ? _imageWidth : _imageHeight;
        //    OverlaySize = minDim * _scale;
        //}

        //private void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

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