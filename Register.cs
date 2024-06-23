using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Risc_Processor
{
    public class Register
    {
        private int[] Data;
        private int Size;

        public Register(int size)
        {
            Data = new int[8];
            Size = size;
        }

        public void Load(int[] data)
        {
            for (int i = 0; i < Data.Length; i++)
            {
                Data[i] = data[i];
            }
        }

        public void Debug(int index, bool bits)
        {
            Console.Write("r{0} ", index);

            if (bits)
            {
                for (int j = 8 - Size; j < Data.Length; j++)
                {
                    Console.Write(Data[j]);
                }
                Console.WriteLine();
            }
            else
            {
                Console.Write("{0}\n", Processor.BitsToByte(Data));
            }
        }

        public int[] GetData()
        {
            return Data;
        }
    }
}
