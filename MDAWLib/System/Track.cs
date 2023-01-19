using NAudio.Utils;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDAWLib1
{
    public class Track : MixerBase
    {
        public Parts Parts { get; private set; } = new Parts();
        public override IEnumerable<ISampleProvider> Inputs => this.Parts;

        public Track()
        {
        }

        public override int Read(float[] buffer, int offset, int count)
        {
            if (this.Failed)
            {
                return 0;
            }

            return base.Read(buffer, offset, count);
        }
    }
}
