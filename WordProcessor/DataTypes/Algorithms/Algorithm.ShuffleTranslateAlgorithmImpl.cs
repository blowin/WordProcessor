using System;
using System.Collections.Generic;
using WordProcessor.Util.Extension;

namespace WordProcessor.DataTypes.Algorithms
{
  public abstract partial class Algorithm
  {
    private sealed class ShuffleTranslateAlgorithmImpl : Algorithm
    {
      private static readonly ReadOnlyMemory<char>[] IgnoreValues = 
      {
        " ".AsMemory()
      };

      public override string Name => LocalizationManager.GetLocalizationString("m_algo_name_ShuffleTranslate");

      public override bool IsRequiredAlgorithmData => true;

      public override string ErrorMessageForEmptyInput 
        => LocalizationManager.GetLocalizationString("m_valid_AlgorithmData_ShuffleTranslate");

      protected override IEnumerable<string> ProcessInternal(IEnumerable<ReadOnlyMemory<char>> input, string algorithmData)
      {
        var column1 = new List<ReadOnlyMemory<char>>();
        var column2 = new List<ReadOnlyMemory<char>>();
        foreach (var s in input)
        {
          var separatorIdx = s.Span.IndexOf(algorithmData.AsSpan());
          if (separatorIdx > 0)
          {
            var firstPart = s.Slice(0, separatorIdx).Trim();
            var secondPart = s.Slice(separatorIdx + 1).Trim();
            column1.Add(firstPart);
            column2.Add(secondPart);
          }
          else
          {
            column1.Add(separatorIdx != 0 ? s.Trim() : string.Empty.AsMemory());
            column2.Add(string.Empty.AsMemory());
          }
        }

        Random.Shuffle(column1, IgnoreValues);

        for (var i = 0; i < column1.Count; i++)
        {
          yield return column1[i] + algorithmData + column2[i];
        }
      }

      public override bool IsValidAlgorithmData(string data) => !string.IsNullOrEmpty(data);
    }
  }
}