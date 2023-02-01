using MDAWLib1;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MDAWLib1
{
    public class AudioFileProvider : BaseProvider
    {
        public string RelativePath { get; private set; }
        public double LengthInSeconds { get; private set; }

        private AudioFileReader? afr;

        public AudioFileProvider(string path = "")
        {
            this.RelativePath = path;
        }

        public override void Reset()
        {
            base.Reset();

            this.afr = null;
            this.LengthInSeconds = 0;

            var fullPath = Path.Combine(this.Context.RootPath, this.RelativePath);
            if (File.Exists(fullPath))
            {
                this.afr = new AudioFileReader(fullPath);
                this.LengthInSeconds = this.afr.Length / (this.afr.WaveFormat.BitsPerSample / 8) /
                    this.afr.WaveFormat.Channels / this.afr.WaveFormat.SampleRate;
            }
        }

        public override int Read(float[] buffer, int offset, int count)
        {
            if (this.afr == null)
            {
                return 0;
            }

            return this.afr.Read(buffer, offset, count);
        }
    }
}
