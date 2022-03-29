namespace PlutoNetCoreTemplate.Application.Models.ProductModels
{
    using Generics;

    using System.ComponentModel.DataAnnotations;

    public class ProductPagedRequestDto : PageRequestDto
    {
        [MaxLength(3)]
        public string Keyword { get; set; }
    }
}