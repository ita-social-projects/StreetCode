using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.MediatR.AdditionalContent.Tag.GetAll;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.AdditionalContent.Tag.Test
{
    public record TestQuery : IRequest<Result<int>>;
    public class TestHandler : IRequestHandler<TestQuery, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;

        public TestHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        //        CREATE TABLE Persons(
        //    PersonID int,
        //    LastName varchar(255),
        //    FirstName varchar(255),
        //    Address varchar(255),
        //    City varchar(255)
        //);

        public async Task<Result<int>> Handle(TestQuery request, CancellationToken cancellationToken)
        {
            _repositoryWrapper.ArtRepository
                .ExecuteSQL("Insert into Persons VALUES (1,'last','first','adress','city') ");
            await Method1();
            return Result.Ok(0);
        }

        public static async Task<int> Method1()
        {
            int count = 0;
            await Task.Run(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    Console.WriteLine(" Method 1");
                    count += 1;
                }
            });
            return count;
        }
    }
}
