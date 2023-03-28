using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.AdditionalContent.Tag.Create
{
    public class CreateTagHandler : IRequestHandler<CreateTagQuery, Result<TagDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;

        public CreateTagHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        public async Task<Result<TagDTO>> Handle(CreateTagQuery request, CancellationToken cancellationToken)
        {
            var newTag = (await _repositoryWrapper.TagRepository.CreateAsync(new DAL.Entities.AdditionalContent.Tag()
            {
                Title = request.tag.Title
            })).Entity;

            try
            {
                _repositoryWrapper.SaveChanges();
            }
            catch(Exception ex)
            {
                return Result.Fail(ex.ToString());
            }

            return Result.Ok(_mapper.Map<TagDTO>(newTag));
        }
    }
}
