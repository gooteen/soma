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
    public Button _toMagicModeButton;
    public GameObject _spellsPanel;
    public Text _apLeft;
    public List<GameObject> _magicSpellsList;
    public GameObject _magicAbilityButtonPrefab;

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

    public void ConfigureSpellsButton()
    {
        Debug.Log("MAGIC: Configuring");
        _toMagicModeButton.onClick.RemoveAllListeners();
        ClearSpellsPanel();
        HideSpellsPanel();
        _toMagicModeButton.onClick.AddListener(FillSpellsPanel);
    }

    public void FillSpellsPanel()
    {
        List<MagicAbility> _abilities = Engine.Instance.TacticalPlayer.GetMagicAbilitiesOnCurrentWeapon();
        if (_abilities.Count > 0)
        {
            ShowSpellsPanel();
            foreach (MagicAbility ability in _abilities)
            {
                GameObject _abilityButton = Instantiate(_magicAbilityButtonPrefab, _spellsPanel.transform);
                _abilityButton.GetComponent<Button>().onClick.AddListener(ability.OnChosen);
                _magicSpellsList.Add(_abilityButton);
            }
            _toMagicModeButton.onClick.AddListener(HideSpellsPanel);
            _toMagicModeButton.onClick.AddListener(delegate { _toMagicModeButton.onClick.AddListener(FillSpellsPanel); });
        }
    }

    public void ClearSpellsPanel()
    {
        foreach (GameObject button in _magicSpellsList)
        {
            Destroy(button);
        }
        _magicSpellsList.Clear();
    }

    public void ShowSpellsPanel()
    {
        _spellsPanel.SetActive(true);
    }

    public void HideSpellsPanel()
    {
        ClearSpellsPanel();
        _spellsPanel.SetActive(false);
    }

    public void TransitionToBattle()
    {
        StartCoroutine("Transition");
        _battleLostWindow.SetActive(false);
        _battleWonWindow.SetActive(false);
    }

    public void TransitionOutOfLostBattle()
    {
        StartCoroutine("Transition");
        MapManager.Instance.ClearArena();
    }

    public void TransitionOutOfWonBattle()
    {
        StartCoroutine("TransitionWithLoot");
        
    }

    private IEnumerator Transition()
    {
        TransitionPanelOn();
        yield return new WaitForSeconds(_transitionTimeSeconds);
        TransitionPanelOff();
    }

    private IEnumerator TransitionWithLoot()
    {
        TransitionPanelOn();
        yield return new WaitForSeconds(_transitionTimeSeconds);
        MapManager.Instance.AddEnemyLootToInventory();
        MapManager.Instance.ClearArena();
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
