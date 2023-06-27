using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Analytics;
using Streetcode.BLL.MediatR.Analytics.StatisticRecord.Create;
using Streetcode.BLL.MediatR.Analytics.StatisticRecord.Delete;
using Streetcode.BLL.MediatR.Analytics.StatisticRecord.ExistByQrId;
using Streetcode.BLL.MediatR.Analytics.StatisticRecord.GetAll;
using Streetcode.BLL.MediatR.Analytics.StatisticRecord.GetAllByStreetcodeId;
using Streetcode.BLL.MediatR.Analytics.StatisticRecord.GetByQrId;
using Streetcode.BLL.MediatR.Analytics.StatisticRecord.UpdateCount;

namespace Streetcode.WebApi.Controllers.Analytics
{
    public class StatisticRecordController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return HandleResult(await Mediator.Send(new GetAllStatisticRecordsQuery()));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByQrId(int id)
        {
            return HandleResult(await Mediator.Send(new GetStatisticRecordByQrIdQuery(id)));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> ExistByQrId(int id)
        {
            return HandleResult(await Mediator.Send(new ExistStatisticRecordByQrIdCommand(id)));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAllByStreetcodeId(int id)
        {
            return HandleResult(await Mediator.Send(new GetAllStatisticRecordsByStreetcodeIdQuery(id)));
        }

        [HttpPost]

        public async Task<IActionResult> Create(StatisticRecordDTO statisticRecordDTO)
        {
            return HandleResult(await Mediator.Send(new CreateStatisticRecordCommand(statisticRecordDTO)));
        }

        [HttpPut("{id:int}")]

        public async Task<IActionResult> Update(int id)
        {
            return HandleResult(await Mediator.Send(new UpdateCountStatisticRecordCommand(id)));
        }

        [HttpDelete("{id:int}")]

        public async Task<IActionResult> Delete(int id)
        {
            return HandleResult(await Mediator.Send(new DeleteStatisticRecordCommand(id)));
        }
    }
}
