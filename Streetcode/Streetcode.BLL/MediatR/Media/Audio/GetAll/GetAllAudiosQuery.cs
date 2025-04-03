using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media.Audio;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Media.Audio.GetAll;

public record GetAllAudiosQuery(UserRole? UserRole) : IRequest<Result<IEnumerable<AudioDTO>>>;