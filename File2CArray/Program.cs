// MIT License

// Copyright(c) 2020 Stephen Traskal

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.IO;

namespace File2CArray
{
    class Program
    {
        const string ARG_ERROR_MESSAGE = "ARG ERROR: Expected format '[filepath] optional[outputpath] optional[arrayname]'";
        const int MAX_LINE_LENGTH = 128;

        static void Main(string[] args)
        {
            if (args.Length == 0)
                throw new ArgumentException(ARG_ERROR_MESSAGE);

            string path = args[0];

            if (!File.Exists(path))
                throw new ArgumentException($"{path} does not exist.");

            string fileName = Path.GetFileNameWithoutExtension(path);
            string output = args.Length > 1 ? args[1] : $"{fileName}.h";
            string arrayName = args.Length > 2 ? args[2] : $"{fileName.ToUpper()}_DATA";
            string headerGuard = $"__{fileName.ToUpper()}_H__";
            byte[] bytes = File.ReadAllBytes(path);
            using StreamWriter hFile = File.CreateText(output);

            hFile.WriteLine($"#ifndef {headerGuard}");
            hFile.WriteLine($"#define {headerGuard}");
            hFile.WriteLine("#include <stdint.h>");
            hFile.WriteLine($"const uint8_t {arrayName}[] = ");
            hFile.WriteLine("{");

            int lineLength = 0;
            foreach (byte b in bytes) 
            {
                if (lineLength++ > MAX_LINE_LENGTH)
                {
                    hFile.WriteLine();
                    lineLength = 0;
                }

                hFile.Write(b);
                hFile.Write(',');
            }

            hFile.WriteLine("\n};");
            hFile.WriteLine("#endif");
        }
    }
}
