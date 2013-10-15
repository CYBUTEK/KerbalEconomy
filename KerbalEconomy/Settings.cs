// PROJECT: Kerbal Economy
// AUTHOR:  CYBUTEK
// LICENSE: Attribution-NonCommercial-ShareAlike 3.0 Unported

using System.Collections.Generic;
using System.IO;

namespace KerbalEconomy
{
    public class Settings
    {
        #region Constants

        public const string FILENAME = "Settings.txt";
        
        #endregion

        #region Instance

        private static Settings instance;
        /// <summary>
        /// Gets the current instance of the Settings object.
        /// </summary>
        public static Settings Instance
        {
            get
            {
                if (instance == null)
                    instance = new Settings();

                return instance;
            }
        }

        #endregion

        #region Fields

        private Dictionary<string, string> settings = new Dictionary<string, string>();

        #endregion

        #region Initialisation

        private Settings()
        {
            this.Load();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a setting from the list or created it and returns with a default value.
        /// </summary>
        public string Get(string key, string defaultValue = "")
        {
            if (this.settings.ContainsKey(key))
                return this.settings[key];

            if (defaultValue != null && defaultValue.Length > 0)
                this.settings[key] = defaultValue;

            return defaultValue;
        }

        /// <summary>
        /// Sets or creates a setting with the supplied key/value.
        /// </summary>
        public void Set(string key, string value)
        {
            this.settings[key] = value;
        }

        /// <summary>
        /// Saves settings to file.
        /// </summary>
        public void Save()
        {
            List<string> lines = new List<string>();

            foreach (KeyValuePair<string, string> setting in this.settings)
                lines.Add(setting.Key + " = " + setting.Value);

            File.WriteAllLines(KerbalEconomy.AssemblyPath + FILENAME, lines.ToArray());
        }

        /// <summary>
        /// Loads settings from file.
        /// </summary>
        public void Load()
        {
            if (File.Exists(KerbalEconomy.AssemblyPath + FILENAME))
            {
                this.settings.Clear();

                string[] lines = File.ReadAllLines(KerbalEconomy.AssemblyPath + FILENAME);

                foreach (string line in lines)
                {
                    string[] setting = line.Split('=');

                    if (setting.Length > 1)
                        this.settings[setting[0].Trim()] = setting[1].Trim();
                    else
                        this.settings[setting[0].Trim()] = string.Empty;
                }
            }
        }

        #endregion
    }
}
