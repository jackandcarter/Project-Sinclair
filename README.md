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

## Contributing
1. Fork the repository and create a feature branch.
2. Follow the existing C# coding style and include descriptive commit messages.
3. Open a pull request against `main` once the feature is ready.
4. All contributions should include any relevant Unity scene or asset changes.

## License and Assets
This project is distributed under the terms of the **GNU General Public License v3**. See the [LICENSE](LICENSE) file for details.

Some models and textures originate from third‑party sources. These assets remain under their respective licenses. Please consult the corresponding directories for attribution and license information where provided.
