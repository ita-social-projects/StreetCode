using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Users.Expertise;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Expertises.GetAll;

public class GetAllExpertisesHandler : IRequestHandler<GetAllExpertisesQuery, Result<List<ExpertiseDto>>>
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;

    public GetAllExpertisesHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public async Task<Result<List<ExpertiseDto>>> Handle(GetAllExpertisesQuery request, CancellationToken cancellationToken)
    {
       var expertises = await _repositoryWrapper.ExpertiseRepository.GetAllAsync();
       var expertisesDto = _mapper.Map<List<ExpertiseDto>>(expertises);

       return Result.Ok(expertisesDto);
    }
}