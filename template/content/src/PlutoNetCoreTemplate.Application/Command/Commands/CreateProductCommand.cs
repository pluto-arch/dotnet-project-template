namespace PlutoNetCoreTemplate.Application.Command
{
    using MediatR;

    using System.ComponentModel.DataAnnotations;

    public class CreateProductCommand : BaseCommand, IRequest<Unit>
    {
        [Required(ErrorMessage = "产品名称不能为空")]
        public string ProductName { get; set; }
    }
}