using System;

namespace Meowth.TagFSDriver
{
    /// <summary> File phantoms visible under tags </summary>
    public class PhantomFile
    {
        /// <summary> Phantom id </summary>
        public int Id { get; set; }

        /// <summary> Tag id in which phantom resides </summary>
        public int TagId { get; set; }

        /// <summary> Reference to original file </summary>
        /// <remarks> Should it be denormalized and save filename? </remarks>
        public int RealFileId { get; set; }
    }
}