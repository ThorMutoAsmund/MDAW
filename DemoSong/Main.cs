using MDAWLib1;
using System.Diagnostics;

namespace DemoSong
{
    public class Main : Song
    {
        public override IProvider Provider { get; } = new MyMixer();
        public override string Title { get; } = "My Demo Song2";
    }


    public class MyMixer : Mixer
    {
        public MyMixer() :
            base()
        {
            //var sine1 = new RisingProvider(440.0, 880.0, 100000.0);
            //var sine2 = new RisingProvider(660.0, 2*660.0, 100000.0);
            //var sine1 = new PrimitiveWaveProvider(PrimitiveWaveType.Sine, 440.0);
            //var sine2 = new PrimitiveWaveProvider(PrimitiveWaveType.Sine, 880.0);

            //Track track = new();
            //track.ConnectTo(this.Tracks);
            //sine1.ConnectTo(track.Parts);

            Track track = new();
            track.ConnectTo(this.Tracks);
            for (int i = 0; i < 36; i++)
            {
                var sine = new PrimitiveWaveProvider(PrimitiveWaveType.Sine, 440.0 * Math.Pow(2, i / 12.0));
                sine.ConnectTo(track.Parts, Position.FromSeconds(i / 36.0), gain: 0.003, name: $"Sine {i}");
            }

        }
    }
    public class RisingProvider : BaseProvider
    {
        public double StartFrequency { get; private set; }
        public double EndFrequency { get; private set; }
        public double Speed { get; private set; }

        public RisingProvider(double startFrequency = 440.0, double endFrequency = 880.0, double speed = 100000.0)
        {
            this.StartFrequency = startFrequency;
            this.EndFrequency = endFrequency;
            this.Speed = speed;
        }

        public override int Read(float[] buffer, int offset, int count)
        {
            if (this.Finished)
            {
                return 0;
            }

            Debug.WriteLine($"{offset}");

            int i = 0;
            double freq = 0.0;
            while (i < count / this.Channels)
            {
                freq = this.StartFrequency + (this.EndFrequency - this.StartFrequency) * (this.Index / 2 + i) / this.Speed;
                if (freq > this.EndFrequency)
                {
                    Finish();
                    break;
                }

                var f = 1.0 / this.SampleRate * 2 * Math.PI * freq;

                buffer[offset + i * this.Channels] = (float)(Math.Sin((i + this.Index / 2) * f) * 2f - 1f);
                if (this.Channels > 1)
                {
                    buffer[offset + i * this.Channels + 1] = buffer[offset + i * this.Channels];
                }

                i++;
            }

            this.Index += i * 2;

            return i * 2;
        }
    }
}