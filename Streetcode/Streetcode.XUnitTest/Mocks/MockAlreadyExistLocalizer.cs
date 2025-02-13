﻿using Streetcode.BLL.SharedResource;

namespace Streetcode.XUnitTest.Mocks;

public class MockAlreadyExistLocalizer : BaseMockStringLocalizer<AlreadyExistSharedResource>
{
    protected override Dictionary<int, List<string>> DefineGroupedErrors()
    {
        return new Dictionary<int, List<string>>
        {
            {
                2, new List<string>
                {
                    "PartnerWithFieldAlreadyExist",
                }
            },
        };
    }
}