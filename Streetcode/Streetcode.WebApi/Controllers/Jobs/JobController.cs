using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Jobs;
using Streetcode.BLL.DTO.News;
using Streetcode.BLL.MediatR.Jobs.ChangeStatus;
using Streetcode.BLL.MediatR.Jobs.Create;
using Streetcode.BLL.MediatR.Jobs.Delete;
using Streetcode.BLL.MediatR.Jobs.GetActiveJobs;
using Streetcode.BLL.MediatR.Jobs.GetAll;
using Streetcode.BLL.MediatR.Jobs.GetById;
using Streetcode.BLL.MediatR.Jobs.Update;
using Streetcode.BLL.MediatR.Newss.Create;
using Streetcode.BLL.MediatR.Newss.Delete;
using Streetcode.BLL.MediatR.Newss.Update;

namespace Streetcode.WebApi.Controllers.Jobs
{
	public class JobController : BaseApiController
	{
		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			return HandleResult(await Mediator.Send(new GetAllJobsQuery()));
		}

		[HttpGet]
		public async Task<IActionResult> GetAllShort()
		{
			return HandleResult(await Mediator.Send(new GetAllShortJobsQuery()));
		}

		[HttpGet]
		public async Task<IActionResult> GetActiveJobs()
		{
			return HandleResult(await Mediator.Send(new GetActiveJobsQuery()));
		}

		[HttpGet("{id:int}")]
		public async Task<IActionResult> GetById(int id)
		{
            return HandleResult(await Mediator.Send(new GetJobByIdQuery(id)));
        }

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] JobDto jobDto)
		{
			return HandleResult(await Mediator.Send(new CreateJobCommand(jobDto)));
		}

		[HttpDelete("{id:int}")]
		public async Task<IActionResult> Delete(int id)
		{
			return HandleResult(await Mediator.Send(new DeleteJobCommand(id)));
		}

		[HttpPut]
		public async Task<IActionResult> Update([FromBody]JobDto jobDto)
		{
			return HandleResult(await Mediator.Send(new UpdateJobCommand(jobDto)));
		}

		[HttpPut]
		public async Task<IActionResult> ChangeJobStatus([FromBody] JobChangeStatusDto jobChangeStatusDto)
		{
			return HandleResult(await Mediator.Send(new ChangeJobStatusCommand(jobChangeStatusDto)));
		}
	}
}
