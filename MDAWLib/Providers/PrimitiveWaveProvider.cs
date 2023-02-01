using NAudio.Utils;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDAWLib1
{
    public class PrimitiveWaveProvider : BaseProvider
    {
        public PrimitiveWaveType WaveType { get; private set; }
        public Parameter Frequency { get; private set; }

        public PrimitiveWaveProvider(PrimitiveWaveType waveType, double frequency = 440.0)
        {
            this.WaveType = waveType;
            this.Frequency = new Parameter(frequency);
        }

        public override void Reset()
        {
            base.Reset();

            this.Frequency.Reset();
        }


        public override int Read(float[] buffer, int offset, int count)
        {
            if (this.Finished)
            {
                return 0;
            }

            if (this.outputBuffer == null || this.Index + count < this.outputBuffer.Length)
            {
                EnsureOutputBuffer(this.Index + count);

                if (this.outputBuffer == null)
                {
                    return 0;
                }

                this.Frequency.Read(count);

                for (int i = 0; i < count / this.Channels; ++i)
                {
                    var f = 2 * Math.PI * this.Frequency[this.Index + i * this.Channels] / this.SampleRate;

                    this.outputBuffer[this.Index + i * this.Channels] = (float)(Math.Sin((i + this.Index / 2) * f));

                    if (this.Channels > 1)
                    {
                        this.outputBuffer[this.Index + i * this.Channels + 1] = this.outputBuffer[this.Index + i * this.Channels];
                    }
                }
            }

            Copy(count, outputBuffer, this.Index, buffer, offset);

            this.Index += count;

            return count;
        }
    }
}

