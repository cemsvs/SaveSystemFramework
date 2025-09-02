using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SaveSystemFramework.Core;

namespace SaveSystemFramework.Providers
{
    public class JsonSaveProvider : ISaveManager
    {
        private readonly string _saveDirectory;
        private readonly JsonSerializerSettings _jsonSettings;

        public JsonSaveProvider(string saveDirectory = "Saves")
        {
            _saveDirectory = Path.GetFullPath(saveDirectory);
            _jsonSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                Formatting = Formatting.Indented,
                DateFormatHandling = DateFormatHandling.IsoDateFormat
            };

            // Klasör yoksa oluştur
            Directory.CreateDirectory(_saveDirectory);
            Console.WriteLine($"JSON Save Directory: {_saveDirectory}");
        }

        public async Task<bool> SaveAsync<T>(string saveId, T data) where T : SaveData
        {
            try
            {
                data.SaveId = saveId;
                data.OnBeforeSave();

                string filePath = Path.Combine(_saveDirectory, $"{saveId}.json");
                string jsonData = JsonConvert.SerializeObject(data, _jsonSettings);

                await File.WriteAllTextAsync(filePath, jsonData);

                Console.WriteLine($"✅ JSON Save başarılı: {saveId}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ JSON Save hatası: {ex.Message}");
                return false;
            }
        }

        public async Task<T> LoadAsync<T>(string saveId) where T : SaveData, new()
        {
            try
            {
                string filePath = Path.Combine(_saveDirectory, $"{saveId}.json");

                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"⚠️ Save dosyası bulunamadı: {saveId}");
                    return null;
                }

                string jsonData = await File.ReadAllTextAsync(filePath);
                T data = JsonConvert.DeserializeObject<T>(jsonData, _jsonSettings);

                data?.OnAfterLoad();
                Console.WriteLine($"✅ JSON Load başarılı: {saveId}");
                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ JSON Load hatası: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> DeleteSaveAsync(string saveId)
        {
            try
            {
                string filePath = Path.Combine(_saveDirectory, $"{saveId}.json");

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    Console.WriteLine($"✅ Save silindi: {saveId}");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Save silme hatası: {ex.Message}");
                return false;
            }
        }

        public async Task<List<SaveMetadata>> GetSaveListAsync()
        {
            try
            {
                var saveList = new List<SaveMetadata>();
                var files = Directory.GetFiles(_saveDirectory, "*.json");

                foreach (var file in files)
                {
                    var fileInfo = new FileInfo(file);
                    var saveId = Path.GetFileNameWithoutExtension(file);

                    saveList.Add(new SaveMetadata
                    {
                        SaveId = saveId,
                        DisplayName = saveId,
                        CreatedDate = fileInfo.CreationTimeUtc,
                        LastModified = fileInfo.LastWriteTimeUtc,
                        FileSizeBytes = fileInfo.Length,
                        DataType = "JSON"
                    });
                }

                return saveList.OrderByDescending(s => s.LastModified).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Save listesi alınamadı: {ex.Message}");
                return new List<SaveMetadata>();
            }
        }

        public async Task<bool> SaveExistsAsync(string saveId)
        {
            string filePath = Path.Combine(_saveDirectory, $"{saveId}.json");
            return File.Exists(filePath);
        }
    }
}