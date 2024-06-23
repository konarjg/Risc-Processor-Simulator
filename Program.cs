using Risc_Processor;
using System.Diagnostics;

Processor processor = new Processor(4);

int time = 0;
processor.DebugRegisters();
Assembler.AssembleAndExecute(ref processor, "test.asm", out time, true);
processor.DebugRegisters();

Console.WriteLine("\nCzas wykonania sekwencyjnie: {0} ps", time);