using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Hweny.Utility
{
    public sealed class GameDebuger : SingletonObject<GameDebuger>
    {
        private List<DebugInfo> debugInfo;
        public event EventHandler Update;

        private GameDebuger()
        {
            debugInfo = new List<DebugInfo>();
        }

        [Conditional("DEBUG")]
        public void AddDebugInfo(string key, string value, bool isRepeatKey = true)
        {
            if (isRepeatKey)
            {
                debugInfo.Add(new DebugInfo() { Key = key, Value = value });
                OnUpdate();
            }
            else
            {
                var item = debugInfo.Find(a => a.Key == key);
                if (item == null)
                {
                    debugInfo.Add(new DebugInfo() { Key = key, Value = value });
                }
                else
                {
                    item.Value = value;
                }
                OnUpdate();
            }
        }

        [Conditional("DEBUG")]
        public void AddErrorInfo(string key, string value, bool isRepeatKey = true)
        {
            AddDebugInfo(key + " error", value, isRepeatKey);
        }

        [Conditional("DEBUG")]
        public void Clear()
        {
            debugInfo.Clear();
            OnUpdate();
        }

        public string OutputDebugInfo()
        {
            StringBuilder debugMsg = new StringBuilder(256);
            debugInfo.ForEach(a => { debugMsg.Append(a + Environment.NewLine); });
            return debugMsg.ToString();
        }

        private void OnUpdate()
        {
            if (Update != null)
            {
                Update(this, null);
            }
        }

        class DebugInfo
        {
            public string Key { get; set; }
            public string Value { get; set; }

            public override string ToString()
            {
                return Key + ":" + Value;
            }
        }
    }
}
