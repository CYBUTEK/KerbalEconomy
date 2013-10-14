using UnityEngine;

namespace KerbalEconomy
{
    [KSPAddon(KSPAddon.Startup.EveryScene, false)]
    public class LedgerLoader : MonoBehaviour
    {
        #region Fields

        private GameScenes currentScene;

        private Rect buttonPosition = new Rect(Screen.width - 250f - 50f, Screen.height - 35f, 250f, 33f);
        private GUIStyle buttonStyle;
        private bool hasInitStyles = false;
        private bool showLedger = false;

        #endregion

        #region Initialisation

        // Runs when the object is created.
        private void Start()
        {
            this.currentScene = HighLogic.LoadedScene;

            if (this.currentScene == GameScenes.MAINMENU && KerbalEconomy.Instance.Ledger != null)
                KerbalEconomy.Instance.Ledger = null;
            else if (this.currentScene == GameScenes.SPACECENTER)
                RenderingManager.AddToPostDrawQueue(0, this.DrawSpaceCentreButton);
            else
                RenderingManager.AddToPostDrawQueue(0, LedgerDisplay.Instance.Draw);
        }

        // Initialises the styles upon request.
        private void InitialiseStyles()
        {
            this.hasInitStyles = true;

            this.buttonStyle = new GUIStyle(HighLogic.Skin.button);
            this.buttonStyle.normal.textColor = Color.white;
        }

        #endregion

        #region Drawing

        private void DrawSpaceCentreButton()
        {
            if (!this.hasInitStyles) this.InitialiseStyles();

            LedgerDisplay.Instance.Visible = GUI.Toggle(this.buttonPosition, LedgerDisplay.Instance.Visible, "Kerbal Economy Ledger", this.buttonStyle);
            LedgerDisplay.Instance.Draw();
        }

        #endregion

        // Runs when the object is being destroyed.
        private void OnDestroy()
        {
            if (this.currentScene == GameScenes.MAINMENU && HighLogic.LoadedScene == GameScenes.SPACECENTER)
                KerbalEconomy.Instance.Ledger = new Ledger.Book(HighLogic.SaveFolder + ".txt");

            if (KerbalEconomy.Instance.Ledger != null)
            {
                LedgerDisplay.Instance.Visible = false;
                KerbalEconomy.Instance.Ledger.Save();
            }
        }
    }
}
