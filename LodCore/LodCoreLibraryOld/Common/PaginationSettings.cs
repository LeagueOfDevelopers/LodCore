using Journalist;

namespace LodCoreLibraryOld.Common
{
    public class PaginationSettings
    {
        public PaginationSettings(int pageSize)
        {
            Require.Positive(pageSize, nameof(pageSize));

            PageSize = pageSize;
        }

        public int PageSize { get; private set; }
    }
}