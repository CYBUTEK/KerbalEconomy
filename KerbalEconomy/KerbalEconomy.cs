// PROJECT: Kerbal Economy
// AUTHOR:  CYBUTEK
// LICENSE: Attribution-NonCommercial-ShareAlike 3.0 Unported

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using KerbalEconomy.Ledger;
using UnityEngine;

namespace KerbalEconomy
{
    [KSPAddon(KSPAddon.Startup.Instantly, false)]
    public class KerbalEconomy : MonoBehaviour
    {
        #region Constants

        /// <summary>
        /// Current version of the Kerbal Economy assembly.
        /// </summary>
        public const string AssemblyVersion = "1.1.0";

        public const float EASY = 2000f;
        public const float NORMAL = 1000f;
        public const float HARD = 500f;

        #endregion

        #region Static Properties

        private static string assemblyFile;
        /// <summary>
        /// Gets the file name including path of the Kerbal Economy assembly.
        /// </summary>
        public static string AssemblyFile
        {
            get
            {
                if (assemblyFile == null)
                    assemblyFile = Assembly.GetExecutingAssembly().Location;

                return assemblyFile;
            }
        }


        private static string assemblyName;
        /// <summary>
        /// Gets the file name of the Kerbal Economy assembly.
        /// </summary>
        public static string AssemblyName
        {
            get
            {
                if (assemblyName == null)
                    assemblyName = new FileInfo(AssemblyFile).Name;

                return assemblyName;
            }
        }

        private static string assemblyPath;
        /// <summary>
        /// Gets the path of the Kerbal Economy assembly.
        /// </summary>
        public static string AssemblyPath
        {
            get
            {
                if (assemblyPath == null)
                    assemblyPath = AssemblyFile.Replace(AssemblyName, string.Empty);

                return assemblyPath;
            }
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Converts a monitary value into science.
        /// </summary>
        public static float ToScience(float monies)
        {
            return Mathf.Round((monies / Instance.CostRatio) * 100f) / 100f;
        }

        /// <summary>
        /// Converts a science value into monies.
        /// </summary>
        public static float ToMonies(float science)
        {
            return science * Instance.CostRatio;
        }

        #endregion

        #region Fields

        private Dictionary<Row, bool> rowsToProcess = new Dictionary<Row, bool>();
        private Stopwatch storeScienceTimer = new Stopwatch();
        private bool storeScienceTimerEnabled = false;
        private long storeScienceTimerDuration = 5000;

        #endregion

        #region Instance

        /// <summary>
        /// Gets the current instance of the KerbalEconomy object.
        /// </summary>
        public static KerbalEconomy Instance { get; private set; }

        #endregion

        #region Properties

        private Book ledger;
        /// <summary>
        /// Gets and sets the ledger book.
        /// </summary>
        public Book Ledger
        {
            get { return this.ledger; }
            set { this.ledger = value; }
        }

        private float costRatio = 1000f;
        /// <summary>
        /// Gets and sets the monies to science ratio.
        /// </summary>
        public float CostRatio
        {
            get { return this.costRatio; }
            set
            {
                this.costRatio = value;
                Settings.Instance.Set("Cost Ratio", this.costRatio.ToString());
                print("[KerbalEconomy]: Changed Cost Ratio to " + this.costRatio + ".");
            }
        }

        /// <summary>
        /// Gets if KSP returns a not null value for science..
        /// </summary>
        public bool ScienceIsNotNull
        {
            get { return ResearchAndDevelopment.Instance != null; }
        }

        private bool storageMode = true;
        /// <summary>
        /// Gets and sets whether KerbalEconomy should store changes to be processed later.
        /// </summary>
        public bool StorageMode
        {
            get
            {
                return this.storageMode;
            }
            set
            {
                if (!this.storageMode && value)
                {
                    print("[KerbalEconomy]: Storage Mode Enabled.");
                    this.storageMode = value;
                }
                else if (this.storageMode && !value)
                {
                    this.storeScienceTimer.Reset();
                    this.storeScienceTimer.Start();
                    this.storeScienceTimerEnabled = true;
                }
            }
        }

        private float science = 0f;
        /// <summary>
        /// Gets and sets the amount of stored science.
        /// </summary>
        public float Science
        {
            get
            {
                return ResearchAndDevelopment.Instance.Science;
            }
            set
            {
                float previous = ResearchAndDevelopment.Instance.Science;
                ResearchAndDevelopment.Instance.Science = Mathf.Round(value * 100f) / 100f;
                print("[KerbalEconomy]: Changed Science by " + (ResearchAndDevelopment.Instance.Science - previous) + " to " + ResearchAndDevelopment.Instance.Science + ".");
            }
        }

        /// <summary>
        /// Gets and sets the budget calculated from the stored science.
        /// </summary>
        public float Monies
        {
            get { return this.Science * this.costRatio; }
            set { this.Science = value / this.costRatio; }
        }

        #endregion

        #region Initialisation

        // Initialiser used to load settings.
        private void Awake()
        {
            if (Instance == null)
            {
                DontDestroyOnLoad(this);
                Instance = this;
            }

            this.costRatio = float.Parse(Settings.Instance.Get("Cost Ratio", this.costRatio.ToString()));
        }

        #endregion

        #region Update

        // Update used to manage science and ledger processing.
        private void Update()
        {
            if (HighLogic.CurrentGame != null && HighLogic.CurrentGame.Mode == Game.Modes.CAREER)
            {
                if (this.storeScienceTimerEnabled && this.storeScienceTimer.ElapsedMilliseconds >= this.storeScienceTimerDuration)
                {
                    this.storeScienceTimerEnabled = false;
                    this.storeScienceTimer.Stop();
                    this.storageMode = false;
                    print("[KerbalEconomy]: Storage Mode Disabled.");
                }

                if (this.rowsToProcess.Count > 0 && !this.storageMode && this.ScienceIsNotNull)
                {
                    foreach (KeyValuePair<Row, bool> row in this.rowsToProcess)
                    {
                        if (row.Value)
                            this.AddScience(row.Key.Credit - row.Key.Debit);

                        this.ledger.AddRow(row.Key.UniversalTime, row.Key.Transaction, row.Key.Debit, row.Key.Credit, this.Science);
                        print("[KerbalEconomy]: Added Row to Ledger.");
                    }

                    this.rowsToProcess.Clear();
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Credit your economy's science. (Put in)
        /// </summary>
        public void Credit(string transaction, float science, bool adjust = true)
        {
            if (!this.storageMode && this.ScienceIsNotNull)
            {
                if (adjust) this.AddScience(science);
                this.ledger.AddRow(HighLogic.CurrentGame.UniversalTime, transaction, 0f, science, this.Science);
                print("[KerbalEconomy]: Added Row to Ledger.");
            }
            else
            {
                this.rowsToProcess.Add(new Row(HighLogic.CurrentGame.UniversalTime, transaction, 0f, science, 0f), adjust);
                print("[KerbalEconomy]: Added Row to Process Queue.");
            }
        }

        /// <summary>
        /// Debit your economy's science. (Take out)
        /// </summary>
        public void Debit(string transaction, float science, bool adjust = true)
        {
            if (!this.storageMode && this.ScienceIsNotNull)
            {
                if (adjust) this.SubScience(science);
                this.ledger.AddRow(HighLogic.CurrentGame.UniversalTime, transaction, science, 0f, this.Science);
                print("[KerbalEconomy]: Added Row to Ledger.");
            }
            else
            {
                this.rowsToProcess.Add(new Row(HighLogic.CurrentGame.UniversalTime, transaction, science, 0f, 0f), adjust);
                print("[KerbalEconomy]: Added Row to Ledger Process Queue.");
            }
        }

        /// <summary>
        /// Adds an amount of science to the pool.
        /// </summary>
        public void AddScience(float amount)
        {
            if (!this.storageMode)
                this.Science += amount;
            else
                this.science += amount;
        }

        /// <summary>
        /// Subtracts an amount of science from the pool.
        /// </summary>
        public void SubScience(float amount)
        {
            if (!this.storageMode)
                this.Science -= amount;
            else
                this.science -= amount;
        }

        #endregion
    }
}
