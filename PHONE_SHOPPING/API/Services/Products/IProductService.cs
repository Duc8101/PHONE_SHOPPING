using Common.Base;
using Common.DTO.ProductDTO;

namespace API.Services.Products
{
    public interface IProductService
    {
        ResponseBase List(bool isAdmin, string? name, int? CategoryID, int page);
        ResponseBase Create(ProductCreateUpdateDTO DTO);
        ResponseBase Detail(Guid ProductID);
        ResponseBase Update(Guid ProductID, ProductCreateUpdateDTO DTO);
        ResponseBase Delete(Guid ProductID);
    }
}
