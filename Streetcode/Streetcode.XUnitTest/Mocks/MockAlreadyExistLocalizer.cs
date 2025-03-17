﻿using Streetcode.BLL.SharedResource;

namespace Streetcode.XUnitTest.Mocks;

public class MockAlreadyExistLocalizer : BaseMockStringLocalizer<AlreadyExistSharedResource>
{
    protected override Dictionary<int, List<string>> DefineGroupedErrors()
    {
        var groupedErrors = new Dictionary<int, List<string>>()
        {
            {
                0, new List<string>()
                {
                    "FavouriteAlreadyExists",
                }
            },
            {
                2, new List<string>()
                {
                    "PartnerWithFieldAlreadyExist",
                }
            },
        };
        return groupedErrors;
    }
}