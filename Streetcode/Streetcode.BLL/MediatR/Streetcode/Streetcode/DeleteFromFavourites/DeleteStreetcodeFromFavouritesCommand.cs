using FluentResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.DeleteFromFavourites
{
    public record DeleteStreetcodeFromFavouritesCommand(int streetcodeId, string userId)
    : IRequest<Result<Unit>>;
}
