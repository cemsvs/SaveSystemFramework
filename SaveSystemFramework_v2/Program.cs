using System;
using System.Threading.Tasks;
using SaveSystemFramework.Core;
using SaveSystemFramework.Providers;
using SaveSystemFramework.Examples;

namespace SaveSystemFramework
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Save System Framework Test");
            Console.WriteLine("==========================");

            // Create save manager
            var saveManager = new JsonSaveProvider("TestSaves");

            // Create test player
            var player = CreateTestPlayer();

            // Save test
            Console.WriteLine("\nSAVE TEST");
            Console.WriteLine("---------");
            bool saveResult = await saveManager.SaveAsync("player_save_1", player);

            if (saveResult)
            {
                Console.WriteLine("[SUCCESS] Save operation completed!");
            }
            else
            {
                Console.WriteLine("[ERROR] Save operation failed!");
                return;
            }

            // Load test
            Console.WriteLine("\nLOAD TEST");
            Console.WriteLine("---------");
            var loadedPlayer = await saveManager.LoadAsync<PlayerSaveData>("player_save_1");

            if (loadedPlayer != null)
            {
                Console.WriteLine("[SUCCESS] Load operation completed!");
                DisplayPlayerInfo(loadedPlayer);
            }
            else
            {
                Console.WriteLine("[ERROR] Load operation failed!");
                return;
            }

            // Save list
            Console.WriteLine("\nSAVE LIST");
            Console.WriteLine("---------");
            var saveList = await saveManager.GetSaveListAsync();

            foreach (var save in saveList)
            {
                Console.WriteLine($"File: {save.SaveId} - Size: {save.FormattedFileSize} - Modified: {save.FormattedLastModified}");
            }

            // Advanced tests
            await AdvancedTests(saveManager);

            Console.WriteLine("\nAll tests completed successfully!");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        static PlayerSaveData CreateTestPlayer()
        {
            var player = new PlayerSaveData
            {
                PlayerName = "TestHero",
                Level = 25,
                Experience = 18500.5f,
                Health = 85,
                MaxHealth = 100,
                Gold = 2500,
                Position = new Vector3(150.5f, 200.0f, 75.3f),
                Rotation = new Vector3(0, 45.0f, 0)
            };

            // Add inventory items
            player.AddItem("HealthPotion", 5);
            player.AddItem("ManaPotion", 3);
            player.AddItem("IronSword", 1);
            player.AddItem("LeatherArmor", 1);

            // Complete quests
            player.CompleteQuest("tutorial_complete");
            player.CompleteQuest("first_boss_defeated");
            player.CompleteQuest("village_saved");

            // Game settings
            player.GameSettings["master_volume"] = 0.8f;
            player.GameSettings["graphics_quality"] = "High";
            player.GameSettings["auto_save_enabled"] = true;

            return player;
        }

        static void DisplayPlayerInfo(PlayerSaveData player)
        {
            Console.WriteLine("\nPLAYER INFORMATION");
            Console.WriteLine("------------------");
            Console.WriteLine($"Name: {player.PlayerName}");
            Console.WriteLine($"Level: {player.Level}");
            Console.WriteLine($"Experience: {player.Experience:F1}/{player.Level * 1000}");
            Console.WriteLine($"Health: {player.Health}/{player.MaxHealth}");
            Console.WriteLine($"Gold: {player.Gold}");
            Console.WriteLine($"Position: {player.Position}");
            Console.WriteLine($"Rotation: {player.Rotation}");

            Console.WriteLine($"\nINVENTORY ({player.Inventory.Count} items):");
            foreach (var item in player.Inventory)
            {
                Console.WriteLine($"   - {item.Key}: {item.Value}");
            }

            Console.WriteLine($"\nCOMPLETED QUESTS ({player.CompletedQuests.Count} total):");
            foreach (var quest in player.CompletedQuests)
            {
                Console.WriteLine($"   * {quest}");
            }
        }

        static async Task AdvancedTests(ISaveManager saveManager)
        {
            Console.WriteLine("\nADVANCED TESTS");
            Console.WriteLine("--------------");

            // Multiple saves test
            Console.WriteLine("\nMultiple save test...");
            for (int i = 1; i <= 3; i++)
            {
                var testPlayer = CreateTestPlayer();
                testPlayer.PlayerName = $"Player{i}";
                testPlayer.Level = i * 10;
                testPlayer.Gold = i * 1000;

                await saveManager.SaveAsync($"test_player_{i}", testPlayer);
            }

            // Save exists test
            Console.WriteLine("\nSave existence check...");
            bool exists = await saveManager.SaveExistsAsync("test_player_1");
            Console.WriteLine($"Does test_player_1 exist? {(exists ? "YES" : "NO")}");

            // Delete test
            Console.WriteLine("\nSave deletion test...");
            bool deleted = await saveManager.DeleteSaveAsync("test_player_3");
            Console.WriteLine($"Was test_player_3 deleted? {(deleted ? "YES" : "NO")}");

            // Final save list
            Console.WriteLine("\nFinal save list:");
            var finalList = await saveManager.GetSaveListAsync();
            foreach (var save in finalList)
            {
                Console.WriteLine($"   File: {save.SaveId} - Size: {save.FormattedFileSize}");
            }
        }
    }
}