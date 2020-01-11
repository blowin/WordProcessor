using System.Collections.Generic;
using WordProcessor.Util.Extension;

namespace WordProcessor.DataTypes.Algorithms
{
  public abstract partial class Algorithm
  {
    private sealed class ShuffleTranslateAlgorithmImpl : Algorithm
    {
      private static readonly string[] IgnoreValues = 
      {
        " "
      };

      public override string Name => "Перемешать перевод";

      public override bool IsRequiredAlgorithmData => true;

      protected override IEnumerable<string> ProcessInternal(IEnumerable<string> input, string algorithmData)
      {
        var column1 = new List<string>();
        var column2 = new List<string>();
        foreach (var s in input)
        {
          var splitWords = s.Split(algorithmData);
          if (splitWords.Length > 1)
          {
            column1.Add(splitWords[0]);
            column2.Add(splitWords[1]);
          }
          else
          {
            column1.Add(splitWords[0]);
            column2.Add(string.Empty);
          }
        }

        Random.Shuffle(column1, IgnoreValues);

        for (var i = 0; i < column1.Count; i++)
        {
          yield return column1[i].Trim() + " - " + column2[i].Trim();
        }
      }

      public override bool IsValidAlgorithmData(string data) => !string.IsNullOrEmpty(data);
    }
  }
}