# Copilot Instructions for Durable Clothes (Continued) Mod

## Mod Overview and Purpose
Durable Clothes (Continued) is an updated continuation of the original mod by mpwoz. The primary objective of this mod is to prevent clothing items worn by pawns in RimWorld from deteriorating due to normal wear and tear. This mod intercepts the degradation process to maintain apparel integrity, allowing clothes to stay in pristine condition, albeit with special exceptions for bullet damage.

## Key Features and Systems
- **Apparel Durability**: This mod affects all items of the type Apparel, ensuring they do not deteriorate while worn by pawns.
- **Damage Settings**: Introduces configuration options to determine damage resistance based on a percentage of the original.
- **Exclusion Settings**: Offers settings to exclude specific apparel categories or items based on quality.
- **Autorepair Control**: Allows users to disable the autorepair feature, stopping degradation but not repairing bullet damage until the next degradation tick.
- **Harmony Patching**: Utilizes Harmony to apply patches to the game's methods for seamless integration.

## Coding Patterns and Conventions
- **Namespaces and Classes**: Use clear and consistent naming conventions for namespaces and classes. For example, classes like `DurableClothesMod` and `HarmonyPatches` are appropriately named.
- **Access Modifiers**: Use internal access for mod-specific classes (`internal class DurableClothesMod`) and public access for classes intended to interact with Harmony or other mods (`public class HarmonyPatches`).
- **Method Design**: Encapsulate specific functionalities with private methods where necessary and ensure public methods serve intended external interactions.
- **Coding Style**: Follow C# coding conventions, including camelCase for local variables, PascalCase for types and methods, and descriptive method names.

## XML Integration
- Define mod settings and configurations within XML files for user accessibility and ease of adjustment.
- Ensure proper linkage between XML-defined settings and C# logic.
- Integrate XML settings with Harmony patches to dynamically adjust behavior based on user selections.

## Harmony Patching
- Use Harmony, a popular library for patching .NET methods, to hook into RimWorld's apparel degradation logic.
- Ensure patches are non-destructive and reversible, maintaining compatibility with other mods.
- Implement patches like those in `Pawn_ApparelTracker_TakeWearoutDamageForDay.cs` to modify how wear and tear damage is applied to apparel.

## Suggestions for Copilot
- Generate method stubs for settings handling and Harmony patches to streamline development.
- Provide code examples for common modding tasks such as settings integration, Harmony patch application, and error logging.
- Create utility functions for managing mod configurations and handling user inputs.
- Suggest test scenarios for validating mod behavior, including the impact of toggling new settings.

Use these instructions to help guide your mod development with GitHub Copilot, ensuring consistent, efficient, and effective implementation of features within the Durable Clothes (Continued) mod project.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.
