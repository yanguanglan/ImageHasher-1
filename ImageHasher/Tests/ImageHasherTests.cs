namespace ImageHasher.Tests
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;

    using NUnit.Framework;

    using Common;

    [TestFixture]
    public class ImageHashTests
    {
        public static Image ImageA = Image.FromFile(IOUtils.MapServerPath(
            "~/Tests/Images/Alyson.jpg"));

        public static Image ImageB = Image.FromFile(IOUtils.MapServerPath(
            "~/Tests/Images/Alyson-9x8-Color.jpg"));

        public static Image ImageC = Image.FromFile(IOUtils.MapServerPath(
            "~/Tests/Images/Alyson-9x8-Gray.jpg"));

        [Test]
        public void ComputeAverageHash()
        {
            ulong hash = ImageHasher.ComputeAverageHash(ImageA);

            Assert.That(hash, Is.EqualTo(16701559407061564136));
        }

        [Test]
        public void ComputeDifferenceHash()
        {
            ulong hash = ImageHasher.ComputeDifferenceHash(ImageA);

            Assert.That(hash, Is.EqualTo(8100370218449873837));
        }

        [Test]
        public void ComputeHammingDistanceWithAverageHash()
        {
            ulong imageHashA = ImageHasher.ComputeAverageHash(ImageA);
            ulong imageHashC = ImageHasher.ComputeAverageHash(ImageC);

            int distanceAA = ImageHasher.ComputeHammingDistance(imageHashA, imageHashA);
            int distanceAC = ImageHasher.ComputeHammingDistance(imageHashA, imageHashC);

            Assert.That(distanceAA, Is.EqualTo(0));
            Assert.That(distanceAC, Is.EqualTo(10));
        }

        [Test]
        public void ComputeHammingDistanceWithDifferenceHash()
        {
            ulong imageHashB = ImageHasher.ComputeDifferenceHash(ImageB);
            ulong imageHashC = ImageHasher.ComputeDifferenceHash(ImageC);

            int distanceBB = ImageHasher.ComputeHammingDistance(imageHashB, imageHashB);
            int distanceBC = ImageHasher.ComputeHammingDistance(imageHashB, imageHashC);

            Assert.That(distanceBB, Is.EqualTo(0));
            Assert.That(distanceBC, Is.EqualTo(1));
        }

        [Test]
        public void ComputeSimilarityWithAverageHash()
        {
            ulong imageHashA = ImageHasher.ComputeAverageHash(ImageA);
            ulong imageHashC = ImageHasher.ComputeAverageHash(ImageC);

            double similarityAA = ImageHasher.ComputeSimilarity(imageHashA, imageHashA);
            double similarityAC = ImageHasher.ComputeSimilarity(imageHashA, imageHashC);

            Assert.That(similarityAA, Is.EqualTo(1.0));
            Assert.That(similarityAC, Is.InRange<double>(0.8, 0.85));
        }

        [Test]
        public void ComputeSimilarityWithDifferenceHash()
        {
            ulong imageHashB = ImageHasher.ComputeDifferenceHash(ImageB);
            ulong imageHashC = ImageHasher.ComputeDifferenceHash(ImageC);

            double similarityBB = ImageHasher.ComputeSimilarity(imageHashB, imageHashB);
            double similarityBC = ImageHasher.ComputeSimilarity(imageHashB, imageHashC);

            Assert.That(similarityBB, Is.EqualTo(1.0));
            Assert.That(similarityBC, Is.InRange<double>(0.95, 1.0));
        }

        /*
        [Test]
        public void TestReduceAndGrayscaleImage()
        {
            Bitmap color = ImageUtils.ReduceImage(TestImage, 9, 8);
            string colorPath = IOUtils.MapServerPath("~/Tests/Images/Alyson-9x8-Color.jpg");

            color.Save(colorPath, ImageFormat.Jpeg);

            Bitmap gray = ImageUtils.GrayscaleImage(color);
            string grayPath = IOUtils.MapServerPath("~/Tests/Images/Alyson-9x8-Gray.jpg");

            gray.Save(grayPath, ImageFormat.Jpeg);

            color.Dispose();
            gray.Dispose();
        }
        */
    }
}