using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.MediatR.Streetcode.Text.Delete;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;

namespace Streetcode.XUnitTest.StreetcodeTest.TextTest
{
    public class DeleteTextTest
    {
        [Theory]
        [InlineData(1)]
        public async Task DeleteTextSuccessfull(int id)
        {
            var repository = new Mock<IRepositoryWrapper>();
            var mockMapper = new Mock<IMapper>();

            repository.Setup(x => x.TextRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Text, bool>>>(), 
                It.IsAny<Func<IQueryable<Text>,IIncludableQueryable<Text, object>>>()))
                .ReturnsAsync(new Text { Id= id });

            repository.Setup(x => x.TextRepository.Delete(new Text()
            { Id = id }));

            var handler = new DeleteTextHandler(repository.Object);

            var result = await handler.Handle(new DeleteTextCommand(id), CancellationToken.None);

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);

        }
       
   }
}
