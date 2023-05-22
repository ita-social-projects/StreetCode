using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.GetAllStreetcodesMainPage;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetAllMainPage
{
    public class GetAllStreetcodesMainPageHandler : IRequestHandler<GetAllStreetcodesMainPageQuery,
        Result<IEnumerable<StreetcodeMainPageDTO>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;

        public GetAllStreetcodesMainPageHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<StreetcodeMainPageDTO>>> Handle(GetAllStreetcodesMainPageQuery request, CancellationToken cancellationToken)
        {
            var streetcodes = await _repositoryWrapper.StreetcodeRepository.GetAllAsync(
                 include: src => src.Include(item => item.Text).Include(item => item.Images));

            if (streetcodes != null)
            {
                return Result.Ok(_mapper.Map<IEnumerable<StreetcodeMainPageDTO>>(streetcodes));
            }

            return Result.Fail("No streetcodes exist now");
        }
    }
}
