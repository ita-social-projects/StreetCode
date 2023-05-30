using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.Streetcode.Update;
using Streetcode.BLL.DTO.Streetcode.Update.Interface;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.Create;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Timeline;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.DAL.Repositories.Realizations.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.Update
{
	internal class UpdateStreetcodeHandler : IRequestHandler<UpdateStreetcodeCommand, Result<StreetcodeUpdateDTO>>
	{
		private readonly IMapper _mapper;
		private readonly IRepositoryWrapper _repositoryWrapper;

		public UpdateStreetcodeHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper)
		{
			_mapper = mapper;
			_repositoryWrapper = repositoryWrapper;
		}

		public async Task<Result<StreetcodeUpdateDTO>> Handle(UpdateStreetcodeCommand request, CancellationToken cancellationToken)
		{
            var streetcodeToUpdate = _mapper.Map<StreetcodeContent>(request.Streetcode);

            _repositoryWrapper.StreetcodeRepository.Update(streetcodeToUpdate);
            _repositoryWrapper.SaveChanges();

            // code to remove after inmplementation
            return await GetOld(streetcodeToUpdate.Id);
		}

		private async Task<StreetcodeUpdateDTO> GetOld(int id)
		{
			var updatedStreetcode = await _repositoryWrapper.StreetcodeRepository.GetFirstOrDefaultAsync(s => s.Id == id, include:
				x => x.Include(s => s.Text)
				.Include(s => s.Subtitles)
				.Include(s => s.TransactionLink));

			var updatedDTO = _mapper.Map<StreetcodeUpdateDTO>(updatedStreetcode);
			return updatedDTO;
		}

		private void Delete<T>(IEnumerable<T> entities)
              where T : IChanged
        {
            foreach(var entity in entities)
            {
				if (entity?.Changed == false)
                {
                    if(entity.GetType() == typeof(DAL.Entities.Streetcode.TextContent.Fact))
                    {
                        var fact = _mapper.Map<DAL.Entities.Streetcode.TextContent.Fact>(entity);
                        _repositoryWrapper.FactRepository.Delete(fact);
                    }
                }
            }
        }
    }
}
