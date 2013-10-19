// PROJECT: Kerbal Economy
// AUTHOR:  CYBUTEK
// LICENSE: Attribution-NonCommercial-ShareAlike 3.0 Unported

using KerbalEconomy.Extensions;
using UnityEngine;

namespace KerbalEconomy
{
    [KSPAddon(KSPAddon.Startup.EveryScene, false)]
    public class LedgerLoader : MonoBehaviour
    {
        #region Fields

        private GameScenes currentScene = HighLogic.LoadedScene;

        private Rect buttonPosition = new Rect(Screen.width - 250f - 50f, Screen.height - 35f, 250f, 33f);
        private GUIStyle buttonStyle;
        private bool hasInitStyles = false;
        private bool hasInitScience = false;
        private bool showLedger = false;

        private float science = 0f;

        #endregion

        #region Initialisation

        // Runs when the object is created.
        private void Start()
        {
            if (Recovery.Instance == null)
                print("[KerbalEconomy]: Recovery system could not be started.");

            if (this.currentScene == GameScenes.MAINMENU && KerbalEconomy.Instance.Ledger != null)
                KerbalEconomy.Instance.Ledger = null;
            else if (this.currentScene == GameScenes.SPACECENTER)
            {
                if (HighLogic.CurrentGame != null && HighLogic.CurrentGame.Mode == Game.Modes.CAREER)
                {
                    RenderingManager.AddToPostDrawQueue(0, this.DrawSpaceCentreButton);
                }
            }
        }

        // Initialises the styles upon request.
        private void InitialiseStyles()
        {
            this.hasInitStyles = true;

            this.buttonStyle = new GUIStyle(HighLogic.Skin.button);
            this.buttonStyle.normal.textColor = Color.white;
        }

        #endregion

        #region Update and Drawing

        // Runs continuously whilst the object is alive.
        private void Update()
        {
            if (HighLogic.CurrentGame != null && HighLogic.CurrentGame.Mode == Game.Modes.CAREER)
            {
                if (KerbalEconomy.Instance.ScienceIsNotNull)
                {
                    // Initialise the economy on scene change.
                    if (!this.hasInitScience)
                    {
                        this.hasInitScience = true;
                        KerbalEconomy.Instance.StorageMode = false;
                        this.science = KerbalEconomy.Instance.Science;
                    }

                    if (this.currentScene == GameScenes.SPACECENTER)
                    {
                        // Science lost through research and development.
                        if (this.science > KerbalEconomy.Instance.Science)
                        {
                            KerbalEconomy.Instance.Debit("Research & Development", this.science - KerbalEconomy.Instance.Science, false);
                            this.science = KerbalEconomy.Instance.Science;
                        }
                        else if (Recovery.Instance.Recovered)
                        {
                            // Science recovered from vessel.
                            float recoverdScience = Mathf.Round((KerbalEconomy.Instance.Science - (this.science + Recovery.Instance.RecoveryScience)) * 100f) / 100f;

                            if (recoverdScience > 0f)
                            {
                                KerbalEconomy.Instance.Credit("Recovered Science", recoverdScience, false);
                                Recovery.Instance.Recovered = false;
                                this.science = KerbalEconomy.Instance.Science;
                            }
                        }
                    }
                    else if (this.currentScene == GameScenes.TRACKSTATION)
                    {
                        if (Recovery.Instance.Recovered) 
                        {
                            // Science recovered from vessel.
                            float recoveredScience = Mathf.Round((Recovery.Instance.RecoveryScience - this.science) * 100f) / 100f;

                            if (recoveredScience > 0f)
                            {
                                KerbalEconomy.Instance.Credit("Recovered Science", recoveredScience, false);
                                Recovery.Instance.Recovered = false;
                                this.science = KerbalEconomy.Instance.Science;
                            }
                        }

                    }
                    else if (this.currentScene == GameScenes.EDITOR)
                    {
                        if (Recovery.Instance.Reverted)
                        {
                            Recovery.Instance.Reverted = false;
                        }
                    }
                }
            }
        }

        // Runs when in the space centre scene.
        private void DrawSpaceCentreButton()
        {
            if (!this.hasInitStyles) this.InitialiseStyles();

            this.showLedger = GUI.Toggle(this.buttonPosition, this.showLedger, "Kerbal Economy Ledger", this.buttonStyle);
            if (this.showLedger)
                LedgerDisplay.Instance.Draw();
        }

        #endregion

        // Runs when the object is being destroyed.
        private void OnDestroy()
        {
            if (HighLogic.CurrentGame != null && HighLogic.CurrentGame.Mode == Game.Modes.CAREER)
            {
                if (this.currentScene == GameScenes.MAINMENU && HighLogic.LoadedScene == GameScenes.SPACECENTER)
                    KerbalEconomy.Instance.Ledger = new Ledger.Book(HighLogic.SaveFolder + ".csv");

                // Science gained through missions.
                if (this.currentScene == GameScenes.FLIGHT)
                {
                    if (KerbalEconomy.Instance.ScienceIsNotNull)
                    {
                        if (HighLogic.LoadedScene != GameScenes.FLIGHT)
                        {
                            Recovery.Instance.FlightStarted = false;

                            if (this.science < KerbalEconomy.Instance.Science)
                                KerbalEconomy.Instance.Credit("Transmitted Science", KerbalEconomy.Instance.Science - Recovery.Instance.FlightStartScience, false);
                        }

                        if (HighLogic.LoadedScene == GameScenes.EDITOR)
                        {
                            KerbalEconomy.Instance.StorageMode = true;
                            KerbalEconomy.Instance.Credit("==== Reverted Mission ====", 0f, false);
                        }
                    }          
                }

                if (KerbalEconomy.Instance.Ledger != null)
                    KerbalEconomy.Instance.Ledger.Save();

                Settings.Instance.Save();
            }
        }
    }
}
