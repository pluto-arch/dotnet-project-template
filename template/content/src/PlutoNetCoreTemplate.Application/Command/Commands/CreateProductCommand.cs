namespace PlutoNetCoreTemplate.Application.Command
{
    using System.ComponentModel.DataAnnotations;
    using MediatR;

    public class CreateProductCommand:BaseCommand,IRequest<Unit>
    {
        [Required(ErrorMessage = "产品名称不能为空")]
        public string ProductName { get; set; }
    }
}