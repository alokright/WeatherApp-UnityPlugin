using UnityEngine;

public class EditorBridge : IPlatformBridge
{
    // Stub methods for Unity Editor...
    public void ShowToast(string message)
    {
        Debug.Log($"[Editor Toast]: {message}");
    }

    public void ShowTemperatureToast()
    {
        Debug.Log("[Editor Toast]: Showing temperature");
    }

    public void FetchWeeklyTemperature()
    {
        // Editor stub...
    }

    public void FetchCurrentTemperature()
    {
        // Editor stub...
    }
}
