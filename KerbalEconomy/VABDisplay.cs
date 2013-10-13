using KerbalEconomy.Extensions;
using UnityEngine;

namespace KerbalEconomy
{
    [KSPAddon(KSPAddon.Startup.EditorAny, false)]
    public class VABDisplay : MonoBehaviour
    {
        #region Fields

        private Rect position1 = new Rect(Screen.width / 2f + 280f, 0f, 125f, 20f);     // Position of the budget label.
        private Rect position2 = new Rect(Screen.width / 2f + 280f, 12f, 125f, 20f);    // Position of the cost label.

        private GUIStyle labelStyle;
        private bool hasInitStyles = false;

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

            this.labelStyle = new GUIStyle(HighLogic.Skin.label);
            this.labelStyle.normal.textColor = Color.white;
            this.labelStyle.margin = new RectOffset();
            this.labelStyle.padding = new RectOffset();
            this.labelStyle.fontSize = 12;
            this.labelStyle.fontStyle = FontStyle.Bold;
        }

        #endregion

        #region Drawing

        // Runs when KSP calls the draw queue.
        private void Draw()
        {
            if (!this.hasInitStyles) this.InitialiseStyles();
            GUI.Label(this.position1, "Budget: " + KerbalEconomy.Instance.Budget, this.labelStyle);
            GUI.Label(this.position2, "Cost: " + (EditorLogic.fetch.ship.Parts.Count > 0 ? EditorLogic.fetch.ship.Parts.Cost().ToString() : "0"), this.labelStyle);
        }

        #endregion
    }
}
