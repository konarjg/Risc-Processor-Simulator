using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Risc_Processor
{
    public static class Assembler
    {
        private static int FindOperand(string instruction, char separator)
        {
            for (int i = 1; i < instruction.Length; i++)
            {
                if (instruction[i - 1] == separator)
                {
                    return instruction[i] - '0';
                }
            }

            return -1;
        }

        private static int Encode(string instruction)
        {
            int encoding = 0;
            int[] bits = { 0, 0, 0, 0, 0, 0, 0, 0 };

            if (instruction.Contains("ld"))
            {
                var register = FindOperand(instruction, 'r');
                var immediate = FindOperand(instruction, '$');

                bits[7] = 1;
                bits[6] = immediate % 2;
                bits[5] = (immediate / 2) % 2;
                bits[4] = register % 2;
                bits[3] = (register / 2) % 2;
            }
            else if (instruction.Contains("add"))
            {
                var halves = instruction.Split(',');
                var destination = FindOperand(halves[1], 'r');
                var source = FindOperand(halves[0], 'r');

                bits[7] = 0;
                bits[6] = destination % 2;
                bits[5] = (destination / 2) % 2;
                bits[4] = source % 2;
                bits[3] = (source / 2) % 2;
            }

            return Processor.BitsToByte(bits);
        }

        public static int[] Assemble(string path, out string[] mnemonics)
        {
            var lines = File.ReadAllLines(path);
            var length = lines.Max(x => x.Length);
            var code = new int[lines.Length];

            for (int i = 0; i < lines.Length; i++)
            {
                code[i] = Encode(lines[i]);

                while (lines[i].Length < length)
                {
                    lines[i] += " ";
                }
            }

            mnemonics = lines;
            return code;
        }

        public static void AssembleAndExecute(ref Processor processor, string path, out int executionTime, bool debug = false)
        {
            string[] mnemonics;
            int[] code = Assemble(path, out mnemonics);

            if (debug)
            {
                Console.WriteLine("Kody rozkazów: ");
            }

            for (int i = 0; i < code.Length; i++)
            {
                var bits = Processor.ByteToBits(code[i]);

                if (debug)
                {
                    Console.Write("| {0} | ", mnemonics[i]);

                    for (int j = 3; j < bits.Length; j++)
                    {
                        Console.Write(bits[j]);
                    }

                    Console.WriteLine(" |");
                }
            }

            Console.WriteLine();
            processor.ExecuteProgram(code, out executionTime);
        }
    }
}
