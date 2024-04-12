namespace AuthService.Infrastructure.Helpers;

/// <summary>
/// Транслит с русских символов на аглийские 
/// </summary>
public static class WordsTranslate
{
    private static Dictionary<string, string> dictionaryChar = new()
    {
        {"а","a"},
        {"б","b"},
        {"в","v"},
        {"г","g"},
        {"д","d"},
        {"е","e"},
        {"ё","yo"},
        {"ж","zh"},
        {"з","z"},
        {"и","i"},
        {"й","y"},
        {"к","k"},
        {"л","l"},
        {"м","m"},
        {"н","n"},
        {"о","o"},
        {"п","p"},
        {"р","r"},
        {"с","s"},
        {"т","t"},
        {"у","u"},
        {"ф","f"},
        {"х","h"},
        {"ц","ts"},
        {"ч","ch"},
        {"ш","sh"},
        {"щ","sch"},
        {"ъ",""},
        {"ы","yi"},
        {"ь",""},
        {"э","e"},
        {"ю","yu"},
        {"я","ya"}
    };
    
    /// <summary>
    /// Метод для траслирования слов
    /// </summary>
    /// <param name="sourceWord">Слово которое надо траслировать</param>
    /// <returns>Транслированное слово</returns>
    public static string WordTranslate(string sourceWord)
    {
        string result = "";

        foreach (var ch in sourceWord)
        {
            var ss = "";
            if (dictionaryChar.TryGetValue(ch.ToString(), out ss))
            {
                result += ss;
            }
            else
            {
                result += ch;
            }
        }

        return result;
    }
}