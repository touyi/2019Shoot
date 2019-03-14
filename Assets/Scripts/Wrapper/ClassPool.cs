using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Wrapper
{
    public static class ClassPool<T> where T : new()
    {
        private static Queue<T> _queue = new Queue<T>();

        public static T Create()
        {
            if (_queue.Count > 0)
            {
                T item = _queue.Dequeue();
                return item;
            }
            else
            {
                T item = new T();
                return item;
            }
        }

        public static void Return(T item)
        {
            if (item != null)
            {
                _queue.Enqueue(item);
            }
        }
    }
    public class Poolable<T> where T : Poolable<T>, new()
    {
        public virtual void Init()
        {
            
        }

        public virtual void Uninit()
        {
            
        }
        
        public void Release()
        {
            this.Uninit();
            ClassPool<T>.Return(this as T);
        }
        private static Queue<T> _queue = new Queue<T>();

        public static T Get()
        {
            T item = ClassPool<T>.Create();
            item.Init();
            return item;
        }
        
        

        

    }
}