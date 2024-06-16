using Common.Base;
using Common.DTO.ProductDTO;
using Common.Pagination;

namespace API.Services.IService
{
    public interface IProductService
    {
        ResponseBase<Pagination<ProductListDTO>?> List(bool isAdmin, string? name, int? CategoryID, int page);
        ResponseBase<bool> Create(ProductCreateUpdateDTO DTO);
        ResponseBase<ProductListDTO?> Detail(Guid ProductID);
        ResponseBase<ProductListDTO?> Update(Guid ProductID, ProductCreateUpdateDTO DTO);
        ResponseBase<bool> Delete(Guid ProductID);
    }
}
