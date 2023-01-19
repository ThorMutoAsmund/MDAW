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
        public bool Failed { get; set; }
        public string Failure { get; set; } = String.Empty;

        private PlaybackContext Context => PlaybackContext.Current;

        protected void Fail(string failure)
        {
            this.Failed = true;
            this.Failure = failure;
        }

        public abstract int Read(float[] buffer, int offset, int count);
    }
}

