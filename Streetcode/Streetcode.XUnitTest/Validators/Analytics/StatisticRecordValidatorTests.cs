using FluentValidation.TestHelper;
using Streetcode.BLL.DTO.AdditionalContent.Coordinates.Types;
using Streetcode.BLL.DTO.Analytics;
using Streetcode.BLL.MediatR.Analytics.StatisticRecord.Create;
using Streetcode.BLL.Validators.Analytics;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.Validators.Analytics;

public class StatisticRecordValidatorTests
{
    private readonly MockFieldNamesLocalizer mockNamesLocalizer;
    private readonly MockFailedToValidateLocalizer mockValidationLocalizer;
    private readonly StatisticRecordValidator validator;

    public StatisticRecordValidatorTests()
    {
        mockNamesLocalizer = new MockFieldNamesLocalizer();
        mockValidationLocalizer = new MockFailedToValidateLocalizer();
        validator = new StatisticRecordValidator(mockValidationLocalizer, mockNamesLocalizer);
    }

    [Fact]
    public void ShouldReturnError_WhenAddressIsEmpty()
    {
        var expectedError = mockValidationLocalizer["CannotBeEmpty", mockNamesLocalizer["Address"]];
        var statisticRecordTest = GetStatisticRecordDTO();
        statisticRecordTest.Address = string.Empty;
        var command = new CreateStatisticRecordCommand(statisticRecordTest);

        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.StatisticRecordDTO.Address)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenAddressLengthIsMoreThanMaximum()
    {
        var expectedError = mockValidationLocalizer["MaxLength", mockNamesLocalizer["Address"], StatisticRecordValidator.AddressMaxLength];
        var statisticRecordTest = GetStatisticRecordDTO();
        statisticRecordTest.Address = new string('a', StatisticRecordValidator.AddressMaxLength + 1);
        var command = new CreateStatisticRecordCommand(statisticRecordTest);

        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.StatisticRecordDTO.Address)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenLatitudeIsEmpty()
    {
        var expectedError = mockValidationLocalizer["IsRequired", mockNamesLocalizer["Latitude"]];
        var statisticRecordTest = GetStatisticRecordDTO();
        statisticRecordTest.StreetcodeCoordinate.Latitude = default;
        var command = new CreateStatisticRecordCommand(statisticRecordTest);

        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.StatisticRecordDTO.StreetcodeCoordinate.Latitude)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenLatitudePrecisionScaleIsInvalid()
    {
        var expectedError = mockValidationLocalizer["InvalidPrecision", mockNamesLocalizer["Latitude"]];
        var statisticRecordTest = GetStatisticRecordDTO();
        statisticRecordTest.StreetcodeCoordinate.Latitude = 0.11111m;
        var command = new CreateStatisticRecordCommand(statisticRecordTest);

        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.StatisticRecordDTO.StreetcodeCoordinate.Latitude)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenLongtitudeIsEmpty()
    {
        var expectedError = mockValidationLocalizer["IsRequired", mockNamesLocalizer["Longtitude"]];
        var statisticRecordTest = GetStatisticRecordDTO();
        statisticRecordTest.StreetcodeCoordinate.Longtitude = default;
        var command = new CreateStatisticRecordCommand(statisticRecordTest);

        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.StatisticRecordDTO.StreetcodeCoordinate.Longtitude)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenLongtitudePrecisionScaleIsInvalid()
    {
        var expectedError = mockValidationLocalizer["InvalidPrecision", mockNamesLocalizer["Longtitude"]];
        var statisticRecordTest = GetStatisticRecordDTO();
        statisticRecordTest.StreetcodeCoordinate.Longtitude = 0.11111m;
        var command = new CreateStatisticRecordCommand(statisticRecordTest);

        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.StatisticRecordDTO.StreetcodeCoordinate.Longtitude)
            .WithErrorMessage(expectedError);
    }

    private StatisticRecordDTO GetStatisticRecordDTO()
    {
        return new StatisticRecordDTO
        {
            StreetcodeCoordinate = new StreetcodeCoordinateDTO
            {
                Id = 1,
                Latitude = 50,
                Longtitude = 50,
                StreetcodeId = 1,
            },
            QrId = 1,
            Count = 0,
            Address = "test_address",
        };
    }
}