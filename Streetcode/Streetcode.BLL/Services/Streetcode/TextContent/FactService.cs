using AutoMapper;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.Interfaces.Streetcode.TextContent;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using StreetCode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.Services.Streetcode.TextContent;

public class FactService : IFactService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;
    public FactService(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public string GetFactsByStreetcodeAsync()
    {
        // TODO clean after merge
        Fact fact = new Fact();
        fact.Id = 1;
        fact.Title = "Test1";
        FactDTO factDTO = _mapper.Map<Fact, FactDTO>(fact);

        return factDTO.Text;
    }
}