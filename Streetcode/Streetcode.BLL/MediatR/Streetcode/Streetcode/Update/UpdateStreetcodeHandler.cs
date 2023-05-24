using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode.Update;
using Streetcode.BLL.DTO.Streetcode.Update.TextContent;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.Create;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.Update.Interfaces;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;

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

			// code to remove after implementation
			var updatedStreetcode = await _repositoryWrapper.StreetcodeRepository.GetFirstOrDefaultAsync(s => s.Id == streetcodeToUpdate.Id);
			var updatedDTO = _mapper.Map<StreetcodeUpdateDTO>(updatedStreetcode);
			return updatedDTO;
		}

        public async Task Delete<T>(List<T> entities)
            where T : IDeleted
        {
              foreach(var entity in entities)
              {
                  if (entity.IsDeleted)
                  {
                      if(entity.GetType() == typeof(DAL.Entities.Streetcode.TextContent.Fact))
                      {
                          var fact = _mapper.Map<DAL.Entities.Streetcode.TextContent.Fact>(entity);
                          _repositoryWrapper.GetRepository<Fact>().Delete(fact);
                      }
                  }
              }
        }
	}
}
