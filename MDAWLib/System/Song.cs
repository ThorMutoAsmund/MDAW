using NAudio.Wave;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace MDAWLib1
{
    public class Song
    {
        private readonly static int DefaultSampleRate = 48000;
        public virtual IProvider Provider { get; } = EmptyProvider.Instance;
        public virtual int SampleRate { get; } = DefaultSampleRate;

        public WaveFormat WaveFormat
        {
            get
            {
                this.waveFormat ??= WaveFormat.CreateIeeeFloatWaveFormat(this.SampleRate, 2);

                return this.waveFormat;
            }
        }

        public virtual string Title { get; } = "untitled";

        private int sampleRate = DefaultSampleRate;
        private WaveFormat? waveFormat;

        public Song()
        {
        }
    }
}
