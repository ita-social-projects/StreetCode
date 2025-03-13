using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentResults;
using MediatR;

namespace Streetcode.BLL.MediatR.Event.Delete
{
    public record DeleteEventCommand(int id)
    : IRequest<Result<int>>;
}
