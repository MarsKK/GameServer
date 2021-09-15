using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetEvent
{
    public class Event
    {
        //事件类别
        public int eventType;
        //事件参数
        public Object eventParams;

        public Event()
        {

        }

        public Event(int eventType, Object eventParams)
        {
            this.eventType = eventType;
            this.eventParams = eventParams;
        }
    }
}
