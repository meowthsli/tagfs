using System;
using System.Collections;
using System.IO;

using Dokan;
using log4net;

namespace Meowth.TagFSDriver
{
    /// <summary> Our virtual filesystem </summary>
    public class TaggedFileSystem : DokanOperations
    {
        public TaggedFileSystem(TaggedFileSystemOptions options)
        {
            _options = options;

            Init(); // exception
        }

        public virtual int CreateFile(string filename, FileAccess access, FileShare share, FileMode mode, FileOptions options, DokanFileInfo info)
        {
            try { return -1; }
            catch { return -1; }
        }

        public virtual int OpenDirectory(string filename, DokanFileInfo info)
        {
            try { return -1; }
            catch { return -1; }
        }

        public virtual int CreateDirectory(string filename, DokanFileInfo info)
        {
            return -1;
        }

        public virtual int Cleanup(string filename, DokanFileInfo info)
        {
            try { return -1; }
            catch { return -1; }
        }

        public virtual int CloseFile(string filename, DokanFileInfo info)
        {
            try { return -1; }
            catch { return -1; }
        }

        public virtual int ReadFile(string filename, byte[] buffer, ref uint readBytes, long offset, DokanFileInfo info)
        {
            try { return -1; }
            catch { return -1; }
        }

        public virtual int WriteFile(string filename, byte[] buffer, ref uint writtenBytes, long offset, DokanFileInfo info)
        {
            try { return -1; }
            catch { return -1; }
        }

        public virtual int FlushFileBuffers(string filename, DokanFileInfo info)
        {
            try { return -1; }
            catch { return -1; }
        }

        public virtual int GetFileInformation(string filename, FileInformation fileinfo, DokanFileInfo info)
        {
            try { return -1; }
            catch { return -1; }
        }

        public virtual int FindFiles(string filename, ArrayList files, DokanFileInfo info)
        {
            try { return -1; }
            catch { return -1; }
        }

        public virtual int SetFileAttributes(string filename, FileAttributes attr, DokanFileInfo info)
        {
            try { return -1; }
            catch { return -1; }
        }

        public virtual int SetFileTime(string filename, DateTime ctime, DateTime atime, DateTime mtime, DokanFileInfo info)
        {
            try { return -1; }
            catch { return -1; }
        }

        public virtual int DeleteFile(string filename, DokanFileInfo info)
        {
            throw new NotImplementedException();
            try { return -1; }
            catch { return -1; }
        }

        public virtual int DeleteDirectory(string filename, DokanFileInfo info)
        {
            try { return -1; }
            catch { return -1; }
        }

        public virtual int MoveFile(string filename, string newname, bool replace, DokanFileInfo info)
        {
            try { return -1; }
            catch { return -1; }
        }

        public virtual int SetEndOfFile(string filename, long length, DokanFileInfo info)
        {
            try { return -1; }
            catch { return -1; }
        }

        public virtual int SetAllocationSize(string filename, long length, DokanFileInfo info)
        {
            try { return -1; }
            catch { return -1; }
        }

        public virtual int LockFile(string filename, long offset, long length, DokanFileInfo info)
        {
            try { return -1; }
            catch { return -1; }
        }

        public virtual int UnlockFile(string filename, long offset, long length, DokanFileInfo info)
        {
            try { return -1; }
            catch { return -1; }
        }

        public virtual int GetDiskFreeSpace(ref ulong freeBytesAvailable, ref ulong totalBytes, ref ulong totalFreeBytes, DokanFileInfo info)
        {
            try { return -1; }
            catch { return -1; }
        }

        public virtual int Unmount(DokanFileInfo info)
        {
            return DOKAN_SUCCESS;
        }

        private const int DOKAN_SUCCESS = 0;

        /// <summary> Validates settings against pc configuration </summary>
        private void Init() // exception
        {
            try
            {
                // Check if exists
                if (File.Exists(_options.RootPath))
                {
                    // This is a directory, not a fle
                    throw new ApplicationException(
                        string.Format("{0} is a file, not a directory", _options.RootPath)); // exception
                }

                if (CreateDirIfNo(_options.RootPath)) // exception
                {
                    CreateDirIfNo(_options.ContentPath); // exception
                    CreateDirIfNo(_options.ServicePath); // exception
                }
            }
            catch (Exception ex)
            {
                s_logger.Fatal("Error preparing target file system", ex);
                throw;
            }
        }

        private static readonly ILog s_logger = LogManager.GetLogger(typeof(TaggedFileSystem));

        /// <summary> </summary>
        /// <returns> Whether dir has been created </returns>
        private static bool CreateDirIfNo(string path) // exception
        {
            if (!Directory.Exists(path))
            {
                s_logger.DebugFormat("Directory '{0}' not exists", path);
                Directory.CreateDirectory(path); // exception
                s_logger.DebugFormat("Directory '{0}' has been created", path);

                return true;
            }

            s_logger.DebugFormat("Directory '{0}' already exists", path);
            return false;
        }

        /// <summary> File system options </summary>
        private TaggedFileSystemOptions _options;
    }

    /// <summary> Options of creating tagged fs </summary>
    public class TaggedFileSystemOptions
    {
        /// <summary> Path to archive root directory </summary>
        /// <remarks> Sets up path to root dir and it's subdirs </remarks>
        public string RootPath
        {
            get { return _rootPath; }
            set
            {
                _rootPath = value;
                ContentPath = Path.Combine(_rootPath, "Content");
                ServicePath = Path.Combine(_rootPath, "Service");
            }
        }

        /// <summary> Whether to compress (gzip) files while pushing them into storage </summary>
        public bool Compressed { get; set; }

        /// <summary> Path to content directory </summary>
        public string ContentPath { get; private set; }

        /// <summary> Path to service directory </summary>
        public string ServicePath { get; private set; }

        /// <summary> Creates path from synthetic file id </summary>
        public string GenerateFilePath(Guid fileId)
        {
            return Path.Combine(RootPath, fileId.ToString());
        }

        /// <summary> Path to storage root dir </summary>
        private string _rootPath;
    }
}