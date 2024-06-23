using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Risc_Processor
{
    public class Processor
    {
        private int Time;
        private int ArchitectureSize;
        private RegisterFile Registers;
        private int InstructionPointer;
        private int[] OperationMemory;

        private AluSequential SequentialExecutionUnit;

        public Processor(int architectureSize)
        {
            ArchitectureSize = architectureSize;
            Registers = new RegisterFile((int)(architectureSize/2));
            InstructionPointer = 0;
            OperationMemory = new int[(int)Math.Pow(2, architectureSize)];
            SequentialExecutionUnit = new AluSequential();

            AluSequential.FetchEvent += OnFetchInstructionEvent;
            AluSequential.ReadEvent += OnReadOperandsEvent;
            AluSequential.WriteEvent += OnWriteResultEvent;
            AluSequential.UpdateTimeEvent += OnUpdateTimeEvent;
        }

        public void DebugRegisters(bool bits = false)
        {
            Registers.Debug(bits);
            Console.WriteLine();
        }

        public void OnUpdateTimeEvent()
        {
            Time += 250;
        }

        public void ExecuteProgram(int[] program, out int time)
        {
            Time = 0;

            for (int i = 0; i < program.Length; i++)
                OperationMemory[i] = program[i];

            while (InstructionPointer < program.Length)
            {
                SequentialExecutionUnit.BeginExecution();
            }

            time = Time;
        }

        public static int[] ByteToBits(int value)
        {
            int[] bits = { 0, 0, 0, 0, 0, 0, 0, 0 };

            int index = 7;

            while (value > 0)
            {
                bits[index] = value % 2;
                value /= 2;
                index--;
            }

            return bits;
        }

        public static int BitsToByte(int[] bits)
        {
            int value = 0;
            int weight = 1;

            for (int i = bits.Length - 1; i >= 0; i--)
            {
                value += bits[i] * weight;
                weight *= 2;
            }

            return value;
        }

        private void OnFetchInstructionEvent()
        {
            int[] instruction = ByteToBits(OperationMemory[InstructionPointer]);
            SequentialExecutionUnit.FetchInstruction(instruction);
            InstructionPointer++;
        }
        
        private void OnReadOperandsEvent((int, int) registers, ref (int, int) operands)
        {
            int firstOperand = BitsToByte(Registers.GetRegisterData(registers.Item1));
            int secondOperand = BitsToByte(Registers.GetRegisterData(registers.Item2));

            operands = (firstOperand, secondOperand);
        }

        private void OnWriteResultEvent(int index, int value)
        {
            Thread.Sleep(5);
            Registers.LoadRegister(index, ByteToBits(value));
        }
    }
}
