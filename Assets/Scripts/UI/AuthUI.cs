using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AuthUI : MonoBehaviour, IActiveUI, ICommonUI, IButtonUI, IToggleUI
{
    [SerializeField]
    private ToggleGroup signInTypeToggleGroup;

    [SerializeField]
    private TMP_InputField usernameInputField;
    [SerializeField]
    private TMP_InputField passwordInputField;
    [SerializeField]
    private Toggle showPasswordToggle;

    [SerializeField]
    private Toggle rememberIdToggle;
    [SerializeField]
    private Toggle autoSignInToggle;

    [SerializeField]
    private Button signInButton;

    private static readonly Color LabelDefaultColor = new(0.572549f, 0.572549f, 0.572549f, 1f);
    private static readonly Color UnderLineDefaultColor = new(0.7882353f, 0.7882353f, 0.7882353f, 1f);
    private static readonly Color PrimaryColor = new(0.4823529f, 0.1137255f, 0.4f, 1f);

    private AuthManager authManager;
    private AuthController authController;

    private void Awake()
    {
        authManager = ManagerFactory.GetAuthManager();
        authController = FindObjectOfType<AuthController>();
    }

    private void Start()
    {
        RegisterToggleEvents();
        RegisterButtonEvents();
        SetPasswordFieldContentType();
        InitializeUI();
    }

    private void OnDestroy()
    {
        UnregisterToggleEvents();
        UnregisterButtonEvents();
    }

#region  IActiveUI
    public void SetActive(bool isActive)
    {
        usernameInputField.interactable = isActive;
        passwordInputField.interactable = isActive;

        foreach (Toggle toggle in signInTypeToggleGroup.GetComponentsInChildren<Toggle>())
        {
            toggle.interactable = isActive;
        }
        showPasswordToggle.interactable = isActive;
        rememberIdToggle.interactable = isActive;
        autoSignInToggle.interactable = isActive;

        signInButton.interactable = isActive;
    }
#endregion

#region  ICommonUI
    public void InitializeUI()
    {
        var lastSignInType = authManager.TryGetLastSignInType();
        var toggleIndex = 1;
        
        foreach (Toggle toggle in signInTypeToggleGroup.GetComponentsInChildren<Toggle>())
        {
            if (lastSignInType == toggleIndex)
            {
                var toggleText = toggle.GetComponentInChildren<TextMeshProUGUI>().text;

                toggle.isOn = true;
                SetInputFieldPlaceholders(toggleText);
                SetInputFieldTexts(toggleText);
                SetRememberIdToggleTexts(toggleText);
                SetToggleColors(toggle.GetComponentInChildren<Image>(), toggle.GetComponentInChildren<TextMeshProUGUI>(), PrimaryColor);
            }
            else
            {
                SetToggleColors(toggle.GetComponentInChildren<Image>(), toggle.GetComponentInChildren<TextMeshProUGUI>(), UnderLineDefaultColor, LabelDefaultColor);
            }

            toggleIndex += 1;
        }

        authController.AutoSignInIfNeeded();
    }
#endregion

#region  IButtonUI
    public void RegisterButtonEvents()
    {
        signInButton.onClick.AddListener(OnSignInButtonClicked);
    }

    public void UnregisterButtonEvents()
    {
        signInButton?.onClick.RemoveListener(OnSignInButtonClicked);
    }

    /// <summary>
    /// 로그인 버튼 클릭 이벤트를 처리합니다.
    /// </summary>
    private void OnSignInButtonClicked()
    {
        authController.SignIn(usernameInputField.text, passwordInputField.text, rememberIdToggle.isOn, autoSignInToggle.isOn);
    }
#endregion

#region IToggleUI
    public void RegisterToggleEvents()
    {
        foreach (Toggle toggle in signInTypeToggleGroup.GetComponentsInChildren<Toggle>())
        {
            toggle.onValueChanged.AddListener(OnSignInTypeToggleValueChanged);
        }
        showPasswordToggle.onValueChanged.AddListener(OnShowPasswordToggleValueChanged);
    }

    public void UnregisterToggleEvents()
    {
        foreach (Toggle toggle in signInTypeToggleGroup.GetComponentsInChildren<Toggle>())
        {
            toggle.onValueChanged.RemoveListener(OnSignInTypeToggleValueChanged);
        }
        showPasswordToggle.onValueChanged.RemoveListener(OnShowPasswordToggleValueChanged);
    }

    /// <summary>
    /// 선택된 Toggle의 값이 변경되었을 때 호출되는 이벤트 핸들러입니다.
    /// </summary>
    /// <param name="isOn">Toggle의 상태 (선택됨 또는 선택되지 않음)</param>
    private void OnSignInTypeToggleValueChanged(bool isOn)
    {
        foreach (Toggle toggle in signInTypeToggleGroup.GetComponentsInChildren<Toggle>())
        {
            Image toggleUnderLine = toggle.GetComponentInChildren<Image>();
            TextMeshProUGUI toggleLabel = toggle.GetComponentInChildren<TextMeshProUGUI>();

            if (toggle.isOn)
            {
                SetToggleColors(toggleUnderLine, toggleLabel, PrimaryColor);
                SetInputFieldPlaceholders(toggleLabel.text);
                SetInputFieldTexts(toggleLabel.text);
                SetRememberIdToggleTexts(toggleLabel.text);
            }
            else
            {
                SetToggleColors(toggleUnderLine, toggleLabel, UnderLineDefaultColor, LabelDefaultColor);
            }
        }
    }

    /// <summary>
    /// showPasswordToggle의 값이 변경되었을 때 호출되는 이벤트 핸들러입니다.
    /// </summary>
    /// <param name="isOn">Toggle의 상태 (선택됨 또는 선택되지 않음)</param>
    private void OnShowPasswordToggleValueChanged(bool isOn)
    {
        Text toggleLabel = showPasswordToggle.GetComponentInChildren<Text>();

        if (isOn)
        {
            toggleLabel.text = "숨기기";
            passwordInputField.contentType = TMP_InputField.ContentType.Standard;
        }
        else
        {
            toggleLabel.text = "비밀번호 표시";
            passwordInputField.contentType = TMP_InputField.ContentType.Password;
        }
        passwordInputField.ForceLabelUpdate();
    }
#endregion

    /// <summary>
    /// 비밀번호 입력 필드의 contentType을 Password로 설정하고, 이를 UI에 반영합니다.
    /// </summary>
    private void SetPasswordFieldContentType()
    {
        passwordInputField.contentType = TMP_InputField.ContentType.Password;
        passwordInputField.ForceLabelUpdate();
    }

    /// <summary>
    /// Toggle의 텍스트에 따라 usernameInputField에 저장된 ID를 설정합니다.
    /// </summary>
    /// <param name="signInTypeText">선택된 Toggle의 텍스트</param>
    private void SetInputFieldTexts(string signInTypeText)
    {
        if (usernameInputField != null)
        {
            bool isIdRemembered = authManager.TryGetRememberedId(signInTypeText, out string rememberedId);
            rememberIdToggle.isOn = isIdRemembered;
            usernameInputField.text = rememberedId;
        }
    }

    /// <summary>
    /// Toggle의 텍스트에 따라 usernameInputField의 Placeholder를 설정합니다.
    /// </summary>
    /// <param name="signInTypeText">선택된 Toggle의 텍스트</param>
    private void SetInputFieldPlaceholders(string signInTypeText)
    {
        if (usernameInputField != null && usernameInputField.placeholder is TextMeshProUGUI usernamePlaceholder)
        {
            usernamePlaceholder.text = $"{signInTypeText}를 입력하세요";
        }
    }

    /// <summary>
    /// rememberIdToggle의 텍스트를 설정합니다.
    /// </summary>
    /// <param name="signInTypeText">SignInType에 해당하는 텍스트</param>
    private void SetRememberIdToggleTexts(string signInTypeText)
    {
        var label = rememberIdToggle.GetComponentInChildren<Text>();
        if (label != null)
        {
            label.text = $"{signInTypeText}저장";
        }
    }

    /// <summary>
    /// Toggle의 언더라인 및 텍스트의 색을 설정합니다.
    /// </summary>
    /// <param name="underline">Toggle의 언더라인 이미지</param>
    /// <param name="label">Toggle의 텍스트</param>
    /// <param name="color">설정할 색상</param>
    private void SetToggleColors(Image underline, TextMeshProUGUI label, Color color)
    {
        if (underline != null)
        {
            underline.color = color;
        }

        if (label != null)
        {
            label.color = color;
        }
    }

    /// <summary>
    /// Toggle의 언더라인 및 텍스트의 색을 각각 설정합니다.
    /// </summary>
    /// <param name="underline">Toggle의 언더라인 이미지</param>
    /// <param name="label">Toggle의 텍스트</param>
    /// <param name="underlineColor">설정할 언더라인 색상</param>
    /// <param name="labelColor">설정할 텍스트 색상</param>
    private void SetToggleColors(Image underline, TextMeshProUGUI label, Color underlineColor, Color labelColor)
    {
        if (underline != null)
        {
            underline.color = underlineColor;
        }

        if (label != null)
        {
            label.color = labelColor;
        }
    }
}
