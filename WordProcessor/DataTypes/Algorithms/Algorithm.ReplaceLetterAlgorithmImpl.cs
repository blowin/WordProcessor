﻿using System;
using System.Collections.Generic;
using WordProcessor.Util.Extension;

namespace WordProcessor.DataTypes.Algorithms
{
  public abstract partial class Algorithm
  {
    private sealed class ReplaceLetterAlgorithmImpl : Algorithm
    {
      public override string Name => LocalizationManager.GetLocalizationString("m_algo_name_ReplaceLetter");

      public override bool IsRequiredAlgorithmData => true;

      public override string ErrorMessageForEmptyInput =>
        LocalizationManager.GetLocalizationString("m_valid_AlgorithmData_ReplaceLetter");

      protected override IEnumerable<string> ProcessInternal(IEnumerable<ReadOnlyMemory<char>> input, string algorithmData)
      {
        const char replaceLetter = '_';

        var replaceLetterCount = ExtractReplaceCount(algorithmData);
        foreach (var s in input)
        {
          if (s.Length <= replaceLetterCount)
          {
            yield return new string(replaceLetter, replaceLetterCount);
          }
          else
          {
            var replaceCount = 0;
            var charArr = s.ToArray();
            while (replaceCount < replaceLetterCount)
            {
              var randomIdx = Random.Next(0, s.Length);
              if (charArr[randomIdx] == replaceLetter) 
                continue;
              
              charArr[randomIdx] = replaceLetter;
              replaceCount += 1;
            }

            yield return charArr.BuildString();
          }
        }
      }

      public override bool IsValidAlgorithmData(string data)
      {
        return int.TryParse(data, out _);
      }

      private static int ExtractReplaceCount(string val)
      {
        int r;
        return int.TryParse(val, out r) ? Math.Abs(r) : 1;
      }
    }
  }
}