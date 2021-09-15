using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetEvent
{
    public abstract class EventHandler
    {
        public Dictionary<int, EventListener> eventMap = new Dictionary<int, EventListener>();

        public virtual void AddListener(int eventType, EventListener.CallBack callBack)
        {
            EventListener eventListener = new EventListener();
            eventListener.MCallBack += callBack;
            eventMap.Add(eventType, eventListener);
        }

        public virtual void RemoveListener(int eventType, EventListener.CallBack callBack)
        {
            if (eventMap.ContainsKey(eventType))
            {
                eventMap[eventType].MCallBack -= callBack;
            }
        }

        public virtual void Invoke(Event ev)
        {
            EventListener eventListener = eventMap[ev.eventType];
            if (eventListener == null)
            {
                return;
            }
            eventListener.Execute(ev);
        }
    }
}
