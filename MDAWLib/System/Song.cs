using NAudio.Wave;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDAWLib1
{
    public class Song
    {
        private readonly static int DefaultSampleReate = 48000;
        public virtual IProvider Provider => EmptyProvider.Instance;
        public int SampleRate
        {
            get => this.sampleRate;
            set
            {
                this.sampleRate = value;
                this.waveFormat = null;
            }
        }

        public WaveFormat WaveFormat
        {
            get
            {
                this.waveFormat ??= WaveFormat.CreateIeeeFloatWaveFormat(this.SampleRate, 2);

                return this.waveFormat;
            }
        }

        public virtual string Title { get; } = "untitled";

        private int sampleRate = DefaultSampleReate;
        private WaveFormat? waveFormat;

        public Song()
        {
        }
    }
}
