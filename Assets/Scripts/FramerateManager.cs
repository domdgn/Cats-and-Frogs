using UnityEngine;

public class FramerateManager : MonoBehaviour
{
    void Awake()
    {
        // Enable VSync (1 = match monitor's refresh rate)
        QualitySettings.vSyncCount = 1;

        capFramerate((int)Screen.currentResolution.refreshRateRatio.value);
    }

    public void capFramerate (int fps)
    {
        Application.targetFrameRate = fps;
    }
}
