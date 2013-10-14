using KerbalEconomy.Ledger;
using UnityEngine;

namespace KerbalEconomy
{
    [KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
    public class LedgerDisplay : MonoBehaviour
    {
        #region Constants

        public const float WINDOW_WIDTH = 700f;
        public const float WINDOW_HEIGHT = 200f;

        #endregion

        #region Fields

        private Rect buttonPosition = new Rect(Screen.width - 250f, 0f, 250f, 30f);
        private Rect windowPosition = new Rect(Screen.width / 2f - WINDOW_WIDTH / 2f, Screen.height / 2f - WINDOW_HEIGHT / 2f, WINDOW_WIDTH, WINDOW_HEIGHT);
        private int windowID = Random.Range(100, int.MaxValue);

        private GUIStyle windowStyle, buttonStyle, scrollStyle, labelTitleStyle, labelNormalStyle;
        private bool hasInitStyles = false;

        private bool showDisplay = false;
        private Vector2 scrollPosition = Vector2.zero;

        #endregion

        #region Initialisation

        // Runs when the object is created.
        private void Start()
        {
            RenderingManager.AddToPostDrawQueue(0, this.Draw);
        }

        // Initialises the styles upon request.
        private void InitialiseStyles()
        {
            this.hasInitStyles = true;

            this.windowStyle = new GUIStyle(HighLogic.Skin.window);

            this.buttonStyle = new GUIStyle(HighLogic.Skin.button);
            this.buttonStyle.normal.textColor = Color.white;

            this.scrollStyle = new GUIStyle(HighLogic.Skin.scrollView);

            this.labelTitleStyle = new GUIStyle(HighLogic.Skin.label);
            this.labelTitleStyle.normal.textColor = Color.white;
            this.labelTitleStyle.fontStyle = FontStyle.Bold;
            this.labelTitleStyle.stretchWidth = true;

            this.labelNormalStyle = new GUIStyle(HighLogic.Skin.label);
            this.labelNormalStyle.fontStyle = FontStyle.Bold;
            this.labelNormalStyle.stretchWidth = true;
        }

        #endregion

        #region Drawing

        // Runs when KSP calls the draw queue.
        private void Draw()
        {
            if (!this.hasInitStyles) this.InitialiseStyles();

            this.showDisplay = GUI.Toggle(this.buttonPosition, this.showDisplay, "Kerbal Economy - Show Ledger", this.buttonStyle);

            if (this.showDisplay)
                this.windowPosition = GUILayout.Window(this.windowID, this.windowPosition, this.Window, "Kerbal Economy Ledger", this.windowStyle);
        }

        // Runs when the display is being shown.
        private void Window(int windowID)
        {
            this.scrollPosition = GUILayout.BeginScrollView(this.scrollPosition, this.scrollStyle);
            GUILayout.BeginHorizontal();

            this.DrawTime();
            this.DrawTransaction();
            this.DrawDebit();
            this.DrawCredit();

            // NOTE: Not displaying balance as it will most likely be out of synch.
            //this.DrawBalance();

            GUILayout.EndHorizontal();
            GUILayout.EndScrollView();

            GUI.DragWindow();
        }

        private void DrawTime()
        {
            GUILayout.BeginVertical();

            GUILayout.Label("TIME", this.labelTitleStyle);
            foreach (Row row in KerbalEconomy.Instance.Ledger.Rows)
                GUILayout.Label(row.UniversalTime.ToString("0."), this.labelNormalStyle);

            GUILayout.EndVertical();
        }

        private void DrawTransaction()
        {
            GUILayout.BeginVertical();

            GUILayout.Label("TRANSACTION", this.labelTitleStyle);
            foreach (Row row in KerbalEconomy.Instance.Ledger.Rows)
                GUILayout.Label(row.Transaction, this.labelNormalStyle);

            GUILayout.EndVertical();
        }

        private void DrawDebit()
        {
            GUILayout.BeginVertical();

            GUILayout.Label("PAID OUT", this.labelTitleStyle);
            foreach (Row row in KerbalEconomy.Instance.Ledger.Rows)
                GUILayout.Label(KerbalEconomy.ToMonies(row.Debit).ToString("#,0."), this.labelNormalStyle);

            GUILayout.EndVertical();
        }

        private void DrawCredit()
        {
            GUILayout.BeginVertical();

            GUILayout.Label("PAID IN", this.labelTitleStyle);
            foreach (Row row in KerbalEconomy.Instance.Ledger.Rows)
                GUILayout.Label(KerbalEconomy.ToMonies(row.Credit).ToString("#,0."), this.labelNormalStyle);

            GUILayout.EndVertical();
        }

        private void DrawBalance()
        {
            GUILayout.BeginVertical();

            GUILayout.Label("BALANCE", this.labelTitleStyle);
            foreach (Row row in KerbalEconomy.Instance.Ledger.Rows)
                GUILayout.Label(KerbalEconomy.ToMonies(row.Balance).ToString("#,0."), this.labelNormalStyle);
            
            GUILayout.EndVertical();
        }

        #endregion
    }
}
