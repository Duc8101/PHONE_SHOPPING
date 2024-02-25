using AutoMapper;

namespace API.Services
{
    public class BaseService
    {
        internal readonly IMapper mapper;
        public BaseService(IMapper mapper)
        {
            this.mapper = mapper;
        }
    }
}
