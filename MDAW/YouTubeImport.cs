using MediaToolkit;
using MediaToolkit.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoLibrary;

namespace MDAW
{
    // https://www.youtube.com/watch?v=KeMn13Qwcto
    // https://www.youtube.com/watch?v=usXhiWE2Uc0
    public static class YouTubeImport
    {
        public static async Task Import(string youTubeURL, string destinationFolder)
        {
            string? videoFilePath = null;
            try
            {
                var youtube = YouTube.Default;
                var vid = await youtube.GetVideoAsync(youTubeURL);
                videoFilePath = Path.Combine(destinationFolder, vid.FullName);
                var data = await vid.GetBytesAsync();
                File.WriteAllBytes(videoFilePath, data);

                var inputFile = new MediaFile { Filename = videoFilePath };
                var outputFile = new MediaFile { Filename = Path.Combine(destinationFolder, Path.ChangeExtension(vid.FullName, "mp3")) };

                using (var engine = new Engine())
                {
                    engine.GetMetadata(inputFile);
                    engine.Convert(inputFile, outputFile);
                }
            }
            catch (Exception e)
            {
                Env.OnAddMessage($"Error importing YouTube file: {e.Message}");
            }
            finally
            {
                if (videoFilePath != null && File.Exists(videoFilePath))
                {
                    File.Delete(videoFilePath);
                }
            }
        }
    }
}
