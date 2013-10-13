using KerbalEconomy.Extensions;
using UnityEngine;

namespace KerbalEconomy
{
    [KSPAddon(KSPAddon.Startup.EditorAny, false)]
    public class EditorDisplay : MonoBehaviour
    {
        #region Fields

        private Rect buttonPosition = new Rect(Screen.width / 2f + 280f, 1f, 125f, 24f);
        private Rect windowPosition = new Rect(Screen.width / 2f + 280f, 30f, 125f, 0f);
        private int windowID = Random.Range(100, int.MaxValue);

        private GUIStyle windowStyle, buttonStyle, labelLeftStyle, labelRightStyle;
        private bool hasInitStyles = false;

        private bool showDisplay = false;
        private int cost = 0;

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
            this.windowStyle.padding = new RectOffset(5, 5, 5, 5);

            this.buttonStyle = new GUIStyle(HighLogic.Skin.button);
            this.buttonStyle.normal.textColor = Color.white;

            this.labelLeftStyle = new GUIStyle(HighLogic.Skin.label);
            this.labelLeftStyle.normal.textColor = Color.white;
            this.labelLeftStyle.margin = new RectOffset();
            this.labelLeftStyle.padding = new RectOffset();
            this.labelLeftStyle.alignment = TextAnchor.MiddleLeft;
            this.labelLeftStyle.fontSize = 12;
            this.labelLeftStyle.fontStyle = FontStyle.Bold;
            this.labelLeftStyle.stretchWidth = true;

            this.labelRightStyle = new GUIStyle(this.labelLeftStyle);
            this.labelRightStyle.alignment = TextAnchor.MiddleRight;
        }

        #endregion

        #region Update and Drawing

        // Runs when the object is told to update.
        private void Update()
        {
            cost = EditorLogic.fetch.ship.Parts.Count > 0 ? EditorLogic.fetch.ship.Parts.Cost() : 0;
        }

        // Runs when KSP calls the draw queue.
        private void Draw()
        {
            if (!this.hasInitStyles) this.InitialiseStyles();

            this.showDisplay = GUI.Toggle(this.buttonPosition, this.showDisplay, "Kerbal Economy", this.buttonStyle);

            if (this.showDisplay)
                GUILayout.Window(this.windowID, this.windowPosition, this.Window, string.Empty, this.windowStyle);
        }

        // Runs when the display is being shown.
        private void Window(int windowID)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Available", this.labelLeftStyle);
            GUILayout.Label(KerbalEconomy.Instance.Monies.ToString("#,0."), this.labelRightStyle);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Cost", this.labelLeftStyle);
            GUILayout.Label(cost.ToString("#,0."), this.labelRightStyle);
            GUILayout.EndHorizontal();
        }

        #endregion

        // Runs when the object is destroyed.
        private void OnDestroy()
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                if (EditorLogic.fetch.editorType == EditorLogic.EditorMode.VAB)
                    KerbalEconomy.Instance.Debit("Ship Construction", KerbalEconomy.ToScience(cost));
                else if (EditorLogic.fetch.editorType == EditorLogic.EditorMode.SPH)
                    KerbalEconomy.Instance.Debit("Plane Construction", KerbalEconomy.ToScience(cost));
            }
        }
    }
}
