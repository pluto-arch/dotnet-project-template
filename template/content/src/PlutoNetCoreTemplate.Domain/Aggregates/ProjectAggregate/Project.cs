using PlutoNetCoreTemplate.Domain.Entities;

using System;
using System.Collections.Generic;

namespace PlutoNetCoreTemplate.Domain.Aggregates.ProjectAggregate
{
    public class Project : BaseAggregateRoot<int>
    {
        public string Name { get; set; } = null!;

        public DateTimeOffset CreationTime { get; set; } = DateTimeOffset.Now;

        public List<ProjectGroup> Groups { get; set; } = null!;
    }
}