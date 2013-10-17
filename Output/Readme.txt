Kerbal Economy v1.0
===================

ABOUT
-----
With the 0.22 release bringing a new scope of play to the table, it is still not
fully featured.  This plugin mod will give an extra dimension to the science system
that has been implemented.  It will keep track of your science flow and transactions
via a ledger.  Science is directly equal to money and the amount of money per science
point is determined by the cost ratio.  It will cost you money or science points to
launch crafts from the SPH and VAB.


INSTALLATION
------------
Copy over the 'KerbalEconomy' folder completely into your 'GameData' directory that
resides within your Kerbal Space Program installation.


BASIC
-----
Whenever you launch a ship or plane from the editor it will deduct science points
from your overall pool, based on the cost ratio.

There are 3 difficulty settings which change the cost ratio, and these are available
via the ledger.
  - EASY    = 2,000
  - NORMAL  = 1,000
  - HARD    = 500

The ledger will record transactions such as:
  - Ship/Plane construction.
  - Science gained on missions.
  - Science gained from recovered vessels.
  - Science spent in the R&D department.

Ledgers are available within the 'GameData/KerbalEconomy/Ledgers' folder, and are
stored in a tab separated '.csv' files.  The files are named after the associated
save which is used within Kerbal Space Program.


ADVANCED
--------
Settings are adjustable in the 'Settings.txt' file.  I recommend starting the game,
going into the space centre and opening the ledger first.  That will create the
settings file with all values that are adjustable.
  - Cost Ratio (for something other than default difficulties). 
  - Ledger sizing (width/height).


CREDITS & LICENSE
-----------------
AUTHOR:   CYBUTEK
LICENSE:  Attribution-NonCommercial-ShareAlike 3.0 Unported
SOURCE:   https://github.com/CYBUTEK/KerbalEconomy