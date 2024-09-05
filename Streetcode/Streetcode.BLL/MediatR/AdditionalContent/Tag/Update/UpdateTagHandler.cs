using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;
using TagEntity = Streetcode.DAL.Entities.AdditionalContent.Tag;

namespace Streetcode.BLL.MediatR.AdditionalContent.Tag.Update
{
	public class UpdateTagHandler : IRequestHandler<UpdateTagCommand, Result<TagDTO>>
	{
		private readonly IRepositoryWrapper _repositoryWrapper;
		private readonly IMapper _mapper;
		private readonly ILoggerService _logger;
		public UpdateTagHandler(IRepositoryWrapper repository, IMapper mapper, ILoggerService logger)
		{
			_repositoryWrapper = repository;
			_mapper = mapper;
			_logger = logger;
		}

		public async Task<Result<TagDTO>> Handle(UpdateTagCommand request, CancellationToken cancellationToken)
		{
			var existedTag =
				await _repositoryWrapper.TagRepository.GetFirstOrDefaultAsync(x => x.Id == request.tag.Id);
			if (existedTag is null)
			{
				string exMessage = $"No tag found by entered Id - {request.tag.Id}";
				_logger.LogError(request, exMessage);
				return Result.Fail(exMessage);
			}

			existedTag = await _repositoryWrapper.TagRepository.GetFirstOrDefaultAsync(x => x.Title == request.tag.Title);

			if (existedTag is not null)
			{
                var errMessage = $"Tag with title {request.tag.Title} already exists";
                _logger.LogError(request, errMessage);
                return Result.Fail(errMessage);
            }

			try
			{
				var tagToUpdate = _mapper.Map<TagEntity>(request.tag);
				_repositoryWrapper.TagRepository.Update(tagToUpdate);
				await _repositoryWrapper.SaveChangesAsync();
				return Result.Ok(_mapper.Map<TagDTO>(tagToUpdate));
			}
			catch(Exception ex)
			{
				_logger.LogError(request, ex.Message);
				return Result.Fail(ex.Message);
			}
		}
	}
}
