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
        private static PlaybackContext defaultContext = new PlaybackContext(new Song());
        public static PlaybackContext Current { get; private set; } = defaultContext;
        public Song Song { get; private set; }
        private ISampleProvider SampleProvider => this.Song.SampleProvider;
        public WaveFormat WaveFormat => this.Song.WaveFormat;
        public int SampleRate => this.Song.WaveFormat.SampleRate;
        public int SamplePosition { get; set; }


        public long Length => dataChunkSize;
        
        private long dataChunkSize;
        //private readonly byte[] value24 = new byte[3];
        private MemoryStream? outStream;

        private PlaybackContext(Song song)
        {
            this.Song = song;
            this.SamplePosition = 0;
        }

        public static void CreateFromSong(Song song, double offset = 0d)
        {
            var context = new PlaybackContext(song);

            context.Render(song.Duration ?? 10.0);
            context.ResetPosition();

            Current = context;
        }

        private void ResetPosition()
        {
            if (this.outStream != null)
            {
                this.outStream.Seek(0, SeekOrigin.Begin);
                this.SamplePosition = 0;
            }
        }

        private void Render(double seconds = 10.0)
        {
            if (this.WaveFormat.Encoding != WaveFormatEncoding.IeeeFloat)
            {
                throw new InvalidOperationException("Only 16, 24 or 32 bit PCM or IEEE float audio data supported");
            }

            var buffer = new float[14400];

            this.outStream = new MemoryStream();
            
            var writer = new BinaryWriter(this.outStream, Encoding.UTF8);

            int remaining = (int)(this.SampleRate * seconds);

            while (remaining > 0)
            {
                var count = Math.Min(14400, remaining);
                var actual = this.SampleProvider.Read(buffer, 0, count);

                remaining -= count;

                for (int i = 0; i < count; i++)
                {
                    writer.Write(buffer[i]);
                    this.dataChunkSize += 4L;
                }
            }

            this.outStream.Seek(0, SeekOrigin.Begin);

            byte[] test = new byte[32];

            this.outStream.Read(test, 0, 32);
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
            if (buffer != null && this.outStream != null)
            {
                this.outStream.Read(buffer, offset, count);

                this.SamplePosition += count / 2;
            }

            return count;
        }
    }
}
