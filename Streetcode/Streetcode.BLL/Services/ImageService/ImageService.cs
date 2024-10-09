using Repositories.Interfaces;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Image;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.DAL.Repositories.Interfaces.Media.Images;
using Streetcode.DAL.Repositories.Interfaces.Newss;
using Streetcode.DAL.Repositories.Interfaces.Partners;
using Streetcode.DAL.Repositories.Interfaces.Source;
using Streetcode.DAL.Repositories.Interfaces.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Team;

namespace Streetcode.BLL.Services.ImageService;

public class ImageService : IImageService
{
    private readonly ILoggerService _loggerService;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IBlobService _blobService;

    private readonly IImageRepository _imageRepository;
    private readonly IArtRepository _artRepository;
    private readonly IPartnersRepository _partnersRepository;
    private readonly INewsRepository _newsRepository;
    private readonly ISourceCategoryRepository _sourceCategoryRepository;
    private readonly IFactRepository _factRepository;
    private readonly IStreetcodeImageRepository _streetcodeImageRepository;
    private readonly ITeamRepository _teamRepository;

    public ImageService(ILoggerService loggerService, IRepositoryWrapper repositoryWrapper, IBlobService blobService)
    {
        _loggerService = loggerService;
        _repositoryWrapper = repositoryWrapper;
        _blobService = blobService;

        _imageRepository = repositoryWrapper.ImageRepository;
        _artRepository = repositoryWrapper.ArtRepository;
        _partnersRepository = repositoryWrapper.PartnersRepository;
        _newsRepository = repositoryWrapper.NewsRepository;
        _sourceCategoryRepository = repositoryWrapper.SourceCategoryRepository;
        _factRepository = repositoryWrapper.FactRepository;
        _streetcodeImageRepository = repositoryWrapper.StreetcodeImageRepository;
        _teamRepository = repositoryWrapper.TeamRepository;
    }

    public async Task CleanUnusedImagesAsync()
    {
        var imagesFromArts = _artRepository
            .FindAll()
            .Select(art => art.ImageId)
            .ToList();

        var imagesFromNews = _newsRepository
            .FindAll()
            .Where(news => news.ImageId.HasValue)
            .Select(news => news.ImageId!.Value)
            .ToList();

        var imagesFromSourceCategory = _sourceCategoryRepository
            .FindAll()
            .Select(sourceCategory => sourceCategory.ImageId)
            .ToList();

        var imagesFromFacts = _factRepository
            .FindAll()
            .Where(fact => fact.ImageId.HasValue)
            .Select(fact => fact.ImageId!.Value)
            .ToList();

        var imageFormStreetcodeImage = _streetcodeImageRepository
            .FindAll()
            .Select(streetcodeImage => streetcodeImage.ImageId)
            .ToList();

        var imagesFromTeam = _teamRepository
            .FindAll()
            .Select(team => team.ImageId)
            .ToList();

        var imagesFromPartners = _partnersRepository
            .FindAll()
            .Select(partner => partner.LogoId)
            .ToList();

        var referencedImageIds = imagesFromArts
            .Concat(imagesFromNews)
            .Concat(imagesFromSourceCategory)
            .Concat(imagesFromFacts)
            .Concat(imageFormStreetcodeImage)
            .Concat(imagesFromTeam)
            .Concat(imagesFromPartners);

        var unreferencedImages = _imageRepository
            .FindAll(image => !referencedImageIds.Contains(image.Id))
            .ToList();

        _loggerService.LogInformation("Starting deleting these images:");

        foreach (var image in unreferencedImages)
        {
            _loggerService.LogInformation(image.BlobName!);
        }

        if (unreferencedImages.Any())
        {
            foreach (var image in unreferencedImages)
            {
                _blobService.DeleteFileInStorage(image.BlobName);
            }

            _imageRepository.DeleteRange(unreferencedImages);
            await _repositoryWrapper.SaveChangesAsync();
        }

        _loggerService.LogInformation("Deleting completed");
    }
}