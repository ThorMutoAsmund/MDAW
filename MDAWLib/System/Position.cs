using NAudio.Wave;

namespace MDAWLib1
{
    public struct Position
    {
        private bool isTime;
        private Func<double> seconds;
        private int samples;
        
        public int GetIndex(WaveFormat waveFormat)
        {
            if (this.isTime)
            {
                return (int)(this.seconds() * waveFormat.SampleRate * waveFormat.Channels);
            }
            else
            {
                return this.samples * waveFormat.Channels;
            }
        }

        public static Position FromSeconds(double value)
        {
            return new Position()
            {
                seconds = () => value,
                isTime = true
            };
        }

        public static Position FromSeconds(Func<double> value)
        {
            return new Position()
            {
                seconds = value,
                isTime = true
            };
        }

        public static Position FromSamples(int value)
        {
            return new Position()
            {
                samples = value,
                isTime = false
            };
        }

        public Position()
        {
            this.isTime = false;
            this.samples = 0;
            this.seconds = () => 0.0;
        }
    }
}
