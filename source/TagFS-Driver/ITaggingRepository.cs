using System.Linq;
using BLToolkit.Data.Linq;

namespace Meowth.TagFSDriver
{
    public interface ITaggingRepository
    {
        IQueryable<Tag> Tags { get; }

        IQueryable<PhantomFile> Phantoms { get; }

        IQueryable<RealFile> RealFiles { get; }

        IDataContext DataContext { get; }
    }
}