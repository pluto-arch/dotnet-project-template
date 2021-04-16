namespace PlutoNetCoreTemplate.Application.Permissions
{
    using Domain.Aggregates.System;

    public static class SystemPermission
    {
        public const string GroupName = "System";

        /// <summary>
        /// 用户
        /// </summary>
        public static class SysUser
        {
            public const string Default = GroupName + ".Users";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
        }

        /// <summary>
        /// 角色
        /// </summary>
        public static class SysRole
        {
            public const string Default = GroupName + ".Roles";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
        }
    }
}