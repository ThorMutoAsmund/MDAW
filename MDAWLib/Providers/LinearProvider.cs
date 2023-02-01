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
    public class LinearProvider : BaseProvider
    {
        public double StartValue { get; private set; }
        public double EndValue { get; private set; }
        public double Time { get; private set; }

        public LinearProvider(double startValue, double endValue, double time)
        {
            this.StartValue = startValue;
            this.EndValue = endValue;
            this.Time = time;
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

                var valuesAreOk = this.Time > 0 && this.EndValue > this.StartValue;

                for (int i = 0; i < count / this.Channels; ++i)
                {
                    double value = this.StartValue;

                    if (valuesAreOk)
                    {
                        value += (this.EndValue - this.StartValue) *
                            (this.Index / 2 + i) / (this.Time * this.SampleRate);
                    }

                    this.outputBuffer[this.Index + i * this.Channels] = (float)Math.Min(value, this.EndValue);

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

