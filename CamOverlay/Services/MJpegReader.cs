using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CamOverlay.Services
{
    public static class MJpegReader
    {
        public static async IAsyncEnumerable<byte[]> ReadFramesAsync(Stream input, [EnumeratorCancellation] CancellationToken ct)
        {
            var buffer = ArrayPool<byte>.Shared.Rent(8192);
            try
            {
                var frame = new MemoryStream();
                bool inFrame = false;
                int prev = -1;

                while (!ct.IsCancellationRequested)
                {
                    int read = await input.ReadAsync(buffer.AsMemory(0, buffer.Length), ct);
                    if (read <= 0) yield break;

                    for (int i = 0; i < read; i++)
                    {
                        int b = buffer[i];
                        if (!inFrame)
                        {
                            if (prev == 0xFF && b == 0xD8)
                            {
                                frame.SetLength(0);
                                frame.WriteByte(0xFF);
                                frame.WriteByte(0xD8);
                                inFrame = true;
                                prev = -1;
                                continue;
                            }
                        }
                        else
                        {
                            frame.WriteByte((byte)b);
                            if (prev == 0xFF && b == 0xD9)
                            {
                                yield return frame.ToArray();
                                inFrame = false;
                                prev = -1;
                                continue;
                            }
                        }
                        prev = b;
                    }
                }
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }
    }
}