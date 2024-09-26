using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private static UIController _instance;

    [SerializeField] private ScenePopup scenePopup;

    private enum DisplayPopup
    {
        Start,
        End
    };

    private Canvas _canvas;
    private GameObject _menuPanel;
    private GameObject _popupPanel;

    private TextMeshProUGUI _wizardText;

    private bool _isMenuOpen;
    private bool _isPopupOpen;

    private Button _restartBtn;
    private Button _quitBtn;

    private List<string> _dialogueAtStart;
    private List<string> _dialogueAtEnd;

    private DisplayPopup _displayPopup = DisplayPopup.Start;

    private void Awake()
    {
        _instance = this;

        _canvas = GetComponent<Canvas>();
        _menuPanel = GameObject.Find("menuPanel");
        _popupPanel = GameObject.Find("popupPanel");
        _wizardText = GameObject.Find("wizardText").GetComponent<TextMeshProUGUI>();

        _restartBtn = GameObject.Find("restartBtn").GetComponent<Button>();
        _quitBtn = GameObject.Find("quitBtn").GetComponent<Button>();
    }

    private void Start()
    {
        _restartBtn.onClick.AddListener(SceneController.Restart);
        _quitBtn.onClick.AddListener(SceneController.Quit);

        _canvas.enabled = false;
        _popupPanel.SetActive(false);
        _menuPanel.SetActive(false);

        var sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (sceneIndex <= scenePopup.dialogues.Length)
        {
            _dialogueAtStart = new List<string>(scenePopup.dialogues[sceneIndex - 1].dialogueAtStart);
            _dialogueAtEnd = new List<string>(scenePopup.dialogues[sceneIndex - 1].dialogueAtEnd);
        }
        else
        {
            _dialogueAtStart = new List<string>();
            _dialogueAtEnd = new List<string>();
        }
    }

    private void ShowPopup(string text)
    {
        _isPopupOpen = true;
        _canvas.enabled = true;
        _popupPanel.SetActive(_isPopupOpen);

        _wizardText.text = text;
    }

    public static void ShowMenuPanel()
    {
        _instance._isMenuOpen = true;
        _instance._canvas.enabled = true;
        _instance._menuPanel.SetActive(_instance._isMenuOpen);
    }

    private void Update()
    {
        var dialogueList = _displayPopup == DisplayPopup.Start ? _dialogueAtStart : _dialogueAtEnd;

        if (!_isPopupOpen && dialogueList.Count > 0)
        {
            ShowPopup(dialogueList[0]);
            dialogueList.RemoveAt(0);
        } else if (_isPopupOpen && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)))
        {
            if (dialogueList.Count > 0)
            {
                ShowPopup(dialogueList[0]);
                dialogueList.RemoveAt(0);
            }
            else
            {
                _isPopupOpen = false;
                _canvas.enabled = false;
                _popupPanel.SetActive(_isPopupOpen);
            }
        }

        if (!_isPopupOpen && _displayPopup == DisplayPopup.End && _dialogueAtEnd.Count == 0)
        {
            SceneController.NextLevel();
        }
    }

    public static bool IsDisplaying()
    {
        return _instance._isPopupOpen || _instance._isMenuOpen;
    }

    public static void DisplayEndPopups()
    {
        _instance._displayPopup = DisplayPopup.End;
    }
}