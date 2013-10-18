// PROJECT: Kerbal Economy
// AUTHOR:  CYBUTEK
// LICENSE: Attribution-NonCommercial-ShareAlike 3.0 Unported

using KerbalEconomy.Extensions;
using UnityEngine;

namespace KerbalEconomy
{
    public class Recovery
    {
        #region Instance

        private static Recovery instance;
        /// <summary>
        /// Gets the current instance of the Globals object.
        /// </summary>
        public static Recovery Instance
        {
            get
            {
                if (instance == null)
                    instance = new Recovery();

                return instance;
            }
        }

        #endregion

        #region Initialisaton

        private Recovery()
        {
            GameEvents.OnVesselRecoveryRequested.Add(this.OnRecovery);
        }

        #endregion

        #region Properties

        private bool recovered = false;
        /// <summary>
        /// Gets and sets whether the ship has been recovered.
        /// </summary>
        public bool Recovered
        {
            get { return this.recovered; }
            set { this.recovered = value; }
        }

        private float recoveryMoney = 0f;
        /// <summary>
        /// Gets the value of the recovered ship in money.
        /// </summary>
        public float RecoveryMoney
        {
            get { return this.recoveryMoney; }
        }

        /// <summary>
        /// Gets the value of the recovered ship in science.
        /// </summary>
        public float RecoveryScience
        {
            get { return KerbalEconomy.ToScience(this.recoveryMoney); }
        }

        #endregion

        #region Private Events

        private void OnRecovery(Vessel vessel)
        {
            this.recovered = true;
            this.recoveryMoney = vessel.Parts.Cost();

            KerbalEconomy.Instance.StorageMode = true;
            KerbalEconomy.Instance.Credit("Recovered Vessel", KerbalEconomy.ToScience(this.recoveryMoney));
        }

        #endregion
    }
}
