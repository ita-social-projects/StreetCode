
namespace Services.Interfaces
{
    public interface IMediaService 
    {
        public string GetPictureAsync();
        public void UploadPictureAsync();
        public void DeletePictureAsync();
        public void GetVideoAsync();
        public void UploadVideoAsync();
        public void DeleteVideoAsync();
        public void GetAudioAsync();
        public void UploadAudioAsync();
        public void DeleteAudioAsync();
       
    }
}