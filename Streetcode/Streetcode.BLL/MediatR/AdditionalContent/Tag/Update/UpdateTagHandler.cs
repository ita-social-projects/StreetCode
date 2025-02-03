using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;
using TagEntity = Streetcode.DAL.Entities.AdditionalContent.Tag;

namespace Streetcode.BLL.MediatR.AdditionalContent.Tag.Update
{
	public class UpdateTagHandler : IRequestHandler<UpdateTagCommand, Result<TagDto>>
	{
		private readonly IRepositoryWrapper _repositoryWrapper;
		private readonly IMapper _mapper;
		private readonly ILoggerService _logger;
		private readonly IStringLocalizer<FailedToValidateSharedResource> _stringLocalizerFailedToValidate;
		private readonly IStringLocalizer<FieldNamesSharedResource> _stringLocalizerFieldNames;
		private readonly IStringLocalizer<CannotFindSharedResource> _stringLocalizerCannotFind;
		public UpdateTagHandler(
			IRepositoryWrapper repository,
			IMapper mapper,
			ILoggerService logger,
			IStringLocalizer<FailedToValidateSharedResource> stringLocalizerFailedToValidate,
			IStringLocalizer<FieldNamesSharedResource> stringLocalizerFieldNames,
			IStringLocalizer<CannotFindSharedResource> stringLocalizerCannotFind)
		{
			_repositoryWrapper = repository;
			_mapper = mapper;
			_logger = logger;
			_stringLocalizerCannotFind = stringLocalizerCannotFind;
			_stringLocalizerFailedToValidate = stringLocalizerFailedToValidate;
			_stringLocalizerFieldNames = stringLocalizerFieldNames;
		}

		public async Task<Result<TagDto>> Handle(UpdateTagCommand request, CancellationToken cancellationToken)
		{
			var existedTag =
				await _repositoryWrapper.TagRepository.GetFirstOrDefaultAsync(x => x.Id == request.tag.Id);
			if (existedTag is null)
			{
				string exMessage = _stringLocalizerCannotFind["CannotFindTagWithCorrespondingId", request.tag.Id];
				_logger.LogError(request, exMessage);

				return Result.Fail(exMessage);
			}

			var exists = await _repositoryWrapper.TagRepository.GetFirstOrDefaultAsync(t => request.tag.Title == t.Title);

			if (exists is not null && exists.Id != request.tag.Id)
			{
				var errMessage = _stringLocalizerFailedToValidate["MustBeUnique", _stringLocalizerFieldNames["Tag"]];
				_logger.LogError(request, errMessage);

				return Result.Fail(errMessage);
			}

			try
			{
				var tagToUpdate = _mapper.Map<TagEntity>(request.tag);
				_repositoryWrapper.TagRepository.Update(tagToUpdate);
				await _repositoryWrapper.SaveChangesAsync();

				return Result.Ok(_mapper.Map<TagDto>(tagToUpdate));
			}
			catch(Exception ex)
			{
				_logger.LogError(request, ex.Message);

				return Result.Fail(ex.Message);
			}
		}
	}
}
