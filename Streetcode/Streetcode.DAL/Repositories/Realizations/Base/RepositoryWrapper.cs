using Repositories.Interfaces;
using Streetcode.DAL.Persistence;
using Streetcode.DAL.Repositories.Interfaces.AdditionalContent;
using Streetcode.DAL.Repositories.Interfaces.Partners;
using Streetcode.DAL.Repositories.Interfaces.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Timeline;
using Streetcode.DAL.Repositories.Interfaces.Toponyms;
using Streetcode.DAL.Repositories.Interfaces.Transactions;
using Streetcode.DAL.Repositories.Realizations.AdditionalContent;
using Streetcode.DAL.Repositories.Realizations.Partners;
using Streetcode.DAL.Repositories.Realizations.Streetcode;
using Streetcode.DAL.Repositories.Realizations.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Realizations.Timeline;
using Streetcode.DAL.Repositories.Realizations.Toponyms;
using Streetcode.DAL.Repositories.Realizations.Transactions;
using StreetCode.DAL.Repositories.Interfaces.Base;


namespace Repositories.Realizations;

public class RepositoryWrapper : IRepositoryWrapper 
{

    public RepositoryWrapper(StreetcodeDbContext streetcodeDbContext) 
    {
        _streetcodeDbContext = streetcodeDbContext;
    }

    private StreetcodeDbContext _streetcodeDbContext;

    private IVideoRepository _videoRepository;

    private IAudioRepository _audioRepository;

    private IImageRepository _imageRepository;

    private IArtRepository _artRepository;

    private IFactRepository _factRepository;

    private IPartnersRepository _partnersRepository;

    private IStreetcodeRepository _streetcodeRepository;

    private ISubtitleRepository _subtitleRepository;

    private ITagRepository _tagRepository;

    private ITermRepository _termRepository;

    private ITextRepository _textRepository;

    private ITimelineRepository _timelineRepository;

    private IToponymRepository _toponymRepository;

    private ITransactLinksRepository _transactLinksRepository;


    public IFactRepository FactRepository {
        get
        {
            if (_factRepository == null)
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
            if (_imageRepository == null)
            {
                _imageRepository = new ImageRepository(_streetcodeDbContext);
            }
            return _imageRepository;
        }
    }

    public IAudioRepository AudioRepository
    {
        get
        {
            if (_audioRepository == null)
            {
                _audioRepository = new AudioRepository(_streetcodeDbContext);
            }
            return _audioRepository;
        }
    }
    public IVideoRepository VideoRepository
    {
        get
        {
            if (_videoRepository == null)
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
            if (_artRepository == null)
            {
                _artRepository = new ArtRepository(_streetcodeDbContext);
            }
            return _artRepository;
        }
    }

    public IPartnersRepository PartnersRepository
    {
        get
        {
            if (_partnersRepository == null)
            {
                _partnersRepository = new PartnersRepository(_streetcodeDbContext);
            }
            return _partnersRepository;
        }
    }

    public IStreetcodeRepository StreetcodeRepository
    {
        get
        {
            if (_streetcodeRepository == null)
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
            if (_subtitleRepository == null)
            {
                _subtitleRepository = new SubtitleRepository(_streetcodeDbContext);
            }
            return _subtitleRepository;
        }
    }

    public ITagRepository TagRepository
    {
        get
        {
            if (_tagRepository == null)
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
            if (_termRepository == null)
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
            if (_textRepository == null)
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
            if (_timelineRepository == null)
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
            if (_toponymRepository == null)
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
            if (_transactLinksRepository == null)
            {
                _transactLinksRepository = new TransactLinksRepository(_streetcodeDbContext);
            }
            return _transactLinksRepository;
        }
    }


    //private void All_Repoes;

    public void All_Interfaces()
    {
        throw new NotImplementedException();
    }

    public void Save()
    {
        _streetcodeDbContext.SaveChanges();
    }

    public async Task SaveAsync()
    {
        await _streetcodeDbContext.SaveChangesAsync();
    }
}