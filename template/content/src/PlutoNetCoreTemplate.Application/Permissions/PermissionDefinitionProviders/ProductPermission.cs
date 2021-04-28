namespace PlutoNetCoreTemplate.Application.Permissions
{
    public static class ProductPermission
    {
        public const string GroupName = "ProductManager";

        
        public static class Product
        {
            public const string Default = GroupName + ".Products";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
        }

        
        public static class Device
        {
            public const string Default = GroupName + ".Devices";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
        }
    }
}