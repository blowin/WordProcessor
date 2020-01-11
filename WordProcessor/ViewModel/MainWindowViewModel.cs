using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Windows.Forms;
using DynamicData.Binding;
using ReactiveUI;
using WordProcessor.DataTypes.Algorithms;
using WordProcessor.Enums;
using WordProcessor.Util;
using WordProcessor.Validator;

namespace WordProcessor.ViewModel
{
  public sealed class MainWindowViewModel : ReactiveObject, IDataErrorInfo
  {
    private MainWindowViewModelValidator _validator;

    private bool _needAskRewriteFile;

    private Algorithm _algorithmType;
    private string _algorithmData;

    private Separator _separatorType;
    private string _customSeparator;

    private string _savePath;
    private string _processText;

    private bool _enableSeparatorData;
    private bool _enableAlgorithmData;
    
    private bool _openFileAfterProcess;

    public string Error
    {
      get
      {
        var result = _validator.Validate(this);
        if (result == null || !result.Errors.Any())
          return string.Empty;

        return string.Join(", ", result.Errors.Select(s => s.ErrorMessage));
      }
    }

    public string this[string columnName]
    {
      get
      {
        foreach (var validationFailure in _validator.Validate(this).Errors)
        {
          if (validationFailure.PropertyName == columnName)
            return validationFailure.ErrorMessage ?? string.Empty;
        }

        return string.Empty;
      }
    }

    public ObservableCollection<Algorithm> Algorithms { get; }
    public ObservableCollection<Separator> Separators { get; }

    public bool EnableSeparatorData
    {
      get => _enableSeparatorData;
      set => this.RaiseAndSetIfChanged(ref _enableSeparatorData, value);
    }

    public bool EnableAlgorithmData
    {
      get => _enableAlgorithmData;
      set => this.RaiseAndSetIfChanged(ref _enableAlgorithmData, value);
    }
    
    public bool NeedAskRewriteFile
    {
      get => _needAskRewriteFile;
      set => this.RaiseAndSetIfChanged(ref _needAskRewriteFile, value);
    }

    public bool OpenFileAfterProcess
    {
      get => _openFileAfterProcess;
      set => this.RaiseAndSetIfChanged(ref _openFileAfterProcess, value);
    }

    public Algorithm AlgorithmType
    {
      get => _algorithmType;
      set => this.RaiseAndSetIfChanged(ref _algorithmType, value);
    }

    public string AlgorithmData
    {
      get => _algorithmData;
      set => this.RaiseAndSetIfChanged(ref _algorithmData, value);
    }

    public Separator SeparatorType
    {
      get => _separatorType;
      set => this.RaiseAndSetIfChanged(ref _separatorType, value);
    }
    
    public string CustomSeparator
    {
      get => _customSeparator;
      set => this.RaiseAndSetIfChanged(ref _customSeparator, value);
    }
    
    public string SavePath
    {
      get => _savePath;
      set => this.RaiseAndSetIfChanged(ref _savePath, value);
    }

    public string ProcessText
    {
      get => _processText;
      set => this.RaiseAndSetIfChanged(ref _processText, value);
    }

    public ReactiveCommand<Unit, Unit> ProcessCommand { get; }

    public ReactiveCommand<Unit, Unit> ChoiceSavePath { get; }

    public MainWindowViewModel()
    {
      _validator = new MainWindowViewModelValidator();

      var checkTrigger = this.WhenAnyValue(x => x.AlgorithmType,
        x => x.SeparatorType,
        x => x.SavePath,
        s => s.AlgorithmData,
        s => s.CustomSeparator,
        s => s.ProcessText,
        (algorithmType, separatorType, savePath, algorithmData, customSeparator, processText) =>
        {
          if (algorithmType == null || separatorType == null || string.IsNullOrWhiteSpace(savePath) || string.IsNullOrWhiteSpace(processText))
            return false;

          if (algorithmType.IsRequiredAlgorithmData && !algorithmType.IsValidAlgorithmData(algorithmData))
            return false;

          if (IsRequiredSeparator(separatorType) && string.IsNullOrEmpty(customSeparator))
            return false;
          
          return true;
        });
      checkTrigger.Subscribe(x => {});

      ProcessCommand = ReactiveCommand.Create(StartProcess, checkTrigger);

      ChoiceSavePath = ReactiveCommand.Create(() =>
      {
        var saveFileDialog = new SaveFileDialog();
        if(saveFileDialog.ShowDialog() == DialogResult.OK)
            SavePath = saveFileDialog.FileName;
      });
      
      Algorithms = new ObservableCollection<Algorithm>(Algorithm.Values);
      AlgorithmType = Algorithms.First();
      
      Separators = new ObservableCollection<Separator>(FlexEnumHelper.Values<Separator>());
      SeparatorType = Separators.First();

      this.WhenValueChanged(model => model.SeparatorType).Subscribe(separator =>
      {
        EnableSeparatorData = IsRequiredSeparator(separator);
        if (!EnableSeparatorData)
        {
          CustomSeparator = null;
        }

        this.RaisePropertyChanged(nameof(CustomSeparator)); 
      });

      this.WhenValueChanged(m => m.AlgorithmType).Subscribe(a =>
      {
        EnableAlgorithmData = a.IsRequiredAlgorithmData;
        this.RaisePropertyChanged(nameof(AlgorithmData));
      });
      
      OpenFileAfterProcess = true;
    }

    private void StartProcess()
    {
      var savePath = SavePath;
      var fullFilePath = savePath.LastIndexOf('.') >= 0 ? savePath : savePath + ".txt";
      if (NeedAskRewriteFile)
      {
        if (File.Exists(fullFilePath))
        {
          var questionResult = MessageBox.Show("Данный файл уже существует, вы уверены, что хотите перезаписать его?",
            "Внимание", 
            MessageBoxButtons.OKCancel, 
            MessageBoxIcon.Question);
          
          if(questionResult == DialogResult.Cancel)
            return;
        }
      }

      var separator = GetSeparator();
      var result = AlgorithmType.Process(ProcessText.Split(separator), AlgorithmData);

      File.WriteAllLines(fullFilePath, result);

      if (OpenFileAfterProcess)
      {
        var p = new Process {StartInfo = new ProcessStartInfo(fullFilePath) {UseShellExecute = true}};
        p.Start();
      }
    }

    private string GetSeparator()
    {
      if (SeparatorType == Separator.NewLine)
        return Environment.NewLine;

      if (SeparatorType == Separator.Custom)
        return CustomSeparator;

      throw new InvalidOperationException("Invalid SeparatorType");
    }

    private static bool IsRequiredSeparator(Separator separator) => separator == Separator.Custom;
  }
}
