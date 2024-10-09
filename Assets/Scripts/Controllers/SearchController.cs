using System;
using System.Collections.Generic;
using UnityEngine;

public class SearchController : MonoBehaviour
{
    private SearchManager searchManager;

    // 역 코드 매핑을 위한 사전 (드롭박스 인덱스 1부터 시작)
    private Dictionary<int, string> stationCodeMap = new()
    {
        { 0, "0551" },  // 수서
        { 1, "0552" },  // 동탄
        { 2, "0015" },  // 동대구
        { 3, "0506" }   // 서대구
    };

    private void Awake()
    {
        searchManager = ManagerFactory.GetSearchManager();
    }

    public void Search(int departureStation, int destinationStation, string year, string month, string day, string hour)
    {
        var validDate = ValidateDate(year, month, day, hour);

        var departureDate = validDate.ToString("yyyyMMdd");
        var departureTime = validDate.ToString("HH");
        var departureStationCode = stationCodeMap[departureStation];
        var destinationStationCode = stationCodeMap[destinationStation];

        var searchRequest = new SearchRequest(departureStationCode, destinationStationCode, departureDate, departureTime);

        searchManager.Search(searchRequest, (result, response) =>
        {
            if (result)
            {
                Debug.Log("Search successful");
                
            }
            else
            {
                Debug.LogError("Search failed");
            }
        });
    }

    private DateTime ValidateDate(string year, string month, string day, string hour)
    {
        var now = DateTime.Now;

        if (!int.TryParse(year, out int inputYear) || 
            !int.TryParse(month, out int inputMonth) || 
            !int.TryParse(day, out int inputDay) ||
            !int.TryParse(hour, out int inputHour))
        {
            return now;
        }

        try
        {
            var inputDateTime = new DateTime(inputYear, inputMonth, inputDay, inputHour, 0, 0);

            if (inputDateTime < now)
            {
                return now;
            }

            return inputDateTime;
        }
        catch
        {
            return now;
        }
    }
}
