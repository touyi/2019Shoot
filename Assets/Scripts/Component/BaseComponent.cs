namespace Component
{
    public interface IBaseComponent
    {
        void Init();
        void Start();
        void Uninit();
        void Update(float deltaTime);
    }
    
}