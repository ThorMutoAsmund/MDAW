using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDAWLib1
{
    public static class IProviderExtensions
    {
        public static void ConnectTo(this IProvider source, Parts destination, int startAt = 0, float gain = 1f)
        {
            destination.Add(new Part(source, startAt: startAt, gain: gain));
        }
        public static void ConnectTo(this IProvider source, Providers destination)
        {
            destination.Add(source);
        }
    }
}
