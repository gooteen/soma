using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[System.Serializable]
public class LineEvent
{
    public UnityEvent _event;
    public string _lineAlias;
}

public class DialogueController : MonoBehaviour
{
    [SerializeField] private Dialogue _dialogueSO;
    [SerializeField] private Vector2 _npcDirectionAtStart;
    [SerializeField] private float _distance;
    [SerializeField] private LineEvent[] _events;
    private List<GameObject> _currentOptionButtons;
    private CharacterAnimation _anim;

    void Awake()
    {
        _anim = GetComponent<CharacterAnimation>();
        _currentOptionButtons = new List<GameObject>();
    }

    private void Start()
    {
        _anim.SetDirection(_npcDirectionAtStart);
    }

    void Update()
    {
        if (IsFocusedOnCharacter())
        {
            if (Engine.Instance.InputManager.LeftMouseButtonPressed())
            {
                if (Vector3.Distance(Engine.Instance.Player.transform.position, transform.position) <= _distance)
                {
                    Engine.Instance.Player.Freeze();
                    InitializeDialogue();
                    Engine.Instance.Player.SetDirection(transform.position - Engine.Instance.Player.transform.position);
                    _anim.SetDirection(Engine.Instance.Player.transform.position - transform.position);
                }
            }
        }
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
        if (line._options.Length == 0) 
        {
            UIManager.Instance._replyBox.SetActive(false);
            UIManager.Instance._continueButton.gameObject.SetActive(true);
            UIManager.Instance._continueButton.onClick.RemoveAllListeners();

            UIManager.Instance._speakerName.text = line.GetSpeakerName();
            UIManager.Instance._dialogueBox.text = line.GetLineText();
            InvokeLineEvent(line._alias);
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
    public bool IsFocusedOnCharacter()
    {
        LayerMask _mask = LayerMask.GetMask("NPC");

        Vector3 _mousePos = Camera.main.ScreenToWorldPoint(Engine.Instance.InputManager.GetMousePosition());
        Vector2 _mousePos2d = new Vector2(_mousePos.x, _mousePos.y);

        RaycastHit2D _hit = Physics2D.Raycast(_mousePos2d, Vector2.zero, Mathf.Infinity, _mask, -Mathf.Infinity, Mathf.Infinity);

        if (_hit.collider != null && Engine.Instance.TacticalPlayer == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void InvokeLineEvent(string alias)
    {
        foreach (LineEvent _event in _events)
        {
            if (_event._lineAlias == alias)
            {
                _event._event.Invoke();
            }
        }
    }

    private void FinishDialogue()
    {
        UIManager.Instance._dialoguePanel.SetActive(false);
        _anim.SetDirection(_npcDirectionAtStart);
        Engine.Instance.Player.Freeze();
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
}
