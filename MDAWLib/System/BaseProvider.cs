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
        public WaveFormat WaveFormat => Context.Song.WaveFormat;
        public int SampleRate => Context.Song.WaveFormat.SampleRate;
        public int Channels => Context.Song.WaveFormat.Channels;
        public bool Failed { get; set; }
        public string Failure { get; set; } = String.Empty;

        protected static PlaybackContext Context => PlaybackContext.Current;

        protected void Fail(string failure)
        {
            this.Failed = true;
            this.Failure = failure;
        }

        public abstract int Read(float[] buffer, int offset, int count);
    }
}

