namespace ImageHasher.Tests
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;

    using NUnit.Framework;

    using Common;

    [TestFixture]
    public class ImageUtilsTests
    {
        public static Image ImageFullSize = Image.FromFile(IOUtils.MapServerPath(
            "~/Tests/Images/Alyson.jpg"));

        public static Image ImageGrayscale = Image.FromFile(IOUtils.MapServerPath(
            "~/Tests/Images/Alyson-9x8-Gray.jpg"));

        [Test]
        public void ReduceImage()
        {
            using (var image = ImageUtils.ReduceImage(ImageFullSize, 9, 8))
            {
                image.Save(
                    IOUtils.MapServerPath("~/Tests/Images/Alyson-9x8-Color-Generated.jpg"),
                    ImageFormat.Jpeg);
            }

            Image reducedImage = Image.FromFile(IOUtils.MapServerPath(
                "~/Tests/Images/Alyson-9x8-Color-Generated.jpg"));

            Image expectedImage = Image.FromFile(IOUtils.MapServerPath(
                "~/Tests/Images/Alyson-9x8-Color.jpg"));

            ulong reducedImageHash = ImageHasher.ComputeDifferenceHash(reducedImage);
            ulong expectedImageHash = ImageHasher.ComputeDifferenceHash(expectedImage);

            Assert.That(reducedImageHash, Is.EqualTo(expectedImageHash));

            double similarity = ImageHasher.ComputeSimilarity(reducedImageHash, expectedImageHash);
            int distance = ImageHasher.ComputeHammingDistance(reducedImageHash, expectedImageHash);

            Assert.That(similarity, Is.EqualTo(1.0));
            Assert.That(distance, Is.EqualTo(0));
        }

        [Test]
        public void GrayscaleImage()
        {
            using (var image = ImageUtils.GrayscaleImage(ImageUtils.ReduceImage(ImageFullSize, 9, 8)))
            {
                image.Save(
                    IOUtils.MapServerPath("~/Tests/Images/Alyson-9x8-Gray-Generated.jpg"),
                    ImageFormat.Jpeg);
            }

            Image grayscaleImage = Image.FromFile(IOUtils.MapServerPath(
                "~/Tests/Images/Alyson-9x8-Gray-Generated.jpg"));

            Image expectedImage = Image.FromFile(IOUtils.MapServerPath(
                "~/Tests/Images/Alyson-9x8-Gray.jpg"));

            ulong grayscaleImageHash = ImageHasher.ComputeDifferenceHash(grayscaleImage);
            ulong expectedImageHash = ImageHasher.ComputeDifferenceHash(expectedImage);

            Assert.That(grayscaleImageHash, Is.EqualTo(expectedImageHash));

            double similarity = ImageHasher.ComputeSimilarity(grayscaleImageHash, expectedImageHash);
            int distance = ImageHasher.ComputeHammingDistance(grayscaleImageHash, expectedImageHash);

            Assert.That(similarity, Is.EqualTo(1.0));
            Assert.That(distance, Is.EqualTo(0));
        }

        [Test]
        public void GetGrayscaleBytes()
        {
            //
        }
    }
}