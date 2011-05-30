using System;
using Castle.DynamicProxy;
using log4net;

namespace Meowth.TagFSDriver
{
    /// <summary> Intercepts calls to methods which should be transacted </summary>
    public class TransactionManagementInterceptor : IInterceptor
    {
        /// <summary> When called </summary>
        public void Intercept(IInvocation invocation)
        {
            if(!TransactionManager.InTransaction())
            {
                try
                {
                    s_logger.Debug("About to begin transaction");
                    TransactionManager.BeginTransaction();
                    s_logger.Debug("Transaction began");

                    invocation.Proceed();

                    s_logger.Debug("About to commit transaction");
                    TransactionManager.CommitTransaction();
                    s_logger.Debug("Transaction commited");
                }
                catch(Exception ex)
                {
                    s_logger.Error(string.Format("Error proceeding method '{0}'", invocation.Method.Name), ex);
                    TransactionManager.RollbackTransaction();
                    s_logger.Error("Transaction rolled back");
                    throw;
                }
            }
            else
            {
                s_logger.Debug("Already in transaction");
                invocation.Proceed();
            }
        }

        /// <summary> Transaction-manager's log </summary>
        private static readonly ILog s_logger = LogManager.GetLogger(typeof (TransactionManager));
    }
}