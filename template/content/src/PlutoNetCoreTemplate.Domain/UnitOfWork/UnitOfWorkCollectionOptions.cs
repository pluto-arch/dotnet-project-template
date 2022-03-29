namespace PlutoNetCoreTemplate.Domain.UnitOfWork
{
    using System;
    using System.Collections.Generic;

    public class UnitOfWorkCollectionOptions
    {
        public Dictionary<string, Type> DbContexts { get; set; } = new();
    }
}