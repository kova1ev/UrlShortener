﻿using MediatR;
using UrlShortener.Application.Common.Links;

namespace UrlShortener.Application.Links.Queries.GetLinkByShortName;

public class GetLinkByShortNameQuery : IRequest<LinkDto>
{
    public string Alias { get; set; }

    public GetLinkByShortNameQuery(string alias)
    {
        Alias = alias;
    }
}
