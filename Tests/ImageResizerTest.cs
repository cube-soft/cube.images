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
    [TestFixture]
    class ImageResizerTest : FileResource
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Resized_Height
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
            var filename = "lena.png";
            using (var resizer = Create(filename))
            {
                var ext = IoEx.Path.GetExtension(filename);
                resizer.PreserveAspectRatio = preserve;
                resizer.ShrinkOnly = shrink;
                resizer.Width = width;
                resizer.Save(CreateFilePath(resizer, "resized", ext));

                return resizer.Resized.Height;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Save_Stream
        ///
        /// <summary>
        /// Stream に書き出すテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("lena.png",       600, ExpectedResult = 801429L)]
        [TestCase("lena-24bpp.jpg", 600, ExpectedResult =  37817L)]
        [TestCase("lena.png",       512, ExpectedResult = 801429L)]
        [TestCase("lena-24bpp.jpg", 512, ExpectedResult =  37817L)]
        [TestCase("lena.png",       256, ExpectedResult = 197792L)]
        [TestCase("lena-24bpp.jpg", 256, ExpectedResult =  12540L)]
        [TestCase("lena.png",       128, ExpectedResult =  50941L)]
        [TestCase("lena-24bpp.jpg", 128, ExpectedResult =   4690L)]
        public long Save_Stream(string filename, int width)
        {
            using (var dest = new System.IO.MemoryStream())
            using (var resizer = Create(filename))
            {
                resizer.ResizeMode = ImageResizeMode.HighQuality;
                resizer.PreserveAspectRatio = true;
                resizer.ShrinkOnly = true;
                resizer.Width = width;
                resizer.Save(dest);
                return dest.Length;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Save_File_Jpeg
        ///
        /// <summary>
        /// JPEG 形式で Stream に書き出すテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase(100, 256, ExpectedResult = 59309L)]
        [TestCase( 75, 256, ExpectedResult = 12389L)]
        [TestCase( 50, 256, ExpectedResult =  8548L)]
        [TestCase( 25, 256, ExpectedResult =  5763L)]
        public long Save_Stream_Jpeg(long quality, int width)
        {
            var filename = "lena.png";
            using (var dest = new System.IO.MemoryStream())
            using (var resizer = Create(filename))
            {
                resizer.ResizeMode = ImageResizeMode.HighQuality;
                resizer.Width = width;
                resizer.Save(dest, Jpeg.Format, Jpeg.Quality(quality));
                return dest.Length;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Save_ResizeMode
        ///
        /// <summary>
        /// ResizeMode を変更して保存するテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase(ImageResizeMode.Default)]
        [TestCase(ImageResizeMode.HighQuality)]
        [TestCase(ImageResizeMode.HighSpeed)]
        public void Save_ResizeMode(ImageResizeMode mode)
        {
            var filename = "lena.png";
            using (var resizer = Create(filename))
            {
                resizer.ResizeMode = mode;
                resizer.Width = 256;

                var ext  = IoEx.Path.GetExtension(filename);
                var dest = CreateFilePath(resizer, "mode", ext);
                resizer.Save(dest);

                Assert.That(IoEx.File.Exists(dest));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Overwrite
        ///
        /// <summary>
        /// 上書きのテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("lena.png")]
        [TestCase("lena-24bpp.jpg")]
        public void Overwrite(string filename)
        {
            using (var resizer = Create(filename))
            {
                resizer.Width = 128;

                var ext  = IoEx.Path.GetExtension(filename);
                var dest = CreateFilePath(resizer, "overwrite", ext);
                resizer.Save(dest);
                resizer.Save(dest); // overwrite

                Assert.That(IoEx.File.Exists(dest));
            }
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
        private ImageResizer Create(string filename)
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
        private string CreateFilePath(ImageResizer resizer, string prefix, string extension)
        {
            var items = new List<string>();

            items.Add(prefix);
            items.Add($"w{resizer.Width}");
            items.Add($"h{resizer.Height}");
            items.Add(resizer.ResizeMode.ToString().ToLower());

            if (resizer.PreserveAspectRatio) items.Add("preserve");
            if (resizer.ShrinkOnly) items.Add("shrink");

            return IoEx.Path.Combine(Results, $"{string.Join("-", items.ToArray())}{extension}");
        }

        #endregion
    }
}
