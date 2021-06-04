﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ConversationText : MonoBehaviour
{
    public static ConversationText instance;
    StreamWriter writer;
    string path;

    // Start is called before the first frame update
    void Start()
    {
        if (ConversationText.instance) Destroy(this);
        ConversationText.instance = this;

        path = ManageCollectors.instance.pathForCurrentIteration() + "/ConversationText.csv";
        writer = new StreamWriter(path, true);
        writer.WriteLine("IsUser, Sentence, Timestamp");
        writer.Close();
        print(path);
    }

    public void StoreSentence(bool user, string text)
    {
        writer = new StreamWriter(path, true);
        writer.WriteLine(user.ToString() + ',' + text + Time.time.ToString().Replace(",", "."));
        writer.Close();
    }
}
