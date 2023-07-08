using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetAllShort
{
    public class GetAllStreetcodesShortHandler : IRequestHandler<GetAllStreetcodesShortQuery,
        Result<IEnumerable<StreetcodeShortDTO>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IStringLocalizer<NoSharedResource> _stringLocalizerNo;

        public GetAllStreetcodesShortHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, IStringLocalizer<NoSharedResource> stringLocalizerNo)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _stringLocalizerNo = stringLocalizerNo;
        }

        public async Task<Result<IEnumerable<StreetcodeShortDTO>>> Handle(GetAllStreetcodesShortQuery request, CancellationToken cancellationToken)
        {
            var streetcodes = await _repositoryWrapper.StreetcodeRepository.GetAllAsync();
            if (streetcodes != null)
            {
                return Result.Ok(_mapper.Map<IEnumerable<StreetcodeShortDTO>>(streetcodes));
            }

            return Result.Fail(_stringLocalizerNo["NoStreetcodesExistNow"].Value);
        }
    }
}
