/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2010 CubeSoft, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
/* ------------------------------------------------------------------------- */
using Cube.Images.BuiltIn;
using NUnit.Framework;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace Cube.Images.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// ImageTest
    ///
    /// <summary>
    /// Image オブジェクトへの拡張メソッドのテストを行うクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class ImageTest : FileHelper
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// GetColorDepth
        ///
        /// <summary>
        /// ビット深度を取得するテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("alpha.png",      ExpectedResult = 32)]
        [TestCase("lena.png",       ExpectedResult = 32)]
        [TestCase("lena-24bpp.jpg", ExpectedResult = 24)]
        [TestCase("lena-24bpp.png", ExpectedResult = 24)]
        [TestCase("lena-8bpp.png",  ExpectedResult =  8)]
        [TestCase("lena-4bpp.png",  ExpectedResult =  4)]
        [TestCase("gray-16bpp.png", ExpectedResult = 24)]
        [TestCase("gray-8bpp.png",  ExpectedResult =  8)]
        [TestCase("gray-4bpp.png",  ExpectedResult =  4)]
        [TestCase("gray-1bpp.png",  ExpectedResult =  1)]
        public int GetColorDepth(string filename)
        {
            using (var image = new Bitmap(Example(filename))) return image.GetColorDepth();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetColorDepth_Null
        ///
        /// <summary>
        /// null オブジェクトに対して GetColorDepth を実行した時の挙動を
        /// 確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void GetColorDepth_Null()
        {
            var image = default(Bitmap);
            Assert.That(image.GetColorDepth(), Is.EqualTo(0));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetRgbFormat
        ///
        /// <summary>
        /// RGB で表現する場合の PixelFormat を取得するテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("alpha.png",      ExpectedResult = PixelFormat.Format32bppArgb)]
        [TestCase("lena.png",       ExpectedResult = PixelFormat.Format32bppArgb)]
        [TestCase("lena-24bpp.jpg", ExpectedResult = PixelFormat.Format24bppRgb )]
        [TestCase("lena-24bpp.png", ExpectedResult = PixelFormat.Format24bppRgb )]
        [TestCase("lena-8bpp.png",  ExpectedResult = PixelFormat.Format32bppArgb)]
        [TestCase("lena-4bpp.png",  ExpectedResult = PixelFormat.Format32bppArgb)]
        [TestCase("gray-16bpp.png", ExpectedResult = PixelFormat.Format24bppRgb )]
        [TestCase("gray-8bpp.png",  ExpectedResult = PixelFormat.Format32bppArgb)]
        [TestCase("gray-4bpp.png",  ExpectedResult = PixelFormat.Format32bppArgb)]
        [TestCase("gray-1bpp.png",  ExpectedResult = PixelFormat.Format32bppArgb)]
        public PixelFormat GetRgbFormat(string filename)
        {
            using (var image = new Bitmap(Example(filename))) return image.GetRgbFormat();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetRgbFormat_Null
        ///
        /// <summary>
        /// null オブジェクトに対して GetRgbFormat を実行した時の挙動を
        /// 確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void GetRgbFormat_Null()
        {
            var image = default(Bitmap);
            Assert.That(image.GetRgbFormat(), Is.EqualTo(PixelFormat.Undefined));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// IsIndexedColor
        ///
        /// <summary>
        /// インデックスカラーかどうかを判別するテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("alpha.png",      ExpectedResult = false)]
        [TestCase("lena.png",       ExpectedResult = false)]
        [TestCase("lena-24bpp.jpg", ExpectedResult = false)]
        [TestCase("lena-24bpp.png", ExpectedResult = false)]
        [TestCase("lena-8bpp.png",  ExpectedResult =  true)]
        [TestCase("lena-4bpp.png",  ExpectedResult =  true)]
        [TestCase("gray-16bpp.png", ExpectedResult = false)]
        [TestCase("gray-8bpp.png",  ExpectedResult =  true)]
        [TestCase("gray-4bpp.png",  ExpectedResult =  true)]
        [TestCase("gray-1bpp.png",  ExpectedResult =  true)]
        public bool IsIndexedColor(string filename)
        {
            using (var image = new Bitmap(Example(filename))) return image.IsIndexedColor();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// IsFormat
        ///
        /// <summary>
        /// 指定された ImageFormat かどうかを判別するテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCaseSource(nameof(IsFormat_TestCases))]
        public bool IsFormat(string filename, ImageFormat format)
        {
            using (var image = new Bitmap(Example(filename))) return image.IsFormat(format);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// IsFormat_Null
        ///
        /// <summary>
        /// null オブジェクトに対して IsFormat を実行した時の挙動を
        /// 確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void IsFormat_Null()
        {
            var image = default(Bitmap);
            Assert.That(image.IsFormat(ImageFormat.Png), Is.False);
        }

        private static IEnumerable<TestCaseData> IsFormat_TestCases
        {
            get
            {
                yield return new TestCaseData("lena.png", ImageFormat.Png).Returns(true);
                yield return new TestCaseData("lena.png", ImageFormat.Tiff).Returns(false);
                yield return new TestCaseData("lena-24bpp.jpg", ImageFormat.Jpeg).Returns(true);
            }
        }

        #endregion
    }
}
