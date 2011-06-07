using System.Linq;
using BLToolkit.Data.Linq;

namespace Meowth.TagFSDriver
{
    /// <summary> Repository to store tags </summary>
    public interface ITaggingRepository
    {
        IQueryable<Tag> Tags { get; }

        IQueryable<PhantomFile> Phantoms { get; }

        IQueryable<RealFile> RealFiles { get; }

        IDataContext DataContext { get; }
    }
}