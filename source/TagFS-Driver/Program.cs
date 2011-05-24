using Dokan;

namespace Meowth.TagFSDriver
{
    /// <summary> Entry point for program. Will be on Topshelf later </summary>
    public class Program
    {
        public static int Main(string[] args)
        {
            var dokanOptions = new DokanOptions
                          {
                              MountPoint = "r:\\", 
                              DebugMode = true, 
                              UseStdErr = true, 
                              VolumeLabel = "TAGFS",
                          };

            var status = DokanNet.DokanMain(
                dokanOptions, 
                new TaggedFileSystem(new TaggedFileSystemOptions())
                );
            return status;
        }
    }
}