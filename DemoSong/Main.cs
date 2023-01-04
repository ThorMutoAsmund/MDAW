using MDAWLib1;
using System.Diagnostics;

namespace DemoSong
{
    public class Main : Song
    {
        public override IProvider SampleProvider { get; } = new MyMixer();
        public override string Title { get; } = "My Demo Song2";
    }


    public class MyMixer : Mixer
    {
        public MyMixer() :
            base()
        {
            var sine = new RisingProvider();
            //var sine = new PrimitiveWaveProvider(PrimitiveWaveType.Sine, 440.0);
            //var sine2 = new PrimitiveWaveProvider(PrimitiveWaveType.Sine, 1220.0);

            sine.ConnectTo(this.IN);
            //sine2.ConnectTo(this.IN);

            // Set render length
            // Set song title etc.
        }
    }
    public class RisingProvider : BaseProvider
    {
        public double StartFrequency { get; private set; }
        public double EndFrequency { get; private set; }
        public double Speed { get; private set; }

        private int ramp = 0;
        
        public RisingProvider(double startFrequency = 440.0, double endFrequency = 880.0, double speed = 50000.0)
        {
            this.StartFrequency = startFrequency;
            this.EndFrequency = endFrequency;
            this.Speed = speed;
        }

        public override int Read(float[] buffer, int offset, int count)
        {
            if (this.Failed)
            {
                return 0;
            }

            Debug.WriteLine($"{offset}");

            for (int i = 0; i < count; ++i)
            {
                var freq = this.StartFrequency + (this.EndFrequency - this.StartFrequency) * (this.ramp + i) / this.Speed;
                var f = 1.0 / this.WaveFormat.Channels / this.SampleRate * 2 * Math.PI * freq;
                buffer[offset + i] = (float)(Math.Sin((i + ramp) * f) * 2f - 1f);
            }
            ramp += count;

            return count;
        }
    }
}