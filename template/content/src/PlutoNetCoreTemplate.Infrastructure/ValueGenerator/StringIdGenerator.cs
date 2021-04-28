namespace PlutoNetCoreTemplate.Infrastructure.ValueGenerator
{
    using System;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using Microsoft.EntityFrameworkCore.ValueGeneration;

    public class StringIdGenerator:ValueGenerator<string>
    {
        public override string Next(EntityEntry entry)
        {
            return Guid.NewGuid().ToString("N");
        }

        public override bool GeneratesTemporaryValues => false;
    }
}