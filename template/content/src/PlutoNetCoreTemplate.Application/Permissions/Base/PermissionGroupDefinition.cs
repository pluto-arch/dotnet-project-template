namespace PlutoNetCoreTemplate.Application.Permissions
{
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// 权限分组定义
    /// </summary>
    public class PermissionGroupDefinition
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; }


        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }


        private readonly List<PermissionDefinition> _permissions = new ();


        public IReadOnlyList<PermissionDefinition> Permissions => _permissions.ToImmutableList();


        protected internal PermissionGroupDefinition([NotNull] string name, string displayName = null)
        {
            Name = name;
            DisplayName = displayName;
        }


        public virtual PermissionDefinition AddPermission([NotNull] string name, string displayName = null, bool isEnabled = true)
        {
            var permission = new PermissionDefinition(name, displayName, isEnabled);
            _permissions.Add(permission);
            return permission;
        }


        public virtual List<PermissionDefinition> GetPermissionsWithChildren()
        {
            var permissions = new List<PermissionDefinition>();

            foreach (var permission in _permissions)
            {
                AddPermissionToListRecursively(permissions, permission);
            }

            return permissions;
        }

        public PermissionDefinition GetPermissionOrNull([NotNull] string name)
        {
            return GetPermissionOrNullRecursively(Permissions, name);
        }

        private void AddPermissionToListRecursively(List<PermissionDefinition> permissions, PermissionDefinition permission)
        {
            permissions.Add(permission);

            foreach (var child in permission.Children)
            {
                AddPermissionToListRecursively(permissions, child);
            }
        }

        private PermissionDefinition GetPermissionOrNullRecursively(IReadOnlyList<PermissionDefinition> permissions, string name)
        {
            foreach (var permission in permissions)
            {
                if (permission.Name == name)
                {
                    return permission;
                }

                var childPermission = GetPermissionOrNullRecursively(permission.Children, name);
                if (childPermission != null)
                {
                    return childPermission;
                }
            }

            return null!;
        }
    }
}