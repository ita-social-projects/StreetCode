
using EFTask.Persistence;
using Repositories.Interfaces;
using StreetCode.DAL.Repositories.Interfaces.Base;


namespace Repositories.Realizations;

public class RepositoryWrapper : IRepositoryWrapper 
{

    public RepositoryWrapper(StreetcodeDbContext streetcodeDbContext) 
    {
        _streetcodeDbContext = streetcodeDbContext;
    }

    private StreetcodeDbContext _streetcodeDbContext;

    private IFactRepository _factRepository;

    private IMediaRepository _mediaRepository;

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

    public IMediaRepository MediaRepository
    {
        get
        {
            if (_mediaRepository == null)
            {
                _mediaRepository = new MediaRepository(_streetcodeDbContext);
            }
            return _mediaRepository;
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