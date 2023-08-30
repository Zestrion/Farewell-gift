using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Survivors
{
    public static class StringArraySaver
    {
        private const char SEPARATION_SIGN = '.';

        public static void AddString(string _name, string _saveKey)
        {
            List<string> _items = Load(_saveKey);
            _items.Add(_name);
            Save(_items, _saveKey);
        }

        public static void RemoveString(string _name, string _saveKey)
        {
            List<string> _items = Load(_saveKey);
            if (_items.Contains(_name))
            {
                _items.Remove(_name);
                Save(_items, _saveKey);
            }
            else
            {
                Debug.LogError("StringArraySaver: string is not removed " + _name + " " + _saveKey);
            }
        }
        
        public static List<string> GetStringsArray(string _saveKey)
        {
            return Load(_saveKey);
        }

        public static void Clear(string _saveKey)
        {
            PlayerPrefs.SetString(_saveKey, "");
        }

        private static void Save(List<string> _items, string _saveKey)
        {
            System.Text.StringBuilder _stringBuilder = new System.Text.StringBuilder();
            for (int i = 0; i < _items.Count; i++)
            {
                _stringBuilder.Append(_items[i]);
                if (i != _items.Count - 1)
                {
                    _stringBuilder.Append(SEPARATION_SIGN);
                }
            }
            PlayerPrefs.SetString(_saveKey, _stringBuilder.ToString());
        }

        private static List<string> Load(string _saveKey)
        {
            List<string> _loadedData = new List<string>();
            string _saveString = PlayerPrefs.GetString(_saveKey);
            if (!string.IsNullOrEmpty(_saveString))
            {
                _loadedData = _saveString.Split(SEPARATION_SIGN).ToList();
            }
            return _loadedData;
        }
    }
}
