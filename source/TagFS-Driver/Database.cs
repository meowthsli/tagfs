using System;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using BLToolkit.Data;
using BLToolkit.Data.DataProvider;
using BLToolkit.Data.Linq;

namespace Meowth.TagFSDriver
{
    /// <summary> Out transaction manager </summary>
    public class Database : ITransactionManagement, ITaggingRepository, IDisposable
    {
        /// <summary> Creation </summary>
        /// <param name="pathToServiceDir"></param>
        public Database(string pathToServiceDir)
        {
            _serviceDir = pathToServiceDir;
            Init();
        }

        /// <summary> Rolling all back </summary>
        public void BeginTransaction() // exception
        {
            if(InTransaction)
                throw new InvalidOperationException("Already in transaction"); // exception

            // Begin transaction
            CurrentDbSession = new DbManager(new SQLiteDataProvider(), _databaseConnection)
                .BeginTransaction(); // exception
        }

        /// <summary> Rolling all back </summary>
        public void CommitTransaction() // exception
        {
            // commit current transaction
            if(!InTransaction)
                throw new InvalidOperationException("Not in transaction yet"); // exception
            
            CurrentDbSession.CommitTransaction(); // exception
        }
        
        /// <summary> Rolling all back </summary>
        public void RollbackTransaction()
        {
            if (!InTransaction)
                throw new InvalidOperationException("Not in transaction yet"); // exception

            CurrentDbSession.RollbackTransaction(); // exception
            CurrentDbSession.Dispose();
            CurrentDbSession = null;
        }

        /// <summary> Is in transaction now? </summary>
        public bool InTransaction { get { return CurrentDbSession != null; } }

        /// <summary> Releases all </summary>
        public void Dispose()
        {
            if(InTransaction)
                RollbackTransaction();

            if (_databaseConnection == null) 
                return;

            _databaseConnection.Close();
            _databaseConnection.Dispose();
            _databaseConnection = null;
        }
        
        public IQueryable<Tag> Tags
        {
            get
            {
                return CurrentDbSession.GetTable<Tag>();
            }
        }

        public IQueryable<PhantomFile> Phantoms
        {
            get
            {
                return CurrentDbSession.GetTable<PhantomFile>();
            }
        }

        public IQueryable<RealFile> RealFiles
        {
            get
            {
                return CurrentDbSession.GetTable<RealFile>();
            }
        }

        public IDataContext DataContext
        {
            get { return CurrentDbSession; }
        }

        /// <summary> Initialize infrastructure </summary>
        private void Init() // exception
        {
            _databaseConnection = new SQLiteConnection(GetConnectionString());
            _databaseConnection.Open(); // exception

            // Initialize data
            const string initStatement = @"
CREATE TABLE IF NOT EXISTS Tag (
    Id INTEGER PRIMARY KEY AUTOINCREMENT, 
    TagPath TEXT UNIQUE NOT NULL,
    TagName TEXT NOT NULL,
    ParentTagId INTEGER);

CREATE INDEX IF NOT EXISTS IX_TagName ON Tag (
    TagName ASC
    );

CREATE INDEX IF NOT EXISTS IX_ParentTag ON Tag (
    ParentTagId ASC
    );

";
            using(var cmd = _databaseConnection.CreateCommand())
            {
                cmd.CommandText = initStatement;
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary> Creates connection string to SQLite file </summary>
        /// <returns> String </returns>
        private string GetConnectionString()
        {
            return string.Format("Data Source={0}; Version=3; Synchronous=Full", 
                Path.Combine(_serviceDir, "tagfs"));
        }

        /// <summary> Current session </summary>
        private static DbManager CurrentDbSession
        {
            get { return (DbManager)CallContext.GetData(TX); }
            set { CallContext.SetData(TX, value); }
        }

        /// <summary>  Tx context marker </summary>
        private const string TX = "Database.Tx";

        /// <summary> Path to directory with database </summary>
        private readonly string _serviceDir;

        /// <summary> Connection to database file </summary>
        private SQLiteConnection _databaseConnection;
    }
}