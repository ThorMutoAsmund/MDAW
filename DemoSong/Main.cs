using MDAWLib1;
using System.Diagnostics;

namespace DemoSong
{
    public class Main : Song
    {
        public override IProvider Provider { get; } = new MyMixer();
        public override string Title { get; } = "My Demo Song";
        public override int SampleRate { get; } = 44100;
        public Main()
        {
        }
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

            var doky1 = new AudioFileProvider("Samples/doky01.wav");
            var doky2 = new AudioFileProvider("Samples/doky02.wav");

            var track1 = new Track().ConnectTo(this.Tracks);
            var track2 = new Track().ConnectTo(this.Tracks);

            doky1.ConnectTo(track1.Parts);
            doky2.ConnectTo(track2.Parts, startAt: Position.FromSeconds(() => doky1.LengthInSeconds));
            //var sine1 = new PrimitiveWaveProvider(PrimitiveWaveType.Sine, 440.0).ConnectTo(track.Parts);

            //new LinearProvider(440.0, 880.0, 5.0).ConnectTo(sine1.Frequency);



            //for (int i = 0; i < 3; i++)
            //{
            //    Track track = new();
            //    track.ConnectTo(this.Tracks);
            //    var sine = new PrimitiveWaveProvider(PrimitiveWaveType.Sine, 440.0 * Math.Pow(2, i / 12.0));
            //    sine.ConnectTo(track.Parts, Position.FromSeconds(i), gain: 0.1, name: $"Sine {i}");
            //}

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

                buffer[offset + i * this.Channels] = (float)(Math.Sin((i + this.Index / 2) * f));
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