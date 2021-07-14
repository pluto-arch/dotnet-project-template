namespace PlutoNetCoreTemplate.Application.Permissions
{
    public static class TenantPermission
    {
        public const string GroupName = "TenantManager";


        public static class Tenant
        {
            public const string Default = GroupName + ".Tenant";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
        }

    }
}