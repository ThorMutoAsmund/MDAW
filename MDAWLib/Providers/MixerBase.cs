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

        protected float[]? mixBuffer;
        

        public MixerBase()
        {
        }

        protected void EnsureMixBuffer(int count)
        {
            this.mixBuffer = BufferHelpers.Ensure(this.mixBuffer, count);
        }
    }
}
