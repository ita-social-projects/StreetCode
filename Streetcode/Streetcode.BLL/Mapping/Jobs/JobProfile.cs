using AutoMapper;
using Streetcode.BLL.DTO.Jobs;
using Streetcode.DAL.Entities.Jobs;

namespace Streetcode.BLL.Mapping.Jobs
{
	public class JobProfile : Profile
	{
		public JobProfile()
		{
			CreateMap<Job, JobDto>().ReverseMap();
			CreateMap<JobCreateDto, Job>();
			CreateMap<JobUpdateDto, Job>();
            CreateMap<Job, JobShortDto>();
		}
	}
}
