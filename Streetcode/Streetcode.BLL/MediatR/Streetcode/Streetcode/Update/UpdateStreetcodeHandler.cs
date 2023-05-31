using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
using Streetcode.BLL.DTO.Streetcode.Update;
using Streetcode.BLL.DTO.Streetcode.Update.TextContent;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.Create;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;

using Model = Streetcode.DAL.Entities.Streetcode.RelatedFigure;

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

			// _repositoryWrapper.StreetcodeRepository.Update(streetcodeToUpdate);

			UpdateRelatedFiguresRelation(request.Streetcode.RelatedFigures);

			_repositoryWrapper.SaveChanges();

			// code to remove after inmplementation
			return await GetOld(streetcodeToUpdate.Id);
		}

		private void UpdateRelatedFiguresRelation(IEnumerable<RelatedFigureUpdateDTO> relatedFigureUpdates)
		{
            foreach (var relatedFigureUpdate in relatedFigureUpdates)
            {
                var newRelation = _mapper.Map<Model>(relatedFigureUpdate);
                _repositoryWrapper.RelatedFigureRepository.Create(newRelation);
            }
        }

		private async Task<StreetcodeUpdateDTO> GetOld(int id)
		{
			var updatedStreetcode = await _repositoryWrapper.StreetcodeRepository.GetFirstOrDefaultAsync(s => s.Id == id, include:
				x => x.Include(s => s.Text)
				.Include(s => s.Subtitles));
			var updatedDTO = _mapper.Map<StreetcodeUpdateDTO>(updatedStreetcode);
			return updatedDTO;
		}
    }
}
