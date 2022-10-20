using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private Dialogue _dialogueSO;
    private List<GameObject> _currentOptionButtons;

    private void Awake()
    {
        _currentOptionButtons = new List<GameObject>();
    }

    public void InitializeDialogue()
    {
        UIManager.Instance._combatPanel.SetActive(false);
        UIManager.Instance._dialoguePanel.SetActive(true);

        SetNextLine(_dialogueSO.GetDialogueLines()[0]);
    }

    public void SetNextLine(DialogueLine line)
    {
        Debug.Log("line text: " + line.GetLineText() + " line alias: " + line._alias);
        if (!line._hasOptionsOfReply) 
        {
            UIManager.Instance._replyBox.SetActive(false);
            UIManager.Instance._continueButton.gameObject.SetActive(true);
            UIManager.Instance._continueButton.onClick.RemoveAllListeners();

            UIManager.Instance._speakerName.text = line.GetSpeakerName();
            UIManager.Instance._dialogueBox.text = line.GetLineText();

            if(!line._isFinalLine)
            {
                Debug.Log("CONTINUE button next line alias: " + _dialogueSO.GetDialogueLines()[_dialogueSO.GetLineIndex(line) + 1]._alias);
                UIManager.Instance._continueButton.onClick.AddListener(delegate { SetNextLine(_dialogueSO.GetDialogueLines()[_dialogueSO.GetLineIndex(line) + 1]); });
            } else
            {
                UIManager.Instance._continueButton.onClick.AddListener(FinishDialogue);
            }
        } else
        {
            ClearButtons();

            UIManager.Instance._replyBox.SetActive(true);
            UIManager.Instance._continueButton.gameObject.SetActive(false);

            UIManager.Instance._speakerName.text = line.GetSpeakerName();
            UIManager.Instance._dialogueBox.text = line.GetLineText();

            foreach (DialogueOption _option in line._options)
            {
                Button _newButton = Instantiate(UIManager.Instance._replyOptionButton, UIManager.Instance._replyBoxContent.transform);
                _newButton.GetComponentInChildren<Text>().text = _option.GetOptionText();
                _newButton.onClick.AddListener(delegate { SetNextLine(GetLineByAlias(_option._nextLineAlias)); });
                Debug.Log("next line alias: " + _option._nextLineAlias);
                _currentOptionButtons.Add(_newButton.gameObject);
            }
        }
    }

    private void FinishDialogue()
    {
        UIManager.Instance._dialoguePanel.SetActive(false);
        
        //temp
        UIManager.Instance._combatPanel.SetActive(true);
        ClearButtons();
    }

    private void ClearButtons()
    {
        if (_currentOptionButtons != null)
        {
            foreach (GameObject _button in _currentOptionButtons)
            {
                Destroy(_button);
            }
            _currentOptionButtons.Clear();
        } else
        {
            Debug.Log("Already clear");
        }
    }

    private DialogueLine GetLineByAlias(string alias)
    {
        DialogueLine[] _lines = _dialogueSO.GetDialogueLines();
        foreach (DialogueLine _line in _lines)
        {
            if (_line._alias == alias)
            {
                return _line;
            }
        }
        return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        InitializeDialogue();
    }
}
