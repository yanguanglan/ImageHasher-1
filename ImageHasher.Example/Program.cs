namespace ImageHasher.Example
{
    using System;
    using System.Drawing;
    using System.IO;

    class Program
    {
        static void Main(string[] args)
        {
            if (args == null || args.Length < 2)
            {
                Console.WriteLine("not enough arguments");
            }
            else if (!File.Exists(args[0]))
            {
                Console.WriteLine("{0} does not exist", args[0]);
            }
            else if (!File.Exists(args[1]))
            {
                Console.WriteLine("{0} does not exist", args[1]);
            }
            else
            {
                string imageAPath = args[0];
                string imageBPath = args[1];

                Image imageA = null;
                Image imageB = null;

                try
                {
                    imageA = Image.FromFile(imageAPath);
                    imageB = Image.FromFile(imageBPath);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                if (imageA != null && imageB != null)
                {
                    ulong imageAHash = ImageHasher.ComputeDifferenceHash(imageA);
                    ulong imageBHash = ImageHasher.ComputeDifferenceHash(imageB);

                    Console.WriteLine(
                        "{0}: {1}", Path.GetFileName(imageAPath), imageAHash);
                    Console.WriteLine(
                        "{0}: {1}", Path.GetFileName(imageBPath), imageBHash);

                    Console.WriteLine(
                        "Similarity: {0}",
                        ImageHasher.ComputeSimilarity(imageAHash, imageBHash));
                    Console.WriteLine(
                        "Hamming Distance: {0}",
                        ImageHasher.ComputeHammingDistance(imageAHash, imageBHash));
                }
            }
        }
    }
}
