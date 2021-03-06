﻿// PROJECT: Kerbal Economy
// AUTHOR:  CYBUTEK
// LICENSE: Attribution-NonCommercial-ShareAlike 3.0 Unported

using KerbalEconomy.Extensions;
using KerbalEconomy.Helpers;
using UnityEngine;

namespace KerbalEconomy
{
    [KSPAddon(KSPAddon.Startup.EditorAny, false)]
    public class EditorDisplay : MonoBehaviour
    {
        #region Fields


        private Rect buttonPosition;
        private Rect ledgerButtonPosition;
        private Rect windowPosition;
        private int windowID = WindowHelper.GetWindowID();

        private GUIStyle windowStyle, boxStyle, buttonStyle, labelLeftStyle, labelRightStyle;
        private bool hasInitStyles = false;

        private bool showDisplay = false;
        private bool showLedger = false;
        private int cost = 0;

        #endregion

        #region Initialisation

        // Runs when the object is created.
        private void Start()
        {
            if (HighLogic.CurrentGame.Mode == Game.Modes.CAREER)
            {
                // Fixes problems with the button displaying over the top the new/save/load at resolutions with less than 1280 width.
                if (Screen.width >= 1280)
                {
                    this.buttonPosition = new Rect(Screen.width / 2f + 280f, 1f, 125f, 24f);
                    this.ledgerButtonPosition = new Rect(Screen.width / 2f + 280f, 29f, 125f, 24f);
                    this.windowPosition = new Rect(Screen.width / 2f + 280f, 55f, 200f, 0f);
                }
                else
                {
                    this.buttonPosition = new Rect(Screen.width / 2f + 280f, 45f, 125f, 24f);
                    this.ledgerButtonPosition = new Rect(Screen.width / 2f + 280f, 71f, 125f, 24f);
                    this.windowPosition = new Rect(Screen.width / 2f + 280f, 97f, 200f, 0f);
                }

                RenderingManager.AddToPostDrawQueue(0, this.Draw);
            }
        }

        // Initialises the styles upon request.
        private void InitialiseStyles()
        {
            this.hasInitStyles = true;

            this.windowStyle = new GUIStyle(HighLogic.Skin.window);
            this.windowStyle.margin = new RectOffset();
            this.windowStyle.padding = new RectOffset(5, 5, 5, 5);

            this.boxStyle = new GUIStyle(HighLogic.Skin.box);
            this.boxStyle.margin = new RectOffset();
            this.boxStyle.padding = new RectOffset(5, 5, 5, 5);

            this.buttonStyle = new GUIStyle(HighLogic.Skin.button);
            this.buttonStyle.normal.textColor = Color.white;

            this.labelLeftStyle = new GUIStyle(HighLogic.Skin.label);
            this.labelLeftStyle.margin = new RectOffset();
            this.labelLeftStyle.padding = new RectOffset();
            this.labelLeftStyle.normal.textColor = Color.white;
            this.labelLeftStyle.alignment = TextAnchor.MiddleLeft;
            this.labelLeftStyle.fontSize = 13;
            this.labelLeftStyle.fontStyle = FontStyle.Bold;
            this.labelLeftStyle.stretchWidth = true;

            this.labelRightStyle = new GUIStyle(HighLogic.Skin.label);
            this.labelRightStyle.margin = new RectOffset();
            this.labelRightStyle.padding = new RectOffset();
            this.labelRightStyle.alignment = TextAnchor.MiddleRight;
            this.labelRightStyle.fontSize = 13;
            this.labelRightStyle.fontStyle = FontStyle.Bold;
            this.labelRightStyle.stretchWidth = true;
        }

        #endregion

        #region Update and Drawing

        // Runs when the object is told to update.
        private void Update()
        {
            if (HighLogic.CurrentGame.Mode == Game.Modes.CAREER)
            {
                cost = EditorLogic.fetch.ship.Parts.Count > 0 ? EditorLogic.fetch.ship.Parts.Cost() : 0;
            }
        }

        // Runs when KSP calls the draw queue.
        private void Draw()
        {
            if (!this.hasInitStyles) this.InitialiseStyles();

            this.showDisplay = GUI.Toggle(this.buttonPosition, this.showDisplay, "Kerbal Economy", this.buttonStyle);

            if (this.showDisplay)
            {
                this.showLedger = GUI.Toggle(this.ledgerButtonPosition, this.showLedger, "Ledger", this.buttonStyle);

                GUILayout.Window(this.windowID, this.windowPosition, this.Window, string.Empty, this.windowStyle);

                if (this.showLedger)
                    LedgerDisplay.Instance.Draw();
            }
        }

        // Runs when the display is being shown.
        private void Window(int windowID)
        {
            GUILayout.BeginVertical(this.boxStyle);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Balance", this.labelLeftStyle);
            GUILayout.Label(KerbalEconomy.Instance.Monies.ToString("#,0."), this.labelRightStyle);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Cost", this.labelLeftStyle);
            GUILayout.Label(cost.ToString("#,0."), this.labelRightStyle);
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }

        #endregion

        // Runs when the object is destroyed.
        private void OnDestroy()
        {
            if (HighLogic.CurrentGame.Mode == Game.Modes.CAREER)
            {
                if (HighLogic.LoadedSceneIsFlight)
                {
                    KerbalEconomy.Instance.StorageMode = true;
                    KerbalEconomy.Instance.Debit(EditorLogic.startPod.vesselType + " Construction", KerbalEconomy.ToScience(cost));
                }
            }
        }
    }
}
