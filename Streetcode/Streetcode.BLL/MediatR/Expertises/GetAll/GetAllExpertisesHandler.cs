using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Jobs;
using Streetcode.BLL.DTO.Users.Expertise;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Jobs.GetAll;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Expertises.GetAll;

public class GetAllExpertisesHandler : IRequestHandler<GetAllExpertisesQuery, Result<List<ExpertiseDTO>>>
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;
    private readonly ILoggerService _logger;

    public GetAllExpertisesHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<List<ExpertiseDTO>>> Handle(GetAllExpertisesQuery request, CancellationToken cancellationToken)
    {
       var expertises = await _repositoryWrapper.ExpertiseRepository.GetAllAsync();
       var expertisesDto = _mapper.Map<List<ExpertiseDTO>>(expertises);

       return Result.Ok(expertisesDto);
    }
}