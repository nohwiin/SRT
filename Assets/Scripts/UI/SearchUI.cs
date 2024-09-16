using TMPro;
using UnityEngine;

public class SearchUI : MonoBehaviour, IActiveUI
{
    [SerializeField]
    private TMP_InputField yearInputField;

    [SerializeField]
    private TMP_InputField monthInputField;

    [SerializeField]
    private TMP_InputField dayInputField;

    [SerializeField]
    private TMP_InputField hourInputField;

    private void Awake()
    {
        
    }

    private void Start()
    {
        // 현재 시간 가져오기
        System.DateTime now = System.DateTime.Now;

        // 각각의 InputField에 현재 시간 채우기
        yearInputField.text = now.Year.ToString();
        monthInputField.text = now.Month.ToString("00");  // 2자리 형식
        dayInputField.text = now.Day.ToString("00");
        hourInputField.text = now.Hour.ToString("00");
    }

    public void SetActive(bool isActive)
    {
        
    }
}
