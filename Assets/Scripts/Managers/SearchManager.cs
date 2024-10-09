using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class SearchManager : MonoBehaviour
{
    public static SearchManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 열차를 검색합니다.
    /// </summary>
    /// <param name="searchRequest">검색 객체</param>
    /// <param name="listener">검색 결과를 수신할 델리게이트</param>
    public void Search(SearchRequest searchRequest, ResultDelegate<bool, List<TrainInfo>> listener)
    {
        StartCoroutine(SearchAsync(searchRequest, listener));
    }

    /// <summary>
    /// 비동기 검색 프로세스
    /// </summary>
    /// <param name="searchRequest">검색 객체</param>
    /// <param name="listener">검색 결과를 수신할 델리게이트</param>
    /// <returns>코루틴</returns>
    private IEnumerator SearchAsync(SearchRequest searchRequest, ResultDelegate<bool, List<TrainInfo>> listener)
    {
        var searchRequestBody = new Dictionary<string, string>
        {
            { "chtnDvCd", "1" }, // 직통
            { "arriveTime", "N" },
            { "seatAttCd", "015" },
            { "psgNum", "1" },
            { "trnGpCd", "109" },
            { "stlbTrnClsfCd", "05" }, // 전체 열차 검색 (SRT 포함)
            { "dptDt", searchRequest.DepartureDate }, // 출발 날짜
            { "dptTm", searchRequest.DepartureTime }, // 출발 시각
            { "arvRsStnCd", searchRequest.DestinationStationCode }, // 도착역 코드
            { "dptRsStnCd", searchRequest.DepartureStationCode }  // 출발역 코드
        };

        using UnityWebRequest request = UnityWebRequest.Post(Constants.API_ENDPOINTS_SEARCH_SCHEDULE, searchRequestBody);
        request.SetRequestHeader("User-Agent", Constants.DEFAULT_HEADERS_USER_AGENT);
        request.SetRequestHeader("Accept", Constants.DEFAULT_HEADERS_ACCEPT);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            listener.Invoke(false, default);
        }
        else
        {
            var responseText = request.downloadHandler.text;
            var searchResult = JsonUtility.FromJson<SearchResponse>(responseText);

            var trains = searchResult.outDataSets.dsOutput1;
            var srtTrains = trains.Where(t => t.stlbTrnClsfCd == "17").ToList();

            listener.Invoke(true, srtTrains);
        }
    }
}
