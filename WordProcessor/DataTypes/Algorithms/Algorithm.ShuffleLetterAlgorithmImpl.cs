﻿using System;
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

      public override string Name => LocalizationManager.GetLocalizationString("m_algo_name_ShuffleLetter");

      public override bool IsRequiredAlgorithmData => false;

      public override string ErrorMessageForEmptyInput => string.Empty;

      protected override IEnumerable<string> ProcessInternal(IEnumerable<ReadOnlyMemory<char>> input, string algorithmData)
      {
        foreach (var s in input)
        {
          var charArr = s.ToArray();
          Random.Shuffle(charArr, IgnoreValues);
          yield return charArr.BuildString();
        }
      }

      public override bool IsValidAlgorithmData(string data) => true;
    }
  }
}