﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using WordProcessor.Util.Extension;

namespace WordProcessor.DataTypes.Algorithms
{
  public abstract partial class Algorithm
  {
    private static Lazy<IReadOnlyList<Algorithm>> _lazyValues = new Lazy<IReadOnlyList<Algorithm>>(() =>
    {
      return typeof(Algorithm)
        .GetFields(BindingFlags.Public | BindingFlags.Static)
        .Select(s => s.GetValue(null) as Algorithm)
        .Where(s => s != null)
        .ToList();
    });

    public static readonly Algorithm ShuffleLetter = new ShuffleLetterAlgorithmImpl();
    public static readonly Algorithm ReplaceLetter = new ReplaceLetterAlgorithmImpl();
    public static readonly Algorithm ShuffleTranslate = new ShuffleTranslateAlgorithmImpl();

    protected static Random Random { get; } = new Random();

    public abstract string Name { get; }

    public abstract bool IsRequiredAlgorithmData { get; }
    
    public static IReadOnlyList<Algorithm> Values
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => _lazyValues.Value;
    }

    public IEnumerable<string> Process(IEnumerable<string> input, string algorithmData)
    {
      return ProcessInternal(input.WhereNotBlank(), algorithmData);
    }
    
    protected abstract IEnumerable<string> ProcessInternal(IEnumerable<string> input, string algorithmData);

    public abstract bool IsValidAlgorithmData(string data);
  }
}