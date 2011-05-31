namespace Meowth.TagFSDriver
{
    /// <summary> Transaction management interface </summary>
    public interface ITransactionManagement
    {
        void BeginTransaction();

        void CommitTransaction();

        void RollbackTransaction();

        bool InTransaction { get; }
    }
}