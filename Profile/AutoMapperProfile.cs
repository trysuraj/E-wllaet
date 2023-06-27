using AutoMapper;
using System;
using WalletTransaction.Model;
using WalletTransaction.Model.DTO;

namespace WalletTransaction.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RegisterNewWalletModel, Wallet>();
            CreateMap <UpdateWalletModel, Wallet>();
            CreateMap< Wallet,GetwalletModel>();
            CreateMap<TransationRequestDto, Transaction>();
        }

    }
}