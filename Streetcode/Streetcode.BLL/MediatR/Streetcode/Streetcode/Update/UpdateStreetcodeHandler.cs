using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
using Streetcode.BLL.DTO.Streetcode.Update;
using Streetcode.BLL.DTO.Streetcode.Update.Interface;
using Streetcode.BLL.DTO.Streetcode.Update.TextContent;
using Streetcode.BLL.DTO.Streetcode.Update.Toponyms;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.Create;
using Streetcode.DAL.Entities.Partners;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Timeline;
using Streetcode.DAL.Entities.Toponyms;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.DAL.Repositories.Realizations.Base;
using PartnersModel = Streetcode.DAL.Entities.Partners.Partner;
using RelatedFigureModel = Streetcode.DAL.Entities.Streetcode.RelatedFigure;

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
            UpdateStreetcodeToponym(request.Streetcode.StreetcodeToponym);

			// UpdateRelatedFiguresRelation(request.Streetcode.RelatedFigures);
			// UpdatePartnersRelation(request.Streetcode.Partners);

            _repositoryWrapper.SaveChanges();

            // code to remove after inmplementation
            return await GetOld(streetcodeToUpdate.Id);
		}

		private void UpdateStreetcodeToponym(IEnumerable<StreetcodeToponymUpdateDTO> streetcodeToponymsDTO)
		{
            var toDelete = streetcodeToponymsDTO.Where(t => t.IsChanged == false);
            var toCreate = streetcodeToponymsDTO.Where(t => t.IsChanged == true);

            foreach (var streetcodeToponymToDelete in toDelete)
			{
                var streetcodeToponym = _mapper.Map<StreetcodeToponym>(streetcodeToponymToDelete);
                _repositoryWrapper.StreetcodeToponymRepository.Delete(streetcodeToponym);
			}

            foreach (var streetcodeToponymToCreate in toCreate)
			{
                var streetcodeToponym = _mapper.Map<StreetcodeToponym>(streetcodeToponymToCreate);
                _repositoryWrapper.StreetcodeToponymRepository.Create(streetcodeToponym);
			}
		}

		private void UpdateRelatedFiguresRelation(IEnumerable<RelatedFigureUpdateDTO> relatedFigureUpdates)
		{
            var relationsToCreate = relatedFigureUpdates.Where(_ => _.IsChanged == true);
            var relationsToDelete = relatedFigureUpdates.Where(_ => _.IsChanged == false);

            foreach (var relationToCreate in relationsToCreate)
            {
                var relation = _mapper.Map<RelatedFigureModel>(relationToCreate);
                _repositoryWrapper.RelatedFigureRepository.Create(relation);
            }

            foreach(var relationToDelete in relationsToDelete)
            {
                var relation = _mapper.Map<RelatedFigureModel>(relationToDelete);
                _repositoryWrapper.RelatedFigureRepository.Delete(relation);
            }
        }

		private void UpdatePartnersRelation(IEnumerable<PartnersUpdateDTO> partnersUpdateDTOs)
        {
            var relationsToCreate = partnersUpdateDTOs.Where(_ => _.IsChanged == true);
            var relationsToDelete = partnersUpdateDTOs.Where(_ => _.IsChanged == false);

            foreach (var relationToCreate in relationsToCreate)
            {
                var relation = _mapper.Map<StreetcodePartner>(relationToCreate);
                _repositoryWrapper.PartnerStreetcodeRepository.Create(relation);
            }

            foreach (var relationToDelete in relationsToDelete)
            {
                var relation = _mapper.Map<StreetcodePartner>(relationToDelete);
                _repositoryWrapper.PartnerStreetcodeRepository.Delete(relation);
            }
        }

		private async Task<StreetcodeUpdateDTO> GetOld(int id)
        {
            var updatedStreetcode = await _repositoryWrapper.StreetcodeRepository.GetFirstOrDefaultAsync(s => s.Id == id, include:
                x => x.Include(s => s.Text)
                .Include(s => s.Subtitles)
                .Include(s => s.TransactionLink)
                .Include(s => s.Toponyms));

            var updatedDTO = _mapper.Map<StreetcodeUpdateDTO>(updatedStreetcode);
            return updatedDTO;
		}

		private void Delete<T>(IEnumerable<T> entities)
              where T : IChanged
        {
            foreach(var entity in entities)
            {
				if (entity?.IsChanged == false)
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
