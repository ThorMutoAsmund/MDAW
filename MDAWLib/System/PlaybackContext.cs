using Microsoft.VisualBasic;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MDAWLib1
{
    public class PlaybackContext : ISampleProvider
    {
        private static PlaybackContext defaultContext = new PlaybackContext(new Song());
        public static PlaybackContext Current { get; private set; } = defaultContext;
        public Song Song { get; private set; }
        private ISampleProvider SampleProvider => this.Song.SampleProvider;
        public WaveFormat WaveFormat => this.Song.WaveFormat;
        public int SampleRate => this.Song.WaveFormat.SampleRate;
        public int SamplePosition { get; set; }

        //private Dictionary<object, ISampleProvider> providers = new Dictionary<object, ISampleProvider>();


        private PlaybackContext(Song song)
        {
            this.Song = song;
            this.SamplePosition = 0;
        }

        public static void CreateFromSong(Song song, double offset = 0d)
        {
            var context = new PlaybackContext(song);

            //context.SampleProvider = context.CreateProvider(song, song.ProviderInfo, new ProviderData()
            //{
            //    { ProviderDataKey.IStartAt, (int)-(offset*song.SampleRate) },
            //});

            Current = context;
        }

        public int Read(float[] buffer, int offset, int count)
        {
            //Env.OnTimeChanged(TimeSpan.FromMilliseconds(1000d * this.SamplePosition / Env.Song.SampleRate));

            var result = this.SampleProvider.Read(buffer, offset, count);

            this.SamplePosition += count / 2;

            return result;
        }
    }
}
