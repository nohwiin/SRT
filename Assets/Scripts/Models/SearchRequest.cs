using System;

[Serializable]
public class SearchRequest
{
    public string DepartureStationCode;
    public string DestinationStationCode;
    public string DepartureDate;
    public string DepartureTime;

    public SearchRequest(string departureStationCode, string destinationStationCode, string departureDate, string departureTime)
    {
        DepartureStationCode = departureStationCode;
        DestinationStationCode = destinationStationCode;
        DepartureDate = departureDate;
        DepartureTime = departureTime;
    }
}
