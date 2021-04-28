namespace PlutoNetCoreTemplate.Application.Models.PermissionModels
{
    using System.Collections.Generic;

    public class PermissionListResponseModel
    {
        public string DisplayName { get; set; } 

        public List<PermissionGroupModel> Groups { get; set; } 
    }
}