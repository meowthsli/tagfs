using System;

namespace Meowth.TagFSDriver
{
    public static class TransactionManager
    {
        public static void BeginTransaction()
        {
            s_transaction = new object(); // Transaction
        }

        public static void CommitTransaction()
        {
            // commit current transaction
        }

        public static void RollbackTransaction()
        {
            if(s_transaction != null)
            {
                // rollback transaction
            }
        }

        public static bool InTransaction()
        {
            return s_transaction != null;
        }

        /// <summary> Объект хранит текущую транзакцию </summary>
        [ThreadStatic]
        private static object s_transaction;
    }
}