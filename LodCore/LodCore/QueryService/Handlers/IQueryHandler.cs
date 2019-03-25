using LodCore.QueryService.Queries;

namespace LodCore.QueryService.Handlers
{
    public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        TResult Handle(TQuery query);
    }
}