using UnityEngine;

namespace KerbalEconomy.Ledger
{
    public class Row
    {
        #region Static Methods

        /// <summary>
        /// Gets a Row from a tabbed delimited string.
        /// </summary>
        public static Row FromTabbedString(string tabbedString)
        {
            string[] row = tabbedString.Split('\t');
            return new Row() {
                UniversalTime = double.Parse(row[0]),
                Transaction = row[1],
                Debit = double.Parse(row[2]),
                Credit = double.Parse(row[3]),
                Balance = double.Parse(row[4])
            };
        }

        #endregion

        #region Properties

        private double universalTime = 0d;
        /// <summary>
        /// Gets and sets the universal time of the transaction.
        /// </summary>
        public double UniversalTime
        {
            get { return this.universalTime; }
            set { this.universalTime = value; }
        }

        private string transaction = string.Empty;
        /// <summary>
        /// Gets and sets the transaction description.
        /// </summary>
        public string Transaction
        {
            get { return this.transaction; }
            set { this.transaction = value; }
        }

        private double debit = 0d;
        /// <summary>
        /// Gets and sets the amount debited.
        /// </summary>
        public double Debit
        {
            get { return this.debit; }
            set { this.debit = value; }
        }

        private double credit = 0d;
        /// <summary>
        /// Gets and sets the amount credited.
        /// </summary>
        public double Credit
        {
            get { return this.credit; }
            set { this.credit = value; }
        }

        private double balance = 0d;
        /// <summary>
        /// Gets and sets the final balance.
        /// </summary>
        public double Balance
        {
            get { return this.balance; }
            set { this.balance = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get a string suitable to be saved in a tab separated file.
        /// </summary>
        public string GetTabbedString()
        {
            return this.universalTime.ToString() + "\t" + this.transaction + "\t" + this.debit + "\t" + this.credit + "\t" + this.balance;
        }

        #endregion
    }
}
