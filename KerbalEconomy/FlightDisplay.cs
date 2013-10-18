// PROJECT: Kerbal Economy
// AUTHOR:  CYBUTEK
// LICENSE: Attribution-NonCommercial-ShareAlike 3.0 Unported

using UnityEngine;

namespace KerbalEconomy
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class FlightDisplay : MonoBehaviour
    {
        #region Fields

        #endregion

        #region Initialisaion

        private void Start()
        {
            if (HighLogic.CurrentGame.Mode == Game.Modes.CAREER)
                RenderingManager.AddToPostDrawQueue(0, this.Draw);
        }

        #endregion

        #region Update and Drawing

        private void Draw()
        {
            if (GUI.Button(new Rect(100f, 100f, 100f, 30f), "KERBAL ECONOMY"))
            {
                //GameEvents.OnVesselRecoveryRequested.Add();
            }
        }

        #endregion
    }
}
