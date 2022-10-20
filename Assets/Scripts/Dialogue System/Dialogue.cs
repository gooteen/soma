using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public string _alias;
    public string _speakerName;
    public string _text;
    public bool _hasOptionsOfReply;
    public bool _isFinalLine;
    public DialogueOption[] _options;

    public string GetSpeakerName()
    {
        if (_speakerName.Contains("{PlayerName}"))
        {
            Debug.Log(Engine.Instance.Hero._name);
            return _speakerName.Replace("{PlayerName}", Engine.Instance.Hero._name);
        } else
        {
            return _speakerName;
        }
    }

    public string GetLineText()
    {
        if (_text.Contains("{PlayerName}"))
        {
            return _text.Replace("{PlayerName}", Engine.Instance.Hero.name);
        }
        else
        {
            return _text;
        }
    }
}

[System.Serializable]
public class DialogueOption
{
    public string _text;
    public string _nextLineAlias;

    public string GetOptionText()
    {
        if (_text.Contains("{PlayerName}"))
        {
            return _text.Replace("{PlayerName}", Engine.Instance.Hero._name);
        }
        else
        {
            return _text;
        }
    }
}

[CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue SO")]
public class Dialogue : ScriptableObject
{
    [SerializeField] private DialogueLine[] _dialogueLines;

    public DialogueLine[] GetDialogueLines()
    {
        return _dialogueLines;
    }
}
