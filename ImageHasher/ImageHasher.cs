namespace ImageHasher
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Drawing.Drawing2D;

    using Common;

    /// <summary>
    /// A static class to handle hashing images
    /// </summary>
    public static class ImageHasher
    {
        /// <summary>
        /// Pre-computed set bit counts for all unsigned 8-bit integers 
        /// </summary>
        private static byte[] BitCounts8 = new byte[byte.MaxValue + 1];

        /// <summary>
        /// Pre-computed set bit counts for all unsigned 16-bit integers
        /// </summary>
        private static ushort[] BitCounts16 = new ushort[ushort.MaxValue + 1];

        static ImageHasher()
        {
            // Pre-compute set bit counts for all unsigned 8-bit integers
            for (int i = 0; i < (byte.MaxValue + 1); i++)
            {
                BitCounts8[i] = (byte)CountSetBits(i);
            }

            // Pre-compute set bit counts for all unsigned 16-bit integers
            // using the SetBitCounts8 table for better performance 
            for (int i = 0; i < (ushort.MaxValue + 1); i++)
            {
                int c = 0;
                for (int n = i; n > 0; n >>= 8)
                {
                    c += BitCounts8[(n & 0xFF)];
                }
                BitCounts16[i] = (ushort)c;
            }
        }

        /// <summary>
        /// Count the number of bits set to 1 in a 32-bit integer
        /// </summary>
        private static int CountSetBits(int n)
        {
            return CountSetBits((ulong)n);
        }

        /// <summary>
        /// Count the number of bits set to 1 in a 64-bit integer
        /// </summary>
        private static int CountSetBits(ulong n)
        {
            int c = 0;
            for (; n > 0; n >>= 1)
            {
                if ((n & 1) == 1)
                {
                    c++;
                }
            }
            return c;
        }

        /// <summary>
        /// Calculates the Hamming Distance between two numbers.
        /// </summary>
        public static int ComputeHammingDistance(ulong n1, ulong n2)
        {
            int distance = 0;

            // XOR the numbers to get the bits that differ
            ulong n3 = n1 ^ n2;

            // Mask each 16-bit segment and lookup the bit count in the
            // BitCounts table, which is a pre-computed table of bit counts,
            // and then sum them together to get the total distance.
            distance = (int)(
                  BitCounts16[(n3 & 0xFFFF000000000000) >> 48]
                + BitCounts16[(n3 & 0xFFFF00000000) >> 32]
                + BitCounts16[(n3 & 0xFFFF0000) >> 16]
                + BitCounts16[(n3 & 0xFFFF)]);

            return distance;
        }

        /// <summary>
        /// Compares two hashes and returns a percentage-based
        /// Hamming Distance. The higher the percentage
        /// the closer the hashes are to being identical.
        /// </summary>
        public static double ComputeSimilarity(ulong hash1, ulong hash2)
        {
            return ((64 - ComputeHammingDistance(hash1, hash2)) * (1.0 / 64));
        }

        /// <summary>
        /// Implementation of the Average Hash algorithm
        /// hackerfactor.com/blog/index.php?/archives/432-Looks-Like-It.html
        ///
        /// This approach crushes the image into a grayscale 8x8 image and 
        /// sets the 64 bits in the hash based on whether the pixel's value 
        /// is greater than the average color for the image.
        /// </summary>
        public static ulong ComputeAverageHash(Image image)
        {
            byte[] bytes;

            ulong hash  = 0;
            int average = 0;
            int width   = 8;
            int height  = 8;

            using (Bitmap reducedImage = ImageUtils.ReduceImage(image, width, height))
            {
                bytes = ImageUtils.GetGrayscaleBytes(reducedImage);
            }

            foreach (byte n in bytes)
            {
                average += n;
            }

            average /= bytes.Length;

            for (int i = 0; i < bytes.Length; i++)
            {
                if (bytes[i] >= average)
                {
                    hash |= 1UL << (63 - i);
                }
            }

            return hash;
        }

        /// <summary>
        /// Implementation of the Difference Hash algorithm
        /// hackerfactor.com/blog/index.php?archives/529-Kind-of-Like-That.html
        /// </summary>
        public static ulong ComputeDifferenceHash(Image image)
        {
            byte[] bytes;

            ulong hash = 0;
            int width  = 9;
            int height = 8;
            int pos    = 0;

            using (Bitmap reducedImage = ImageUtils.ReduceImage(image, width, height))
            {
                bytes = ImageUtils.GetGrayscaleBytes(reducedImage);
            }

            for (int i = 0; i < bytes.Length; i++)
            {
                // Only compare adjacent pixels
                if (0 == i || 0 != (i + 1) % width)
                {
                    if (bytes[i] > bytes[i + 1])
                    {
                        hash |= 1UL << (63 - pos);
                    }

                    pos++;
                }
            }

            return hash;
        }
    }
}