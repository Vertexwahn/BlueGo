using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlueGo
{
    class RingBuffer
    {
        public RingBuffer(int size)
        {
            this.size = size;

            messages = new List<string>();

            for (int i = 0; i < size; i++)
                messages.Add("");

            currentIndex = 0;
        }

        public void addItem(string message)
        {
            messages[currentIndex] = message;
            currentIndex++;

            if (currentIndex == size)
                currentIndex = 0;
        }

        public string getLastItem()
        {
            int i = currentIndex;
            currentIndex--;
            if (currentIndex < 0)
                currentIndex = size - 1;

            return messages[i];
        }

        public int Size
        {
            get { return size; }
        }

        int size;
        int currentIndex;
        List<string> messages;
    }
}
