﻿using MDAWLib1;
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
        public static WaveOutEvent WaveOut { get; private set; } = new WaveOutEvent();

        private static bool EnsureStopped()
        {
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

            WaveOut.Init(PlaybackContext.Current);
            WaveOut.Play();
        }

        public static void Stop()
        {
            

            if (EnsureStopped())
            {
                Env.OnAddMessage($"Stopping");
            }
        }

        public static void Play()
        {
            Env.OnAddMessage($"Playing");

            InternalPlay();
        }

        public static void PlayFromStart()
        {
            Env.OnAddMessage($"Playing from start");

            InternalPlay();
        }

        public static void PlayPattern()
        {
            Env.OnAddMessage($"Playing pattern");

            InternalPlay();
        }
    }
}