using AutoMapper;
using Elevate.Models;
using Elevate.Shared;
using System;
using System.Globalization;
using Unity;

namespace Elevate
{
    public static class AutoMapper
    {
        public static void SetupMappingsAndRegisterWithIOC(UnityContainer container)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<string, DateTime>().ConvertUsing<StringToDateTimeConverter>();

                cfg.CreateMap<ListItemDTO, ListItem>()
                    .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.DisplayName))
                    .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id)).ReverseMap();

                cfg.CreateMap<SignUpMasterDataDTO, SignUpMasterDataModel>()
                    .ForMember(dest => dest.Companies, opt => opt.MapFrom(src => src.Companies))
                    .ForMember(dest => dest.UserTypes, opt => opt.MapFrom(src => src.UserTypes)).ReverseMap();

                cfg.CreateMap<UserModel, UserDTO>()
                    .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));

                cfg.CreateMap<UserDTO, UserModel>()
                    .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt != null ? src.CreatedAt.ToString("MM/dd/yyyy") : string.Empty));

                cfg.CreateMap<EmployeeDependentModel, EmployeeDependentDTO>()
                    .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                    .ForMember(dest => dest.RelationshipDisplayName, opt => opt.MapFrom(src => src.Relationship)).ReverseMap();

                cfg.CreateMap<EmployeeModel, EmployeeDTO>()
                    .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                    .ForMember(dest => dest.CompanyDisplayName, opt => opt.MapFrom(src => src.Company));

                cfg.CreateMap<EmployeeDTO, EmployeeModel>()
                    .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt != null ? src.CreatedAt.ToString("MM/dd/yyyy HH:mm:ss") : string.Empty))
                    .ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.CompanyDisplayName));

                cfg.CreateMap<EmployeeFormMasterDataDTO, EmployeeFormMasterDataModel>()
                    .ForMember(dest => dest.Relationships, opt => opt.MapFrom(src => src.Relationships)).ReverseMap();

                cfg.CreateMap<EBEmployeeListRequestModel, EBEmployeeListRequestDTO>().ReverseMap();

                cfg.CreateMap<TableDTO<EmployeeDTO>, TableModel<EmployeeModel>>()
                    .ForMember(dest => dest.Rows, opt => opt.MapFrom(src => src.Rows));

                cfg.CreateMap<EBDashbaordStatsCardModel, EBDashbaordStatsCardDTO>().ReverseMap();

            });

            IMapper mapper = config.CreateMapper();
            container.RegisterInstance(mapper);

        }
    }
}