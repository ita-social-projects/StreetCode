using AutoMapper;
using FluentResults;
using MediatR;
using Org.BouncyCastle.Asn1.Ocsp;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.DTO.AdditionalContent.Tag;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.DTO.Streetcode.Create;
using Streetcode.BLL.DTO.Streetcode.Create.TextContent;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.Factories.Streetcode;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.Partners;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Entities.Timeline;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.DAL.Repositories.Realizations.Base;
using static System.Formats.Asn1.AsnWriter;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.Create;

public class CreateStreetcodeHandler : IRequestHandler<CreateStreetcodeCommand, Result<Unit>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public CreateStreetcodeHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
    }

    public async Task<Result<Unit>> Handle(CreateStreetcodeCommand request, CancellationToken cancellationToken)
    {
        using(var transactionScope = _repositoryWrapper.BeginTransaction())
        {
            try
            {
                var streetcode = StreetcodeFactory.CreateStreetcode(request.Streetcode.StreetcodeType);
                _mapper.Map(request.Streetcode, streetcode);

                _repositoryWrapper.StreetcodeRepository.Create(streetcode);
                var isResultSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;

                await AddImages(streetcode, request.Streetcode.ImagesId);
                await AddTagsToStreetcode(streetcode, request.Streetcode.Tags.ToList());
                await AddPartnersToStreetcode(streetcode, request.Streetcode.Partners);
                await CreateRelatedFigures(streetcode, request.Streetcode.RelatedFigures);
                await AddToponyms(streetcode, request.Streetcode.Toponyms);

                var timelineItems = request.Streetcode.TimelineItems;
                var newTimelineItems = _mapper.Map<List<TimelineItem>>(timelineItems);
                streetcode.TimelineItems.AddRange(newTimelineItems);

                await _repositoryWrapper.SaveChangesAsync();

                if (isResultSuccess)
                {
                    transactionScope.Complete();
                    return Result.Ok(Unit.Value);
                }
                else
                {
                    return Result.Fail(new Error("Failed to create a streetcode"));
                }
            }
            catch(Exception ex)
            {
                return Result.Fail(new Error("An error occurred while creating a streetcode"));
            }
        }
    }

    private async Task AddTagsToStreetcode(StreetcodeContent streetcode, List<StreetcodeTagDTO> tags)
    {
        var indexedTags = new List<StreetcodeTagIndex>();

        for (int i = 0; i < tags.Count; i++)
        {
            var newTagIndex = new StreetcodeTagIndex
            {
                StreetcodeId = streetcode.Id,
                TagId = tags[i].Id,
                IsVisible = tags[i].IsVisible,
                Index = i,
            };

            if (tags[i].Id <= 0)
            {
                var newTag = _mapper.Map<Tag>(tags[i]);
                newTag.Id = 0;
                newTagIndex.Tag = newTag;
            }

            indexedTags.Add(newTagIndex);
        }

        await _repositoryWrapper.StreetcodeTagIndexRepository.CreateRangeAsync(indexedTags);
    }

    private async Task AddPartnersToStreetcode(StreetcodeContent streetcode, IEnumerable<PartnerShortDTO> partners)
    {
        foreach (var partner in partners)
        {
            streetcode.Partners.Add(await _repositoryWrapper.PartnersRepository.GetFirstOrDefaultAsync(x => x.Id == partner.Id));
        }
    }

    private async Task CreateRelatedFigures(StreetcodeContent streetcode, IEnumerable<StreetcodeDTO> relatedFigures)
    {
        var relatedFiguresToCreate = relatedFigures
            .Select(relatedFigure => new DAL.Entities.Streetcode.RelatedFigure
            {
                ObserverId = streetcode.Id,
                TargetId = relatedFigure.Id,
            })
            .ToList();

        await _repositoryWrapper.RelatedFigureRepository.CreateRangeAsync(relatedFiguresToCreate);
    }

    private async Task AddToponyms(StreetcodeContent streetcode, IEnumerable<string> toponymsName)
    {
        streetcode.Toponyms.AddRange(await _repositoryWrapper.ToponymRepository.GetAllAsync(x => toponymsName.Contains(x.StreetName)));
    }

    private async Task AddImages(StreetcodeContent streetcode, IEnumerable<int> imagesId)
    {
        foreach(int id in imagesId)
        {
            streetcode.Images.Add(await _repositoryWrapper.ImageRepository.GetFirstOrDefaultAsync(x => x.Id == id));
        }
    }
}
