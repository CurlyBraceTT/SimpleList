using AutoMapper;
using SimpleList.Models;

namespace SimpleList.Services
{
    public class ServicesMappingProfile : Profile
    {
        public ServicesMappingProfile()
        {
            CreateMap<ListItem, DataContext.ListItem>();
            CreateMap<DataContext.ListItem, ListItem>();
        }
    }
}
