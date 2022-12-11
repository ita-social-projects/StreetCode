
using Repositories.Realizations;
using Services.Interfaces;
using StreetCode.DAL.Repositories.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Services;

public class MediaService : IMediaService 
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    public MediaService(IRepositoryWrapper repositoryWrapper) 
    {
        _repositoryWrapper = repositoryWrapper;
    }

    public string GetPictureAsync() 
    {
        // return "GetPictureAsync";
        return _repositoryWrapper.MediaRepository.GetPictureAsync();
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