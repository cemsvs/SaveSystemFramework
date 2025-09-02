using System;
using System.Collections.Generic;
using SaveSystemFramework.Core;

namespace SaveSystemFramework.Examples
{
    [Serializable]
    public class PlayerSaveData : SaveData
    {
        public string PlayerName { get; set; } = "Unknown Player";
        public int Level { get; set; } = 1;
        public float Experience { get; set; } = 0f;
        public int Health { get; set; } = 100;
        public int MaxHealth { get; set; } = 100;
        public int Gold { get; set; } = 0;

        public Vector3 Position { get; set; } = Vector3.Zero;
        public Vector3 Rotation { get; set; } = Vector3.Zero;

        public Dictionary<string, int> Inventory { get; set; }
        public List<string> CompletedQuests { get; set; }
        public Dictionary<string, object> GameSettings { get; set; }

        public PlayerSaveData()
        {
            Inventory = new Dictionary<string, int>();
            CompletedQuests = new List<string>();
            GameSettings = new Dictionary<string, object>();
        }

        public override void OnBeforeSave()
        {
            base.OnBeforeSave();
            Console.WriteLine($"[INFO] Saving player: {PlayerName} (Level {Level})");
        }

        public override void OnAfterLoad()
        {
            base.OnAfterLoad();
            Console.WriteLine($"[INFO] Player loaded: {PlayerName} (Level {Level})");
        }

        // Utility methods
        public void AddItem(string item, int quantity = 1)
        {
            if (Inventory.ContainsKey(item))
                Inventory[item] += quantity;
            else
                Inventory[item] = quantity;
        }

        public bool RemoveItem(string item, int quantity = 1)
        {
            if (Inventory.ContainsKey(item) && Inventory[item] >= quantity)
            {
                Inventory[item] -= quantity;
                if (Inventory[item] <= 0)
                    Inventory.Remove(item);
                return true;
            }
            return false;
        }

        public void CompleteQuest(string questId)
        {
            if (!CompletedQuests.Contains(questId))
                CompletedQuests.Add(questId);
        }

        public bool IsQuestCompleted(string questId)
        {
            return CompletedQuests.Contains(questId);
        }

        public float GetExperienceToNextLevel()
        {
            return Level * 1000f - Experience;
        }

        public void GainExperience(float amount)
        {
            Experience += amount;
            while (Experience >= Level * 1000f)
            {
                Experience -= Level * 1000f;
                Level++;
                Console.WriteLine($"[LEVEL UP] New level: {Level}");
            }
        }
    }
}