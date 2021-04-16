namespace PlutoNetCoreTemplate.Application.Dtos.Permission
{
    using System.Collections.Generic;

    public class PermissionListResponseModel
    {
        public string DisplayName { get; set; } 

        public List<PermissionGroupModel> Groups { get; set; } 
    }
}