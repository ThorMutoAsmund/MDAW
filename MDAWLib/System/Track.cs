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
        public override IEnumerable<IProvider> Inputs => this.Parts;

        public Track()
        {
        }
    }
}
