# GDC_CSharpHamster
C#-Hamster für GDC

***
# Setup
Um den C#-Hamster auf deinem PC zum laufen zu bekommen musst Du zunächst das gesamte Projekt herunterladen. Anschließend alles entpacken, merke dir auch den Ordner wo Du es entpacken hast.
Jetzt musst Du nur noch UnityHub starten.

* Falls Du UnityHub noch nicht installiert hast, gehe auf folgende Seite (Wähle dein OS): [https://unity.com/download](https://unity.com/download)

## Importiere das Projekt
Nachdem Du UnityHub installiert hast, kannst Du UnityHub starten. Es sollte ungefähr so aussehen:
![UnityHub_Empty](https://user-images.githubusercontent.com/103567242/163169324-e12be060-1858-4150-a649-496e57e1288e.png)

Jetzt nur noch den "Open" Button oben rechts einmal anklicken und wähle den Ordner aus, den Du nach dem entpacken vorhin erhalten hast.
### Version
Nach dem importieren kann es vorkommen das Unity dir eine Warnmeldung gibt, das Du eine andere Unity Version verwendest. Das ist nicht weiters schlimm, wähle einfach "Choose another Version", danach sollte alles von alleine gehen.

Falls Du ein anderes Betriebssystem als Windows verwendest, könnte ein weiterer Warnhinweis auftauchen, der sagt das die Build Settings nicht auf deinem Betriebssystem passt. Das ist auch nicht schlimm, Unity dürfte alles alleine Regeln.
***
# Arbeiten mit der UnityEngine und dem C#-Hamster
In diesem Abschnitt werde ich dir Step-by-Step erklären wie Du sachen wie Items, Quests, Quest Conditions erstellen kannst und auf was Du achten musst.
## Items
Um ein neues Item zu erstellen, mache eine rechte Maustaste in Unity im Projekt Ordner (Standard Ordner für Items: Asstes/Objects/Items/)
* Rechte Maustaste --> Create --> HamsterGame --> Items --> Item

Der Standardname des Items ist "new Item", ändere diesen nach belieben. Klicke anschließend auf die neu erstellte Datei. Im Inspector solltest Du dann folgendes sehen:

![ItemInspector](https://user-images.githubusercontent.com/103567242/163169299-c046baf7-2a35-488b-99ad-867716cf10f9.png)

Zu beachten:
* Jede Id muss einzigartig sein, also aufpassen das Du keine doppelten Id's vergibst
* Nachdem Du  alles eingestellt hast, musst Du das Item im Manager hinzufügen, dazu später mehr.

Erläuterung zu den einzelnen Punkten:
| Name        | Datentyp           | Beschreibung  |
| ------------- |:-------------:| :-----:|
| **Id**      | int | Einzigartig für jedes Item. |
| **Name**      | string      |   Dieser Name wird ingame dann so angezeigt. Falls man das Item auch auf dem Feld platzieren möchte, muss der Tilename diesem Namen entsprechen. |
| **ItemImage** | sprite      |    Bild für das Item, wird mit diesem Sprite in der Welt und im Inventar dargestellt. Darf nicht null sein! |
| **Description**  |   string             |     (Optional) Beschreibung was das Item macht.             |
|  **Type**            |     enum           |     Equippable oder Usable             |
|  **CanChangeHamsterValues**            |     bool           |      Kann dieses Item die Werte, wie Leben, Ausdauer beeinflussen?            |
|  **HealValue**            |   int             |   Heile die Lebenspunkte des Hamsters um diese Anzahl |
|  **DamageValue**           |   int             |   Schade die Lebenspunkte des Hamsters um diese Anzahl |
|  **EnduranceHealValue**            |     int           |  Heile die Ausdauerpunkte des Hamsters um diese Anzahl|
|  **EnduranceDamageValue**            |    int            |     Schade die Ausdauerpunkte des Hamsters um diese Anzahl              |  
| **IsEquipment** | bool | Kann dieses Item ausgerüstet werden? (Falls Type=Consumable, wird dieser bool ignoriert!) |
| **EquipType** | enum | None, Head, Hands, Foot, Extra 1, Extra 2 (Wo soll das Item ausgerüstet werden?) |
| **BuyPrice** | int | Wie teuer ist dieses Item wenn man es kaufen möchte? |
| **SellPrice** | int | Wie viel erhält man beim Verkauf von diesem Item? |
| **StackAmount** | int | Wieviele Items können im Inventar aufeinandergestapelt werden. |
| **SlotId** | int | NICHT EDITIEREN! Wird automatisch in Laufzeit bestimmt. |
| **HasSpecialEffects** | bool | Besitzt das Item Special Effekte wie schneller laufen etc.? |
| **MoveSpeed** | int | Beeinflusse die Schritte die der Hamster pro bewegung vor geht. |
| **OnEquip()** | UnityEvent | Was soll passieren, wenn der Hamster dieses Item ausrüstet? |
| **OnUnequip()** | UnityEvent | Was soll passieren, wenn der Hamster dieses Item ablegt? |
| **OnUse()** | UnityEvent | Was soll passieren, wenn der Hamster dieses Item verwendet? (Consumable) |

Nachdem Du alles eingestellt hast, musst Du das Item noch in den Manager einfügen, klicke dazu in der Level Hierarchie (Links) einmal auf das GameObject "Manager".

![Inspector_Manager](https://user-images.githubusercontent.com/103567242/163173331-612f348c-917a-403f-af0b-900e223f7eb3.png)

Auf der rechten Seite müsstest Du dann runterscrollen, bis Du die Komponente "Item Collection" findest.

![Inspector_Manager_ItemCollection](https://user-images.githubusercontent.com/103567242/163173619-075883dc-703d-4627-8593-da282ee47ebb.png)

Klicke hier auf das "+" zeichen und füge dein Item hinzu. Du musst anschließend nichts mehr machen, bei Spielstart sortiert das Script die Liste von alleine und list die Infos aus.
