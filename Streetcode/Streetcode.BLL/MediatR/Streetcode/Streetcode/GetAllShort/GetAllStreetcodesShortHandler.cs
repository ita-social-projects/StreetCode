using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetAllShort
{
    public class GetAllStreetcodesShortHandler : IRequestHandler<GetAllStreetcodesShortQuery,
        Result<IEnumerable<StreetcodeShortDTO>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;

        public GetAllStreetcodesShortHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<StreetcodeShortDTO>>> Handle(GetAllStreetcodesShortQuery request, CancellationToken cancellationToken)
        {
            var streetcodes = await _repositoryWrapper.StreetcodeRepository.GetAllAsync();
            if (streetcodes != null)
            {
                return Result.Ok(_mapper.Map<IEnumerable<StreetcodeShortDTO>>(streetcodes));
            }

            const string errorMsg = "No streetcodes exist now";
            _logger.LogError(request, errorMsg);
            return Result.Fail(errorMsg);
        }
    }
}
