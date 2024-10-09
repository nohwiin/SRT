public interface IButtonUI
{
    /// <summary>
    /// 모든 Button의 onClick 이벤트 리스너를 등록합니다.
    /// </summary>
    public void RegisterButtonEvents();

    /// <summary>
    /// 모든 Button의 onClick 이벤트 리스너를 제거합니다.
    /// </summary>
    public void UnregisterButtonEvents();
}
