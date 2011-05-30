using System;
using System.IO;
using Castle.DynamicProxy;
using Dokan;
using NUnit.Framework;

namespace Meowth.TagFSDriver.Tests
{
    [TestFixture]
    public class ProxyFactoryTests
    {
        [Test]
        public void TestCreationFS()
        {
            var pf = new ProxyGenerator()
                .CreateInterfaceProxyWithTarget<DokanOperations>(
                    new TaggedFileSystem(new TaggedFileSystemOptions{ RootPath = Path.GetTempPath()}),
                    new WrappingInterceptor(), 
                    new TransactionManagementInterceptor()
                );
            
            pf.DeleteFile(null, null);
        }
    }
}
