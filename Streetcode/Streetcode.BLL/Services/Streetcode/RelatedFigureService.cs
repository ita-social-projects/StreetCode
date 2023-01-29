using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.Interfaces.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.Services.Streetcode;

public class RelatedFigureService : IRelatedFigureService
{
    private readonly IRepositoryWrapper _repositoryWrapper;

    public RelatedFigureService(IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
    }

    public void GetRelatedFiguresByStreetcodeId(int streetcodeId)
    {
        // TODO implement here
    }
}
