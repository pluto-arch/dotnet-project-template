namespace PlutoNetCoreTemplate.Application.Dtos.Permission
{
    using System.Collections.Generic;

    public class PermissionGroupModel
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public List<PermissionGrantModel> Permissions { get; set; }
    }
}