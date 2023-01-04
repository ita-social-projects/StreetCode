using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.MediatR.Streetcode.Fact.GetAll;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Text.GetByStreetcodeId
{
    public class GetTextByStreetcodeIdQueryHandler : IRequestHandler<GetTextByStreetcodeIdQuery, Result<TextDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;

        public GetTextByStreetcodeIdQueryHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        public async Task<Result<TextDTO>> Handle(GetTextByStreetcodeIdQuery request, CancellationToken cancellationToken)
        {
            var text = await _repositoryWrapper.TextRepository.GetFirstOrDefaultAsync(text => text.StreetcodeId == request.streetcodeId);
            if (text is null)
            {
                return Result.Fail(new Error($"Cannot find a fact by a streetcode Id: {request.streetcodeId}"));
            }

            var textDto = _mapper.Map<TextDTO>(text);
            return Result.Ok(textDto);
        }
    }
}
