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
    }
}
