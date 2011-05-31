using System;
using System.Collections;
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

            var options = new TaggedFileSystemOptions { RootPath = "d:\\tmp"};
            options.Init();

            var database = new Database(options.ServicePath);
            var taggedFileStorage = new TaggedFileStorage(database);
            var target = new TaggedFileSystem(options, taggedFileStorage);

            var fileSystem = new ProxyGenerator()
                .CreateInterfaceProxyWithTarget<DokanOperations>(
                target,
                new WrappingInterceptor(),
                new TransactionManagementInterceptor(database)
            );
            
            // Entry point
            //var status = DokanNet.DokanMain(
            //    dokanOptions,
            //    fileSystem
            //    );
            //return status;
            fileSystem.CreateDirectory("Hello", new DokanFileInfo(0));
            fileSystem.CreateDirectory("Bye", new DokanFileInfo(0));

            var list = new ArrayList();
            fileSystem.FindFiles("ccscc", list, new DokanFileInfo(0));
            return -1;
        }
    }

    
}