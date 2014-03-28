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
        public static Image TestImage =
            Image.FromFile(IOUtils.MapServerPath("~/Tests/Images/Alyson.jpg"));

        [Test]
        public void ComputeAverageHash()
        {
            ulong hash = ImageHasher.ComputeAverageHash(TestImage);

            Assert.That(hash, Is.EqualTo(16701559407061564136));
        }

        [Test]
        public void ComputeDifferenceHash()
        {
            ulong hash = ImageHasher.ComputeDifferenceHash(TestImage);

            Assert.That(hash, Is.EqualTo(8100370218449873837));
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