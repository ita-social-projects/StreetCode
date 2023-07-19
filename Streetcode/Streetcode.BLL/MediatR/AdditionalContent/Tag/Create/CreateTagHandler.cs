using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.AdditionalContent.Tag.Create
{
  public class CreateTagHandler : IRequestHandler<CreateTagQuery, Result<TagDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;

        public CreateTagHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<TagDTO>> Handle(CreateTagQuery request, CancellationToken cancellationToken)
        {
            var newTag = await _repositoryWrapper.TagRepository.CreateAsync(new DAL.Entities.AdditionalContent.Tag()
            {
                Title = request.tag.Title
            });

            try
            {
                _repositoryWrapper.SaveChanges();
            }
            catch(Exception ex)
            {
                _logger.LogError(request, ex.ToString());
                return Result.Fail(ex.ToString());
            }

            return Result.Ok(_mapper.Map<TagDTO>(newTag));
        }
    }
}
