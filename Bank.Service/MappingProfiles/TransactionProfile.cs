using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Bank.Domain.Entities.TransactionModule;
using Bank.Shared.DTO.TransactionsDto;

namespace Bank.Service.MappingProfiles
{
    public class TransactionProfile : Profile
    {
        public TransactionProfile() 
        { 
            CreateMap<Transaction, TransactionDto>()
                    .ForMember(dest => dest.TransactionType, opt => opt.MapFrom(src => src.Type.ToString()));
            CreateMap<TransactionDto, Transaction>()
                    .ForMember(dest => dest.Type, opt => opt.MapFrom(src => Enum.Parse<TransactionType>(src.TransactionType, true)));
            CreateMap<Transaction, TransactionToReturnDto>()
                    .ForMember(dest => dest.TransactionType, opt => opt.MapFrom(src => src.Type.ToString()))
                    .ForMember(dest => dest.TransactionStatus, opt => opt.MapFrom(src => src.Status.ToString()));
        }
    }
}
