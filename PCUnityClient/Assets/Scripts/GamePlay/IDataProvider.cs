namespace GamePlay
{
    public interface IDataProvider
    {
        bool GetIntData(string key, out int value);
        bool GetFloatData(string key, out float value);
        bool GetStrData(string key, out string value);
        
        bool SetIntData(string key, int value);
        bool SetFloatData(string key, float value);
        bool SetStrData(string key, string value);
    }
}