namespace PlutoNetCoreTemplate.Application.Permissions
{
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    /// <summary>
    /// 权限定义
    /// </summary>
    public class PermissionDefinition
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; }
        

        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 上级权限
        /// </summary>
        public PermissionDefinition Parent { get; private set; }

        
        /// <summary>
        /// 允许的提供者，eg. role/dept
        /// </summary>
        public List<string> AllowedProviders { get; set; } = new();


        /// <summary>
        /// 下级权限定义
        /// </summary>

        private readonly List<PermissionDefinition> _children = new();


        public IReadOnlyList<PermissionDefinition> Children => _children.ToImmutableList();

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; set; }


        protected internal PermissionDefinition([NotNull]string name, string displayName = null, bool isEnabled = true)
        {
            Name = name;
            DisplayName = displayName;
            IsEnabled = isEnabled;
        }

        public virtual PermissionDefinition AddChild([NotNull]string name, string displayName = null, bool isEnabled = true)
        {
            var child = new PermissionDefinition(name, displayName, isEnabled) { Parent = this };
            _children.Add(child);
            return child;
        }

        public virtual PermissionDefinition WithProviders(params string[] providers)
        {
            if (providers != null && providers.Any())
            {
                AllowedProviders.AddRange(providers);
            }
            return this;
        }
    }
}