using System.Text;

namespace HammingCodeApp
{
    internal class FileService
    {
        private readonly int _blockLength;
        private readonly HammingCodec _hammingCodec;

        public FileService(string filePath, int blockLength, HammingCodec hammingCodec)
        {
            if (blockLength <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(blockLength));
            }

            _blockLength = blockLength;
            if(hammingCodec == null)
            {
                throw new ArgumentNullException(nameof(hammingCodec));
            }
            else
            {
                _hammingCodec = hammingCodec;
            }


            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"File not found: {filePath}");
            }
        }

        // Reading data from files
        private string ReadTextFile(string filePath)
        {
            return File.ReadAllText(filePath, Encoding.ASCII);
        }

        private List<string> ReadEncodedFile(string filePath)
        {
            return new List<string>(File.ReadAllLines(filePath));
        }

        // ASCII to binary and vice versa conversion
        private string ConvertTextToBits(string text)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(text);
            StringBuilder bits = new StringBuilder();

            foreach (byte b in bytes)
            {
                bits.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
            }

            return bits.ToString();
        }

        private string ConvertBitsToText(string bits)
        {
            StringBuilder text = new StringBuilder();

            for (int i = 0; i + 8 <= bits.Length; i += 8)
            {
                string byteString = bits.Substring(i, 8);
                byte value = Convert.ToByte(byteString, 2);

                text.Append((char)value);
            }

            return text.ToString();
        }

        // Convert a string of bits to a byte array
        private byte[] ConvertBitsToBytes(string bits)
        {
            byte[] data = new byte[bits.Length];

            for (int i = 0; i < bits.Length; i++)
            {
                if (bits[i] != '0' && bits[i] != '1')
                {
                    throw new FormatException($"Encoded file contains an invalid character: {bits[i]}");
                }

                data[i] = (byte)(bits[i] - '0');
            }

            return data;
        }

        // Split bits into blocks
        private List<string> SplitBitsIntoBlocks(string bits, int blockLength)
        {
            List<string> blocks = new List<string>();

            for (int i = 0; i < bits.Length; i += blockLength)
            {
                int length = Math.Min(blockLength, bits.Length - i);
                string block = bits.Substring(i, length).PadRight(blockLength, '0');
                blocks.Add(block);
            }
            return blocks;
        }

        // Encode data blocks
        private List<string> EncodeBits(List<string> blocks)
        {
            List<string> encodedBlocks = new List<string>();

            foreach (string block in blocks)
            {
                byte[] data = ConvertBitsToBytes(block);
                byte[] encodedData = _hammingCodec.EncodeData(data);

                StringBuilder encodedBlock = new StringBuilder();

                foreach (byte bit in encodedData)
                {
                    encodedBlock.Append(bit);
                }
                encodedBlocks.Add(encodedBlock.ToString());
            }

            return encodedBlocks;
        }

        // Write encoded blocks to a file
        private void WriteEncodedBlocksToFile(List<string> encodedBlocks, string outputPath)
        {
            File.WriteAllLines(outputPath, encodedBlocks);
        }

        // Main function to encode a file
        internal bool EncodeFile(string filePath, int blockLength)
        {
            try
            {
                if (blockLength <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(blockLength));
                }

                string text = ReadTextFile(filePath);
                string bits = ConvertTextToBits(text);

                List<string> blocks = SplitBitsIntoBlocks(bits, blockLength);
                List<string> encodedBlocks = EncodeBits(blocks);
                string directory = Path.GetDirectoryName(Path.GetFullPath(filePath))!;

                string fileName = Path.GetFileNameWithoutExtension(filePath);
                string encodedPath = Path.Combine(directory, fileName + "Encoded.txt");
                WriteEncodedBlocksToFile(encodedBlocks, encodedPath);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        // Check syndromes in encoded blocks
        private string CheckEncodedBlocks(List<string> blocks)
        {
            StringBuilder report = new StringBuilder();
            int errorsFound = 0;

            for (int i = 0; i < blocks.Count; i++)
            {
                byte[] encodedData = ConvertBitsToBytes(blocks[i]);
                int syndrome = _hammingCodec.CalculateSyndrome(encodedData);

                if (syndrome == 0)
                {
                    report.AppendLine($"Block {i + 1}: No error detected.");
                }
                else
                {
                    errorsFound++;
                    report.AppendLine($"Block {i + 1}: Error detected. " + $"Syndrome = {syndrome}, " + $"probable bit position = {syndrome}.");
                }
            }

            report.AppendLine();
            report.AppendLine($"Blocks with errors: {errorsFound}");

            return report.ToString();
        }

        //Decode File button: check syndromes only
        internal string DecodeFile(string filePath)
        {
            try
            {
                List<string> blocks = ReadEncodedFile(filePath);
                return CheckEncodedBlocks(blocks);
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        // Inject one random error into each encoded block
        private List<string> InjectErrors(List<string> blocks)
        {
            List<string> injectedBlocks = new List<string>();
            Random random = new Random();

            foreach (string block in blocks)
            {
                if (string.IsNullOrEmpty(block))
                {
                    throw new InvalidDataException("An encoded block is empty.");
                }

                char[] bits = block.ToCharArray();
                int index = random.Next(bits.Length);

                if (bits[index] == '0')
                {
                    bits[index] = '1';
                }
                else if (bits[index] == '1')
                {
                    bits[index] = '0';
                }
                else
                {
                    throw new FormatException("Encoded block contains an invalid character.");
                }

                injectedBlocks.Add(new string(bits));
            }

            return injectedBlocks;
        }


        //Write blocks with injected errors to a new file
        private void WriteInjectedBlocksToFile(List<string> blocks, string filePath)
        {
            string directory = Path.GetDirectoryName(Path.GetFullPath(filePath))!;
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            string outputPath = Path.Combine(directory, fileName + "Injected.txt");

            File.WriteAllLines(outputPath, blocks);
        }

        //Main function to inject errors into a file
        internal bool InjectFile(string filePath)
        {
            try
            {
                List<string> blocks = ReadEncodedFile(filePath);

                if (blocks.Count == 0)
                {
                    throw new InvalidDataException("The input file contains no blocks.");
                }

                List<string> injectedBlocks = InjectErrors(blocks);
                WriteInjectedBlocksToFile(injectedBlocks, filePath);

                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to inject errors: " + ex.Message, ex);
            }
        }


        //Correct errors in encoded blocks
        private List<string> CorrectEncodedBlocks(List<string> blocks)
        {
            List<string> correctedBlocks = new List<string>();

            foreach (string block in blocks)
            {
                byte[] encodedData = ConvertBitsToBytes(block);

                int syndrome = _hammingCodec.CalculateSyndrome(encodedData);

                if (syndrome != 0)
                {
                    if (syndrome > encodedData.Length)
                    {
                        throw new InvalidDataException($"Invalid syndrome: {syndrome}.");
                    }

                    _hammingCodec.CorrectErrors(encodedData, syndrome);
                }

                StringBuilder correctedBlock = new StringBuilder();

                foreach (byte bit in encodedData)
                {
                    correctedBlock.Append(bit);
                }

                correctedBlocks.Add(correctedBlock.ToString());
            }

            return correctedBlocks;
        }

        //Decode corrected blocks into original data bits
        private string DecodeBlocks(List<string> blocks)
        {
            StringBuilder restoredBits = new StringBuilder();

            foreach (string block in blocks)
            {
                byte[] encodedData = ConvertBitsToBytes(block);
                byte[] decodedData = _hammingCodec.DecodeData(encodedData);

                foreach (byte bit in decodedData)
                {
                    restoredBits.Append(bit);
                }
            }

            return restoredBits.ToString();
        }

        //Write restored text to a file
        private bool WriteRestoredFile(string bits, string filePath)
        {
            try
            {
                string text = ConvertBitsToText(bits);

                string directory = Path.GetDirectoryName(Path.GetFullPath(filePath))!;

                string fileName = Path.GetFileNameWithoutExtension(filePath);

                fileName = fileName.Replace("EncodedInjected", "");
                fileName = fileName.Replace("Encoded", "");
                fileName = fileName.Replace("Injected", "");

                string outputPath = Path.Combine(directory, fileName + "Restored.txt");
                File.WriteAllText(outputPath, text, Encoding.ASCII);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }


        //Main function to restore a file
        internal bool RestoreFile(string filePath)
        {
            try
            {
                List<string> blocks = ReadEncodedFile(filePath);
                List<string> correctedBlocks = CorrectEncodedBlocks(blocks);
                string restoredBits = DecodeBlocks(correctedBlocks);

                return WriteRestoredFile(restoredBits, filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }
    }
}
