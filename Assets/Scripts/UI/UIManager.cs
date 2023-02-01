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
    public GameObject _battleWonWindow;
    public GameObject _battleLostWindow;
    public GameObject _tempTransitionScreen;
    public float _transitionTimeSeconds;
    public Image _Healthbar;
    public Transform _combatantsQueue;
    public GameObject _combatantCellPrefab;
    public List<GameObject> _currentCombatantCells;
    public Button _toCombatModeButton;
    public Button _toMovementModeButton;
    public Text _apLeft;

    public static UIManager Instance { get; private set; }

    void Awake()
    {
        _tempTransitionScreen.SetActive(false);
        Instance = this;
    }

    public void ShowBattleWonWindow()
    {
        _battleWonWindow.SetActive(true);
    }

    public void ShowBattleLostWindow()
    {
        _battleLostWindow.SetActive(true);
    }

    public void FillCombatantCell(string text)
    {
        GameObject _cell = Instantiate(_combatantCellPrefab, _combatantsQueue);
        _cell.GetComponentInChildren<Text>().text = text;
        _currentCombatantCells.Add(_cell);
    }

    public void ClearCombatantQueue()
    {
        foreach (GameObject cell in _currentCombatantCells)
        {
            cell.GetComponent<CombatantCell>().Pop();
        }
        _currentCombatantCells.Clear();
    }

    public void RemoveCombatantCellAt(int index)
    {
        _currentCombatantCells[index].GetComponent<CombatantCell>().Pop();
        _currentCombatantCells.RemoveAt(index);
    }

    public void ManageQueueMarkers(int index)
    {
        for (int i = 0; i < _currentCombatantCells.Count; i++)
        {
            if (i == index)
            {
                _currentCombatantCells[i].GetComponent<CombatantCell>().ShowMarker();
            } else
            {
                _currentCombatantCells[i].GetComponent<CombatantCell>().HideMarker();
            }
        }
    }

    public void TransitionToBattle()
    {
        StartCoroutine("Transition");
    }

    public void TransitionOutOfLostBattle()
    {
        StartCoroutine("Transition");
        MapManager.Instance.ClearArena();
    }

    public void TransitionOutOfWonBattle()
    {
        StartCoroutine("Transition");
        MapManager.Instance.AddEnemyLootToInventory();
        MapManager.Instance.ClearArena();
    }

    public void ShowTransitionPanelOut()
    {
        StartCoroutine("TransitionOut");
    }

    private IEnumerator Transition()
    {
        TransitionPanelOn();
        yield return new WaitForSeconds(_transitionTimeSeconds);
        TransitionPanelOff();
    }

    private void TransitionPanelOff()
    {
        CursorController.Instance.UnPause();
        _tempTransitionScreen.SetActive(false);
        Engine.Instance.Player.Freeze();
    }

    private void TransitionPanelOn()
    {
        CursorController.Instance.Pause();
        _tempTransitionScreen.SetActive(true);
        Engine.Instance.Player.Freeze();
    }

    private void Update()
    {
        if (Engine.Instance._currentGameMode == GameMode.Battle && Engine.Instance.TurnManager.GetCurrentCharacter().tag != "EnemyTactical")
        {
            _apLeft.text = $"AP: x{Engine.Instance.TacticalPlayer.GetActionPoints()}";
            _Healthbar.fillAmount = Engine.Instance.TacticalPlayer.GetHealthRatio();
        }
    }
}
