/* ------------------------------------------------------------------------- */
///
/// ImageResizerTest.cs
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
using System.Collections.Generic;
using NUnit.Framework;
using IoEx = Alphaleonis.Win32.Filesystem;

namespace Cube.Images.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// ImageResizerTest
    /// 
    /// <summary>
    /// ImageResizer のテスト用クラスです。
    /// </summary>
    /// 
    /* --------------------------------------------------------------------- */
    [Parallelizable]
    [TestFixture]
    class ImageResizerTest : FileResource
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Resized_Heigh
        ///
        /// <summary>
        /// Width を設定してリサイズ処理を実行するテストです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase( 256,  true,  true, ExpectedResult =  256)]
        [TestCase( 256, false,  true, ExpectedResult =  512)]
        [TestCase(1024,  true, false, ExpectedResult = 1024)]
        [TestCase(1024,  true,  true, ExpectedResult =  512)]
        public int Resized_Height(int width, bool preserve, bool shrink)
        {
            var resizer = Create();

            resizer.PreserveAspectRatio = preserve;
            resizer.ShrinkOnly = shrink;
            resizer.Width = width;
            resizer.Save(CreateFilePath(resizer, "resized"));

            return resizer.Resized.Height;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ResizeMode
        ///
        /// <summary>
        /// ResizeMode のテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase(ImageResizeMode.Default,     256)]
        [TestCase(ImageResizeMode.HighQuality, 256)]
        [TestCase(ImageResizeMode.HighSpeed,   256)]
        public void ResizeMode(ImageResizeMode mode, int width)
        {
            var resizer = Create();
            resizer.ResizeMode = mode;
            resizer.Width = width;

            var dest = CreateFilePath(resizer, "mode");
            resizer.Save(dest);

            Assert.That(IoEx.File.Exists(dest));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Save_Jpeg
        ///
        /// <summary>
        /// JPEG 形式で保存するテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase( 25, 256)]
        [TestCase( 50, 256)]
        [TestCase( 75, 256)]
        [TestCase(100, 256)]
        public void Save_Jpeg(long quality, int width)
        {
            var resizer = Create();
            resizer.Width = width;

            var dest = CreateFilePath(resizer, $"quality{quality}", ".jpg");
            resizer.Save(dest, Jpeg.Format, Jpeg.Quality(quality));

            Assert.That(IoEx.File.Exists(dest));
        }

        #endregion

        #region Helper methods

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// ImageResizer オブジェクトを生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private ImageResizer Create(string filename = "lena.png")
            => new ImageResizer(IoEx.Path.Combine(Examples, filename));

        /* ----------------------------------------------------------------- */
        ///
        /// CreateFilePath
        ///
        /// <summary>
        /// 保存用ファイルパスを生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string CreateFilePath(ImageResizer resizer, string prefix, string extension = ".png")
        {
            var items = new List<string>();

            items.Add(prefix);
            items.Add($"w{resizer.Width}");
            items.Add($"h{resizer.Height}");
            items.Add(resizer.ResizeMode.ToString().ToLower());

            if (resizer.PreserveAspectRatio) items.Add("preserve");
            if (resizer.ShrinkOnly) items.Add("shrink");

            return IoEx.Path.Combine(Results, $"{string.Join("-", items)}{extension}");
        }

        #endregion
    }
}
