using System.Collections.Generic;

namespace NetPack
{
    public class DataManager {
        private static DataManager manager;
        private int normalSize = 30;
        private Stack<Data> stacks;
        
        public static DataManager GetInstance()
        {
            if (manager == null)
            {
                manager = new DataManager();
            }
            return manager;
        }

        public DataManager()
        {
            stacks = new Stack<Data>();
            for (int i = 0; i < normalSize; i++)
            {
                var data = new Data();
                stacks.Push(data);
            }
        }

        ~DataManager()
        {
            while (stacks.Count > 0)
            {
                 var data = stacks.Pop();
                 data = null;
            }
        }

        public Data Get()
        {
            if (stacks.Count <= 0)
            {
                var data = new Data();
                return data;
            }
            else
            {
                var data = stacks.Pop();
                return data;
            }
        }

        public void Release(Data data)
        {
            data.Clear();
            stacks.Push(data);
        }

    }
}