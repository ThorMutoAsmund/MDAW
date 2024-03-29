﻿using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDAWLib1
{
    public interface IProvider : ISampleProvider
    {
        bool Finished { get; }
        void Reset();

        float[]? OutputBuffer { get; }
    }
}
