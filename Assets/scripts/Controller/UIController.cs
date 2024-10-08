using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Controller
{
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
        private GameObject _animationPanel;

        private TextMeshProUGUI _wizardText;

        private bool _isMenuOpen;
        private bool _isPopupOpen;

        private Button _continueBtn;
        private Button _restartBtn;
        private Button _menuBtn;

        private List<string> _dialogueAtStart;
        private List<string> _dialogueAtEnd;
        private List<string> _dialogueAtMenu;

        private DisplayPopup _displayPopup = DisplayPopup.Start;


        private void Awake()
        {
            _instance = this;

            _canvas = GetComponent<Canvas>();

            _menuPanel = GameObject.Find("menuPanel");
            _popupPanel = GameObject.Find("popupPanel");
            _animationPanel = GameObject.Find("animationPanel");

            _wizardText = GameObject.Find("wizardText").GetComponent<TextMeshProUGUI>();

            _continueBtn = GameObject.Find("continueBtn").GetComponent<Button>();
            _restartBtn = GameObject.Find("restartBtn").GetComponent<Button>();
            _menuBtn = GameObject.Find("menuBtn").GetComponent<Button>();
        }

        private void Start()
        {
            _continueBtn.onClick.AddListener(HideMenuPanel);
            _restartBtn.onClick.AddListener(SceneController.Restart);
            _menuBtn.onClick.AddListener(SceneController.GoToMainMenu);

            _canvas.enabled = false;
            _popupPanel.SetActive(false);
            _menuPanel.SetActive(false);
            _animationPanel.SetActive(false);

            var sceneIndex = SceneManager.GetActiveScene().buildIndex;
            _dialogueAtMenu = scenePopup.dialogueAtMenu;

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

        public static void ShowMenuPanel(bool displayContinue)
        {
            if (displayContinue)
            {
                DisplayMenuPanel(true);
            }
            else
            {
                DisplayAnimation();
                _instance.StartCoroutine(_instance.DisplayMenuWithDelay());
            }
        }

        private IEnumerator DisplayMenuWithDelay()
        {
            yield return new WaitForSeconds(3f);
            _animationPanel.SetActive(false);
            DisplayMenuPanel(false);
        }


        private static void DisplayAnimation()
        {
            _instance._canvas.enabled = true;
            _instance._animationPanel.SetActive(true);
        }

        private static void DisplayMenuPanel(bool displayContinue)
        {
            _instance._continueBtn.gameObject.SetActive(displayContinue);

            _instance._isMenuOpen = true;
            _instance._canvas.enabled = true;
            _instance._menuPanel.SetActive(_instance._isMenuOpen);

            var popup = _instance._dialogueAtMenu[Random.Range(0, _instance._dialogueAtMenu.Count)];
            _instance.ShowPopup(popup);
        }

        private static void HideMenuPanel()
        {
            _instance._isMenuOpen = false;
            _instance._canvas.enabled = false;
            _instance._menuPanel.SetActive(_instance._isMenuOpen);

            HidePopupPanel();
        }

        private void Update()
        {
            var dialogueList = _displayPopup == DisplayPopup.Start ? _dialogueAtStart : _dialogueAtEnd;

            if (!_isPopupOpen && dialogueList.Count > 0)
            {
                ShowPopup(dialogueList[0]);
                dialogueList.RemoveAt(0);
            } else if (_isPopupOpen && !_isMenuOpen && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)))
            {
                if (dialogueList.Count > 0)
                {
                    ShowPopup(dialogueList[0]);
                    dialogueList.RemoveAt(0);
                }
                else
                {
                    HidePopupPanel();
                }
            } else if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!_isPopupOpen && !_isMenuOpen)
                {
                    ShowMenuPanel(true);
                }
            }

            if (!_isPopupOpen && _displayPopup == DisplayPopup.End && _dialogueAtEnd.Count == 0)
            {
                SceneController.NextLevel();
            }
        }

        private static void HidePopupPanel()
        {
            _instance._isPopupOpen = false;
            _instance._canvas.enabled = false;
            _instance._popupPanel.SetActive(_instance._isPopupOpen);
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
}