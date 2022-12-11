
using Repositories.Interfaces;

namespace Repositories.Realizations
{
    public class MediaRepository : RepositoryBase , IMediaRepository 
    {

        public MediaRepository(StreetcodeDBContext _streetcodeDBContext) 
        {
        }

        public void GetPictureAsync() 
        {
            // TODO implement here
        }

        public void UploadPictureAsync() 
        {
            // TODO implement here
        }

        public void DeletePictureAsync()
        {
            // TODO implement here
        }

        public void GetVideoAsync() 
        {
            // TODO implement here
        }

        public void UploadVideoAsync() 
        {
            // TODO implement here
        }

        public void DeleteVideoAsync() 
        {
            // TODO implement here
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
}