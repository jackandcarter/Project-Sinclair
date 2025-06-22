# Project-Sinclair
Development Resource for the Upcoming RPG

## Unity Build Workflow

This project builds using GitHub Actions with Unity **2022.3.20f1**. The workflow requires a valid Unity license, provided in the repository secrets as `UNITY_LICENSE`.

To configure the license:

1. Generate or request a Unity license file.
2. In the repository settings, create a secret named `UNITY_LICENSE` containing the license text.

The workflow will build the project and run any Unity tests during pull requests or pushes to `main`.
