// PROJECT: Kerbal Economy
// AUTHOR:  CYBUTEK
// LICENSE: Attribution-NonCommercial-ShareAlike 3.0 Unported

using UnityEngine;

namespace KerbalEconomy.Helpers
{
    public class WindowHelper
    {
        private static int currentWindowID = 6000;
        /// <summary>
        /// Get an unused window ID.
        /// </summary>
        public static int GetWindowID()
        {
            currentWindowID++;
            return currentWindowID - 1;
        }

        /// <summary>
        /// Clamps a rectangle inside the screen region.
        /// </summary>
        public static Rect ClampInsideScreen(Rect windowPosition)
        {
            if (windowPosition.x < 0f) windowPosition.x = 0f;
            if (windowPosition.x + windowPosition.width > Screen.width) windowPosition.x = Screen.width - windowPosition.width;
            if (windowPosition.y < 0f) windowPosition.y = 0f;
            if (windowPosition.y + windowPosition.height > Screen.height) windowPosition.y = Screen.height - windowPosition.height;

            return windowPosition;
        }

        /// <summary>
        /// Clamps a rectangle into the screen region by the specified margin.
        /// </summary>
        public static Rect ClampToScreen(Rect windowPosition, float margin = 25f)
        {
            if (windowPosition.x + windowPosition.width < margin) windowPosition.x = margin - windowPosition.width;
            if (windowPosition.x > Screen.width - margin) windowPosition.x = Screen.width - margin;
            if (windowPosition.y + windowPosition.height < margin) windowPosition.y = margin - windowPosition.height;
            if (windowPosition.y > Screen.height - margin) windowPosition.y = Screen.height - margin;

            return windowPosition;
        }
    }
}
