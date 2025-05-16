using Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.Properties
{
    public class SearchPropertiesQuery : IRequest<List<PropertyDto>>
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public decimal? PriceFrom { get; set; }
        public decimal? PriceTo { get; set; }
    }
}
