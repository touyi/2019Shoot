using System.Collections.Generic;

namespace GamePlay
{
    public class GlobalDataProvider : IDataProvider
    {
        Dictionary<string, int> intdic = new Dictionary<string, int>();
        public bool GetIntData(string key, out int value)
        {
            return this.intdic.TryGetValue(key, out value);
        }

        public bool GetFloatData(string key, out float value)
        {
            throw new System.NotImplementedException();
        }

        public bool GetStrData(string key, out string value)
        {
            throw new System.NotImplementedException();
        }

        public bool SetIntData(string key, int value)
        {
            if (this.intdic.ContainsKey(key))
            {
                this.intdic[key] = value;
            }
            else
            {
                this.intdic.Add(key, value);
            }

            return true;
        }

        public bool SetFloatData(string key, float value)
        {
            throw new System.NotImplementedException();
        }

        public bool SetStrData(string key, string value)
        {
            throw new System.NotImplementedException();
        }
    }
}