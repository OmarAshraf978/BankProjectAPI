using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Bank.Domain.Entities.AccountModule;
using Bank.Shared.DTO.AccountDto;

namespace Bank.Service.MappingProfiles
{
    public class BankAccountProfile : Profile
    {
        public BankAccountProfile()
        {
            CreateMap<BankAccount, BankAccountDto>()
                     .ForMember(dest => dest.AccountType, opt => opt.MapFrom(src => src.AccountType.ToString()));
            CreateMap<BankAccountDto, BankAccount>()
                     .ForMember(dest => dest.AccountType, opt => opt.MapFrom(src => Enum.Parse<AccountType>(src.AccountType)));
        }
    }
}
