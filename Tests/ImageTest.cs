/* ------------------------------------------------------------------------- */
///
/// ImageResizer.cs
/// 
/// Copyright (c) 2010 CubeSoft, Inc.
/// 
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
///
///  http://www.apache.org/licenses/LICENSE-2.0
///
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.
///
/* ------------------------------------------------------------------------- */
using System.Drawing;
using System.Drawing.Imaging;
using Cube.Images.BuiltIn;
using NUnit.Framework;
using IoEx = Alphaleonis.Win32.Filesystem;

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
    class ImageTest : FileResource
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
        [TestCase("grey-8bpp.png",  ExpectedResult =  8)]
        [TestCase("grey-4bpp.png",  ExpectedResult =  4)]
        [TestCase("grey-1bpp.png",  ExpectedResult =  1)]
        public int GetColorDepth(string filename)
        {
            using (var image = Create(filename)) return image.GetColorDepth();
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
        [TestCase("lena-24bpp.jpg", ExpectedResult = PixelFormat.Format24bppRgb)]
        [TestCase("lena-24bpp.png", ExpectedResult = PixelFormat.Format24bppRgb)]
        [TestCase("lena-8bpp.png",  ExpectedResult = PixelFormat.Format32bppArgb)]
        [TestCase("lena-4bpp.png",  ExpectedResult = PixelFormat.Format32bppArgb)]
        [TestCase("grey-8bpp.png",  ExpectedResult = PixelFormat.Format32bppArgb)]
        [TestCase("grey-4bpp.png",  ExpectedResult = PixelFormat.Format32bppArgb)]
        [TestCase("grey-1bpp.png",  ExpectedResult = PixelFormat.Format32bppArgb)]
        public PixelFormat GetRgbFormat(string filename)
        {
            using (var image = Create(filename)) return image.GetRgbFormat();
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
        [TestCase("lena-8bpp.png",  ExpectedResult = true)]
        [TestCase("lena-4bpp.png",  ExpectedResult = true)]
        [TestCase("grey-8bpp.png",  ExpectedResult = true)]
        [TestCase("grey-4bpp.png",  ExpectedResult = true)]
        [TestCase("grey-1bpp.png",  ExpectedResult = true)]
        public bool IsIndexedColor(string filename)
        {
            using (var image = Create(filename)) return image.IsIndexedColor();
        }

        #endregion

        #region Helper methods

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Image オブジェクトを生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private Image Create(string filename)
            => new Bitmap(IoEx.Path.Combine(Examples, filename));

        #endregion
    }
}
