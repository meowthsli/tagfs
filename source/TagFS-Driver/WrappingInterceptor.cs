using System;
using Castle.DynamicProxy;
using Dokan;
using log4net;

namespace Meowth.TagFSDriver
{
    /// <summary> Interceptor to log and swallow all exceptions on 
    /// object of <see cref="TaggedFileSystem"/> class </summary>
    public class WrappingInterceptor : IInterceptor
    {
        /// <summary> Intercept and log method calls </summary>
        /// <returns> Original result or <see cref="DokanNet.DOKAN_ERROR"/> if any error occured </returns>
        public void Intercept(IInvocation info)
        {
            try
            {
                s_logger.DebugFormat("Entering method '{0}'", info.Method.Name);
                info.Proceed();
            }
            catch(Exception ex)
            {
                // Log it
                s_logger.Error(string.Format("Error proceeding method '{0}'", info.Method.Name), ex);
                
                // If we here, there was an exception logged, so return ERROR_CODE;
                s_logger.DebugFormat("..Will return {0}", DokanNet.DOKAN_ERROR);
                info.ReturnValue = DokanNet.DOKAN_ERROR;
            }

            s_logger.DebugFormat("Exiting method '{0}'", info.Method.Name);
        }

        /// <summary> Logger (like TaggedFS) </summary>
        private static ILog s_logger = LogManager.GetLogger(typeof(TaggedFileSystem));
    }
}