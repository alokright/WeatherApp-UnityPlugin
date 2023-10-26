public interface IPlatformBridge
{
    void ShowToast(string message);
    void ShowTemperatureToast();
    void FetchWeeklyTemperature();
    void FetchCurrentTemperature();
}
