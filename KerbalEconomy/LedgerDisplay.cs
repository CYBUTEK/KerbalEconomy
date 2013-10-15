using KerbalEconomy.Helpers;
using KerbalEconomy.Ledger;
using UnityEngine;

namespace KerbalEconomy
{
    public class LedgerDisplay
    {
        #region Constants

        public const float WINDOW_WIDTH = 700f;
        public const float WINDOW_HEIGHT = 200f;

        #endregion

        #region Instance

        private static LedgerDisplay instance;
        /// <summary>
        /// Gets the current instance of the LedgerDisplay object.
        /// </summary>
        public static LedgerDisplay Instance
        {
            get
            {
                if (instance == null)
                    instance = new LedgerDisplay();

                return instance;
            }
        }

        #endregion

        #region Fields

        private Rect windowPosition = new Rect(Screen.width / 2f - WINDOW_WIDTH / 2f, Screen.height / 2f - WINDOW_HEIGHT / 2f, WINDOW_WIDTH, WINDOW_HEIGHT);
        private int windowID = WindowHelper.GetWindowID();

        private GUIStyle windowStyle, boxStyle, buttonStyle, labelTitleLeftStyle, labelTitleRightStyle, labelNormalLeftStyle, labelNormalRightStyle;
        private GUILayoutOption[] boxLayoutOptions, buttonLayoutOptions;
        private bool hasInitStyles = false;

        private Vector2 scrollPosition = Vector2.zero;

        #endregion

        #region Initialisation

        private void Start()
        {
            instance = this;
        }

        // Initialises the styles upon request.
        private void InitialiseStyles()
        {
            this.hasInitStyles = true;

            this.windowStyle = new GUIStyle(HighLogic.Skin.window);

            this.boxStyle = new GUIStyle(HighLogic.Skin.box);
            this.boxStyle.margin = new RectOffset(5, 5, 5, 5);
            this.boxStyle.padding = new RectOffset(10, 10, 5, 5);

            this.buttonStyle = new GUIStyle(HighLogic.Skin.button);
            this.buttonStyle.margin = new RectOffset(5, 5, 5, 5);
            this.buttonStyle.normal.textColor = Color.white;
            this.buttonStyle.stretchHeight = true;

            this.labelTitleLeftStyle = new GUIStyle(HighLogic.Skin.label);
            this.labelTitleLeftStyle.normal.textColor = Color.white;
            this.labelTitleLeftStyle.alignment = TextAnchor.MiddleLeft;
            this.labelTitleLeftStyle.fontStyle = FontStyle.Bold;
            this.labelTitleLeftStyle.stretchWidth = true;

            this.labelTitleRightStyle = new GUIStyle(this.labelTitleLeftStyle);
            this.labelTitleRightStyle.alignment = TextAnchor.MiddleRight;

            this.labelNormalLeftStyle = new GUIStyle(HighLogic.Skin.label);
            this.labelNormalLeftStyle.alignment = TextAnchor.MiddleLeft;
            this.labelNormalLeftStyle.fontStyle = FontStyle.Bold;
            this.labelNormalLeftStyle.stretchWidth = true;

            this.labelNormalRightStyle = new GUIStyle(this.labelNormalLeftStyle);
            this.labelNormalRightStyle.alignment = TextAnchor.MiddleRight;

            this.boxLayoutOptions = new GUILayoutOption[]
            {
                GUILayout.Width(175f),
                GUILayout.Height(30f)
            };

            this.buttonLayoutOptions = new GUILayoutOption[]
            {
                GUILayout.Width(100f),
                GUILayout.Height(30f)
            };
        }

        #endregion

        #region Drawing

        // Runs when the display is called to draw its self.
        public void Draw()
        {
            if (!this.hasInitStyles) this.InitialiseStyles();

            if (KerbalEconomy.Instance.Ledger != null)
                this.windowPosition = GUILayout.Window(this.windowID, this.windowPosition, this.Window, "Kerbal Economy Ledger", this.windowStyle);
        }

        // Runs when the display is being shown.
        private void Window(int windowID)
        {
            GUI.skin = HighLogic.Skin;
            this.scrollPosition = GUILayout.BeginScrollView(this.scrollPosition);
            GUI.skin = null;

            GUILayout.BeginHorizontal();

            this.DrawTime();
            this.DrawTransaction();
            this.DrawDebit();
            this.DrawCredit();

            // NOTE: Balance will be accurate at the time but will probably be out of synch with other transactions.
            this.DrawBalance();

            GUILayout.EndHorizontal();
            GUILayout.EndScrollView();

            GUILayout.BeginHorizontal();

            GUILayout.BeginHorizontal(this.boxStyle, this.boxLayoutOptions);
            GUILayout.Label("Science", this.labelTitleLeftStyle);
            GUILayout.Label(KerbalEconomy.Instance.Science.ToString("#,0.00"), this.labelNormalRightStyle);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(this.boxStyle, this.boxLayoutOptions);
            GUILayout.Label("Cost Ratio", this.labelTitleLeftStyle);
            GUILayout.Label(KerbalEconomy.Instance.CostRatio.ToString("#,0"), this.labelNormalRightStyle);
            GUILayout.EndHorizontal();

            GUILayout.FlexibleSpace();

            bool isEasy = KerbalEconomy.Instance.CostRatio == KerbalEconomy.EASY;
            bool isNormal = KerbalEconomy.Instance.CostRatio == KerbalEconomy.NORMAL;
            bool isHard = KerbalEconomy.Instance.CostRatio == KerbalEconomy.HARD;

            if (GUILayout.Toggle(isEasy, "EASY", this.buttonStyle, this.buttonLayoutOptions) && !isEasy)
                KerbalEconomy.Instance.CostRatio = KerbalEconomy.EASY;

            if (GUILayout.Toggle(isNormal, "NORMAL", this.buttonStyle, this.buttonLayoutOptions) && !isNormal)
                KerbalEconomy.Instance.CostRatio = KerbalEconomy.NORMAL;

            if (GUILayout.Toggle(isHard, "HARD", this.buttonStyle, this.buttonLayoutOptions) && !isHard)
                KerbalEconomy.Instance.CostRatio = KerbalEconomy.HARD;

            GUILayout.EndHorizontal();
        }

        private void DrawTime()
        {
            GUILayout.BeginVertical();

            GUILayout.Label("TIME", this.labelTitleLeftStyle);
            foreach (Row row in KerbalEconomy.Instance.Ledger.Rows)
                GUILayout.Label(TimeHelper.FromUniversalTime(row.UniversalTime), this.labelNormalLeftStyle);

            GUILayout.EndVertical();
        }

        private void DrawTransaction()
        {
            GUILayout.BeginVertical();

            GUILayout.Label("TRANSACTION", this.labelTitleLeftStyle);
            foreach (Row row in KerbalEconomy.Instance.Ledger.Rows)
                GUILayout.Label(row.Transaction, this.labelNormalLeftStyle);

            GUILayout.EndVertical();
        }

        private void DrawDebit()
        {
            GUILayout.BeginVertical();

            GUILayout.Label("PAID OUT", this.labelTitleRightStyle);
            foreach (Row row in KerbalEconomy.Instance.Ledger.Rows)
            {
                if (row.Debit != 0d)
                    GUILayout.Label(KerbalEconomy.ToMonies(row.Debit).ToString("#,0."), this.labelNormalRightStyle);
                else
                    GUILayout.Label(string.Empty, this.labelNormalRightStyle);
            }

            GUILayout.EndVertical();
        }

        private void DrawCredit()
        {
            GUILayout.BeginVertical();

            GUILayout.Label("PAID IN", this.labelTitleRightStyle);
            foreach (Row row in KerbalEconomy.Instance.Ledger.Rows)
            {
                if (row.Credit != 0d)
                    GUILayout.Label(KerbalEconomy.ToMonies(row.Credit).ToString("#,0."), this.labelNormalRightStyle);
                else
                    GUILayout.Label(string.Empty, this.labelNormalRightStyle);
            }

            GUILayout.EndVertical();
        }

        private void DrawBalance()
        {
            GUILayout.BeginVertical();

            GUILayout.Label("BALANCE", this.labelTitleRightStyle);
            foreach (Row row in KerbalEconomy.Instance.Ledger.Rows)
            {
                if (row.Balance != 0d)
                    GUILayout.Label(KerbalEconomy.ToMonies(row.Balance).ToString("#,0."), this.labelNormalRightStyle);
                else
                    GUILayout.Label(string.Empty, this.labelNormalRightStyle);
            }
        
            GUILayout.EndVertical();
        }

        #endregion
    }
}
