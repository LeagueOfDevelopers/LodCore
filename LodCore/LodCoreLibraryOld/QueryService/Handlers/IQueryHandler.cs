using LodCoreLibraryOld.QueryService.Queries;

namespace LodCoreLibraryOld.QueryService.Handlers
{
    public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        TResult Handle(TQuery query);
    }
}