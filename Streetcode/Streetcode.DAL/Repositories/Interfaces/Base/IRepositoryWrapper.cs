
using Repositories.Interfaces;

namespace StreetCode.DAL.Repositories.Interfaces.Base;

public interface IRepositoryWrapper
{
    IFactRepository FactRepository { get; }
    IMediaRepository MediaRepository { get; }
    IPartnersRepository PartnersRepository { get; }
    IStreetcodeRepository StreetcodeRepository { get; }
    ISubtitleRepository SubtitleRepository { get; }
    ITagRepository TagRepository { get; }
    ITermRepository TermRepository { get; }
    ITextRepository TextRepository { get; }
    ITimelineRepository TimelineRepository { get; }
    IToponymRepository ToponymRepository { get; }
    ITransactLinksRepository TransactLinksRepository { get; }
    public void All_Interfaces();

    public void Save();

    public Task SaveAsync();

}