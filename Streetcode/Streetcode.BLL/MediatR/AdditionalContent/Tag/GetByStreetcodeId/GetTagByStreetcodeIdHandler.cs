using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.AdditionalContent.Tag;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.AdditionalContent.Tag.GetByStreetcodeId;

public class GetTagByStreetcodeIdHandler : IRequestHandler<GetTagByStreetcodeIdQuery, Result<IEnumerable<StreetcodeTagDTO>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetTagByStreetcodeIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<StreetcodeTagDTO>>> Handle(GetTagByStreetcodeIdQuery request, CancellationToken cancellationToken)
    {
        StreetcodeContent streetcode = await _repositoryWrapper.StreetcodeRepository.GetFirstOrDefaultAsync(s => s.Id == request.StreetcodeId);
        if(streetcode is null)
        {
            return Result.Fail(new Error($"Streetcode with id: {request.StreetcodeId} doesn`t exist"));
        }

        var tagIndexed = await _repositoryWrapper.StreetcodeTagIndexRepository
            .GetAllAsync(
                t => t.StreetcodeId == request.StreetcodeId,
                include: q => q.Include(t => t.Tag));

        if (tagIndexed is null)
        {
            return Result.Fail(new Error($"Cannot find any tag by the streetcode id: {request.StreetcodeId}"));
        }

        var tagDto = _mapper.Map<IEnumerable<StreetcodeTagDTO>>(tagIndexed.OrderBy(ti => ti.Index));
        return Result.Ok(tagDto);
    }
}
