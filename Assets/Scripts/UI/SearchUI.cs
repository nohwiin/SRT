using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SearchUI : MonoBehaviour, IActiveUI, ICommonUI, IButtonUI
{
    [SerializeField]
    private AdvancedDropdown departureStation;

    [SerializeField]
    private AdvancedDropdown destinationStation;

    [SerializeField]
    private TMP_InputField yearInputField;

    [SerializeField]
    private TMP_InputField monthInputField;

    [SerializeField]
    private TMP_InputField dayInputField;

    [SerializeField]
    private TMP_InputField hourInputField;

    [SerializeField]
    private Button searchButton;

    private SearchController searchController;

    private void Awake()
    {
        searchController = FindObjectOfType<SearchController>();
    }

    private void Start()
    {
        InitializeUI();
        RegisterButtonEvents();
    }

#region IActiveUI
    public void SetActive(bool isActive)
    {
        
    }
#endregion

#region ICommonUI
    public void InitializeUI()
    {
        var now = System.DateTime.Now;

        yearInputField.text = now.Year.ToString();
        monthInputField.text = now.Month.ToString("00");
        dayInputField.text = now.Day.ToString("00");
        hourInputField.text = now.Hour.ToString("00");
    }
#endregion

#region IButtonUI
    public void RegisterButtonEvents()
    {
        searchButton.onClick.AddListener(OnSearchButtonClicked);
    }

    public void UnregisterButtonEvents()
    {
        searchButton.onClick.RemoveListener(OnSearchButtonClicked);
    }

    /// <summary>
    /// 검색 버튼 클릭 이벤트를 처리합니다.
    /// </summary>
    private void OnSearchButtonClicked()
    {
        searchController.Search(departureStation.value, destinationStation.value,
            yearInputField.text, monthInputField.text, dayInputField.text, hourInputField.text);
    }
#endregion
}
