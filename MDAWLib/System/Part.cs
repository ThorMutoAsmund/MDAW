using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDAWLib1
{
    public class Part : IProvider
    {
        public WaveFormat WaveFormat => this.Provider.WaveFormat;
        public IProvider Provider { get; set; }
        public string Name { get; set; }
        public bool Finished => this.Provider.Finished;
        public double Gain { get; set; }
        public int StartIndex 
        { 
            get
            {
                this.startIndex ??= this.StartAt.GetIndex(this.Provider.WaveFormat);
                
                return this.startIndex.Value;
            }
    }
        public Position StartAt { get; private set; }

        public float[]? OutputBuffer => this.Provider.OutputBuffer;
        
        private float gainFloat;
        public int? startIndex;
        public Part(IProvider provider, Position startAt, double gain, string name)
        {
            this.Provider = provider;
            this.Name = name;
            this.StartAt = startAt;
            this.Gain = gain;
            this.gainFloat = (float)gain;
        }

        public void Reset()
        {
            this.Provider.Reset();
        }

        public int Read(float[] buffer, int offset, int count)
        {
            return this.Provider.Read(buffer, offset, count);
        }

        public float ApplyGainTo(float input)
        {
            return input * this.gainFloat;
        }
    }
}
