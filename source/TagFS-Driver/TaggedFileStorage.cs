using System;
using System.IO;
using System.Transactions;
using Community.CsharpSqlite.SQLiteClient;
using log4net;
using log4net.Core;

namespace Meowth.TagFSDriver
{
    /// <summary> Storage of files and their tags </summary>
    public class TaggedFileStorage
    {
        /// <summary> File storage </summary>
        public TaggedFileStorage(string pathToStorageDir, string pathToServiceDir) // exception
        {
            _storageDir = pathToStorageDir;
            _serviceDir = pathToServiceDir;

            Init(); // exception
        }

        /// <summary> Returns tag name</summary>
        public string CreateTag(string tagName, string parentTagName) // exception
        {
            try
            {
                using (var ts = new TransactionScope())
                {
                    if (FindTag(parentTagName) == null)
                        throw new ApplicationException(string.Format("Parent tag '{0}' not found", parentTagName));
                            // exception

                    var tagFullName = Tag.Combine(parentTagName, tagName);
                    if (FindTag(tagFullName) != null) // exception
                    {
                        s_logger.WarnFormat("Tag '{0}' already exists", tagFullName);
                        return tagFullName;
                    }

                    // TODO: create tag entity

                    return tagFullName;
                }
            }
            catch(Exception ex)
            {
                s_logger.Error("Error in 'CreateTag'", ex);
                throw; // exception
            }
        }

        /// <summary> Finds tag by name </summary>
        public Tag FindTag(string tagName) // exception
        {
            using (var ts = new TransactionScope())
            {
                throw new NotImplementedException();
            }
        }

        public Tag GetTag(string tagName) // exception
        {
            using (var ts = new TransactionScope())
            {
                throw new NotImplementedException();
            }
        }

        /// <summary> Deletes tag and it's phantoms. If no references to real file exist, 
        /// file is also deleted </summary>
        /// <param name="tagName"></param>
        public void DeleteTag(string tagName) // exception
        {
            using (var ts = new TransactionScope())
            {
                throw new NotImplementedException();
            }
        }

        /// <summary> Lists all tags under given tag </summary>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public Tag[] ListTags(string tagName)
        {
            // tag name can be null, so use root tags and phantoms
            using (var ts = new TransactionScope())
            {
                throw new NotImplementedException();
            }
        }

        /// <summary> Enlists all phantoms under given tag </summary>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public PhantomFile[] ListPhantoms(string tagName)
        {
            // tag name can be null, so use root tags and phantoms
            using (var ts = new TransactionScope())
            {
                throw new NotImplementedException();
                ts.Complete();
            }
        }

        /// <summary> Deletes phantom file under given tag </summary>
        /// <remarks> If no other phantom exists referencing real file, this file will be deleted physically</remarks>
        public void DeletePhantom(string tagName, string phantomName)
        {
            using (var ts = new TransactionScope())
            {
                s_logger.DebugFormat("Deleting (tag, phantom) = ('{0}', '{1}')", tagName, phantomName);

                var tag = GetTag(tagName);
                var phantom = GetPhantom(tagName, phantomName);
                var phantoms = GetPhantoms(phantom.RealFileId);

                // TODO: delete phantom from database

                if (phantoms.Length == 0)
                {
                    s_logger.DebugFormat("No more phantoms reference file '{0}'; deleting real file", phantomName);
                    var realFile = GetRealFile(phantom.RealFileId); // esxeption
                    var pathToFile = Path.Combine(_storageDir, realFile.SyntheticName);
                    File.Delete(pathToFile); // exception
                }

                ts.Complete();
            }
        }

        private RealFile GetRealFile(int realFileId)
        {
            throw new NotImplementedException();
        }

        private PhantomFile GetPhantom(string tagName, string phantomName)
        {
            throw new NotImplementedException();
        }

        /// <summary> Initialize infrastructure </summary>
        private void Init()
        {
            _sqlConnection = new SqliteConnection("...");
        }

        /// <summary> Finds all phantoms that reference concrete file </summary>
        private PhantomFile[] GetPhantoms(int realFileId)
        {
            throw new NotImplementedException();
        }

        private void DeletePhantom(int phantomId)
        {
            
        }

        /// <summary> Logging facility </summary>
        private static readonly ILog s_logger = LogManager.GetLogger(typeof(TaggedFileSystem));

        /// <summary> Path to directory of storage </summary>
        private string _storageDir;

        /// <summary> Path to directory with database and so on </summary>
        private string _serviceDir;

        /// <summary> Connection </summary>
        private SqliteConnection _sqlConnection;
    }
}