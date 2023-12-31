﻿using Audune.Utils.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Audune.Localization.Settings.Loaders
{
  // Class that defines a locale selector that returns the specified locale
  public sealed class StreamingAssetsLocaleLoader : LocaleLoader
  {
    // Locale loader settings
    [SerializeField, Tooltip("The directory where to load streamed locales from, relative to Application.streamingAssetsPath")]
    private string _directory;
    [SerializeField, Tooltip("The search pattern for streamed locales")]
    private string _pattern = "*.locale";
    [SerializeField, Tooltip("The parser to parse the streamed locales with"), TypeReference(typeof(LocaleParser))]
    private TypeReference _parser = typeof(LocaleParser).GetChildTypes().FirstOrDefault();


    // Load locales according to this loader
    public override IEnumerable<Locale> Load()
    {
      var locales = new List<Locale>();

      // Create the parser
      var parser = Activator.CreateInstance(_parser) as LocaleParser;

      // Get the files in the directory and parse them
      try
      {
        var files = Directory.GetFiles(Path.Combine(Application.streamingAssetsPath, _directory), _pattern, SearchOption.AllDirectories);
        foreach (var file in files)
        {
          try
          {
            var locale = parser.Parse(file);
            locales.Add(locale);
          }
          catch (FormatException ex)
          {
            Debug.LogWarning($"Could not parse locale file {file} in {name}: {ex.Message}");
            continue;
          }
        }
      }
      catch (DirectoryNotFoundException)
      {
        Debug.LogWarning($"Could not load locales in {Path.Combine(Application.streamingAssetsPath, _directory)} because the directory cannot be found");
      }

      return locales;
    }
  }
}
