﻿using Streetcode.BLL.SharedResource;

namespace Streetcode.XUnitTest.Mocks;

public class MockNoSharedResourceLocalizer : BaseMockStringLocalizer<NoSharedResource>
{
    protected override Dictionary<int, List<string>> DefineGroupedErrors()
    {
        return new Dictionary<int, List<string>>
        {
            {
                0, new List<string>
                {
                    "NoFavouritesFound",
                    "NoStreetcodesExistNow",
                }
            },
            {
                1, new List<string>
                {
                    "NoExistingStreetcodeWithId",
                    "NoFavouritesFound",
                    "NoFavouritesWithId",
                }
            },
        };
    }
}