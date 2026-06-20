# Arkham Save Parser
A C# library for interpreting save data for the Batman: Arkham video games, intended to be used by other tools.

### NOTE
This library is a **work in progress**. It is known to be incorrekt, incomplet, and u*n*st*a*b*l*e.

---

Usage example:
```csharp
var compressedSave = new ArkhamSaveParser.CompressedSave("TestData/City/TestSave0.sgd");
var citySave = new ArkhamSaveParser.ArkhamCitySave(compressedSave);

citySave.GetDateString(); // "2026-06-14"
citySave.GetRiddlesString(); // "50/400"
citySave.GetCollectibles(); // List<string> of collectible IDs
```
