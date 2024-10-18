using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Analytics;
using Streetcode.BLL.MediatR.Analytics.StatisticRecord.Create;
using Streetcode.BLL.MediatR.Analytics.StatisticRecord.Delete;
using Streetcode.BLL.MediatR.Analytics.StatisticRecord.ExistByQrId;
using Streetcode.BLL.MediatR.Analytics.StatisticRecord.GetAll;
using Streetcode.BLL.MediatR.Analytics.StatisticRecord.GetAllByStreetcodeId;
using Streetcode.BLL.MediatR.Analytics.StatisticRecord.GetByQrId;
using Streetcode.BLL.MediatR.Analytics.StatisticRecord.UpdateCount;
using Streetcode.DAL.Enums;

namespace Streetcode.WebApi.Controllers.Analytics
{
    public class StatisticRecordController : BaseApiController
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<StatisticRecordDTO>))]
        public async Task<IActionResult> GetAll()
        {
            return HandleResult(await Mediator.Send(new GetAllStatisticRecordsQuery()));
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StatisticRecordDTO))]
        public async Task<IActionResult> GetByQrId(int id)
        {
            return HandleResult(await Mediator.Send(new GetStatisticRecordByQrIdQuery(id)));
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        public async Task<IActionResult> ExistByQrId(int id)
        {
            return HandleResult(await Mediator.Send(new ExistStatisticRecordByQrIdCommand(id)));
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<StatisticRecordDTO>))]
        public async Task<IActionResult> GetAllByStreetcodeId(int id)
        {
            return HandleResult(await Mediator.Send(new GetAllStatisticRecordsByStreetcodeIdQuery(id)));
        }

        [HttpPost]
        [Authorize(Roles = nameof(UserRole.Admin))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StatisticRecordResponseDTO))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]

        public async Task<IActionResult> Create(StatisticRecordDTO statisticRecordDTO)
        {
            return HandleResult(await Mediator.Send(new CreateStatisticRecordCommand(statisticRecordDTO)));
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]

        public async Task<IActionResult> Update(int id)
        {
            return HandleResult(await Mediator.Send(new UpdateCountStatisticRecordCommand(id)));
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]

        public async Task<IActionResult> Delete(int id)
        {
            return HandleResult(await Mediator.Send(new DeleteStatisticRecordCommand(id)));
        }
    }
}
