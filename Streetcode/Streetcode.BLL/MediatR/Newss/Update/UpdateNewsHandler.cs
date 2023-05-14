using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.DAL.Entities.News;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Newss.Update
{
    public class UpdateNewsHandler : IRequestHandler<UpdateNewsCommand, Result<Unit>>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        public UpdateNewsHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        public async Task<Result<Unit>> Handle(UpdateNewsCommand request, CancellationToken cancellationToken)
        {
            var news = _mapper.Map<News>(request.news);
            if (news is null)
            {
                return Result.Fail(new Error("Cannot convert null to news"));
            }

            _repositoryWrapper.NewsRepository.Update(news);
            var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;
            return resultIsSuccess ? Result.Ok(Unit.Value) : Result.Fail(new Error("Failed to update news"));
        }
    }
}
