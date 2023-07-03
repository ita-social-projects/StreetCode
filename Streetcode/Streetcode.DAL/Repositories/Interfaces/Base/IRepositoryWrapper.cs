using System.Transactions;
using Repositories.Interfaces;
using Streetcode.DAL.Repositories.Interfaces.AdditionalContent;
using Streetcode.DAL.Repositories.Interfaces.Analytics;
using Streetcode.DAL.Repositories.Interfaces.Media.Images;
using Streetcode.DAL.Repositories.Interfaces.Newss;
using Streetcode.DAL.Repositories.Interfaces.Partners;
using Streetcode.DAL.Repositories.Interfaces.Source;
using Streetcode.DAL.Repositories.Interfaces.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Team;
using Streetcode.DAL.Repositories.Interfaces.Timeline;
using Streetcode.DAL.Repositories.Interfaces.Toponyms;
using Streetcode.DAL.Repositories.Interfaces.Transactions;
using Streetcode.DAL.Repositories.Interfaces.Users;
namespace Streetcode.DAL.Repositories.Interfaces.Base;

public interface IRepositoryWrapper
{
    IFactRepository FactRepository { get; }
    IArtRepository ArtRepository { get; }
    IStreetcodeArtRepository StreetcodeArtRepository { get; }
    IVideoRepository VideoRepository { get; }
    IImageRepository ImageRepository { get; }
    IImageDetailsRepository ImageDetailsRepository { get; }
    IAudioRepository AudioRepository { get; }
    IStreetcodeCoordinateRepository StreetcodeCoordinateRepository { get; }
    IPartnersRepository PartnersRepository { get; }
    ISourceCategoryRepository SourceCategoryRepository { get; }
    IStreetcodeCategoryContentRepository StreetcodeCategoryContentRepository { get; }
    IRelatedFigureRepository RelatedFigureRepository { get; }
    IStreetcodeRepository StreetcodeRepository { get; }
    ISubtitleRepository SubtitleRepository { get; }
    IStatisticRecordRepository StatisticRecordRepository { get; }
    ITagRepository TagRepository { get; }
    ITeamRepository TeamRepository { get; }
    ITeamPositionRepository TeamPositionRepository { get; }
    ITeamLinkRepository TeamLinkRepository { get; }
    ITermRepository TermRepository { get; }
    IRelatedTermRepository RelatedTermRepository { get; }
    ITextRepository TextRepository { get; }
    ITimelineRepository TimelineRepository { get; }
    IToponymRepository ToponymRepository { get; }
    ITransactLinksRepository TransactLinksRepository { get; }
    IHistoricalContextRepository HistoricalContextRepository { get; }
    IPartnerSourceLinkRepository PartnerSourceLinkRepository { get; }
    IUserRepository UserRepository { get; }
    IStreetcodeTagIndexRepository StreetcodeTagIndexRepository { get; }
    IPartnerStreetcodeRepository PartnerStreetcodeRepository { get;  }
    INewsRepository NewsRepository { get; }
    IPositionRepository PositionRepository { get; }
    IHistoricalContextTimelineRepository HistoricalContextTimelineRepository { get; }
    IStreetcodeToponymRepository StreetcodeToponymRepository { get; }
    IStreetcodeImageRepository StreetcodeImageRepository { get; }
    public int SaveChanges();

    public Task<int> SaveChangesAsync();

    public TransactionScope BeginTransaction();
}
