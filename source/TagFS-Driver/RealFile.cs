using System;

namespace Meowth.TagFSDriver
{
    /// <summary> Original file information. Used to represent info about input files </summary>
    public class RealFile
    {
        /// <summary> Id </summary>
        public int Id { get; set; }

        /// <summary> Original file name </summary>
        public string OriginalName { get; set; }

        /// <summary> Имя, которое выдано файлу во избежание коллизий </summary>
        public string SyntheticName { get; set; }

        /// <summary> File hashsum </summary>
        public int Hashsum { get; set; }

        /// <summary> Generates new sytnthetic name </summary>
        public static string GenerateName()
        {
            return Guid.NewGuid().ToString();
        }
    }
}