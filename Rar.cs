using System.IO;
using System.Linq;
using SharpCompress;
using SharpCompress.Archive;
using SharpCompress.Common;


public class Rar
{
    public static void Extract(string SourceFile, string DestinationFolder)
    {
        using (Stream stream = File.OpenRead(SourceFile))
        using (var archive = ArchiveFactory.Open(stream))
        {
            foreach (var entry in archive.Entries.Where(entry => !entry.IsDirectory))
            {
                entry.WriteToDirectory(DestinationFolder, ExtractOptions.ExtractFullPath | ExtractOptions.Overwrite);
            }
        }
    }

}