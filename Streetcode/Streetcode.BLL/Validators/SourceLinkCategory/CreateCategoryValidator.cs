﻿using FluentValidation;
using FluentValidation.Results;
using Streetcode.BLL.DTO.Sources;
using Streetcode.BLL.MediatR.Sources.SourceLink.Create;

namespace Streetcode.BLL.Validators.SourceLinkCategory;

public class CreateCategoryValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryValidator(BaseCategoryValidator baseCategoryValidator)
    {
        RuleFor(c => c.Category).SetValidator(baseCategoryValidator);
    }
}