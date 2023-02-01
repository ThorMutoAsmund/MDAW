using NAudio.Utils;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDAWLib1
{
    public class Track : MixerBase, IVisualTrack
    {
        public Parts Parts { get; private set; } = new Parts();
        public override IEnumerable<IProvider> Inputs => this.Parts;

        public float[] VisualBuffer => this.visualBuffer;

        protected List<Part> remainingParts = new List<Part>();

        private int totalCount;
        private float[] visualBuffer = new float[0];

        public Track()
        {
        }

        public override void Reset()
        {
            foreach (var part in this.Parts)
            {
                part.Reset();
            }

            this.remainingParts = this.Parts.ToList();
            this.totalCount = 0;
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

            int p = this.remainingParts.Count;

            while (p > 0)
            {
                p--;

                var part = remainingParts[p];

                if (part.StartIndex >= this.Index + count)
                {
                    continue;
                }

                int partOffset = part.StartIndex > this.Index ? part.StartIndex - this.Index : 0;

                int samplesRead = part.Read(this.mixBuffer, partOffset, count - partOffset);
                if (samplesRead == 0)
                {
                    continue;
                }

                samplesRead += partOffset;

                for (int n = 0; n < samplesRead; n++)
                {
                    if (n >= outputCount)
                    {
                        buffer[offset + n] = n < partOffset ? 0f : part.ApplyGainTo(this.mixBuffer[n]);
                    }
                    else if (n >= partOffset)
                    {
                        buffer[offset + n] += part.ApplyGainTo(this.mixBuffer[n]);
                    }
                }
                outputCount = Math.Max(samplesRead, outputCount);
            }


            this.Index += count;

            this.remainingParts.RemoveAll(part => part.Finished);

            Finish(this.remainingParts.Count == 0);

            // Copy to visual buffer
            this.visualBuffer = Ensure(this.visualBuffer, this.totalCount + outputCount / this.Channels);

            for (int i = 0; i < outputCount / this.Channels; i++)
            {
                var value = buffer[offset + i * this.Channels];
                if (this.Channels > 1)
                {
                    value = (value + buffer[offset + i * this.Channels + 1]) * 0.5f;
                }
                this.visualBuffer[this.totalCount + i] = value;
            }

            this.totalCount += outputCount / this.Channels;

            return outputCount;
        }

        public static float[] Ensure(float[] buffer, int samplesRequired)
        {
            if (buffer == null || buffer.Length < samplesRequired)
            {
                var newBuffer = new float[samplesRequired];
                if (buffer != null)
                {
                    Array.Copy(buffer, 0, newBuffer, 0, buffer.Length);
                }

                buffer = newBuffer;
            }

            return buffer;
        }
    }
}
