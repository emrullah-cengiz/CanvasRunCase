using System.Collections;
using UnityEngine;

namespace Assets._Game.Scripts
{
    public static class UtilityHelper
    {
        public static bool IsEditorPlatform() => Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor;

        public static float ClampAngle(float angle, float from, float to)
        {
            if (angle < 0f)
                angle = 360 + angle;

            if (angle > 180f)
                return Mathf.Max(angle, 360 + from);

            return Mathf.Min(angle, to);
        }
    }
}