using FluentValidation;
using WordProcessor.DataTypes.Algorithms;
using WordProcessor.Enums;
using WordProcessor.ViewModel;

namespace WordProcessor.Validator
{
  public sealed class MainWindowViewModelValidator : AbstractValidator<MainWindowViewModel>
  {
    public MainWindowViewModelValidator()
    {
      RuleFor(vm => vm.ProcessText)
        .NotEmpty()
        .WithMessage("Необходимы данные для обработки");
      
      RuleFor(vm => vm.AlgorithmData)
        .NotEmpty()
        .When(s => s.AlgorithmType.IsRequiredAlgorithmData)
        .WithMessage(GetErrorAlgorithmMessage);

      RuleFor(vw => vw.CustomSeparator)
        .NotEmpty()
        .When(vw => vw.EnableSeparatorData)
        .WithMessage(GetErrorSeparatorMessage);

      RuleFor(vw => vw.SavePath)
        .NotEmpty()
        .WithMessage("Необходим путь для сохранения файла");
    }

    private string GetErrorAlgorithmMessage(MainWindowViewModel vm)
    {
      if (vm.AlgorithmType == Algorithm.ShuffleTranslate)
        return "Укажите разделитель между словом и переводом(к прим. '-')";

      if (vm.AlgorithmType == Algorithm.ReplaceLetter)
        return "Укажите количество букв для замены(к прим. '1')";

      return string.Empty;
    }

    private string GetErrorSeparatorMessage(MainWindowViewModel vm)
    {
      if (vm.SeparatorType == Separator.Custom)
        return "Указажите разделитель(к прим. ',')";

      return string.Empty;
    }
  }
}
