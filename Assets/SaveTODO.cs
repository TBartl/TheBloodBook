using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

using System.Text;
using System.IO;

public class SaveTODO : MonoBehaviour {
    static string path = TimeManager.folder + "/todo.txt";

    void Start() {
        if (File.Exists(path))
            this.GetComponent<InputField>().text = File.ReadAllText(path);
        else
            Debug.Log("Unable to open " + path);
    }

    public void OnEndEdit(string s) {
        if (!Directory.Exists(TimeManager.folder))
            Directory.CreateDirectory(TimeManager.folder);

        File.WriteAllText(path, s);
    }
}
