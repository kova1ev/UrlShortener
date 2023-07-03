using MediatR;
using UrlShortener.Application.Common.Result;

namespace UrlShortener.Application.GlobalStatistics.Queries.GetLinksCountByTime;

public class GetLinksCountByTimeQuery : IRequest<Result<int>>
{
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }

    public GetLinksCountByTimeQuery(DateOnly? startDate, DateOnly? endDate)
    {
        StartDate = startDate;
        EndDate = endDate;
    }
}
