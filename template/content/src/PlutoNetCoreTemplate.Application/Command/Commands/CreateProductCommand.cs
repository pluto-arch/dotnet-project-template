namespace PlutoNetCoreTemplate.Application.Command
{
    using System.ComponentModel.DataAnnotations;

    public class CreateProductCommand : BaseCommand<Unit>
    {
        [Required(ErrorMessage = "产品名称不能为空")]
        public string ProductName { get; set; }
    }
}