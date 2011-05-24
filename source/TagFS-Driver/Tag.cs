namespace Meowth.TagFSDriver
{
    /// <summary> Tag </summary>
    public class Tag
    {
        /// <summary> Tag id </summary>
        public int Id { get; set; }

        /// <summary> Full tag name </summary>
        public string TagName { get; set; }

        public static string Combine(string parent, string tag)
        {
            return string.Format("{0}/{1}", parent, tag);
        }
    }
}