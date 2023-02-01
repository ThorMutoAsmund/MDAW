using NAudio.Utils;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDAWLib1
{
    public abstract class BaseProvider : IProvider
    {
        protected const int LEFT = 0;
        protected const int RIGHT = 1;
        public WaveFormat WaveFormat => this.Context.Song.WaveFormat;
        public int SampleRate => this.Context.Song.WaveFormat.SampleRate;
        public int Channels => this.Context.Song.WaveFormat.Channels;
        public bool Finished { get; set; }
        public string Failure { get; set; } = String.Empty;
        protected float[]? OutputBuffer => this.outputBuffer;

        protected int Index { get; set; }

        protected float[]? outputBuffer;


        protected PlaybackContext Context => PlaybackContext.Current;

        protected void Fail(string failure)
        {
            this.Finished = true;
            this.Failure = failure;
        }

        public virtual void Reset()
        {
            this.outputBuffer = null;
            this.Index = 0;
            this.Finished = false;
        }

        protected void Finish(bool finishIf = true)
        {
            this.Finished = finishIf;
        }

        public abstract int Read(float[] buffer, int offset, int count);

        protected void EnsureOutputBuffer(int count)
        {
            this.outputBuffer = BufferHelpers.Ensure(this.outputBuffer, count);
        }

        protected void Copy<T>(int count, T[]? source, int sourceOffset, T[]? destination, int destinationOffset) where T: struct
        {
            if (source == null || destination == null)
            {
                return;
            }

            for (int i=0; i< Math.Min(count, Math.Min(source.Length - sourceOffset, destination.Length - destinationOffset)); ++i)
            {
                destination[i + destinationOffset] = source[i + sourceOffset];
            }
        }
    }
}

