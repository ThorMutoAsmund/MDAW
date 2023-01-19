using MDAWLib1;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDAW
{
    public static class Audio
    {
        private static WaveOutEvent? waveOut;
        public static WaveOutEvent WaveOut
        {
            get
            {
                if (waveOut == null)
                {
                    waveOut = new WaveOutEvent();
                    waveOut.PlaybackStopped += WaveOut_PlaybackStopped;
    }
                return waveOut;
            }
        }

        private static bool resetWhenStopping = false;

        private static void WaveOut_PlaybackStopped(object? sender, StoppedEventArgs e)
        {
            Env.OnAddMessage($"Playback stopped");

            if (resetWhenStopping && WaveOut.PlaybackState == PlaybackState.Stopped)
            {
                PlaybackContext.Current.ResetPosition();
            }
        }

        private static bool EnsureStopped()
        {
            resetWhenStopping = false;

            if (WaveOut.PlaybackState != PlaybackState.Stopped)
            {
                WaveOut.Stop();
                //    if (fileReader != null)
                //    {
                //        fileReader.Dispose();
                //        fileReader = null;
                //    }
                //    //mp3Reader.Dispose();
                //    //FileWaveOut.Dispose();
                return true;
            }

            return false;
        }

        private static void InternalPlay()
        {
            EnsureStopped();

            resetWhenStopping = true;

            WaveOut.Init(PlaybackContext.Current);
            WaveOut.Play();
        }

        public static void Stop()
        {
            EnsureStopped();
        }

        public static void Play()
        {
            if (WaveOut.PlaybackState == PlaybackState.Stopped)
            {
                Env.OnAddMessage($"Playing");

                InternalPlay();
            }
        }

        public static void PlayFromStart()
        {
            Env.OnAddMessage($"Playing from start");
                        
            EnsureStopped(); 
            
            PlaybackContext.Current.ResetPosition();

            InternalPlay();
        }

        public static void PlayPattern()
        {
            Env.OnAddMessage($"Playing pattern");

            InternalPlay();
        }
    }
}
