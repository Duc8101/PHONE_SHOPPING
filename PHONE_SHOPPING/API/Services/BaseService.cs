using AutoMapper;

namespace API.Services
{
    public class BaseService
    {
        protected readonly IMapper mapper;
        public BaseService(IMapper mapper)
        {
            this.mapper = mapper;
        }
    }
}
