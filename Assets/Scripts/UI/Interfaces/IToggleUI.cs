public interface IToggleUI
{
    /// <summary>
    /// 모든 Toggle과 showPasswordToggle의 onValueChanged 이벤트 리스너를 등록합니다.
    /// </summary>
    public void RegisterToggleEvents();

    /// <summary>
    /// 모든 Toggle과 showPasswordToggle의 onValueChanged 이벤트 리스너를 제거합니다.
    /// </summary>
    public void UnregisterToggleEvents();
}
