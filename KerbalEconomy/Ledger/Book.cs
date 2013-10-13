using System.Collections.Generic;
using System.IO;

namespace KerbalEconomy.Ledger
{
    public class Book
    {
        #region Fields

        private string filename = string.Empty;

        #endregion

        #region Properties

        private List<Row> rows = new List<Row>();
        /// <summary>
        /// Gets and sets the ledger rows.
        /// </summary>
        public List<Row> Rows
        {
            get { return this.rows; }
            set { this.rows = value; }
        }

        #endregion

        #region Initialisation

        public Book(string filename)
        {
            this.filename = filename;
            this.Load();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a new ledger row.
        /// </summary>
        public void AddRow(double universalTime, string transaction, double debit, double credit, double balance)
        {
            rows.Add(new Row()
            {
                UniversalTime = universalTime,
                Transaction = transaction,
                Debit = debit,
                Credit = credit,
                Balance = balance
            });
        }

        private void Save()
        {

        }

        private void Load()
        {
        }

        #endregion
    }
}
