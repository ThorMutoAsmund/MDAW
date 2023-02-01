using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDAWLib1
{
    public class Parameter
    {
        private double value;
        private IProvider? source;

        public double this[int idx]
        {
            get 
            { 
                return GetValue(idx); 
            }
        }

        public Parameter(double initialValue)
        {
            this.value = initialValue;
        }

        public void Reset()
        {
            source?.Reset();
        }

        public void Read(int count)
        {
            this.source?.Read(null, 0, count);
        }

        public double GetValue(int idx)
        {
            if (this.source == null || this.source.OutputBuffer == null)
            {
                return value;
            }

            return this.source.OutputBuffer[idx];
        }

        public void SetSource(IProvider source)
        {
            this.source = source;
        }
    }
}
