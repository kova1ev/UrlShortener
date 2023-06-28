using MediatR;
using UrlShortener.Application.Common.Result;

namespace UrlShortener.Application.GlobalStatistics.Queries.GetTotalLinksCount;

public class GetTotalLinksCount : IRequest<Result<int>>
{

}