namespace PlutoNetCoreTemplate.Application.AppServices.Permissions.PermissionDefinitionProviders
{
    using Domain.Aggregates.System;

    public static class PlutoNetCoreTemplatePermission
    {
        public const string GroupName = "PlutoNetCoreTemplate";

        public static class SysUser
        {
            public const string Default = GroupName + ".UserEntity";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
        }
    }
}