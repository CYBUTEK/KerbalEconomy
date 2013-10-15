﻿using KerbalEconomy.Helpers;
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

        private GUIStyle windowStyle, buttonStyle, scrollStyle, labelTitleStyle, labelNormalStyle;
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
            this.scrollPosition = GUILayout.BeginScrollView(this.scrollPosition, this.scrollStyle);
            GUILayout.BeginHorizontal();

            this.DrawTime();
            this.DrawTransaction();
            this.DrawDebit();
            this.DrawCredit();

            // NOTE: Balance will be accurate at the time but will probably be out of synch with other transactions.
            this.DrawBalance();

            GUILayout.EndHorizontal();
            GUILayout.EndScrollView();
        }

        private void DrawTime()
        {
            GUILayout.BeginVertical();

            GUILayout.Label("TIME", this.labelTitleStyle);
            foreach (Row row in KerbalEconomy.Instance.Ledger.Rows)
                GUILayout.Label(TimeHelper.FromUniversalTime(row.UniversalTime), this.labelNormalStyle);

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
            {
                if (row.Debit != 0d)
                    GUILayout.Label(KerbalEconomy.ToMonies(row.Debit).ToString("#,0."), this.labelNormalStyle);
                else
                    GUILayout.Label(string.Empty, this.labelNormalStyle);
            }

            GUILayout.EndVertical();
        }

        private void DrawCredit()
        {
            GUILayout.BeginVertical();

            GUILayout.Label("PAID IN", this.labelTitleStyle);
            foreach (Row row in KerbalEconomy.Instance.Ledger.Rows)
            {
                if (row.Credit != 0d)
                    GUILayout.Label(KerbalEconomy.ToMonies(row.Credit).ToString("#,0."), this.labelNormalStyle);
                else
                    GUILayout.Label(string.Empty, this.labelNormalStyle);
            }

            GUILayout.EndVertical();
        }

        private void DrawBalance()
        {
            GUILayout.BeginVertical();

            GUILayout.Label("BALANCE", this.labelTitleStyle);
            foreach (Row row in KerbalEconomy.Instance.Ledger.Rows)
            {
                if (row.Balance != 0d)
                    GUILayout.Label(KerbalEconomy.ToMonies(row.Balance).ToString("#,0."), this.labelNormalStyle);
                else
                    GUILayout.Label(string.Empty, this.labelNormalStyle);
            }
        
            GUILayout.EndVertical();
        }

        #endregion
    }
}
