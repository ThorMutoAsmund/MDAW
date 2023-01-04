using System.ComponentModel;

namespace MDAWLib1
{
    public class EmptyProvider : BaseProvider
    {
        public static readonly EmptyProvider Instance = new();

        public EmptyProvider()
        {
        }

        public static EmptyProvider CreateFailedProvider(PlaybackContext context, string failure)
        {
            var provider = new EmptyProvider();
            provider.Fail(failure);
            return provider;
        }

        public override int Read(float[] buffer, int offset, int count)
        {
            return 0;
        }
    }
}
