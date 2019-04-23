using Message;

namespace Protocol
{
    public interface SocketInputProvide
    {
        /// <summary>
        /// 按键状态
        /// </summary>
        KeyState GetKeyState(KeyType type);
    }
}