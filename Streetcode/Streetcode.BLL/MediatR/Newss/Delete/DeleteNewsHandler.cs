using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.News;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Newss.Delete
{
    public class DeleteNewsHandler : IRequestHandler<DeleteNewsCommand, Result<Unit>>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IStringLocalizer<FailedToDeleteSharedResource> _stringLocalizerFailedToDelete;
        private readonly IStringLocalizer<NoSharedResource> _stringLocalizerNo;
        public DeleteNewsHandler(IRepositoryWrapper repositoryWrapper, IStringLocalizer<NoSharedResource> stringLocalizerNo, IStringLocalizer<FailedToDeleteSharedResource> stringLocalizerFailedToDelete)
        {
            _repositoryWrapper = repositoryWrapper;
            _stringLocalizerNo = stringLocalizerNo;
            _stringLocalizerFailedToDelete = stringLocalizerFailedToDelete;
        }

        public async Task<Result<Unit>> Handle(DeleteNewsCommand request, CancellationToken cancellationToken)
        {
            int id = request.id;
            var news = await _repositoryWrapper.NewsRepository.GetFirstOrDefaultAsync(n => n.Id == id);
            if (news == null)
            {
                return Result.Fail(_stringLocalizerNo["NoNewsFoundByEnteredId", id].Value);
            }

            if (news.Image is not null)
            {
                _repositoryWrapper.ImageRepository.Delete(news.Image);
            }

            _repositoryWrapper.NewsRepository.Delete(news);
            var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;
            return resultIsSuccess ? Result.Ok(Unit.Value) : Result.Fail(new Error(_stringLocalizerFailedToDelete["FailedToDeleteNews"].Value));
        }
    }
}
