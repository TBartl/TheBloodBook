using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

using System.Text;
using System.IO;

public class SaveTODO : MonoBehaviour {
    public static SaveTODO S;
    static string path;
    string last;
    int lastCount;

    void Awake() {
        S = this;
        path = Application.persistentDataPath + "/SaveData" + "/todo.txt";
        if (File.Exists(path)) {
            last = File.ReadAllText(path);
            lastCount = CountNonEmptyLines(last);
            this.GetComponent<InputField>().text = last;
        }
    }

    public void OnEndEdit(string s) {
        if (!Directory.Exists(TimeManager.folder))
            Directory.CreateDirectory(TimeManager.folder);
        int newCount = CountNonEmptyLines(s);

        Avian.S.OnTODOUpdated(lastCount, newCount);
        Avian.S.ignoreNextFocus = true;

        lastCount = newCount;
        last = s;
        File.WriteAllText(path, s);
    }

    int CountNonEmptyLines(string text) {
        string[] lines = text.Split(new[] { '\r', '\n' });
        int count = 0;
        foreach(string line in lines) {
            if (line != "")
                count += 1;
        }
        return count;
    }

    public int GetTODOCount() {
        return lastCount;
    }
}
