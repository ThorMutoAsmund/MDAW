using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDAWLib1
{
    public class PrimitiveWaveProvider : BaseProvider
    {
        public PrimitiveWaveType WaveType { get; private set; }
        public double Frequency { get; private set; }

        private int ramp = 0;

        public PrimitiveWaveProvider(PrimitiveWaveType waveType, double frequency = 440.0)
        {
            this.WaveType = waveType;
            this.Frequency = frequency;
        }

        public override void Reset()
        {
            this.ramp = 0;
        }

        public override int Read(float[] buffer, int offset, int count)
        {
            if (this.Failed)
            {
                return 0;
            }

            var f = 1.0 / this.WaveFormat.Channels / this.SampleRate * 2 * Math.PI * this.Frequency;

            for (int i = 0; i < count; ++i)
            {
                buffer[offset + i] = (float)(Math.Sin((i + ramp) * f) * 2f - 1f);
            }
            ramp += count;

            return count;
        }
    }
}

