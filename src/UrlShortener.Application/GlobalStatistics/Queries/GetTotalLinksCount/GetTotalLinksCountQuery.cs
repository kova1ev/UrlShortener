using MediatR;
using UrlShortener.Application.Common.Result;

namespace UrlShortener.Application.GlobalStatistics.Queries.GetTotalLinksCount;

public class GetTotalLinksCountQuery : IRequest<Result<int>>
{

}