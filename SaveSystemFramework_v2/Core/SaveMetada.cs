using System;

namespace SaveSystemFramework.Core
{
    public class SaveMetadata
    {
        public string SaveId { get; set; }
        public string DisplayName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModified { get; set; }
        public string Version { get; set; }
        public string GameVersion { get; set; }
        public long FileSizeBytes { get; set; }
        public string DataType { get; set; }

        public string FormattedFileSize
        {
            get
            {
                if (FileSizeBytes < 1024) return $"{FileSizeBytes} B";
                if (FileSizeBytes < 1024 * 1024) return $"{FileSizeBytes / 1024:F1} KB";
                return $"{FileSizeBytes / (1024 * 1024):F1} MB";
            }
        }

        public string FormattedLastModified
        {
            get
            {
                var timeSpan = DateTime.UtcNow - LastModified;
                if (timeSpan.Days > 0) return $"{timeSpan.Days} gün önce";
                if (timeSpan.Hours > 0) return $"{timeSpan.Hours} saat önce";
                if (timeSpan.Minutes > 0) return $"{timeSpan.Minutes} dakika önce";
                return "Az önce";
            }
        }
    }
}