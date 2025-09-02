# 🎮 Save System Framework

Engine-agnostic save/load system for games with multiple storage backends.

## ✨ Features

- 🗃️ **Multiple Storage Backends**
  - JSON (Human-readable)
  - Binary (Compressed)
  - SQLite (Database)
  
- ⚡ **Async/Await Support**
- 🔒 **Type Safety with Generics**
- 🔄 **Auto-Save Functionality**
- 📊 **Save Metadata Management**
- 🎯 **Engine Agnostic Design**

## 🚀 Quick Start

```csharp
// Create save manager
var saveManager = new JsonSaveProvider("MySaves");

// Create player data
var playerData = new PlayerSaveData 
{
    PlayerName = "Hero",
    Level = 10,
    Gold = 1000
};

// Save
await saveManager.SaveAsync("save1", playerData);

// Load
var loaded = await saveManager.LoadAsync<PlayerSaveData>("save1");
