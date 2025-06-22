# Project Sinclair
An open source RPG prototype built with Unity. The goal is to create a top-down adventure game while experimenting with character controllers, UI systems and abilities. Development is at a very early stage.

## Opening the Project in Unity
1. Install **Unity 2022.3.20f1** or a compatible version.
2. Clone this repository.
3. In Unity Hub choose **Add** and select the cloned `Project-Sinclair` folder.
4. Open the project from Unity Hub and allow Unity to import assets and packages.

## Building the Game
- **Automated:** GitHub Actions runs on every push to `main` or pull request and performs a Linux build using the included workflow.
- **Manual:** In the Unity Editor open **File → Build Settings** and build the `StandaloneLinux64` target or your desired platform.
- A valid Unity license is required. For CI builds provide the license text in the repository secret `UNITY_LICENSE`.


## Creating the Olympus Scene
1. Open the project in Unity and create a new scene at `Assets/Scenes/Olympus/Olympus`.
2. Add cubes or placeholder models to outline golden streets and mansion buildings.
3. Create prefabs for Ellen's Alchemical Brews, Helstien's H-Armory, Hephaestus Café and Samson's Office under `Prefabs/Olympus`.
4. Place entrances in the scene that reference these prefabs for interior transitions.


## Contributing
1. Fork the repository and create a feature branch.
2. Follow the existing C# coding style and include descriptive commit messages.
3. Open a pull request against `main` once the feature is ready.
4. All contributions should include any relevant Unity scene or asset changes.

## License and Assets
This project is distributed under the terms of the **GNU General Public License v3**. See the [LICENSE](LICENSE) file for details.

Some models and textures originate from third‑party sources. These assets remain under their respective licenses. Please consult the corresponding directories for attribution and license information where provided.

## Olympus Scene
A new scene `Olympus` resides under `Assets/Scenes/Olympus/`. It contains
placeholders for golden streets and mansion-style buildings as well as entrances
for several interior locations:

- **Ellen's Alchemical Brews** – potion shop prefab.
- **Haphazard Helstien's H-Armory** next to **Hephaestus Café** prefab.
- **Hephaestus Café** restaurant prefab with space for TV monitors.
- **Samson's Office** – a beach-themed office prefab.
- **Tutorial Fountain** demonstrating a `BattleActionEvent` that uses the
  "Water Bottle" item when triggered.

Open the `Olympus` scene from the Unity editor and use the doors placed around
the main street to access each interior area. The interiors are simple prefabs
that can be expanded with models and decorations.

## Accessing Scenes
The project contains several locations that can be loaded directly from the
Unity editor:

- **Olympus** – `Assets/Scenes/Olympus/Olympus.unity`
- **War Simulator** – `Scenes/WarSimulator/WaitingRoom.unity` and
  `Scenes/WarSimulator/Arena.unity`
- **Dúrnir's Pass** – a collection of scenes under
  `Assets/Scenes/DurnirsPass/`.

Open any of these `.unity` files through **File → Open Scene…** or by
double‑clicking them in the Project window.

## Game Systems Overview
### InventoryManager
Tracks consumable items for the party and allows adding or using items at
runtime.

### CaptureTheFlag
Manages a simple capture‑the‑flag mode inside the War Simulator. Players spawn
from a waiting room, flags are spawned in the arena and the system records
scores until a team wins.

### QuestManager
Keeps lists of active and completed quests so other scripts can start or finish
missions.

### Lecture Events
`LectureDefinition` assets describe stat bonuses awarded by `LectureEvent`
components when a character attends a lecture.

### AbilitySystem
Handles casting character abilities during battle including MP costs and
cooldowns. `DamageAbility` provides a simple example spell that deals damage to
a target.

## Developer Console
Press the **backquote (`) key** at runtime to open the developer console. From
here you can run debug commands such as:

- `give <item> <amount>` – add items to the inventory
- `teleport <scene>` – instantly load another scene
- `godmode` – toggle invulnerability for player characters

Additional commands can be registered through the `DevConsole` component for
future debugging needs.

## Item Maker
Use **Window → Sinclair Tools → Item Maker** to create or load a
`ConsumableItem` asset. The window lets you edit the item's **Name**,
**Description** and **Heal Amount** fields and save the changes back to the
asset file.

## Placeholder Assets
Most areas use cubes and basic shapes as temporary models. Create your own
placeholders under the `Prefabs` folders or import free assets from the Unity
Asset Store to prototype environments, then replace them with final meshes when
ready.
