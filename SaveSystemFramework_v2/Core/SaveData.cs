using System;

namespace SaveSystemFramework.Core
{
    [Serializable]
    public abstract class SaveData
    {
        public string SaveId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModified { get; set; }
        public string Version { get; set; } = "1.0";
        public string GameVersion { get; set; } = "1.0.0";

        protected SaveData()
        {
            CreatedDate = DateTime.UtcNow;
            LastModified = DateTime.UtcNow;
        }

        public virtual void OnBeforeSave()
        {
            LastModified = DateTime.UtcNow;
        }

        public virtual void OnAfterLoad()
        {
            // Override edilebilir
        }

        public virtual bool IsCompatible(string gameVersion)
        {
            return GameVersion == gameVersion;
        }
    }
}