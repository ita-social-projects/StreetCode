﻿using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.MediatR.Team.GetNamePosition;

namespace Streetcode.WebApi.Controllers.Team
{
    public class TickerStringController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetNameTickerString()
        {
            return HandleResult(await Mediator.Send(new GetTickerStringQuery()));
        }
    }
}
