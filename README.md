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

Open the `Olympus` scene from the Unity editor and use the doors placed around
the main street to access each interior area. The interiors are simple prefabs
that can be expanded with models and decorations.
