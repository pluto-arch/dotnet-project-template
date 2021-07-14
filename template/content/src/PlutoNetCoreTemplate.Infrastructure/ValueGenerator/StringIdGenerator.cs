namespace PlutoNetCoreTemplate.Infrastructure.ValueGenerator
{
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using Microsoft.EntityFrameworkCore.ValueGeneration;

    using System;

    public class StringIdGenerator : ValueGenerator<string>
    {
        public override string Next(EntityEntry entry)
        {
            var stamp = new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds();
            var id = $"{stamp}{RandomNumberString(5)}";
            return id;
        }

        public override bool GeneratesTemporaryValues => false;



        private static string RandomNumberString(int length)
        {
            string result = "";
            Random random = new Random(Guid.NewGuid().GetHashCode());
            for (int i = 0; i < length; i++)
            {
                result += random.Next(10).ToString();
            }

            return result;
        }
    }
}