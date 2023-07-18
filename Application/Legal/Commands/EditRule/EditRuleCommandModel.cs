﻿using System.ComponentModel.DataAnnotations;
using Poplike.Common.Validation;

namespace Poplike.Application.Legal.Commands.EditRule;

public class EditRuleCommandModel
{
    public int Id { get; set; }

    [Range(1,100)]
    [Required(ErrorMessage = "Ge regeln ett nummer")]
    public int? Number { get; set; }

    [RegularExpression(Pattern.Common.SomeContent)]
    [StringLength(
        MaxLengths.Domain.Rule.Heading,
        ErrorMessage = "Försök att skriva kortare.")]
    public string Heading { get; set; }

    [RegularExpression(Pattern.Common.SomeContent)]
    [StringLength(
        MaxLengths.Domain.Rule.Text,
        ErrorMessage = "Försök att skriva kortare.")]
    public string Text { get; set; }
}
