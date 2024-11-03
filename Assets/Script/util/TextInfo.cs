using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TextInfo
{
    private JObject textMap;
    private string lang;
    public TextInfo() {
        lang = PlayerPrefs.GetString("lang", "CHS");
        string readPath = Application.streamingAssetsPath + "/text.json";
        StreamReader sr = new StreamReader(readPath);
        string s = sr.ReadToEnd();
        textMap = JObject.Parse(s);
    }

    public void changeLang(string l) {
        lang = l;
    }

    public string getText(string id) {
        return (string)textMap[id][lang];
    }
}
