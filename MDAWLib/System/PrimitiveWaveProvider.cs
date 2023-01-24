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

        public PrimitiveWaveProvider(PrimitiveWaveType waveType, double frequency = 440.0)
        {
            this.WaveType = waveType;
            this.Frequency = frequency;
        }

        public override int Read(float[] buffer, int offset, int count)
        {
            if (this.Finished)
            {
                return 0;
            }

            var f = 1.0 / this.SampleRate * 2 * Math.PI * this.Frequency;

            for (int i = 0; i < count / this.Channels; ++i)
            {
                buffer[offset + i * this.Channels] = (float)(Math.Sin((i + this.Index / 2) * f) * 2f - 1f);
                if (this.Channels > 1)
                {
                    buffer[offset + i * this.Channels + 1] = buffer[offset + i * this.Channels];
                }

            }
            this.Index += count;

            return count;
        }
    }
}

