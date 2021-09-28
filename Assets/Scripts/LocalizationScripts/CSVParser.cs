using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CSVParser : MonoBehaviour
{
    private static List<string>  languageList = new List<string>();  
    private static Dictionary<string, List<string>> languageDictionary = new Dictionary<string, List<string>>();

    public static string[] SplitLine(string line)
    {
        return (from System.Text.RegularExpressions.Match m in System.Text.RegularExpressions.Regex.Matches(line,
                @"(((?<x>(?=[,\r\n]+))|""(?<x>([^""]|"""")+)""|(?<x>[^,\r\n]+)),?)",
                System.Text.RegularExpressions.RegexOptions.ExplicitCapture)
            select m.Groups[1].Value).ToArray();
    }

    public static List<string> GetAvailableLanguages()
    {
        if (languageList.Count == 0)
        {
            var csvFlie = Resources.Load<TextAsset>("Localization/Localization");
            string[] lines = csvFlie.text.Split("\n"[0]);
            languageList = new List<string>(SplitLine(lines[0]));
            languageList.RemoveAt(0);
        }

        return languageList;
    }

    public static string GetTextFromId(string id, int languageIndex)
    {
        if (languageDictionary.Count == 0)
        {
            var csvFlie = Resources.Load<TextAsset>("Localization/Localization");
            string[] lines = csvFlie.text.Split("\n"[0]);
            for (int i = 1; i < lines.Length; ++i)
            {
                string[] row = SplitLine(lines[i]);
                if (row.Length > 1)
                {
                    List<string> words = new List<string>(row);
                    words.RemoveAt(0);
                    languageDictionary.Add(row[0], words);
                }
            }
        }

        var values = languageDictionary[id];
        return values[languageIndex];
    }
}
