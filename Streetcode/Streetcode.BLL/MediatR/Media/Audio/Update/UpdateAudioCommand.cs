﻿using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media.Audio;

namespace Streetcode.BLL.MediatR.Media.Audio.Update;

public record UpdateAudioCommand(AudioFileBaseUpdateDTO Audio) : IRequest<Result<AudioDTO>>;
