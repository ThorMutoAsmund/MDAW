using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDAWLib1
{
    public interface IProjectParameters
    {
        public string RootPath { get; }
        public Song? Song { get; }
    }
}
