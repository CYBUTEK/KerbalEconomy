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

        // Constructor initialises the recovery event callback.
        private Recovery()
        {
            GameEvents.onVesselRecovered.Add(this.OnRecovery);
        }

        #endregion

        #region Properties

        private bool reverted = false;
        /// <summary>
        /// Gets and sets whether the ship has been reverted.
        /// </summary>
        public bool Reverted
        {
            get { return this.reverted; }
            set { this.reverted = value; }
        }

        private float revertedScience = 0f;
        /// <summary>
        /// Gets and sets the starting science before revertion.
        /// </summary>
        public float RevertedScience
        {
            get { return this.revertedScience; }
            set { this.revertedScience = value; }
        }

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

        // Called when a vessel is recovered.
        private void OnRecovery(ProtoVessel vessel)
        {
            this.recovered = true;
            this.recoveryMoney = vessel.protoPartSnapshots.Cost();

            KerbalEconomy.Instance.Credit(vessel.vesselType + " Recovery", KerbalEconomy.ToScience(this.recoveryMoney));
        }

        #endregion
    }
}
