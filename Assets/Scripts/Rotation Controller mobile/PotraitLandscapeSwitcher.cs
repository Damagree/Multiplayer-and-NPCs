using UnityEngine;

public class PotraitLandscapeSwitcher : MonoBehaviour
{
#if UNITY_ANDROID
    public void LandscapeMode(string leftOrRight = "LEFT") {
        if (leftOrRight.ToLower() == "left") {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
        } else if (leftOrRight.ToLower() == "right") {
            Screen.orientation = ScreenOrientation.LandscapeRight;
        }
    }

    public void PotraitMode() {
        Screen.orientation = ScreenOrientation.Portrait;
    }

#endif
}
