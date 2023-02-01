using NAudio.Utils;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDAWLib1
{
    public class Mixer : MixerBase
    {
        public Providers Tracks { get; private set; } = new Providers();
        public override IEnumerable<IProvider> Inputs => this.Tracks;

        protected List<IProvider> remainingInputs = new List<IProvider>();

        public override void Reset()
        {
            foreach (var input in this.Inputs)
            {
                input.Reset();
            }

            this.remainingInputs = this.Inputs.ToList();
        }

        public override int Read(float[] buffer, int offset, int count)
        {
            if (this.Finished)
            {
                return 0;
            }

            int outputCount = 0;

            EnsureMixBuffer(count);

            if (this.mixBuffer == null)
            {
                return 0;
            }

            int p = this.remainingInputs.Count;

            while (p > 0)
            {
                p--;

                var source = remainingInputs[p];

                int samplesRead = source.Read(this.mixBuffer, 0, count);
                if (samplesRead == 0)
                {
                    continue;
                }

                for (int n = 0; n < samplesRead; n++)
                {
                    if (n >= outputCount)
                    {
                        buffer[offset + n] = this.mixBuffer[n];
                    }
                    else
                    {
                        buffer[offset + n] += this.mixBuffer[n];
                    }
                }
                outputCount = Math.Max(samplesRead, outputCount);
            }

            this.remainingInputs.RemoveAll(input => input.Finished);

            this.Finished = this.remainingInputs.Count == 0;

            return outputCount;
        }

    }
}
