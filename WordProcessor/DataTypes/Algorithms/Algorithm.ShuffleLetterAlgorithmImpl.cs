using System.Collections.Generic;
using WordProcessor.Util.Extension;

namespace WordProcessor.DataTypes.Algorithms
{
  public abstract partial class Algorithm
  {
    private sealed class ShuffleLetterAlgorithmImpl : Algorithm
    {
      private static readonly char[] IgnoreValues = 
      {
        ' ',
        '-'
      };

      public override string Name => "Перемешать буквы";

      public override bool IsRequiredAlgorithmData => false;

      protected override IEnumerable<string> ProcessInternal(IEnumerable<string> input, string algorithmData)
      {
        foreach (var s in input)
        {
          var charArr = s.ToCharArray();
          Random.Shuffle(charArr, IgnoreValues);
          yield return charArr.BuildString();
        }
      }

      public override bool IsValidAlgorithmData(string data) => true;
    }
  }
}