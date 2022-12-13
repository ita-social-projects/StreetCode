using Repositories.Interfaces;
using System.Globalization;
using Streetcode.DAL.Persistence;

using Streetcode.DAL.Entities.Media.Images;

namespace Repositories.Realizations;

public class AudioRepository : RepositoryBase<Art>, IAudioRepository
{

    public AudioRepository(StreetcodeDbContext _streetcodeDBContext)
    {
    }
    public void GetAudioAsync()
    {
        // TODO implement here
    }

    public void UploadAudioAsync()
    {
        // TODO implement here
    }

    public void DeleteAudioAsync()
    {
        // TODO implement here
    }

}