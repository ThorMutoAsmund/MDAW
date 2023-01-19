using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDAWLib1
{
    public class Part : ISampleProvider
    {
        public WaveFormat WaveFormat => this.SampleProvider.WaveFormat;
        public ISampleProvider SampleProvider { get; set; }
        public int StartAt { get; set; }
        public float Gain { get; set; }

        public Part(ISampleProvider sampleProvider, int startAt = 0, float gain = 1f)
        {
            this.SampleProvider = sampleProvider;
            this.StartAt = startAt;
            this.Gain = gain;
        }

        public int Read(float[] buffer, int offset, int count)
        {
            return this.SampleProvider.Read(buffer, offset, count);
        }
    }
}
