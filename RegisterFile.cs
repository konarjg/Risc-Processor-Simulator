using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Risc_Processor
{
    public class RegisterFile
    {
        private Register[] Registers;

        public RegisterFile(int architectureSize)
        {
            Registers = new Register[(int)Math.Pow(2, architectureSize)];

            for (int i = 0; i < Registers.Length; i++)
            {
                Registers[i] = new Register(architectureSize);
            }
        }

        public int[] GetRegisterData(int index)
        {
            return Registers[index].GetData();
        }

        public void LoadRegister(int index, int[] data)
        {
            Registers[index].Load(data);
        }

        public void Debug(bool bits = false)
        {
            for (int i = 0; i < Registers.Length; i++)
            {
                Registers[i].Debug(i, bits);
            }
        }
    }
}
