using System;
using System.IO;
using BLToolkit.Data.Linq;
using log4net;
using System.Linq;

namespace Meowth.TagFSDriver
{
    /// <summary> Storage of files and their tags </summary>
    public class TaggedFileStorage
    {
        private readonly ITaggingRepository _repository;

        /// <summary> File storage </summary>
        public TaggedFileStorage(ITaggingRepository repository) // exception
        {
            _repository = repository;
        }
        
        /// <summary> Returns tag name</summary>
        public string CreateTag(string tagName, string parentTagName) // exception
        {
            Tag parentTag = null;
            if (parentTagName != null)
            {
                parentTag = FindTag(parentTagName);
                if(parentTag == null)
                    throw new ApplicationException(string.Format("Parent tag '{0}' not found", parentTagName)); // exception
            }

            var tagFullName = (parentTag != null) 
                ? Tag.Combine(parentTagName, tagName) 
                : tagName;

            if (FindTag(tagFullName) != null) // exception
            {
                s_logger.WarnFormat("Tag '{0}' already exists", tagFullName);
                return tagFullName;
            }

            var parentTagId = (parentTag != null)
                                  ? (long?) parentTag.Id
                                  : null;
            _repository.DataContext.Insert(new Tag
            {
                TagName = tagName,
                TagPath = tagFullName,
                ParentTagId = parentTagId
            }); // exception

            return tagFullName;
        }

        /// <summary> Lists all tags under given tag </summary>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public Tag[] ListTags(string tagName)
        {
            // tag name can be null, so use root tags and phantoms
            return (from Tag tag in _repository.Tags select tag).ToArray();
        }

        /// <summary> Enlists all phantoms under given tag </summary>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public PhantomFile[] ListPhantoms(string tagName)
        {
            // tag name can be null, so use root tags and phantoms
            
                throw new NotImplementedException();
        }

        /// <summary> Finds tag by name </summary>
        private Tag FindTag(string tagName) // exception
        {
            return _repository
               .Tags
               .Where(t => t.TagName == tagName).FirstOrDefault();
        }

        private Tag GetTag(string tagName) // exception
        {
            var tag = FindTag(tagName);
            if (tag == null)
                throw new InvalidDataException(string.Format("Tag '{0}' not found", tagName));

            return tag;
        }

        ///// <summary> Deletes phantom file under given tag </summary>
        ///// <remarks> If no other phantom exists referencing real file, this file will be deleted physically</remarks>
        //public void DeletePhantom(string tagName, string phantomName)
        //{
           
        //        s_logger.DebugFormat("Deleting (tag, phantom) = ('{0}', '{1}')", tagName, phantomName);

        //        var tag = GetTag(tagName);
        //        var phantom = GetPhantom(tagName, phantomName);
        //        var phantoms = GetPhantoms(phantom.RealFileId);

        //        // TODO: delete phantom from database

        //        if (phantoms.Length == 0)
        //        {
        //            s_logger.DebugFormat("No more phantoms reference file '{0}'; deleting real file", phantomName);
        //            var realFile = GetRealFile(phantom.RealFileId); // esxeption
        //            var pathToFile = Path.Combine(_storageDir, realFile.SyntheticName);
        //            File.Delete(pathToFile); // exception
        //        }
            
        //}

        /// <summary> Deletes tag and it's phantoms. If no references to real file exist, 
        /// file is also deleted </summary>
        /// <param name="tagName"></param>
        public void DeleteTag(string tagName) // exception
        {
            _repository.DataContext.Delete(GetTag(tagName));
        }

        private RealFile GetRealFile(int realFileId)
        {
            throw new NotImplementedException();
        }

        private PhantomFile GetPhantom(string tagName, string phantomName)
        {
            throw new NotImplementedException();
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
    }
}