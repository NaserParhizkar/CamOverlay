using Avalonia.Media.Imaging;
using Avalonia.Threading;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CamOverlay.Services
{
    public class CameraService
    {
        private Process? _proc;
        private CancellationTokenSource? _cts;

        public event Action<Bitmap>? FrameReady;
        public event Action<string>? StatusChanged;

        public async Task StartAsync()
        {
            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = "libcamera-vid",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false
                };

                psi.ArgumentList.Add("--codec"); psi.ArgumentList.Add("mjpeg");
                psi.ArgumentList.Add("--width"); psi.ArgumentList.Add("480");
                psi.ArgumentList.Add("--height"); psi.ArgumentList.Add("640");
                psi.ArgumentList.Add("--framerate"); psi.ArgumentList.Add("30");
                psi.ArgumentList.Add("-t"); psi.ArgumentList.Add("0");
                psi.ArgumentList.Add("--nopreview");
                psi.ArgumentList.Add("-o"); psi.ArgumentList.Add("-");

                _proc = new Process { StartInfo = psi };
                _proc.Start();
                _cts = new CancellationTokenSource();

                await foreach (var frameBytes in MJpegReader.ReadFramesAsync(_proc.StandardOutput.BaseStream, _cts.Token))
                {
                    using var ms = new MemoryStream(frameBytes, writable: false);
                    var bmp = new Bitmap(ms);

                    Dispatcher.UIThread.Post(() =>
                    {
                        FrameReady?.Invoke(bmp);
                    });
                }
            }
            catch (Exception ex)
            {
                StatusChanged?.Invoke($"Camera error: {ex.Message}");
            }
        }
    }
}