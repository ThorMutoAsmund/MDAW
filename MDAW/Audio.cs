using MDAWLib1;
using NAudio.Utils;
using NAudio.Wave;
using System;
using System.Diagnostics;
using System.Threading;

namespace MDAW
{
    public static class Audio
    {
        public static event Action<TimeSpan>? PositionChanged;

        private static TimeSpan AccumulatedTimeSpan;

        private static WaveStream? PlaySampleFileReader;

        private static Timer PositionTimer = new Timer(
            PositionTimerCallback,
            null,
            Timeout.Infinite,
            Timeout.Infinite);
        
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

        static void PositionTimerCallback(object? state)
        {
            PositionChanged?.Invoke(AccumulatedTimeSpan + WaveOut.GetPositionTimeSpan());
        }

        private static bool resetWhenStopping = false;

        private static void WaveOut_PlaybackStopped(object? sender, StoppedEventArgs e)
        {
            Env.OnAddMessage($"Playback stopped");

            if (resetWhenStopping && WaveOut.PlaybackState == PlaybackState.Stopped)
            {
                PositionTimer.Change(Timeout.Infinite, Timeout.Infinite); 

                PlaybackContext.Current.ResetPosition();
                AccumulatedTimeSpan = TimeSpan.Zero;
            }
        }

        private static bool EnsureStopped()
        {
            resetWhenStopping = false;

            // Free any reference to the play sample file reader
            if (PlaySampleFileReader != null)
            {
                PlaySampleFileReader.Dispose();
                PlaySampleFileReader = null;
            }

            if (WaveOut.PlaybackState != PlaybackState.Stopped)
            {
                AccumulatedTimeSpan += WaveOut.GetPositionTimeSpan();
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
            PositionTimer.Change(TimeSpan.Zero, TimeSpan.FromSeconds(0.1f));
        }

        public static void Stop()
        {
            PositionTimer.Change(Timeout.Infinite, Timeout.Infinite);
            EnsureStopped();
        }

        public static void StopPlay()
        {
            if (WaveOut.PlaybackState == PlaybackState.Stopped)
            {
                Env.OnAddMessage($"Playing");

                InternalPlay();
            }
            else
            {
                PositionTimer.Change(Timeout.Infinite, Timeout.Infinite);
                EnsureStopped();
            }
        }

        public static void PlayFromStart()
        {
            Env.OnAddMessage($"Playing from start");

            PositionTimer.Change(Timeout.Infinite, Timeout.Infinite);
            EnsureStopped(); 
            
            PlaybackContext.Current.ResetPosition();
            AccumulatedTimeSpan = TimeSpan.Zero;

            InternalPlay();
        }

        public static void PlayPattern()
        {
            Env.OnAddMessage($"Playing pattern");

            InternalPlay();
        }

        public static void PlayFile(string filePath)
        {
            EnsureStopped();

            try
            {
                if (filePath.EndsWith("mp3"))
                {
                    PlaySampleFileReader = new Mp3FileReader(filePath);
                }
                else
                {
                    PlaySampleFileReader = new WaveFileReader(filePath);
                }
                WaveOut.Init(PlaySampleFileReader);
                WaveOut.Play();
            }
            catch (Exception ex)
            {
                Env.OnAddMessage($"Error playing file: {ex.Message}");
            }

        }
    }
}
