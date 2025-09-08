using FluentValidation;
using EmailPush.Application.DTOs;

namespace EmailPush.Application.Validators;

public class CreateCampaignValidator : AbstractValidator<CreateCampaignDto>
{
    public CreateCampaignValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Campaign name is required")
            .MaximumLength(200).WithMessage("Campaign name can be maximum 200 characters");

        RuleFor(x => x.Subject)
            .NotEmpty().WithMessage("Email subject is required")
            .MaximumLength(500).WithMessage("Email subject can be maximum 500 characters");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Email content is required");

        RuleFor(x => x.Recipients)
            .NotEmpty().WithMessage("At least one recipient is required")
            .Must(recipients => recipients.Count > 0).WithMessage("At least one recipient is required")
            .Must(recipients => recipients.Count <= 1000).WithMessage("Maximum 1000 recipients allowed");

        RuleForEach(x => x.Recipients)
            .EmailAddress().WithMessage("Invalid email address format");
    }
}

public class CreateCampaignCommandValidator : AbstractValidator<Commands.CreateCampaignCommand>
{
    public CreateCampaignCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Campaign name is required")
            .MaximumLength(200).WithMessage("Campaign name can be maximum 200 characters");

        RuleFor(x => x.Subject)
            .NotEmpty().WithMessage("Email subject is required")
            .MaximumLength(500).WithMessage("Email subject can be maximum 500 characters");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Email content is required");

        RuleFor(x => x.Recipients)
            .NotEmpty().WithMessage("At least one recipient is required")
            .Must(recipients => recipients.Count > 0).WithMessage("At least one recipient is required")
            .Must(recipients => recipients.Count <= 1000).WithMessage("Maximum 1000 recipients allowed");

        RuleForEach(x => x.Recipients)
            .EmailAddress().WithMessage("Invalid email address format");
    }
}