using System.Transactions;
using Repositories.Interfaces;
using Streetcode.DAL.Persistence;
using Streetcode.DAL.Repositories.Interfaces.AdditionalContent;
using Streetcode.DAL.Repositories.Interfaces.Analytics;
using Streetcode.DAL.Repositories.Interfaces.Base;
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
using Streetcode.DAL.Repositories.Realizations.AdditionalContent;
using Streetcode.DAL.Repositories.Realizations.Analytics;
using Streetcode.DAL.Repositories.Realizations.Media;
using Streetcode.DAL.Repositories.Realizations.Media.Images;
using Streetcode.DAL.Repositories.Realizations.Newss;
using Streetcode.DAL.Repositories.Realizations.Partners;
using Streetcode.DAL.Repositories.Realizations.Source;
using Streetcode.DAL.Repositories.Realizations.Streetcode;
using Streetcode.DAL.Repositories.Realizations.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Realizations.Team;
using Streetcode.DAL.Repositories.Realizations.Timeline;
using Streetcode.DAL.Repositories.Realizations.Toponyms;
using Streetcode.DAL.Repositories.Realizations.Transactions;
using Streetcode.DAL.Repositories.Realizations.Users;

namespace Streetcode.DAL.Repositories.Realizations.Base;

public class RepositoryWrapper : IRepositoryWrapper
{
    private readonly StreetcodeDbContext _streetcodeDbContext;

    private IVideoRepository _videoRepository;

    private IAudioRepository _audioRepository;

    private IStreetcodeCoordinateRepository _streetcodeCoordinateRepository;

    private IImageRepository _imageRepository;

    private IImageDetailsRepository _imageDetailsRepository;

    private IArtRepository _artRepository;

    private IStreetcodeArtRepository _streetcodeArtRepository;

    private IFactRepository _factRepository;

    private IPartnersRepository _partnersRepository;

    private ISourceCategoryRepository _sourceCategoryRepository;

    private IStreetcodeCategoryContentRepository _streetcodeCategoryContentRepository;

    private IRelatedFigureRepository _relatedFigureRepository;

    private IRelatedTermRepository _relatedTermRepository;

    private IStreetcodeRepository _streetcodeRepository;

    private ISubtitleRepository _subtitleRepository;

    private IStatisticRecordRepository _statisticRecordRepository;

    private ITagRepository _tagRepository;

    private ITermRepository _termRepository;

    private ITeamRepository _teamRepository;

    private IPositionRepository _positionRepository;

    private ITextRepository _textRepository;

    private ITimelineRepository _timelineRepository;

    private IToponymRepository _toponymRepository;

    private ITransactLinksRepository _transactLinksRepository;

    private IHistoricalContextRepository _historyContextRepository;

    private IPartnerSourceLinkRepository _partnerSourceLinkRepository;

    private IUserRepository _userRepository;

    private IStreetcodeTagIndexRepository _streetcodeTagIndexRepository;

    private IPartnerStreetcodeRepository _partnerStreetcodeRepository;

    private INewsRepository _newsRepository;

    private ITeamLinkRepository _teamLinkRepository;

    private ITeamPositionRepository _teamPositionRepository;

    private IHistoricalContextTimelineRepository _historicalContextTimelineRepository;

    private IStreetcodeToponymRepository _streetcodeToponymRepository;

    private IStreetcodeImageRepository _streetcodeImageRepository;

    public RepositoryWrapper(StreetcodeDbContext streetcodeDbContext)
    {
        _streetcodeDbContext = streetcodeDbContext;
    }

    public INewsRepository NewsRepository
    {
        get
        {
            if (_newsRepository is null)
            {
                _newsRepository = new NewsRepository(_streetcodeDbContext);
            }

            return _newsRepository;
        }
    }

    public IFactRepository FactRepository
    {
        get
        {
            if (_factRepository is null)
            {
                _factRepository = new FactRepository(_streetcodeDbContext);
            }

            return _factRepository;
        }
    }

    public IImageRepository ImageRepository
    {
        get
        {
            if (_imageRepository is null)
            {
                _imageRepository = new ImageRepository(_streetcodeDbContext);
            }

            return _imageRepository;
        }
    }

    public ITeamRepository TeamRepository
    {
        get
        {
            if (_teamRepository is null)
            {
                _teamRepository = new TeamRepository(_streetcodeDbContext);
            }

            return _teamRepository;
        }
    }

    public ITeamPositionRepository TeamPositionRepository
    {
        get
        {
            if (_teamPositionRepository is null)
            {
                _teamPositionRepository = new TeamPositionRepository(_streetcodeDbContext);
            }

            return _teamPositionRepository;
        }
    }

    public IAudioRepository AudioRepository
    {
        get
        {
            if (_audioRepository is null)
            {
                _audioRepository = new AudioRepository(_streetcodeDbContext);
            }

            return _audioRepository;
        }
    }

    public IStreetcodeCoordinateRepository StreetcodeCoordinateRepository
    {
        get
        {
            if (_streetcodeCoordinateRepository is null)
            {
                _streetcodeCoordinateRepository = new StreetcodeCoordinateRepository(_streetcodeDbContext);
            }

            return _streetcodeCoordinateRepository;
        }
    }

    public IVideoRepository VideoRepository
    {
        get
        {
            if (_videoRepository is null)
            {
                _videoRepository = new VideoRepository(_streetcodeDbContext);
            }

            return _videoRepository;
        }
    }

    public IArtRepository ArtRepository
    {
        get
        {
            if (_artRepository is null)
            {
                _artRepository = new ArtRepository(_streetcodeDbContext);
            }

            return _artRepository;
        }
    }

    public IStreetcodeArtRepository StreetcodeArtRepository
    {
        get
        {
            if (_streetcodeArtRepository is null)
            {
                _streetcodeArtRepository = new StreetcodeArtRepository(_streetcodeDbContext);
            }

            return _streetcodeArtRepository;
        }
    }

    public IPartnersRepository PartnersRepository
    {
        get
        {
            if (_partnersRepository is null)
            {
                _partnersRepository = new PartnersRepository(_streetcodeDbContext);
            }

            return _partnersRepository;
        }
    }

    public ISourceCategoryRepository SourceCategoryRepository
    {
        get
        {
            if (_sourceCategoryRepository is null)
            {
                _sourceCategoryRepository = new SourceCategoryRepository(_streetcodeDbContext);
            }

            return _sourceCategoryRepository;
        }
    }

    public IStreetcodeCategoryContentRepository StreetcodeCategoryContentRepository
    {
        get
        {
            if (_streetcodeCategoryContentRepository is null)
            {
                _streetcodeCategoryContentRepository = new StreetcodeCategoryContentRepository(_streetcodeDbContext);
            }

            return _streetcodeCategoryContentRepository;
        }
    }

    public IRelatedFigureRepository RelatedFigureRepository
    {
        get
        {
            if (_relatedFigureRepository is null)
            {
                _relatedFigureRepository = new RelatedFigureRepository(_streetcodeDbContext);
            }

            return _relatedFigureRepository;
        }
    }

    public IStreetcodeRepository StreetcodeRepository
    {
        get
        {
            if (_streetcodeRepository is null)
            {
                _streetcodeRepository = new StreetcodeRepository(_streetcodeDbContext);
            }

            return _streetcodeRepository;
        }
    }

    public ISubtitleRepository SubtitleRepository
    {
        get
        {
            if (_subtitleRepository is null)
            {
                _subtitleRepository = new SubtitleRepository(_streetcodeDbContext);
            }

            return _subtitleRepository;
        }
    }

    public IStatisticRecordRepository StatisticRecordRepository
    {
        get
        {
            if (_statisticRecordRepository is null)
            {
                _statisticRecordRepository = new StatisticRecordsRepository(_streetcodeDbContext);
            }

            return _statisticRecordRepository;
        }
    }

    public ITagRepository TagRepository
    {
        get
        {
            if (_tagRepository is null)
            {
                _tagRepository = new TagRepository(_streetcodeDbContext);
            }

            return _tagRepository;
        }
    }

    public ITermRepository TermRepository
    {
        get
        {
            if (_termRepository is null)
            {
                _termRepository = new TermRepository(_streetcodeDbContext);
            }

            return _termRepository;
        }
    }

    public ITextRepository TextRepository
    {
        get
        {
            if (_textRepository is null)
            {
                _textRepository = new TextRepository(_streetcodeDbContext);
            }

            return _textRepository;
        }
    }

    public ITimelineRepository TimelineRepository
    {
        get
        {
            if (_timelineRepository is null)
            {
                _timelineRepository = new TimelineRepository(_streetcodeDbContext);
            }

            return _timelineRepository;
        }
    }

    public IToponymRepository ToponymRepository
    {
        get
        {
            if (_toponymRepository is null)
            {
                _toponymRepository = new ToponymRepository(_streetcodeDbContext);
            }

            return _toponymRepository;
        }
    }

    public ITransactLinksRepository TransactLinksRepository
    {
        get
        {
            if (_transactLinksRepository is null)
            {
                _transactLinksRepository = new TransactLinksRepository(_streetcodeDbContext);
            }

            return _transactLinksRepository;
        }
    }

    public IHistoricalContextRepository HistoricalContextRepository
    {
        get
        {
            if (_historyContextRepository is null)
            {
                _historyContextRepository = new HistoricalContextRepository(_streetcodeDbContext);
            }

            return _historyContextRepository;
        }
    }

    public IPartnerSourceLinkRepository PartnerSourceLinkRepository
    {
        get
        {
            if (_partnerSourceLinkRepository is null)
            {
                _partnerSourceLinkRepository = new PartnersourceLinksRepository(_streetcodeDbContext);
            }

            return _partnerSourceLinkRepository;
        }
    }

    public IRelatedTermRepository RelatedTermRepository
    {
        get
        {
            if(_relatedTermRepository is null)
            {
                _relatedTermRepository = new RelatedTermRepository(_streetcodeDbContext);
            }

            return _relatedTermRepository;
        }
    }

    public IUserRepository UserRepository
    {
        get
        {
            if (_userRepository is null)
            {
                _userRepository = new UserRepository(_streetcodeDbContext);
            }

            return _userRepository;
        }
    }

    public IStreetcodeTagIndexRepository StreetcodeTagIndexRepository
    {
        get
        {
            if (_streetcodeTagIndexRepository is null)
            {
                _streetcodeTagIndexRepository = new StreetcodeTagIndexRepository(_streetcodeDbContext);
            }

            return _streetcodeTagIndexRepository;
        }
    }

    public IPartnerStreetcodeRepository PartnerStreetcodeRepository
    {
        get
        {
            if(_partnerStreetcodeRepository is null)
            {
                _partnerStreetcodeRepository = new PartnerStreetodeRepository(_streetcodeDbContext);
            }

            return _partnerStreetcodeRepository;
        }
    }

    public IPositionRepository PositionRepository
    {
        get
        {
            if (_positionRepository is null)
            {
                _positionRepository = new PositionRepository(_streetcodeDbContext);
            }

            return _positionRepository;
        }
    }

    public ITeamLinkRepository TeamLinkRepository
    {
        get
        {
            if (_teamLinkRepository is null)
            {
                _teamLinkRepository = new TeamLinkRepository(_streetcodeDbContext);
            }

            return _teamLinkRepository;
        }
    }

    public IImageDetailsRepository ImageDetailsRepository => _imageDetailsRepository??=new ImageDetailsRepository(_streetcodeDbContext);

    public IHistoricalContextTimelineRepository HistoricalContextTimelineRepository
    {
        get
        {
            if (_historicalContextTimelineRepository is null)
            {
                _historicalContextTimelineRepository = new HistoricalContextTimelineRepository(_streetcodeDbContext);
            }

            return _historicalContextTimelineRepository;
        }
    }

    public IStreetcodeToponymRepository StreetcodeToponymRepository
	{
		get
		{
			if (_streetcodeToponymRepository is null)
			{
				_streetcodeToponymRepository = new StreetcodeToponymRepository(_streetcodeDbContext);
			}

			return _streetcodeToponymRepository;
		}
	}

    public IStreetcodeImageRepository StreetcodeImageRepository
	{
		get
		{
			if (_streetcodeImageRepository is null)
			{
				_streetcodeImageRepository = new StreetcodeImageRepository(_streetcodeDbContext);
			}

			return _streetcodeImageRepository;
		}
	}

    public int SaveChanges()
    {
        return _streetcodeDbContext.SaveChanges();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _streetcodeDbContext.SaveChangesAsync();
    }

    public TransactionScope BeginTransaction()
    {
        return new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
    }
}
