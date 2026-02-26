using System;

namespace Core
{
    public abstract class IOInstance<T> where T : IdentifiedObject
    {
        protected T _data;

        public IOInstance(T data)
        {
            _data = data;
        }

        protected IOInstance(IOInstance<T> original)
        {
            _data = original._data;
        }

        protected IOInstance()
        {
            throw new NotImplementedException();
        }

        public T Data => _data;

        protected virtual void Initialize()
        {
        }
    }
}