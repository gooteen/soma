using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject _combatPanel;
    public GameObject _dialoguePanel;

    [Header("Dialogue Panel")]
    public Text _speakerName;
    public Text _dialogueBox;

    public GameObject _replyBox;
    public GameObject _replyBoxContent;

    public Button _replyOptionButton;
    public Button _continueButton;

    [Header("Combat Panel")]
    public Button _toCombatModeButton;
    public Button _toMovementModeButton;
    public Text _apLeft;

    public static UIManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        _apLeft.text = $"AP: x{Engine.Instance.TacticalPlayer.GetActionPoints()}";
    }
}
