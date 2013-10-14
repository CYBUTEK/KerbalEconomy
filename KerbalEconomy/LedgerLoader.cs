using UnityEngine;

namespace KerbalEconomy
{
    [KSPAddon(KSPAddon.Startup.MainMenu | KSPAddon.Startup.EditorAny | KSPAddon.Startup.Flight, false)]
    public class LedgerLoader : MonoBehaviour
    {
        #region Fields

        private GameScenes currentScene;

        #endregion

        // Runs when the object is created.
        private void Start()
        {
            this.currentScene = HighLogic.LoadedScene;

            if (currentScene == GameScenes.MAINMENU)
            {
                if (KerbalEconomy.Instance.Ledger != null)
                    KerbalEconomy.Instance.Ledger = null;
            }
        }

        // Runs when the object is being destroyed.
        private void OnDestroy()
        {
            if (currentScene == GameScenes.MAINMENU && HighLogic.LoadedScene == GameScenes.SPACECENTER)
                KerbalEconomy.Instance.Ledger = new Ledger.Book(HighLogic.SaveFolder + ".txt");

            KerbalEconomy.Instance.Ledger.Save();
        }
    }
}
