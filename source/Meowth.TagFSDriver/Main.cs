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
            log4net.Config.XmlConfigurator.Configure();

            var dokanOptions = new DokanOptions
                          {
                              MountPoint = "r:\\", 
                              DebugMode = true, 
                              UseStdErr = true, 
                              VolumeLabel = "TAGFS",
                              ThreadCount = 1
                          };

            var options = new TaggedFileSystemOptions { RootPath = "d:\\tmp" };
            options.Init();

            var databaseOriginal = new Database(options.ServicePath);
            var taggedFileStorage = new TaggedFileStorage(databaseOriginal);
            var target = new TaggedFileSystem(options, taggedFileStorage);

            var fileSystemPxy = new ProxyGenerator()
                .CreateInterfaceProxyWithTarget<DokanOperations>(
                target,
                new WrappingInterceptor(),
                new TransactionManagementInterceptor(databaseOriginal)
            );
           
            var status = DokanNet.DokanMain(
                dokanOptions,
                fileSystemPxy
                );

            return status;
        }
    }
}