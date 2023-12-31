using Audune.Utils.Types;
using System;
using System.Linq;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace Audune.Localization.Editor
{
  // Class that defines a scripted importer for localized strings
  [ScriptedImporter(202203001, "locale")]
  public class LocaleImporter : ScriptedImporter
  {
    // Locale importer settings
    [SerializeField, Tooltip("The parser to use for parsing the locale"), TypeReference(typeof(LocaleParser))]
    private TypeReference _parser = typeof(LocaleParser).GetChildTypes().FirstOrDefault();


    // Import the asset
    public override void OnImportAsset(AssetImportContext context)
    {
      // Create the parser
      if (_parser.Type == null)
        throw new ArgumentException("Null type");

      var parser = Activator.CreateInstance(_parser) as LocaleParser;

      // Parse the locale
      var locale = parser.Parse(context.assetPath);
      if (locale == null)
        throw new NotImplementedException($"Cannot parse locale with parser of type {_parser.Type}");

      // Add the locale to the asset
      context.AddObjectToAsset("locale", locale);
      context.SetMainObject(locale);
    }
  }
}