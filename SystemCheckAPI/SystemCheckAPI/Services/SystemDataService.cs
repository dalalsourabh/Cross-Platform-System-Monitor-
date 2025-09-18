using System;
using System.Collections.Generic;
using System.Linq;
using SystemCheckAPI.Models;

namespace SystemCheckAPI.Services
{
    public class SystemDataService
    {
        private static readonly List<SystemResourceData> _dataStore = new List<SystemResourceData>();
        private static readonly object _lock = new object();

        public void AddData(SystemResourceData data)
        {
            lock (_lock)
            {
                // Set timestamp if not provided
                if (data.Timestamp == default)
                {
                    data.Timestamp = DateTime.UtcNow;
                }
                
                _dataStore.Add(data);
                
                // Keep only the last 100 entries to prevent memory issues
                if (_dataStore.Count > 100)
                {
                    _dataStore.RemoveAt(0);
                }
            }
        }

        public List<SystemResourceData> GetAllData()
        {
            lock (_lock)
            {
                return _dataStore.ToList();
            }
        }

        public SystemResourceData GetLatestData()
        {
            lock (_lock)
            {
                return _dataStore.LastOrDefault();
            }
        }
    }
}