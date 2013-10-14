﻿using System.Collections.Generic;
using System.IO;
using System.Linq;

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

        public void Save()
        {
            List<string> rows = new List<string>();
            foreach (Row row in this.rows)
                rows.Add(row.GetTabbedString());

            File.WriteAllLines(KerbalEconomy.AssemblyPath + filename, rows.ToArray());
        }

        public void Load()
        {
            if (File.Exists(KerbalEconomy.AssemblyPath + filename))
            {
                string[] rows = File.ReadAllLines(KerbalEconomy.AssemblyPath + filename);

                this.rows = new List<Row>();
                foreach (string row in rows)
                    this.rows.Add(Row.FromTabbedString(row));
            }
        }

        #endregion
    }
}
