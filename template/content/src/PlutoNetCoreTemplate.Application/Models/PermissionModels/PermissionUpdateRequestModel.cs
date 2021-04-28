namespace PlutoNetCoreTemplate.Application.Models.PermissionModels
{
    public class PermissionUpdateRequestModel
    {
        public string Name { get; set; }

        public bool IsGranted { get; set; }
    }
}