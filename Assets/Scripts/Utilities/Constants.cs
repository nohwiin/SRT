using System.Text.RegularExpressions;

public static class Constants
{
    public static readonly Regex EMAIL_REGEX = new Regex(@"[^@]+@[^@]+\.[^@]+");
    public static readonly Regex PHONE_NUMBER_REGEX = new Regex(@"(\d{3})-(\d{3,4})-(\d{4})");

    public static readonly string DEFAULT_USER_AGENT = 
        "Mozilla/5.0 (Linux; Android 5.1.1; LGM-V300K Build/N2G47H) AppleWebKit/537.36 " +
        "(KHTML, like Gecko) Version/4.0 Chrome/39.0.0.0 Mobile Safari/537.36SRT-APP-Android V.1.0.6";

    public static readonly string SRT_MOBILE = "https://app.srail.or.kr:443";
    
    public static readonly string API_ENDPOINTS_MAIN = $"{SRT_MOBILE}/main/main.do";
    public static readonly string API_ENDPOINTS_LOGIN = $"{SRT_MOBILE}/apb/selectListApb01080_n.do";
    public static readonly string API_ENDPOINTS_LOGOUT = $"{SRT_MOBILE}/login/loginOut.do";
    public static readonly string API_ENDPOINTS_SEARCH_SCHEDULE = $"{SRT_MOBILE}/ara/selectListAra10007_n.do";
    public static readonly string API_ENDPOINTS_RESERVE = $"{SRT_MOBILE}/arc/selectListArc05013_n.do";
    public static readonly string API_ENDPOINTS_TICKETS = $"{SRT_MOBILE}/atc/selectListAtc14016_n.do";
    public static readonly string API_ENDPOINTS_TICKET_INFO = $"{SRT_MOBILE}/ard/selectListArd02017_n.do?";
    public static readonly string API_ENDPOINTS_CANCEL = $"{SRT_MOBILE}/ard/selectListArd02045_n.do";
    public static readonly string API_ENDPOINTS_STANDBY_OPTION = $"{SRT_MOBILE}/ata/selectListAta01135_n.do";
    public static readonly string API_ENDPOINTS_PAYMENT = $"{SRT_MOBILE}/ata/selectListAta09036_n.do";

    public static readonly string LOGIN_TYPES_MEMBERSHIP_ID = "1";
    public static readonly string LOGIN_TYPES_EMAIL = "2";
    public static readonly string LOGIN_TYPES_PHONE_NUMBER = "3";

    public static readonly string HEADER_USER_AGENT = "User-Agent";
    public static readonly string HEADER_ACCEPT = "application/json";
}
