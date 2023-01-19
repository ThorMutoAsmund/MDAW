using NAudio.Utils;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDAWLib1
{
    public class Mixer : BaseProvider, IHasInputs
    {
        public Output OUT { get; private set; } = new Output();
        public Inputs IN { get; private set; } = new Inputs();

        private float[]? mixBuffer;

        public Mixer()
        {
        }

        public override int Read(float[] buffer, int offset, int count)
        {
            if (this.Failed)
            {
                return 0;
            }

            int outputCount = 0;

            this.mixBuffer = BufferHelpers.Ensure(this.mixBuffer, count);

            int index = this.IN.Count;

            while (index > 0)
            {
                index--;

                var source = this.IN[index];
                int samplesRead = source.Read(this.mixBuffer, 0, count);
                if (samplesRead == 0)
                {
                    continue;
                }

                int outIndex = offset;
                for (int n = 0; n < samplesRead; n++)
                {
                    if (n >= outputCount)
                    {
                        buffer[outIndex++] = this.mixBuffer[n] * source.Gain;
                    }
                    else
                    {
                        buffer[outIndex++] += this.mixBuffer[n] * source.Gain;
                    }
                }
                outputCount = Math.Max(samplesRead, outputCount);
            }

            // optionally ensure we return a full buffer
            //if (outputCount < count)
            //{
            //    int outputIndex = offset + outputCount;
            //    while (outputIndex < offset + count)
            //    {
            //        buffer[outputIndex++] = 0;
            //    }
            //    outputCount = count;
            //}
            return outputCount;
        }
    }
}
