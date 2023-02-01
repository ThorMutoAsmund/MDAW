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
        public static T ConnectTo<T>(this T source, Parts destination, Position startAt = default, double gain = 1f, string name = "") where T: IProvider
        {
            destination.Add(new Part(source, startAt: startAt, gain: gain, name: name));
            return source;
        }

        public static T ConnectTo<T>(this T source, Providers destination) where T: IProvider
        {
            destination.Add(source);
            return source;
        }

        public static T ConnectTo<T>(this T source, Parameter destination) where T: IProvider
            //, Position startAt = default, double gain = 1f, string name = "")
        {
            destination.SetSource(source);
            return source;
        }
    }
}
