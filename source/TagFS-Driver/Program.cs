using System;
using Castle.DynamicProxy;
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

            var target = new TaggedFileSystem(
                new TaggedFileSystemOptions
                    {
                        RootPath = "d:\\tmp",
                    });

            var fileSystem = new ProxyGenerator()
                .CreateInterfaceProxyWithTarget<DokanOperations>(
                target,
                new WrappingInterceptor()
            );
            
            // Entry point
            var status = DokanNet.DokanMain(
                dokanOptions,
                null
                );
            return status;
        }
    }

    
}