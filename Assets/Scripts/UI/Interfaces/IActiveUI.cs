public interface IActiveUI
{
    /// <summary>
    /// UI 요소(입력 필드, 토글, 버튼)를 활성화 또는 비활성화합니다.
    /// </summary>
    /// <param name="isActive">true면 활성화, false면 비활성화합니다.</param>
    public void SetActive(bool isActive);
}
