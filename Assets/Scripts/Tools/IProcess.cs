namespace Tools
{
    public interface IProcess
    {
        void Init();
        void Start();
        void Update(float deltaTime);
        void Uninit();
    }
}