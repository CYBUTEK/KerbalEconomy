namespace KerbalEconomy
{
    public class KerbalEconomy
    {
        #region Instance

        private static KerbalEconomy instance;
        /// <summary>
        /// Gets the current instance of the KerbalEconomy object.
        /// </summary>
        public static KerbalEconomy Instance
        {
            get
            {
                if (instance == null)
                    instance = new KerbalEconomy();

                return instance;
            }
        }

        #endregion

        #region Properties

        private double costRatio = 1000d;
        /// <summary>
        /// Gets and sets the monies to science ratio.
        /// </summary>
        public double CostRatio
        {
            get { return this.costRatio; }
            set { this.costRatio = value; }
        }

        // This should be replaced with getting and setting actual science values from within KSP.
        private double science = 5d;
        /// <summary>
        /// Gets and sets the amount of stored science.
        /// </summary>
        public double Science
        {
            get { return this.science; }
            set { this.science = value; }
        }

        /// <summary>
        /// Gets and sets the budget calculated from the stored science.
        /// </summary>
        public double Budget
        {
            get { return this.science * this.costRatio; }
            set { this.science = value / this.costRatio; }
        }

        #endregion

        #region Methods



        #endregion
    }
}
