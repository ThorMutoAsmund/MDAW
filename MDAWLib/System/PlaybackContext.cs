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

        private static PlaybackContext defaultContext = new PlaybackContext(new Song());
        public static PlaybackContext Current { get; private set; } = defaultContext;
        public Song Song { get; private set; }
        private IProvider Provider => this.Song.Provider;
        public WaveFormat WaveFormat => this.Song.WaveFormat;
        public int SampleRate => this.Song.WaveFormat.SampleRate;
        public int BytePosition { get; set; }
        public int SamplePosition => this.BytePosition / this.BytesPerSample;

        public long Length => dataChunkSize;

        private int BytesPerSample => this.WaveFormat.BitsPerSample / 8;

        private long dataChunkSize;
        private MemoryStream? outStream;

        private PlaybackContext(Song song)
        {
            this.Song = song;
            this.BytePosition = 0;
        }

        public static void CreateFromSong(Song song, double offset = 0d)
        {
            var context = new PlaybackContext(song);

            context.Render(10.0);
            context.ResetPosition();

            Current = context;
        }

        public void ResetPosition()
        {
            if (this.outStream != null)
            {
                this.outStream.Seek(0, SeekOrigin.Begin);
                this.BytePosition = 0;
            }
        }

        private float[]? buffer;
        private void Render(double seconds = 10.0)
        {
            if (this.WaveFormat.Encoding != WaveFormatEncoding.IeeeFloat)
            {
                throw new InvalidOperationException("Only 16, 24 or 32 bit PCM or IEEE float audio data supported");
            }

            this.buffer = BufferHelpers.Ensure(this.buffer, 14400);

            this.outStream = new MemoryStream();

            var writer = new BinaryWriter(this.outStream);

            int remaining = (int)(this.SampleRate * seconds);

            if (this.Provider is IProvider baseProvider)
            {
                baseProvider.Reset();
            }            

            while (remaining > 0)
            {
                var count = Math.Min(14400, remaining);
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

        //public void WriteSample(float sample)
        //{
        //    if (this.writer == null)
        //    {
        //        return;
        //    }

        //    if (WaveFormat.BitsPerSample == 16)
        //    {
        //        this.writer.Write((short)(32767f * sample));
        //        this.dataChunkSize += 2L;
        //    }
        //    else if (this.WaveFormat.BitsPerSample == 24)
        //    {
        //        byte[] bytes = BitConverter.GetBytes((int)(2.14748365E+09f * sample));
        //        this.value24[0] = bytes[1];
        //        this.value24[1] = bytes[2];
        //        this.value24[2] = bytes[3];
        //        this.writer.Write(value24);
        //        this.dataChunkSize += 3L;
        //    }
        //    else if (this.WaveFormat.BitsPerSample == 32 && this.WaveFormat.Encoding == WaveFormatEncoding.Extensible)
        //    {
        //        this.writer.Write(65535 * (int)sample);
        //        this.dataChunkSize += 4L;
        //    }
        //    else
        //    {
        //        if (this.WaveFormat.Encoding != WaveFormatEncoding.IeeeFloat)
        //        {
        //            throw new InvalidOperationException("Only 16, 24 or 32 bit PCM or IEEE float audio data supported");
        //        }

        //        this.writer.Write(sample);
        //        this.dataChunkSize += 4L;
        //    }
        //}

        //public void WriteSamples(float[] samples, int offset, int count)
        //{
        //    for (int i = 0; i < count; i++)
        //    {
        //        WriteSample(samples[offset + i]);
        //    }
        //}


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
