namespace PlutoNetCoreTemplate.Application.Permissions
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public interface IPermissionDefinitionManager
    {
        PermissionDefinition Get([NotNull] string name);

        PermissionDefinition GetOrNull([NotNull] string name);

        IReadOnlyList<PermissionDefinition> GetPermissions();

        IReadOnlyList<PermissionGroupDefinition> GetGroups();
    }
}