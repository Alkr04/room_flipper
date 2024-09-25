using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private static UIController _instance;

    private Canvas _canvas;
    private GameObject _menuPanel;
    private GameObject _popupPanel;

    private TextMeshProUGUI _wizardText;

    private bool _isMenuOpen;
    private bool _isPopupOpen;

    private Button _restartBtn;
    private Button _quitBtn;

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
        _restartBtn.onClick.AddListener(() => { });
        _quitBtn.onClick.AddListener(() => { });

        _canvas.enabled = false;
        _popupPanel.SetActive(false);
        _menuPanel.SetActive(false);
    }

    public void ShowPopup(string text)
    {
        _isPopupOpen = true;
        _canvas.enabled = true;
        _popupPanel.SetActive(_isPopupOpen);

        _wizardText.text = text;
    }

    public void ShowMenuPanel()
    {
        _isMenuOpen = true;
        _canvas.enabled = true;
        _menuPanel.SetActive(_isMenuOpen);
    }

    private void Update()
    {
        if (_isPopupOpen && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)))
        {
            _isPopupOpen = false;
            _canvas.enabled = false;
            _popupPanel.SetActive(_isPopupOpen);
        }
    }

    public static bool IsDisplaying()
    {
        return _instance._isPopupOpen || _instance._isMenuOpen;
    }
}