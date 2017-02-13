using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ClientNetwork
{
    class SafeQueue<T>
    {
        
        Queue<T> mQueue = new Queue<T>(1024);
        public T popObject()
        {
            T ret = default(T);
            lock(this)
            {
                
                if (mQueue.Count > 0)
                {
                    ret = mQueue.Dequeue();
                }
            }
            return ret;
        }
        public void pushObject(T obj)
        {
            lock(this)
            {
                mQueue.Enqueue(obj);
            }
        }
        public void clear()
        {
            lock(this)
            {
                mQueue.Clear();
            }
        }

    }
}
