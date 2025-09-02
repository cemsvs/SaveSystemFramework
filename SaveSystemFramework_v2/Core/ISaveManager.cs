using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SaveSystemFramework.Core
{
    public interface ISaveManager
    {
        Task<bool> SaveAsync<T>(string saveId, T data) where T : SaveData;
        Task<T> LoadAsync<T>(string saveId) where T : SaveData, new();
        Task<bool> DeleteSaveAsync(string saveId);
        Task<List<SaveMetadata>> GetSaveListAsync();
        Task<bool> SaveExistsAsync(string saveId);
    }
}