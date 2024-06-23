using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Risc_Processor
{
    public enum InstructionOpcodes
    {
        ADD = 0,
        LOAD = 1
    }

    public class AluSequential
    {
        private int[] CurrentInstruction = new int[8];

        public delegate void Fetch();
        public delegate void ReadOperands((int, int) registers, ref (int, int) operands);
        public delegate void WriteResult(int index, int value);
        public delegate void UpdateTime();

        public static event Fetch FetchEvent;
        public static event ReadOperands ReadEvent;
        public static event WriteResult WriteEvent;
        public static event UpdateTime UpdateTimeEvent;

        public void BeginExecution()
        {
            FetchEvent?.Invoke();
        }

        public void FetchInstruction(int[] instruction)
        {
            for (int i = 0; i < instruction.Length; i++)
            {
                CurrentInstruction[i] = instruction[i];
            }

            UpdateTimeEvent?.Invoke();
            Decode();
        }

        public void Decode()
        {
            int length = (int)CurrentInstruction.Length;
            int opcode = CurrentInstruction[length - 1];
            int destination = 0;

            UpdateTimeEvent?.Invoke();

            switch (opcode)
            {
                case (int)InstructionOpcodes.ADD:
                    destination = CurrentInstruction[length - 2] + 2 * CurrentInstruction[length - 3];
                    int source = CurrentInstruction[length - 4] + 2 * CurrentInstruction[length - 5];
                    (int, int) operands = (0, 0);

                    ReadEvent?.Invoke((source, destination), ref operands);
                    UpdateTimeEvent?.Invoke();
                    int result = Execute(InstructionOpcodes.ADD, operands);
                    WriteEvent?.Invoke(destination, result);
                    UpdateTimeEvent?.Invoke();
                    break;

                case(int) InstructionOpcodes.LOAD:
                    int value = CurrentInstruction[length - 2] + 2 * CurrentInstruction[length - 3];
                    destination = CurrentInstruction[length - 4] + 2 * CurrentInstruction[length - 5];
                    WriteEvent?.Invoke(destination, value);
                    UpdateTimeEvent?.Invoke();
                    break;
            }
        }

        public int Execute(InstructionOpcodes operation, (int, int) operands)
        {
            UpdateTimeEvent?.Invoke();

            switch (operation)
            {
                case InstructionOpcodes.ADD:
                    return operands.Item1 + operands.Item2;

                case InstructionOpcodes.LOAD:
                    return 0;
            }

            return 0;
        }
    }
}
