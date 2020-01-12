using FluentValidation;
using WordProcessor.DataTypes;
using WordProcessor.DataTypes.Algorithms;
using WordProcessor.ViewModel;

namespace WordProcessor.Validator
{
  public sealed class MainWindowViewModelValidator : AbstractValidator<MainWindowViewModel>
  {
    public MainWindowViewModelValidator()
    {
      RuleFor(vm => vm.ProcessText)
        .NotEmpty()
        .WithMessage(v => LocalizationManager.GetLocalizationString("m_valid_ProcessText"));
      
      RuleFor(vm => vm.AlgorithmData)
        .NotEmpty()
        .When(s => s.AlgorithmType.IsRequiredAlgorithmData)
        .WithMessage(vm => vm.AlgorithmType.ErrorMessageForEmptyInput);

      RuleFor(vm => vm.CustomSeparator)
        .NotEmpty()
        .When(vm => vm.EnableSeparatorData)
        .WithMessage(_ => LocalizationManager.GetLocalizationString("m_valid_CustomSeparator"));

      RuleFor(vm => vm.SavePath)
        .NotEmpty()
        .WithMessage(_ => LocalizationManager.GetLocalizationString("m_valid_SavePath"));
    }
  }
}
