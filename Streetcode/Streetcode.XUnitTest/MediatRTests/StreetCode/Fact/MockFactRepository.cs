using Moq;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace Streetcode.XUnitTest.MediatRTest.StreetCode.Mocks;

public static class MockFactRepository
{
    public static Mock<IRepositoryWrapper> GetRepositoryWrapper()
    {
        var facts = new List<Streetcode.DAL.Entities.Streetcode.TextContent.Fact>
        {
            new Streetcode.DAL.Entities.Streetcode.TextContent.Fact
            {
                Id = 1,
                Title = "Викуп з кріпацтва",
                ImageId = null,
                FactContent = "Навесні 1838-го Карл Брюллов..."
            },

            new Streetcode.DAL.Entities.Streetcode.TextContent.Fact
            {
                Id = 2,
                Title = "Перший Кобзар",
                ImageId = 5,
                FactContent = "Ознайомившись випадково з рукописними творами"
            }
        };

        var mockRepo = new Mock<IRepositoryWrapper>();

        mockRepo.Setup(x => x.FactRepository
              .GetAllAsync(
                  It.IsAny<Expression<Func<DAL.Entities.Streetcode.TextContent.Fact, bool>>>(),
                    It.IsAny<Func<IQueryable<DAL.Entities.Streetcode.TextContent.Fact>,
              IIncludableQueryable<DAL.Entities.Streetcode.TextContent.Fact, object>>>()))
              .ReturnsAsync(facts);

        return mockRepo;
    }
}