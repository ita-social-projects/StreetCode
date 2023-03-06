using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.WithIndexExist
{
    public class StreetcodeWithIndexExistHandler : IRequestHandler<StreetcodeWithIndexExistQuery, Result<bool>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;

        public StreetcodeWithIndexExistHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        public async Task<Result<bool>> Handle(StreetcodeWithIndexExistQuery request, CancellationToken cancellationToken)
        {
            var streetcode = await _repositoryWrapper.StreetcodeRepository.GetFirstOrDefaultAsync(s => s.Index == request.index);
            if (streetcode == null)
            {
                return Result.Ok(false);
            }

            return Result.Ok(true);
        }
    }
}
