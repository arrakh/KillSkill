using System;
using System.Collections.Generic;

namespace Arr
{
    public class EventTemplate
    {
        bool fired = false;

        public delegate void EventHandler();

        protected event EventHandler eventHandler;
        
        public virtual void Subscribe(EventHandler dlg)
        {
            eventHandler += dlg;
         
            if (fired)
            {
                dlg.Invoke();
            }
        }
        
        public void Unsubscribe(EventHandler dlg)
        {
            eventHandler -= dlg;
        }

        public virtual void Invoke(bool clear = false)
        {
            eventHandler?.Invoke();
            if(!clear) fired = true;
        }
    }
    
    public class EventTemplate<T>
    {
        T data;
        bool fired = false;

        public delegate void EventHandler(T t);

        protected event EventHandler eventHandler;

        public virtual void Subscribe(EventHandler dlg)
        {
            eventHandler += dlg;
         
            if (fired)
            {
                dlg.Invoke(data);
            }
        }

        public void Unsubscribe(EventHandler dlg)
        {
            eventHandler -= dlg;
        }

        public virtual void Invoke(T value, bool clear = false)
        {
            data = value;
            eventHandler?.Invoke(data);
            if(!clear) fired = true;
        }

        internal void InternalInvoke(T value)
        {
            eventHandler?.Invoke(value);
        }
    }
    
    
    public class DataEventTemplate<T>
    {
        public delegate T EventHandler();

        protected event EventHandler eventHandler;

        public void Subscribe(EventHandler dlg)
        {
            eventHandler += dlg;
        }

        public T Get()
        {
            return eventHandler == null ? default : eventHandler.Invoke();
        }
    }

    public class EventTemplate<T1,T2,T3> : EventTemplate<T1>
    {
        bool invoked = false;
        T1 d1;
        T2 d2;
        T3 d3;
        public new delegate void EventHandler(T1 t1,T2 t2,T3 t3);

        public new event EventHandler eventHandler;

        public virtual void Subscribe(EventHandler dlg)
        {
            eventHandler += dlg;
            if (invoked)
            {
                dlg.Invoke(d1, d2, d3);
            }
        }

        public void Unsubscribe(EventHandler dlg)
        {
            eventHandler -= dlg;
        }

        public virtual void Invoke(T1 value1, T2 value2, T3 value3)
        {
            d1 = value1;
            d2 = value2;
            d3 = value3;
            eventHandler?.Invoke(value1, value2, value3);
        }
        
    }
    
    
    public class EventTemplate<T1,T2> : EventTemplate<T1>
    {
        bool invoked = false;
        T1 d1;
        T2 d2;
        public new delegate void EventHandler(T1 t1,T2 t2);

        public new event EventHandler eventHandler;

        public virtual void Subscribe(EventHandler dlg)
        {
            eventHandler += dlg;
            if (invoked)
            {
                dlg.Invoke(d1, d2);
            }
        }

        public virtual void Unsubscribe(EventHandler dlg)
        {
            eventHandler -= dlg;
        }

        public virtual void Invoke(T1 value1, T2 value2)
        {
            d1 = value1;
            d2 = value2;
            eventHandler?.Invoke(value1, value2);
        }
        
    }

    public class PersistentEventTemplate<T> : EventTemplate<T>
    {
        readonly Queue<T> queue = new Queue<T>();
        public override void Subscribe(EventHandler dlg)
        {
            eventHandler += dlg;

            foreach (T dat in queue)
                dlg.Invoke(dat);
        }
        
        public override void Invoke(T value, bool clear = false)
        {
            queue.Enqueue(value);
            InternalInvoke(value);
        }
    }
    
    
    public class PersistentEventTemplate<T1, T2> : EventTemplate<T1,T2>
    {
        readonly Queue<KeyValuePair<T1,T2>> queue = new Queue<KeyValuePair<T1, T2>>();
        

        public override void Subscribe(EventHandler dlg)
        {
            foreach (var dat in queue)
                dlg.Invoke(dat.Key, dat.Value);
            
            base.Subscribe(dlg);
        }
        
        public override void Invoke(T1 t1, T2 t2)
        {
            queue.Enqueue(new KeyValuePair<T1, T2>(t1,t2));
            base.Invoke(t1,t2);
        }
    }
    
    public class PersistentEventTemplate<T1, T2, T3> : EventTemplate<T1,T2,T3>
    {
        readonly Queue<Tuple<T1,T2, T3>> queue = new Queue<Tuple<T1, T2, T3>>();

        public override void Subscribe(EventHandler dlg)
        {
            foreach (var dat in queue)
                dlg.Invoke(dat.Item1, dat.Item2, dat.Item3);
            
            base.Subscribe(dlg);
        }
        
        public override void Invoke(T1 t1, T2 t2, T3 t3)
        {
            queue.Enqueue(new Tuple<T1, T2, T3>(t1,t2,t3));
            base.Invoke(t1,t2,t3);
        }
    }
}