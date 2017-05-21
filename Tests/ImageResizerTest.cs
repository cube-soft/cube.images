/* ------------------------------------------------------------------------- */
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
using System;
using System.Collections.Generic;
using System.Drawing;
using NUnit.Framework;
using IoEx = System.IO;

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
        /// AspectRatio
        ///
        /// <summary>
        /// 縦横比を取得するテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("lena.png",       1.000)]
        [TestCase("portrait.png",   1.146)]
        [TestCase("landscape.png",  3.813)]
        public void AspectRatio(string filename, double expected)
        {
            using (var resizer = new ImageResizer(Example(filename)))
            {
                Assert.That(resizer.AspectRatio, Is.EqualTo(expected).Within(0.01));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Resized_Width
        ///
        /// <summary>
        /// Width を設定してリサイズ処理を実行するテストです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("lena.png",      256,  true,  true, ExpectedResult =  256)]
        [TestCase("lena.png",      256, false,  true, ExpectedResult =  512)]
        [TestCase("lena.png",     1024,  true, false, ExpectedResult = 1024)]
        [TestCase("lena.png",     1024,  true,  true, ExpectedResult =  512)]
        [TestCase("portrait.png",  128,  true,  true, ExpectedResult =  146)]
        [TestCase("landscape.png", 128,  true,  true, ExpectedResult =   33)]
        public int Resized_Width(string filename, int width, bool preserve, bool shrink)
        {
            using (var resizer = new ImageResizer(Example(filename)))
            {
                var ext = IoEx.Path.GetExtension(filename);
                resizer.PreserveAspectRatio = preserve;
                resizer.ShrinkOnly = shrink;
                resizer.Width = width;
                resizer.Save(SavePath(resizer, "wbase", ext));

                return resizer.Resized.Height;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Resized_Height
        ///
        /// <summary>
        /// Height を設定してリサイズ処理を実行するテストです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("lena.png",      128, ExpectedResult = 128)]
        [TestCase("portrait.png",  128, ExpectedResult = 111)]
        [TestCase("landscape.png",  32, ExpectedResult = 122)]
        public int Resized_Height(string filename, int height)
        {
            using (var resizer = new ImageResizer(Example(filename)))
            {
                var ext = IoEx.Path.GetExtension(filename);
                resizer.PreserveAspectRatio = true;
                resizer.ShrinkOnly = true;
                resizer.Height = height;
                resizer.Save(SavePath(resizer, "hbase", ext));

                return resizer.Resized.Width;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Resized_LongSide
        ///
        /// <summary>
        /// LongSide を設定してリサイズ処理を実行するテストです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase(256, 256, 128, ExpectedResult = 128)]
        [TestCase(128, 256, 128, ExpectedResult =  64)]
        [TestCase( 16,  32, 128, ExpectedResult =  16)]
        public int Resized_LongSide(int width, int height, int size)
        {
            using (var resizer = new ImageResizer(new Bitmap(width, height)))
            {
                resizer.PreserveAspectRatio = true;
                resizer.ShrinkOnly = true;
                resizer.LongSide = size;
                return resizer.Resized.Width;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Resized_ShortSide
        ///
        /// <summary>
        /// ShortSide を設定してリサイズ処理を実行するテストです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase(256, 256, 128, ExpectedResult = 128)]
        [TestCase(512, 256, 128, ExpectedResult = 256)]
        [TestCase(128,  64, 128, ExpectedResult = 128)]
        public int Resized_ShortSide(int width, int height, int size)
        {
            using (var resizer = new ImageResizer(new Bitmap(width, height)))
            {
                resizer.PreserveAspectRatio = true;
                resizer.ShrinkOnly = true;
                resizer.ShortSide = size;
                return resizer.Resized.Width;
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
        [TestCase("lena.png",       600, 801000L)]
        [TestCase("lena-24bpp.jpg", 600,  37000L)]
        [TestCase("lena.png",       512, 801000L)]
        [TestCase("lena-24bpp.jpg", 512,  37000L)]
        [TestCase("lena.png",       256, 197000L)]
        [TestCase("lena-24bpp.jpg", 256,  12000L)]
        [TestCase("lena.png",       128,  50000L)]
        [TestCase("lena-24bpp.jpg", 128,   4000L)]
        public void Save_Stream(string filename, int width, long expected)
        {
            using (var dest = new System.IO.MemoryStream())
            using (var src = IoEx.File.Open(Example(filename), IoEx.FileMode.Open, IoEx.FileAccess.Read))
            using (var resizer = new ImageResizer(src))
            {
                resizer.ResizeMode = ImageResizeMode.HighQuality;
                resizer.PreserveAspectRatio = true;
                resizer.ShrinkOnly = true;
                resizer.Width = width;
                resizer.Save(dest);
                Assert.That(dest.Length, Is.AtLeast(expected));
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
        [TestCase(100, 256, 59000L)]
        [TestCase( 75, 256, 12000L)]
        [TestCase( 50, 256,  8000L)]
        [TestCase( 25, 256,  5000L)]
        public void Save_Stream_Jpeg(long quality, int width, long expected)
        {
            var filename = "lena.png";
            using (var dest = new System.IO.MemoryStream())
            using (var resizer = new ImageResizer(Example(filename)))
            {
                resizer.ResizeMode = ImageResizeMode.HighQuality;
                resizer.Width = width;
                resizer.Save(dest, Jpeg.Format, Jpeg.Quality(quality));
                Assert.That(dest.Length, Is.AtLeast(expected));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Save_Stream_Png
        ///
        /// <summary>
        /// PNG 形式で Stream に書き出すテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase(512, 653000L)]
        [TestCase(256, 179000L)]
        [TestCase(128,  46000L)]
        public void Save_Stream_Png(int width, long expected)
        {
            var filename = "lena-24bpp.jpg";
            using (var dest = new System.IO.MemoryStream())
            using (var resizer = new ImageResizer(Example(filename)))
            {
                resizer.ResizeMode = ImageResizeMode.HighQuality;
                resizer.Width = width;
                resizer.Save(dest, Png.Format);
                Assert.That(dest.Length, Is.AtLeast(expected));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Save_Stream_Bmp
        ///
        /// <summary>
        /// BMP 形式で Stream に書き出すテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("lena.png",       60000L)]
        [TestCase("lena-24bpp.jpg", 40000L)]
        public void Save_Stream_Bmp(string filename, long expected)
        {
            using (var dest = new System.IO.MemoryStream())
            using (var resizer = new ImageResizer(Example(filename)))
            {
                resizer.ResizeMode = ImageResizeMode.HighQuality;
                resizer.Width = 128;
                resizer.Save(dest, Bmp.Format);
                Assert.That(dest.Length, Is.AtLeast(expected));
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
            using (var resizer = new ImageResizer(Example(filename)))
            {
                resizer.ResizeMode = mode;
                resizer.Width = 256;

                var ext  = IoEx.Path.GetExtension(filename);
                var dest = SavePath(resizer, "mode", ext);
                resizer.Save(dest);

                Assert.That(IoEx.File.Exists(dest));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Create_Null_Throws
        ///
        /// <summary>
        /// Image オブジェクトが null の場合の挙動を確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Create_Null_Throws()
            => Assert.That(() =>
            {
                using (var resizer = new ImageResizer(default(Bitmap)))
                {
                    Assert.Fail("never reached");
                }
            },
            Throws.TypeOf<ArgumentException>().And.Message.EqualTo("original"));

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
            using (var resizer = new ImageResizer(Example(filename)))
            {
                resizer.Width = 128;

                var ext  = IoEx.Path.GetExtension(filename);
                var dest = SavePath(resizer, "overwrite", ext);
                resizer.Save(dest);
                resizer.Save(dest); // overwrite

                Assert.That(IoEx.File.Exists(dest));
            }
        }

        #endregion

        #region Helper methods

        /* ----------------------------------------------------------------- */
        ///
        /// SavePath
        ///
        /// <summary>
        /// 保存用ファイルパスを生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string SavePath(ImageResizer resizer, string prefix, string extension)
        {
            var items = new List<string>
            {
                prefix,
                $"w{resizer.Width}",
                $"h{resizer.Height}",
                resizer.ResizeMode.ToString().ToLower()
            };
            if (resizer.PreserveAspectRatio) items.Add("preserve");
            if (resizer.ShrinkOnly) items.Add("shrink");

            return Result($"{string.Join("-", items)}{extension}");
        }

        #endregion
    }
}
