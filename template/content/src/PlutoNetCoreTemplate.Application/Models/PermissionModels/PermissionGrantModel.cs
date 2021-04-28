namespace PlutoNetCoreTemplate.Application.Models.PermissionModels
{
    using System.Collections.Generic;

    public class PermissionGrantModel
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string ParentName { get; set; }

        public bool IsGranted { get; set; }

        public List<string> AllowedProviders { get; set; } 
    }
}