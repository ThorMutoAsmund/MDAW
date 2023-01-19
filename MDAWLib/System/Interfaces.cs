using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDAWLib1
{
    public interface IHasInputs
    {
        public Inputs IN { get; }
    }

    public interface IVisualTrack
    {
        public IEnumerable<double> MonoData { get; }
    }
}
