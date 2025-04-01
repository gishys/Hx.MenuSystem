using AutoMapper;
using Hx.MenuSystem.Application.Contracts;
using Hx.MenuSystem.Domain;

namespace Hx.MenuSystem.Application
{
    internal class MenuSystemProfile : Profile
    {
        public MenuSystemProfile()
        {
            CreateMap<Menu, MenuDto>(MemberList.None);
        }
    }
}
