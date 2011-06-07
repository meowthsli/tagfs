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
        const string ROOT_NAME = "\\";
        private const char SEPARATOR = '\\';

        private readonly ITaggingRepository _repository;

        /// <summary> File storage </summary>
        public TaggedFileStorage(ITaggingRepository repository) // exception
        {
            _repository = repository;
        }
        
        /// <summary> Returns tag name</summary>
        public void CreateTag(string tagPath) // exception
        {
            if (FindTagByPath(tagPath) != null) // exception
            {
                s_logger.WarnFormat("Tag '{0}' already exists", tagPath);
                throw new ApplicationException("Tag already exists");
            }

            var pathComponents = tagPath.Split(new[]{SEPARATOR}, StringSplitOptions.None);
            if(pathComponents.Length < 2)
                throw new ApplicationException("Invalid tag name");
            
            var newTagName = pathComponents[pathComponents.Length - 1];
            if(tagPath.IndexOfAny(Path.GetInvalidFileNameChars()) > 0)
                throw new ApplicationException("Invalid tag name");

            var parentTagPathComponents = new string[pathComponents.Length - 1];
            Array.Copy(pathComponents, 0, parentTagPathComponents, 0, parentTagPathComponents.Length);
            var parentTagPath = string.Join(new string(new[]{SEPARATOR}), parentTagPathComponents);

            var parentTag = (pathComponents.Length == 2)
                ? FindTagByPath(ROOT_NAME)
                : FindTagByPath(parentTagPath);

            if(parentTag == null)
                throw new ApplicationException(string.Format("Parent tag, Path = '{0}' not found", parentTagPath));
            
            _repository.DataContext.Insert(new Tag
            {
                TagName = newTagName,
                TagPath = tagPath,
                ParentTagId = parentTag.Id
            }); // exception

            return;
        }

        /// <summary> Lists all tags under given tag </summary>
        /// <param name="tagPath"></param>
        /// <returns></returns>
        public Tag[] ListTags(string tagPath)
        {
            return (from Tag tag in _repository.Tags
                    join parentTag in _repository.Tags on tag.ParentTagId equals parentTag.Id
                    where parentTag.TagPath == tagPath && tag.TagName != ROOT_NAME
                    select tag).ToArray();
        }

        /// <summary> Enlists all phantoms under given tag </summary>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public PhantomFile[] ListPhantoms(string tagName)
        {
            return new PhantomFile[0];
        }

        /// <summary> Deletes tag from repository </summary>
        /// <param name="tagPath"> Path to tag to delete </param>
        public void DeleteTag(string tagPath)
        {
            var tag = FindTagByPath(tagPath);
            if (tag == null)
            {
                var msg = string.Format("Tag '{0}' not found", tagPath);
                s_logger.Error(msg);
                throw new ApplicationException(msg);
            }

            _repository.Tags
                .Delete(t => t.TagPath.StartsWith(tagPath));
        }

        /// <summary> Finds tag by path. </summary>
        /// <returns> Returns <see langword="null"/>, if no such tag found. </returns>
        private Tag FindTagByPath(string tagPath) // exception
        {
            return _repository.Tags
               .Where(t => t.TagPath == tagPath).FirstOrDefault();
        }

        /// <summary> Logging facility </summary>
        private static readonly ILog s_logger = LogManager.GetLogger(typeof(TaggedFileSystem));
    }
}