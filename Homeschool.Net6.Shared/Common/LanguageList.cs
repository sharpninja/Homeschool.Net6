namespace Homeschool.App.Common;

using System.Collections.Generic;

class LanguageList
{
    private List<Language> _languages;
    public List<Language> Languages => _languages;

    public LanguageList()
    {
        if (_languages == null)
        {
            _languages = new();
        }

        _languages.Add(new("English", "en"));
        _languages.Add(new("Arabic", "ar"));
        _languages.Add(new("Afrikaans", "af"));
        _languages.Add(new("Albanian", "sq"));
        _languages.Add(new("Amharic", "am"));
        _languages.Add(new("Armenian", "hy"));
        _languages.Add(new("Assamese", "as"));
        _languages.Add(new("Azerbaijani", "az"));
        _languages.Add(new("Basque ", "eu"));
        _languages.Add(new("Belarusian", "be"));
        _languages.Add(new("Bangla", "bn"));
        _languages.Add(new("Bosnian", "bs"));
        _languages.Add(new("Bulgarian", "bg"));
        _languages.Add(new("Catalan", "ca"));
        _languages.Add(new("Chinese (Simplified)", "zh"));
        _languages.Add(new("Croatian", "hr"));
        _languages.Add(new("Czech", "cs"));
        _languages.Add(new("Danish", "da"));
        _languages.Add(new("Dari", "prs"));
        _languages.Add(new("Dutch", "nl"));
        _languages.Add(new("Estonian", "et"));
        _languages.Add(new("Filipino", "fil"));
        _languages.Add(new("Finnish", "fi"));
        _languages.Add(new("French ", "fr"));
        _languages.Add(new("Galician", "gl"));
        _languages.Add(new("Georgian", "ka"));
        _languages.Add(new("German", "de"));
        _languages.Add(new("Greek", "el"));
        _languages.Add(new("Gujarati", "gu"));
        _languages.Add(new("Hausa", "ha"));
        _languages.Add(new("Hebrew", "he"));
        _languages.Add(new("Hindi", "hi"));
        _languages.Add(new("Hungarian", "hu"));
        _languages.Add(new("Icelandic", "is"));
        _languages.Add(new("Indonesian", "id"));
        _languages.Add(new("Irish", "ga"));
        _languages.Add(new("isiXhosa", "xh"));
        _languages.Add(new("isiZulu", "zu"));
        _languages.Add(new("Italian", "it"));
        _languages.Add(new("Japanese ", "ja"));
        _languages.Add(new("Kannada", "kn"));
        _languages.Add(new("Kazakh", "kk"));
        _languages.Add(new("Khmer", "km"));
        _languages.Add(new("Kinyarwanda", "rw"));
        _languages.Add(new("KiSwahili", "sw"));
        _languages.Add(new("Konkani", "kok"));
        _languages.Add(new("Korean", "ko"));
        _languages.Add(new("Lao", "lo"));
        _languages.Add(new("Latvian", "lv"));
        _languages.Add(new("Lithuanian", "lt"));
        _languages.Add(new("Luxembourgish", "lb"));
        _languages.Add(new("Macedonian", "mk"));
        _languages.Add(new("Malay", "ms"));
        _languages.Add(new("Malayalam", "ml"));
        _languages.Add(new("Maltese", "mt"));
        _languages.Add(new("Maori ", "mi"));
        _languages.Add(new("Marathi", "mr"));
        _languages.Add(new("Nepali", "ne"));
        _languages.Add(new("Norwegian", "nb"));
        _languages.Add(new("Odia", "or"));
        _languages.Add(new("Persian", "fa"));
        _languages.Add(new("Polish", "pl"));
        _languages.Add(new("Portuguese", "pt"));
        _languages.Add(new("Punjabi", "pa"));
        _languages.Add(new("Quechua", "quz"));
        _languages.Add(new("Romanian", "ro"));
        _languages.Add(new("Russian", "ru"));
        _languages.Add(new("Serbian (Latin)", "sr"));
        _languages.Add(new("Sesotho sa Leboa", "nso"));
        _languages.Add(new("Setswana", "tn"));
        _languages.Add(new("Sinhala", "si"));
        _languages.Add(new("Slovak ", "sk"));
        _languages.Add(new("Slovenian", "sl"));
        _languages.Add(new("Spanish", "es"));
        _languages.Add(new("Swedish", "sv"));
        _languages.Add(new("Tamil", "ta"));
        _languages.Add(new("Telugu", "te"));
        _languages.Add(new("Thai", "th"));
        _languages.Add(new("Tigrinya", "ti"));
        _languages.Add(new("Turkish", "tr"));
        _languages.Add(new("Ukrainian", "uk"));
        _languages.Add(new("Urdu", "ur"));
        _languages.Add(new("Uzbek (Latin)", "uz"));
        _languages.Add(new("Vietnamese", "vi"));
        _languages.Add(new("Welsh", "cy"));
        _languages.Add(new("Wolof", "wo"));
            
    }

    public class Language
    {
        public string Name { get; set; }
        public string Code { get; set; }

        public Language (string name, string code)
        {
            Name = name;
            Code = code;
        }
    }
}