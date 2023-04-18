using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.GetAllShort;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetCount
{
    public class GetStreetcodesCountHander : IRequestHandler<GetStreetcodesCountQuery,
        Result<int>>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public GetStreetcodesCountHander(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<Result<int>> Handle(GetStreetcodesCountQuery request, CancellationToken cancellationToken)
        {
            var streetcodes = await _repositoryWrapper.StreetcodeRepository.GetAllAsync();

            if (streetcodes != null)
            {
                int count = streetcodes.Count();
                return Result.Ok(count);
            }

            return Result.Fail("No streetcodes exist now");
        }
    }
}
