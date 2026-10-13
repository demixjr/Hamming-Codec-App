namespace HammingCodeApp
{
    internal class HammingCodec
    {
        // Calculate the number of parity bits
        private int CalculateParityBits(int dataLength)
        {
            int parityBits = 0;

            while (Math.Pow(2, parityBits) <
                   dataLength + parityBits + 1)
            {
                parityBits++;
            }

            return parityBits;
        }

        // Encode data using Hamming code
        internal byte[] EncodeData(byte[] data)
        {
            foreach (byte bit in data)
            {
                if (bit != 0 && bit != 1)
                {
                    throw new ArgumentException(
                        "Data must contain only 0 and 1.");
                }
            }

            int dataLength = data.Length;
            int parityBits = CalculateParityBits(dataLength);
            int totalLength = dataLength + parityBits;

            byte[] encodedData = new byte[totalLength];

            // Place data bits and reserve parity positions
            for (int i = 0, j = 0; i < totalLength; i++)
            {
                int position = i + 1;

                if ((position & (position - 1)) == 0)
                {
                    encodedData[i] = 0;
                }
                else
                {
                    encodedData[i] = data[j++];
                }
            }

            // Calculate parity bits
            for (int i = 0; i < parityBits; i++)
            {
                int parityPosition = (int)Math.Pow(2, i);
                byte parityValue = 0;

                for (int j = 1; j <= totalLength; j++)
                {
                    if ((j & parityPosition) != 0)
                    {
                        parityValue ^= encodedData[j - 1];
                    }
                }

                encodedData[parityPosition - 1] = parityValue;
            }

            return encodedData;
        }

        // Decode data from an encoded block
        internal byte[] DecodeData(byte[] encodedData)
        {
            ValidateBits(encodedData);

            int parityBits = 0;

            while ((1 << parityBits) <= encodedData.Length)
            {
                parityBits++;
            }

            int dataLength = encodedData.Length - parityBits;

            if (dataLength < 0)
            {
                throw new ArgumentException("Invalid encoded block length.");
            }

            byte[] decodedData = new byte[dataLength];

            for (int i = 0, j = 0; i < encodedData.Length; i++)
            {
                int position = i + 1;

                // Position skip
                if ((position & (position - 1)) != 0)
                {
                    decodedData[j++] = encodedData[i];
                }
            }

            return decodedData;
        }

        // Calculate the syndrome
        internal int CalculateSyndrome(byte[] encodedData)
        {
            ValidateBits(encodedData);

            int syndrome = 0;
            int totalLength = encodedData.Length;

            for (int parityPosition = 1; parityPosition <= totalLength; parityPosition <<= 1)
            {
                byte parityValue = 0;

                for (int j = 1; j <= totalLength; j++)
                {
                    if ((j & parityPosition) != 0)
                    {
                        parityValue ^= encodedData[j - 1];
                    }
                }

                if (parityValue != 0)
                {
                    syndrome |= parityPosition;
                }
            }

            return syndrome;
        }

        // Correct a single-bit error
        internal void CorrectErrors(byte[] encodedData, int syndrome)
        {
            if (syndrome == 0)
            {
                return;
            }

            if (syndrome < 1 || syndrome > encodedData.Length)
            {
                throw new ArgumentException("Invalid syndrome: error position is outside the block.");
            }

            encodedData[syndrome - 1] ^= 1;
        }

        //Validate that array contains only bits
        private void ValidateBits(byte[] data)
        {
            foreach (byte bit in data)
            {
                if (bit != 0 && bit != 1)
                {
                    throw new ArgumentException("Data must contain only 0 and 1.");
                }
            }
        }
    }
}
