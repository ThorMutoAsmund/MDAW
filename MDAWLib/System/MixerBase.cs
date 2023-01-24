using NAudio.Utils;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDAWLib1
{
    public abstract class MixerBase : BaseProvider, IHasInputs
    {
        public abstract IEnumerable<IProvider> Inputs { get; }

        private float[]? mixBuffer;

        public MixerBase()
        {
        }

        public override void Reset()
        {
            foreach (var input in this.Inputs)
            {
                input.Reset();
            }
        }

        public override int Read(float[] buffer, int offset, int count)
        {
            if (this.Failed)
            {
                return 0;
            }

            int outputCount = 0;

            this.mixBuffer = BufferHelpers.Ensure(this.mixBuffer, count);

            var inputArray = this.Inputs.ToArray();
            int index = inputArray.Length;

            while (index > 0)
            {
                index--;

                var source = inputArray[index];

                // Here: Tell source what the current song position is? Relative position?

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
                        buffer[outIndex++] = this.mixBuffer[n];// * source.Gain;
                    }
                    else
                    {
                        buffer[outIndex++] += this.mixBuffer[n];// * source.Gain;
                    }
                }
                outputCount = Math.Max(samplesRead, outputCount);
            }

            return outputCount;
        }
    }
}
