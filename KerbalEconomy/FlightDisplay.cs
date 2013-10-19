// PROJECT: Kerbal Economy
// AUTHOR:  CYBUTEK
// LICENSE: Attribution-NonCommercial-ShareAlike 3.0 Unported

using System.IO;
using KerbalEconomy.Extensions;
using KerbalEconomy.Helpers;
using UnityEngine;

namespace KerbalEconomy
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class FlightDisplay : MonoBehaviour
    {
        #region Fields

        private Rect buttonPosition = new Rect(Screen.width - 17f, Screen.height / 2f, 17f, 100f);
        private Texture2D buttonClosedNormal = new Texture2D(17, 100, TextureFormat.RGBA32, false);
        private Texture2D buttonClosedHover = new Texture2D(17, 100, TextureFormat.RGBA32, false);
        private Texture2D buttonClosedDown = new Texture2D(17, 100, TextureFormat.RGBA32, false);
        private Texture2D buttonOpenNormal = new Texture2D(17, 100, TextureFormat.RGBA32, false);
        private Texture2D buttonOpenHover = new Texture2D(17, 100, TextureFormat.RGBA32, false);
        private Texture2D buttonOpenDown = new Texture2D(17, 100, TextureFormat.RGBA32, false);
        private bool isClicked = false;

        private Rect windowPosition = new Rect(Screen.width, Screen.height / 2f - 10f, 200f, 0f);
        private GUIStyle windowStyle, boxStyle, labelTitleStyle, labelLeftStyle, labelRightStyle;
        private int windowID = WindowHelper.GetWindowID();
        private float openAmount = 0f;
        private bool isOpen = false;

        private bool hasInitStyles = false;
        private float currentScience = 0f;
        private float transmittedScience = 0f;

        #endregion

        #region Initialisaion

        private void Start()
        {
            if (HighLogic.CurrentGame.Mode == Game.Modes.CAREER)
                RenderingManager.AddToPostDrawQueue(0, this.Draw);
        }

        // Initialises all the styles upon request.
        private void InitialiseStyles()
        {
            // Loading textures manually because the GameDatabase adds compression which looks horrible.
            this.buttonClosedNormal.LoadImage(File.ReadAllBytes(KerbalEconomy.AssemblyPath + "GUI/FlightButton/ClosedNormal.png"));
            this.buttonClosedHover.LoadImage(File.ReadAllBytes(KerbalEconomy.AssemblyPath + "GUI/FlightButton/ClosedHover.png"));
            this.buttonClosedDown.LoadImage(File.ReadAllBytes(KerbalEconomy.AssemblyPath + "GUI/FlightButton/ClosedDown.png"));
            this.buttonOpenNormal.LoadImage(File.ReadAllBytes(KerbalEconomy.AssemblyPath + "GUI/FlightButton/OpenNormal.png"));
            this.buttonOpenHover.LoadImage(File.ReadAllBytes(KerbalEconomy.AssemblyPath + "GUI/FlightButton/OpenHover.png"));
            this.buttonOpenDown.LoadImage(File.ReadAllBytes(KerbalEconomy.AssemblyPath + "GUI/FlightButton/OpenDown.png"));

            this.windowStyle = new GUIStyle(HighLogic.Skin.window);

            this.boxStyle = new GUIStyle(HighLogic.Skin.box);
            this.boxStyle.margin = new RectOffset();
            this.boxStyle.padding = new RectOffset(5, 5, 5, 5);

            this.labelTitleStyle = new GUIStyle(HighLogic.Skin.label);
            this.labelTitleStyle.margin = new RectOffset();
            this.labelTitleStyle.padding = new RectOffset(5, 5, 5, 5);
            this.labelTitleStyle.normal.textColor = Color.white;
            this.labelTitleStyle.alignment = TextAnchor.MiddleLeft;
            this.labelTitleStyle.fontSize = 13;
            this.labelTitleStyle.fontStyle = FontStyle.Bold;
            this.labelTitleStyle.stretchWidth = true;

            this.labelLeftStyle = new GUIStyle(HighLogic.Skin.label);
            this.labelLeftStyle.margin = new RectOffset();
            this.labelLeftStyle.padding = new RectOffset(3, 3, 3, 3);
            this.labelLeftStyle.normal.textColor = Color.white;
            this.labelLeftStyle.alignment = TextAnchor.MiddleLeft;
            this.labelLeftStyle.fontSize = 12;
            this.labelLeftStyle.fontStyle = FontStyle.Bold;
            this.labelLeftStyle.stretchWidth = true;

            this.labelRightStyle = new GUIStyle(HighLogic.Skin.label);
            this.labelRightStyle.margin = new RectOffset();
            this.labelRightStyle.padding = new RectOffset(3, 3, 3, 3);
            this.labelRightStyle.alignment = TextAnchor.MiddleRight;
            this.labelRightStyle.fontSize = 12;
            this.labelRightStyle.fontStyle = FontStyle.Bold;
            this.labelRightStyle.stretchWidth = true;
        }

        #endregion

        #region Update and Drawing

        // Runs when the object is called to update.
        private void Update()
        {
            // Update science variables.
            if (KerbalEconomy.Instance.ScienceIsNotNull)
            {
                if (!Recovery.Instance.FlightStarted) // Set starting science on first update.
                {
                    Recovery.Instance.FlightStarted = true;
                    Recovery.Instance.FlightStartScience = KerbalEconomy.Instance.Science;
                }

                this.currentScience = KerbalEconomy.Instance.Science;
                this.transmittedScience = this.currentScience - Recovery.Instance.FlightStartScience;
            }

            // Update window scroll state.
            if (this.isOpen && this.openAmount < 1f) // Opening
            {
                this.openAmount += ((10f * (1f - this.openAmount)) + 0.5f) * Time.deltaTime;

                if (this.openAmount > 1f)
                    this.openAmount = 1f;
            }
            else if (!this.isOpen && this.openAmount > 0f) // Closing
            {
                this.openAmount -= ((10f * this.openAmount) + 0.5f) * Time.deltaTime;

                if (this.openAmount < 0f)
                    this.openAmount = 0f;
            }

            // Set the scrolls positions.
            this.windowPosition.x = Screen.width - (this.windowPosition.width * this.openAmount);
            this.buttonPosition.x = this.windowPosition.x - this.buttonPosition.width;
        }

        // Runs when the object is called to draw.
        private void Draw()
        {
            if (!this.hasInitStyles) InitialiseStyles();

            this.DrawButton();

            if (this.windowPosition.x < Screen.width)
                GUILayout.Window(this.windowID, this.windowPosition, this.Window, "Kerbal Economy " + KerbalEconomy.AssemblyVersion, this.windowStyle);
        }

        private void Window(int windowID)
        {
            this.DrawTransmittedScience();
            //this.DrawStoredScience();
            this.DrawTotalScience();
        }

        // Draws the transmitted science section.
        private void DrawTransmittedScience()
        {
            GUILayout.Label("Transmitted", this.labelTitleStyle);
            GUILayout.BeginVertical(this.boxStyle);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Science", this.labelLeftStyle);
            GUILayout.Label(this.transmittedScience.ToString("#,0.00"), this.labelRightStyle);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Money", this.labelLeftStyle);
            GUILayout.Label(KerbalEconomy.ToMonies(this.transmittedScience).ToString("#,0."), this.labelRightStyle);
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        // Draws the stored science section.
        private void DrawStoredScience()
        {
            GUILayout.Label("Stored", this.labelTitleStyle);
            GUILayout.BeginVertical(this.boxStyle);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Science", this.labelLeftStyle);
            GUILayout.Label(this.transmittedScience.ToString("#,0.00"), this.labelRightStyle);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Money", this.labelLeftStyle);
            GUILayout.Label(KerbalEconomy.ToMonies(this.transmittedScience).ToString("#,0."), this.labelRightStyle);
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        // Draws the current science section.
        private void DrawTotalScience()
        {
            GUILayout.Label("Total at KSC", this.labelTitleStyle);
            GUILayout.BeginVertical(this.boxStyle);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Science", this.labelLeftStyle);
            GUILayout.Label(this.currentScience.ToString("#,0.00"), this.labelRightStyle);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Money", this.labelLeftStyle);
            GUILayout.Label(KerbalEconomy.ToMonies(this.currentScience).ToString("#,0."), this.labelRightStyle);
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        // Draws the handle button.
        private void DrawButton()
        {
            if (this.isClicked) // Button has been clicked whilst being hovered over.
            {
                if (this.buttonPosition.Contains(Event.current.mousePosition)) // Button is being hovered over.
                {
                    if (this.isOpen)
                        GUI.DrawTexture(this.buttonPosition, this.buttonOpenDown);
                    else
                        GUI.DrawTexture(this.buttonPosition, this.buttonClosedDown);

                    // User has released the mouse button whilst hovering.
                    if (Mouse.Left.GetButtonUp())
                    {
                        this.isClicked = false;
                        this.isOpen = !this.isOpen;
                    }
                }
                else // Button is not being hovered over.
                {
                    if (this.isOpen)
                        GUI.DrawTexture(this.buttonPosition, this.buttonOpenNormal);
                    else
                        GUI.DrawTexture(this.buttonPosition, this.buttonClosedNormal);

                    // User has released the mouse button whilst not hovering.
                    if (Mouse.Left.GetButtonUp())
                        this.isClicked = false;
                }
            }
            else // Button has not been clicked.
            {
                if (this.buttonPosition.Contains(Event.current.mousePosition)) // Button is being hovered over.
                {
                    if (!this.isClicked && Mouse.Left.GetButtonDown()) this.isClicked = true; // Button has just been clicked.

                    if (this.isClicked) // If the button has just been clicked.
                    {
                        if (this.isOpen)
                            GUI.DrawTexture(this.buttonPosition, this.buttonOpenDown);
                        else
                            GUI.DrawTexture(this.buttonPosition, this.buttonClosedDown);
                    }
                    else
                    {
                        if (this.isOpen)
                            GUI.DrawTexture(this.buttonPosition, this.buttonOpenHover);
                        else
                            GUI.DrawTexture(this.buttonPosition, this.buttonClosedHover);
                    }
                }
                else // Button is not being hovered over.
                {
                    if (this.isOpen)
                        GUI.DrawTexture(this.buttonPosition, this.buttonOpenNormal);
                    else
                        GUI.DrawTexture(this.buttonPosition, this.buttonClosedNormal);
                }
            }
        }

        #endregion
    }
}
