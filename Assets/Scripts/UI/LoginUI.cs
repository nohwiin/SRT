using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginUI : MonoBehaviour
{
    public ToggleGroup loginTypes;
    public TMP_InputField usernameInputField;
    public TMP_InputField passwordInputField;
    public Toggle showPassword;

    private static readonly Color LabelDefaultColor = new(0.572549f, 0.572549f, 0.572549f, 1f);
    private static readonly Color UnderLineDefaultColor = new(0.7882353f, 0.7882353f, 0.7882353f, 1f);
    private static readonly Color PrimaryColor = new(0.4823529f, 0.1137255f, 0.4f, 1f);

    private void Start()
    {
        RegisterToggleEvents();
        SetPasswordFieldContentType();
        InitializeUI();
    }

    private void OnDestroy()
    {
        UnregisterToggleEvents();
    }

    private void RegisterToggleEvents()
    {
        foreach (Toggle toggle in loginTypes.GetComponentsInChildren<Toggle>())
        {
            toggle.onValueChanged.AddListener((isOn) => OnLoginTypesToggleValueChanged(toggle));
        }
        showPassword.onValueChanged.AddListener(OnShowPasswordToggleValueChanged);
    }

    private void UnregisterToggleEvents()
    {
        foreach (Toggle toggle in loginTypes.GetComponentsInChildren<Toggle>())
        {
            toggle.onValueChanged.RemoveListener((isOn) => OnLoginTypesToggleValueChanged(toggle));
        }
        showPassword.onValueChanged.RemoveListener(OnShowPasswordToggleValueChanged);
    }

    private void InitializeUI()
    {
        // 초기 상태 설정 (예: 첫 번째 토글 선택, Placeholder 텍스트 설정 등)
        foreach (Toggle toggle in loginTypes.GetComponentsInChildren<Toggle>())
        {
            if (toggle.isOn)
            {
                SetInputFieldPlaceholders(toggle.GetComponentInChildren<TextMeshProUGUI>().text);
                SetToggleColors(toggle.GetComponentInChildren<Image>(), toggle.GetComponentInChildren<TextMeshProUGUI>(), PrimaryColor);
            }
            else
            {
                SetToggleColors(toggle.GetComponentInChildren<Image>(), toggle.GetComponentInChildren<TextMeshProUGUI>(), UnderLineDefaultColor, LabelDefaultColor);
            }
        }
    }

    private void OnLoginTypesToggleValueChanged(Toggle changedToggle)
    {
        foreach (Toggle toggle in loginTypes.GetComponentsInChildren<Toggle>())
        {
            Image toggleUnderLine = toggle.GetComponentInChildren<Image>();
            TextMeshProUGUI toggleLabel = toggle.GetComponentInChildren<TextMeshProUGUI>();

            if (toggle == changedToggle && toggle.isOn)
            {
                // Toggle이 선택되었을 때 색 변경
                SetToggleColors(toggleUnderLine, toggleLabel, PrimaryColor);
                // Toggle이 선택되었을 때 Placeholder 텍스트 변경
                SetInputFieldPlaceholders(toggleLabel.text);
            }
            else
            {
                // Toggle이 선택되지 않았을 때 색 초기화
                SetToggleColors(toggleUnderLine, toggleLabel, UnderLineDefaultColor, LabelDefaultColor);
            }
        }
    }

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

    private void SetInputFieldPlaceholders(string toggleText)
    {
        if (usernameInputField != null && usernameInputField.placeholder is TextMeshProUGUI usernamePlaceholder)
        {
            usernamePlaceholder.text = $"{toggleText}를 입력하세요";
        }

        if (passwordInputField != null && passwordInputField.placeholder is TextMeshProUGUI passwordPlaceholder)
        {
            passwordPlaceholder.text = $"비밀번호를 입력하세요";
        }
    }

    private void OnShowPasswordToggleValueChanged(bool isOn)
    {
        Text toggleLabel = showPassword.GetComponentInChildren<Text>();

        if (isOn)
        {
            // Toggle이 선택되었을 때 텍스트 변경
            toggleLabel.text = "숨기기";
            passwordInputField.contentType = TMP_InputField.ContentType.Standard;
        }
        else
        {
            // Toggle이 선택되지 않았을 때 텍스트 초기화
            toggleLabel.text = "비밀번호 표시";
            passwordInputField.contentType = TMP_InputField.ContentType.Password;
        }
        passwordInputField.ForceLabelUpdate();
    }

    private void SetPasswordFieldContentType()
    {
        passwordInputField.contentType = TMP_InputField.ContentType.Password;
        passwordInputField.ForceLabelUpdate();
    }
}
