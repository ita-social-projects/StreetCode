namespace Streetcode.DAL.Enums;

public enum StreetcodeArtSlideTemplate
{
    OneToTwo = 0,
    OneToTwoAndThreeToFour = 1,
    OneToFour = 2,
    OneToFourAndFiveToSix = 3,
    OneToTwoAndThreeToFourAndFiveToSix = 4,
    OneToFourAndFiveAndSix = 5,
    OneToTwoAndThreeToFourAndFive = 6,
    OneAndTwoAndThreeToFour = 7,
    OneAndTwoAndThreeAndFour = 8,
    OneAndTwoAndThreeToFourAndFive = 9,
    OneAndTwoAndThreeToFourAndFiveToSix = 10,
    OneAndTwoAndThreeAndFourAndFive = 11,
    OneAndTwoAndThreeToFourAndFiveAndSix = 12,
    OneAndTwoAndThreeAndFourAndFiveAndSix = 13,
}

public static class StreetcodeArtSlideTemplateConsts
{
    public static Dictionary<StreetcodeArtSlideTemplate, int> CountOfArtsInTemplateDictionary = new()
        {
            { StreetcodeArtSlideTemplate.OneToFour, 1 },
            { StreetcodeArtSlideTemplate.OneToTwo, 1 },
            { StreetcodeArtSlideTemplate.OneToFourAndFiveToSix, 2 },
            { StreetcodeArtSlideTemplate.OneToTwoAndThreeToFour, 2 },
            { StreetcodeArtSlideTemplate.OneToTwoAndThreeToFourAndFiveToSix, 3 },
            { StreetcodeArtSlideTemplate.OneToFourAndFiveAndSix, 3 },
            { StreetcodeArtSlideTemplate.OneAndTwoAndThreeToFour, 3 },
            { StreetcodeArtSlideTemplate.OneToTwoAndThreeToFourAndFive, 3 },
            { StreetcodeArtSlideTemplate.OneAndTwoAndThreeAndFour, 4 },
            { StreetcodeArtSlideTemplate.OneAndTwoAndThreeToFourAndFiveToSix, 4 },
            { StreetcodeArtSlideTemplate.OneAndTwoAndThreeToFourAndFive, 4 },
            { StreetcodeArtSlideTemplate.OneAndTwoAndThreeToFourAndFiveAndSix, 5 },
            { StreetcodeArtSlideTemplate.OneAndTwoAndThreeAndFourAndFive, 5 },
            { StreetcodeArtSlideTemplate.OneAndTwoAndThreeAndFourAndFiveAndSix, 6 },
        };
}