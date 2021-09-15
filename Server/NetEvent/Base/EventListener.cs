using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetEvent
{
    public class EventListener
    {
        //监听委托
        public delegate void CallBack(Event ev);

        //委托事件
        public event CallBack MCallBack;

        //执行
        public void Execute(Event ev)
        {
            MCallBack(ev);
        }
    }
}
