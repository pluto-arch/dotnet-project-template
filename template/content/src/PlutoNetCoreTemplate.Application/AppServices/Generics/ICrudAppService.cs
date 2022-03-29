using System.Threading.Tasks;

namespace PlutoNetCoreTemplate.Application.AppServices.Generics
{
    using Models.Generics;

    public interface ICrudAppService<in TKey, TGetResponseDto, in TGetListRequestDto, TGetListResponseDto, in TUpdateRequestDto, in TCreateRequestDto> :
        IGetApService<TKey, TGetResponseDto>,
        IGetListAppService<TGetListRequestDto, TGetListResponseDto>,
        IUpdateAppService<TKey, TUpdateRequestDto, TGetResponseDto>,
        IDeleteAppService<TKey>,
        ICreateAppService<TCreateRequestDto, TGetResponseDto>
    where TGetListRequestDto : PageRequestDto
    {

    }


    public interface IGetApService<in TKey, TGetResponseDto>
    {
        Task<TGetResponseDto> GetAsync(TKey id);
    }


    public interface IGetListAppService<in TGetListRequestDto, TGetListResponseDto> where TGetListRequestDto : PageRequestDto
    {
        Task<PageResponseDto<TGetListResponseDto>> GetListAsync(TGetListRequestDto requestModel);

    }


    public interface IDeleteAppService<in TKey>
    {
        Task DeleteAsync(TKey id);
    }


    public interface ICreateAppService<in TCreateRequestDto, TGetResponseDto>
    {
        Task<TGetResponseDto> CreateAsync(TCreateRequestDto requestModel);
    }


    public interface IUpdateAppService<in TKey, in TUpdateRequestDto, TGetResponseDto>
    {
        Task<TGetResponseDto> UpdateAsync(TKey id, TUpdateRequestDto requestModel);
    }

}