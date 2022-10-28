using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Phrase {
    public string _text;
}

[CreateAssetMenu(fileName = "WordcloudSpeech", menuName = "Dialogue/WordcloudSpeech SO")]
public class WordcloudSpeech : ScriptableObject
{
     public Phrase[] _phrases;
}
