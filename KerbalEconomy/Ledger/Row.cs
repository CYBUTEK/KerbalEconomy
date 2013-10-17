// PROJECT: Kerbal Economy
// AUTHOR:  CYBUTEK
// LICENSE: Attribution-NonCommercial-ShareAlike 3.0 Unported

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
                Debit = float.Parse(row[2]),
                Credit = float.Parse(row[3]),
                Balance = float.Parse(row[4])
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

        private float debit = 0f;
        /// <summary>
        /// Gets and sets the amount debited.
        /// </summary>
        public float Debit
        {
            get { return this.debit; }
            set { this.debit = value; }
        }

        private float credit = 0f;
        /// <summary>
        /// Gets and sets the amount credited.
        /// </summary>
        public float Credit
        {
            get { return this.credit; }
            set { this.credit = value; }
        }

        private float balance = 0f;
        /// <summary>
        /// Gets and sets the final balance.
        /// </summary>
        public float Balance
        {
            get { return this.balance; }
            set { this.balance = value; }
        }

        #endregion

        #region Initialisation

        public Row() { }

        public Row(double universalTime, string transaction, float debit, float credit, float balance)
        {
            this.universalTime = universalTime;
            this.transaction = transaction;
            this.debit = debit;
            this.credit = credit;
            this.balance = balance;
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
