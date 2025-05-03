using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Users.Expertise;

namespace Streetcode.BLL.MediatR.Expertises.GetAll;

public record GetAllExpertisesQuery : IRequest<Result<List<ExpertiseDTO>>>;