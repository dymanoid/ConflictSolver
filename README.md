# Conflict Solver

A mod for the **Cities: Skylines** game. Detects possible mod conflicts.

## Where to get it

Grab the mod binaries on the [Releases](https://github.com/dymanoid/ConflictSolver/releases) page. Alternatively, clone the repo and build the mod by yourself. You will need **Visual Studio 2019** or **MSBuild 16** for that.

## How it works

**Conflict Solver** uses the [Harmony](https://github.com/pardeike/Harmony) library to patch various `mscorlib` methods that are used by other mods to query the types via Reflection. For example:

- `Type.GetMethods()`
- `PropertyInfo.GetValue(object)`
- and many others

When patching game methods or changing values via Reflection, all mods call the corresponding Reflection methods (directly or indirectly). These calls will be intercepted by **Conflict Solver**. Every call will be registered in an internal storage. This storage can be used to determine which mods query or change the same methods, fields, or properties.

## How to use

This mod has to be loaded before any other mod, so that the Reflection methods interceptors are enabled before any other mod starts its job.

There is currently no reliable way to ensure the mod loading order in **Cities: Skylines** without low-level game hacks. However, for inspecting the conflicts among the **Steam Workshop** mods, it is sufficient to install **Conflict Solver** as a local mod. All local mods will be loaded before **Steam Workshop** mods.

Place the binaries of **Conflict Solver** in the local mod directory for **Cities: Skylines**, which is:

- `%LOCALAPPDATA%\Colossal Order\Cities_Skylines\Addons\Mods\` on Windows
- `/home/${USER}/.local/share/Colossal Order/Cities_Skylines/Addons/Mods` on Linux
- `/Users/${USER}/Library/Application Support/Colossal Order/Cities_Skylines/Addons/Mods` on Mac

After installation, start the game, enable **Conflict Solver**, and *restart the game again*. This is to ensure that **Conflict Solver** activates the interceptors before any other **Steam Workshop** mod which might be already enabled when you first activate **Conflict Solver**. No need to restart the game after that, as long as **Conflict Solver** stays enabled.

Activate all mods of interest, start a new city or load an existing one. Many mods initialize their patches only after a city has been loaded, so looking for conflicts in the main menu of the game without loading a city is not enough.

Once the city has been loaded, go to the pause menu and open the **Conflict Solver** window by pressing the `Mod Conflict Solver` button.

Hit the `Take Snapshot` button in the **Conflict Solver** window. The current state will be captured and displayed. You can delete a snapshot and capture a new one at any time in the mod's window. This might be required if some mods are activated by user input rather than by a loaded city.

Inspect the possible conflicts. You can also copy the whole content of the data by hitting the `Copy to Clipboard` button.

## What does a Possible Conflict mean

**Conflict Solver** does not check if there is an *actual* conflict. The mod only shows all methods, fields, and properties that are queried via Reflection by other mods. It is up to you to check whether these queries in fact cause any conflicts.

## Performance notice

**Conflict Solver** heavily affects performance of the managed part of the game (Mono runtime). This is because the low-level `mscorlib` methods will be patched. Those methods are called very often by the game, especially on loading a city.

Do not use **Conflict Solver** while playing. Disable the mod when you don't need it. Disabling is enough, no need to delete or uninstall the mod completely.
