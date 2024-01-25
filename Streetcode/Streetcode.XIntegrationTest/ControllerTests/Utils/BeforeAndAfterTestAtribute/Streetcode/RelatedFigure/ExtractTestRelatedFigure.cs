using Streetcode.DAL.Entities.Streetcode;
using System.Reflection;
using Xunit.Sdk;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Streetcode.RelatedFigure
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    internal class ExtractTestRelatedFigure : BeforeAfterTestAttribute
    {
        public static DAL.Entities.Streetcode.RelatedFigure RelatedFigureForTest;

        public override void Before(MethodInfo methodUnderTest)
        {
            var sqlDbHelper = BaseControllerTests.GetSqlDbHelper();
            RelatedFigureForTest = sqlDbHelper.GetExistItem<DAL.Entities.Streetcode.RelatedFigure>();

            if (RelatedFigureForTest == null)
            {
                var streetcodes = sqlDbHelper.GetAll<StreetcodeContent>();
                StreetcodeContent firstStreetcode, secondStreetcode;
                if (streetcodes.Count() < 2)
                {
                    firstStreetcode = sqlDbHelper.AddNewItem(new StreetcodeContent()
                    {
                        Index = 100,
                        Teaser = "Teaser",
                        UpdatedAt = DateTime.Now,
                        CreatedAt = DateTime.Now,
                        ViewCount = 1,
                        EventEndOrPersonDeathDate = DateTime.Now,
                        EventStartOrPersonBirthDate = DateTime.Now,
                    });
                    secondStreetcode = sqlDbHelper.AddNewItem(new StreetcodeContent()
                    {
                        Index = 101,
                        Teaser = "Teaser",
                        UpdatedAt = DateTime.Now,
                        CreatedAt = DateTime.Now,
                        ViewCount = 1,
                        EventEndOrPersonDeathDate = new DateTime(2002, 11, 20),
                        EventStartOrPersonBirthDate = new DateTime(2001, 11, 20),
                    });
                }
                else
                {
                    firstStreetcode = streetcodes.ElementAt(0);
                    secondStreetcode = streetcodes.ElementAt(1);
                }

                sqlDbHelper.SaveChanges();

                RelatedFigureForTest = sqlDbHelper.AddNewItem(new DAL.Entities.Streetcode.RelatedFigure()
                {
                    ObserverId = firstStreetcode.Id,
                    TargetId = secondStreetcode.Id,
                });
                sqlDbHelper.SaveChanges();
            }
        }
    }
}
