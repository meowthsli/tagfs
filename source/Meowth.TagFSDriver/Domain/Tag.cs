using BLToolkit.DataAccess;

namespace Meowth.TagFSDriver
{
    /// <summary> Tag </summary>
    public class Tag
    {
        /// <summary> Tag id </summary>
        [PrimaryKey, Identity]
        public long Id { get; set; }

        /// <summary> Full tag name </summary>
        public string TagPath { get; set; }

        /// <summary> Tag name </summary>
        public string TagName { get; set; }

        /// <summary> Id of parent tag </summary>
        public long? ParentTagId { get; set;}

        public static string Combine(string parent, string tag)
        {
            return string.Format("{0}\\{1}", parent, tag);
        }
    }
}