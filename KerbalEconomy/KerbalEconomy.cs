// PROJECT: Kerbal Economy
// AUTHOR:  CYBUTEK
// LICENSE: Attribution-NonCommercial-ShareAlike 3.0 Unported

using System.IO;
using System.Reflection;
using KerbalEconomy.Ledger;

namespace KerbalEconomy
{
    public class KerbalEconomy
    {
        #region Constants

        /// <summary>
        /// Current version of the Kerbal Economy assembly.
        /// </summary>
        public const string AssemblyVersion = "1.0";

        public const double EASY = 2000d;
        public const double NORMAL = 1000d;
        public const double HARD = 500d;

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
        public static double ToScience(double monies)
        {
            return monies / Instance.costRatio;
        }

        /// <summary>
        /// Converts a science value into monies.
        /// </summary>
        public static double ToMonies(double science)
        {
            return science * Instance.costRatio;
        }

        #endregion

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

        private Book ledger;
        /// <summary>
        /// Gets and sets the ledger book.
        /// </summary>
        public Book Ledger
        {
            get { return this.ledger; }
            set { this.ledger = value; }
        }

        private double costRatio = 1000d;
        /// <summary>
        /// Gets and sets the monies to science ratio.
        /// </summary>
        public double CostRatio
        {
            get { return this.costRatio; }
            set
            {
                this.costRatio = value;
                Settings.Instance.Set("Cost Ratio", this.costRatio.ToString());
            }
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
        public double Monies
        {
            get { return this.science * this.costRatio; }
            set { this.science = value / this.costRatio; }
        }

        #endregion

        #region Initialisation

        private KerbalEconomy()
        {
            this.CostRatio = double.Parse(Settings.Instance.Get("Cost Ratio", this.costRatio.ToString()));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Credit your economy's science. (Put in)
        /// </summary>
        public void Credit(string transaction, double science)
        {
            this.science += science;
            this.ledger.AddRow(HighLogic.CurrentGame.UniversalTime, transaction, 0d, science, this.science);
        }

        /// <summary>
        /// Debit your economy's science. (Take out)
        /// </summary>
        public void Debit(string transaction, double science)
        {
            this.science -= science;
            this.ledger.AddRow(HighLogic.CurrentGame.UniversalTime, transaction, science, 0d, this.science);
        }

        #endregion
    }
}
