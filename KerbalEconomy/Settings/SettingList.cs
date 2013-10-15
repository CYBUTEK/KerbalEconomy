using System.Collections.Generic;
using System.IO;

namespace KerbalEconomy.Settings
{
    public class SettingList
    {
        #region Fields

        private Dictionary<string, string> settings = new Dictionary<string, string>();

        #endregion

        #region Methods

        /// <summary>
        /// Gets a setting from the list or returns with a default value.
        /// </summary>
        public string Get(string key, string defaultValue = "")
        {
            if (this.settings.ContainsKey(key))
                return this.settings[key];

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
        /// Saves the setting list to the given file.
        /// </summary>
        public void Save(string filename)
        {
            List<string> lines = new List<string>();

            foreach (KeyValuePair<string, string> setting in this.settings)
                lines.Add(setting.Key + " = " + setting.Value);

            File.WriteAllLines(KerbalEconomy.AssemblyPath + filename, lines.ToArray());
        }

        /// <summary>
        /// Loads the setting list from a given file.
        /// </summary>
        public void Load(string filename)
        {
            if (File.Exists(KerbalEconomy.AssemblyPath + filename))
            {
                string[] lines = File.ReadAllLines(KerbalEconomy.AssemblyPath + filename);

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
