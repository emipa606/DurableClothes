# GitHub Copilot Instructions for "Durable Clothes (Continued)" Mod

## Mod Overview and Purpose

"Durable Clothes (Continued)" is a mod for RimWorld designed to prevent the degradation of worn apparel by pawns. The mod modifies the deterioration mechanism to maintain the durability of clothing at maximum health, except in specific circumstances such as being hit by bullets. This mod aims to enhance gameplay by ensuring that apparel lasts longer and requires less frequent replacement.

### Key Features and Systems

- **Apparel Durability**: Prevents worn clothes from deteriorating on pawns by setting the item's HP to maximum during the scheduled "degradation tick."
- **Damage Adjustment**: Allows clothes to take damage based on a percentage of the original damage.
- **Configurability**: Includes settings to:
  - Exclude specific apparel categories from auto-repair.
  - Exclude apparel based on quality.
  - Disable autorepair if desired, stopping only natural degradation.
- **No Dependency on HugsLib for Settings**: Removed reliance on HugsLib, although it is still required for C# method patching.

## Coding Patterns and Conventions

- **Class Structure**: Use internal classes for implementation details (`DurableClothesMod`, `DurableClothesSettings`) and public classes for broader access (`HarmonyPatches`, `Pawn_ApparelTracker_TakeWearoutDamageForDay`).
- **C# Version**: Utilizes .NET Framework versions 4.7.2 and 4.8.
- **Modularity**: Separate settings handling (`DurableClothesSettings`) from the mod's main logic (`DurableClothesMod`).
- **Method Naming**: Adopting camelCase for private methods and PascalCase for public methods and classes.

## XML Integration

- Define new ModSettings in XML to enable easy user configuration.
- Ensure that XML files are structured to complement C# class properties for seamless data integration.
- Utilize XML for defining default values and allowable configuration ranges.

## Harmony Patching

- Use Harmony to patch specific methods that handle apparel degradation.
- Key Entry Point: `Pawn_ApparelTracker_TakeWearoutDamageForDay`, which intercepts the standard wearout process.
- Ensure patches are applied correctly to maintain compatibility with other mods and preserve game balance.

## Suggestions for Copilot

- **Code Suggestions**: Guide Copilot to suggest implementations that are compatible with Harmony and RimWorld's modding API.
- **Error Handling**: Encourage Copilot to incorporate robust error handling, especially around XML file parsing and mod setting management.
- **Performance Considerations**: Optimize suggestions to reduce lag, particularly during apparel wearout checks.
- **Expandability**: Generate suggestions with an eye towards expanding settings, such as adding more granular control over apparel conditions.
- **Debugging Tips**: Propose additional logging in Harmony patches for easier troubleshooting and version control.

This file provides guidance on the structure and integration of key components within "Durable Clothes (Continued)" and offers strategies for using GitHub Copilot effectively in extending and maintaining the mod.
