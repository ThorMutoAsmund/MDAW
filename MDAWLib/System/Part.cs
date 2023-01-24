using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDAWLib1
{
    public class Part : IProvider
    {
        public WaveFormat WaveFormat => this.Provider.WaveFormat;
        public IProvider Provider { get; set; }
        public int StartAt { get; set; }
        public float Gain { get; set; }

        public Part(IProvider provider, int startAt = 0, float gain = 1f)
        {
            this.Provider = provider;
            this.StartAt = startAt;
            this.Gain = gain;
        }

        public void Reset()
        {
            this.Provider.Reset();
        }

        public int Read(float[] buffer, int offset, int count)
        {
            return this.Provider.Read(buffer, offset, count);
        }
    }
}
