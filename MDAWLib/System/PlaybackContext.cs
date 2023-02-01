using Microsoft.VisualBasic;
using NAudio.Utils;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MDAWLib1
{
    public class PlaybackContext : IWaveProvider
    {
        public static event Action<double>? RenderFinished;

        private static PlaybackContext defaultContext = new PlaybackContext(new Song(), string.Empty);
        public static PlaybackContext Current { get; private set; } = defaultContext;
        public Song Song { get; private set; }
        public string RootPath { get; private set; }
        private IProvider Provider => this.Song.Provider;
        public WaveFormat WaveFormat => this.Song.WaveFormat;
        public int SampleRate => this.Song.WaveFormat.SampleRate;
        public int Channels => this.Song.WaveFormat.Channels;
        public int BytePosition { get; set; }
        public int SamplePosition => this.BytePosition / this.BytesPerSample;

        public long Length => dataChunkSize;

        private const int MinBufferAmount = 14400;
        private int BytesPerSample => this.WaveFormat.BitsPerSample / 8;

        private long dataChunkSize;
        private MemoryStream? outStream;
        private float[]? buffer;

        private PlaybackContext(Song song, string rootPath)
        {
            this.Song = song;
            this.RootPath = rootPath;
            this.BytePosition = 0;
        }

        public static void CreateFromProject(IProjectParameters projectParameters, double offset = 0d)
        {
            if (projectParameters?.Song == null)
            {
                return;
            }

            Current = new PlaybackContext(projectParameters.Song, projectParameters.RootPath);

            Current.Render(20.0);
            Current.ResetPosition();
        }

        public void ResetPosition()
        {
            if (this.outStream != null)
            {
                this.outStream.Seek(0, SeekOrigin.Begin);
                this.BytePosition = 0;
            }
        }

        private void Render(double seconds = 10.0)
        {
            if (this.WaveFormat.Encoding != WaveFormatEncoding.IeeeFloat)
            {
                throw new InvalidOperationException("Only 16, 24 or 32 bit PCM or IEEE float audio data supported");
            }

            this.buffer = BufferHelpers.Ensure(this.buffer, MinBufferAmount);

            this.outStream = new MemoryStream();

            var writer = new BinaryWriter(this.outStream);

            int remaining = (int)(this.SampleRate * seconds * this.Channels);

            this.Provider.Reset();

            while (remaining > 0)
            {
                var count = Math.Min(MinBufferAmount, remaining);
                var actual = this.Provider.Read(this.buffer, 0, count);

                if (actual == 0)
                {
                    break;
                }

                remaining -= actual;

                for (int i = 0; i < actual; i++)
                {
                    writer.Write(this.buffer[i]);
                    this.dataChunkSize += 4L;
                }
            }

            RenderFinished?.Invoke(seconds - (remaining / (double)this.SampleRate));
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            if (buffer == null || this.outStream == null)
            {
                return 0;
            }
            if (this.BytePosition == this.outStream.Length)
            {
                return 0;
            }

            count = this.outStream.Read(buffer, offset, count);

            this.BytePosition += count;

            return count;
        }
    }
}
