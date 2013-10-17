// PROJECT: Kerbal Economy API
// AUTHOR:  CYBUTEK
// LICENSE: Attribution-NonCommercial-ShareAlike 3.0 Unported

using UnityEngine;

namespace KerbalEconomyAPI
{
    public class KerbalEconomyAPI
    {
        private static KerbalEconomyAPI instance;
        /// <summary>
        /// Gets the current instance of the KerbalEconomyAPI object.
        /// </summary>
        public static KerbalEconomyAPI Instance
        {
            get
            {
                if (instance == null)
                    instance = new KerbalEconomyAPI();

                return instance;
            }
        }

        #region Properties

        private bool kerbalEconomyInstalled = false;
        /// <summary>
        /// Gets whether the Kerbal Economy plugin is installed.
        /// </summary>
        public bool KerbalEconomyInstalled
        {
            get { return this.kerbalEconomyInstalled; }
        }

        #endregion

        #region Initialisation

        // Constructor used to check if Kerbal Economy is installed.
        private KerbalEconomyAPI()
        {
            this.kerbalEconomyInstalled = this.IsPluginInstalled("KerbalEconomy");
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds an amount of science to the pool.
        /// </summary>
        public bool AddScience(float amount)
        {
            if (this.kerbalEconomyInstalled)
                KerbalEconomy.KerbalEconomy.Instance.AddScience(amount);
            else if (ResearchAndDevelopment.Instance != null)
                ResearchAndDevelopment.Instance.Science += amount;
            else return false;

            return true;
        }

        /// <summary>
        /// Deducts an amount of science from the pool.
        /// </summary>
        public bool DeductScience(float amount)
        {
            if (this.kerbalEconomyInstalled)
                KerbalEconomy.KerbalEconomy.Instance.SubScience(amount);
            else if (ResearchAndDevelopment.Instance != null)
                ResearchAndDevelopment.Instance.Science -= amount;
            else return false;

            return true;
        }

        /// <summary>
        /// Add a credit row to the ledger.
        /// </summary>
        public bool LedgerDebit(string transaction, float science)
        {
            return false;
        }

        /// <summary>
        /// Add a debit row to the ledger.
        /// </summary>
        public bool LedgerCredit(string transaction, float science)
        {
            return false;
        }

        /// <summary>
        /// Returns whether a pluggin is installed.
        /// </summary>
        public bool IsPluginInstalled(string pluginName)
        {
            foreach (AssemblyLoader.LoadedAssembly assembly in AssemblyLoader.loadedAssemblies)
            {
                if (assembly.assembly.ToString().Split(',')[0] == pluginName)
                {
                    MonoBehaviour.print("[KerbalEconomyAPI]: " + pluginName + " is installed.");
                    return true;
                }
            }

            MonoBehaviour.print("[KerbalEconomyAPI]: " + pluginName + " is not installed.");
            return false;
        }

        #endregion
    }
}
