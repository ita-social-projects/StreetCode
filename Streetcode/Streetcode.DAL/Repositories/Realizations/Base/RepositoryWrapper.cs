
using Repositories.Interfaces;
using StreetCode.DAL.Repositories.Interfaces.Base;


namespace Repositories.Realizations
{
    public class RepositoryWrapper : IRepositoryWrapper 
    {

        public RepositoryWrapper(StreetcodeDBContext streetcodeDBContext) 
        {
            _streetcodeDBContext = streetcodeDBContext;
        }

        private StreetcodeDBContext _streetcodeDBContext;

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
                    _factRepository = new FactRepository(_streetcodeDBContext);
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
                    _mediaRepository = new MediaRepository(_streetcodeDBContext);
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
                    _partnersRepository = new PartnersRepository(_streetcodeDBContext);
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
                    _streetcodeRepository = new StreetcodeRepository(_streetcodeDBContext);
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
                    _subtitleRepository = new SubtitleRepository(_streetcodeDBContext);
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
                    _tagRepository = new TagRepository(_streetcodeDBContext);
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
                    _termRepository = new TermRepository(_streetcodeDBContext);
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
                    _textRepository = new TextRepository(_streetcodeDBContext);
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
                    _timelineRepository = new TimelineRepository(_streetcodeDBContext);
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
                    _toponymRepository = new ToponymRepository(_streetcodeDBContext);
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
                    _transactLinksRepository = new TransactLinksRepository(_streetcodeDBContext);
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
            _streetcodeDBContext.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _streetcodeDBContext.SaveChangesAsync();
        }
    }
}