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
using Cube.Tests;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;

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
    class ImageResizerTest : FileFixture
    {
        #region Tests

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
            using (var resizer = new ImageResizer(GetSource(filename)))
            {
                var ext = IO.Get(filename).Extension;
                resizer.PreserveAspectRatio = preserve;
                resizer.ShrinkOnly = shrink;
                resizer.Width = width;
                resizer.Width = width; // ignore
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
        [TestCase("lena.png",     256,  true,  true, ExpectedResult =  256)]
        [TestCase("lena.png",     256, false,  true, ExpectedResult =  512)]
        [TestCase("lena.png",    1024,  true, false, ExpectedResult = 1024)]
        [TestCase("lena.png",    1024,  true,  true, ExpectedResult =  512)]
        [TestCase("portrait.png", 128,  true,  true, ExpectedResult =  111)]
        [TestCase("landscape.png", 32,  true,  true, ExpectedResult =  122)]
        public int Resized_Height(string filename, int height, bool preserve, bool shrink)
        {
            using (var resizer = new ImageResizer(GetSource(filename)))
            {
                var ext = IO.Get(filename).Extension;
                resizer.PreserveAspectRatio = preserve;
                resizer.ShrinkOnly = shrink;
                resizer.Height = height;
                resizer.Height = height; // ignore
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
        [TestCase(256, 256, 128, 128, 128)]
        [TestCase(128, 256, 128,  64,  64)]
        [TestCase( 16,  32, 128,  64,  16)]
        [TestCase(192, 576, 128,  42,  42)]
        [TestCase( 32,  96, 128,  42,  32)]
        public void Resized_LongSide(int w, int h, int ls, int ss, int expected)
        {
            using (var resizer = new ImageResizer(new Bitmap(w, h)))
            {
                resizer.PreserveAspectRatio = true;
                resizer.ShrinkOnly = true;
                resizer.LongSide = ls;

                var img = resizer.Resized;
                Assert.That(resizer.ShortSide, Is.EqualTo(ss));
                Assert.That(Math.Min(img.Width, img.Height), Is.EqualTo(expected));
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
        [TestCase(256, 256, 128, 128, 128)]
        [TestCase(512, 256, 128, 256, 256)]
        [TestCase(128,  64, 128, 256, 128)]
        [TestCase(192, 576, 128, 384, 384)]
        [TestCase( 32,  96, 128, 384,  96)]
        public void Resized_ShortSide(int w, int h, int ss, int ls, int expected)
        {
            using (var resizer = new ImageResizer(new Bitmap(w, h)))
            {
                resizer.PreserveAspectRatio = true;
                resizer.ShrinkOnly = true;
                resizer.ShortSide = ss;

                var img = resizer.Resized;
                Assert.That(resizer.LongSide, Is.EqualTo(ls));
                Assert.That(Math.Max(img.Width, img.Height), Is.EqualTo(expected));
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
            using (var resizer = new ImageResizer(IO.OpenRead(GetSource(filename))))
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
        /// Save_Jpeg
        ///
        /// <summary>
        /// JPEG 形式で保存するテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase(100)]
        [TestCase( 75)]
        [TestCase( 50)]
        [TestCase( 25)]
        public void Save_Jpeg(long quality)
        {
            var filename = "lena.png";
            using (var resizer = new ImageResizer(GetSource(filename)))
            {
                resizer.ResizeMode = ImageResizeMode.HighQuality;
                resizer.Width = 256;

                var dest = SavePath(resizer, "jpeg", ".jpg");
                resizer.Save(dest, Jpeg.Format, Jpeg.Quality(quality));
                Assert.That(IO.Exists(dest), Is.True);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Save_Png
        ///
        /// <summary>
        /// PNG 形式で保存するテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase(512)]
        [TestCase(256)]
        [TestCase(128)]
        public void Save_Png(int width)
        {
            var filename = "lena-24bpp.jpg";
            using (var resizer = new ImageResizer(GetSource(filename)))
            {
                resizer.ResizeMode = ImageResizeMode.HighQuality;
                resizer.Width = width;

                var dest = SavePath(resizer, "png", ".png");
                resizer.Save(dest, Png.Format);
                Assert.That(IO.Exists(dest), Is.True);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Save_Bmp
        ///
        /// <summary>
        /// BMP 形式で保存するテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("lena.png")]
        [TestCase("lena-24bpp.jpg")]
        public void Save_Bmp(string filename)
        {
            using (var resizer = new ImageResizer(GetSource(filename)))
            {
                resizer.ResizeMode = ImageResizeMode.HighQuality;
                resizer.Width = 128;

                var dest = SavePath(resizer, "bmp", ".bmp");
                resizer.Save(dest, Bmp.Format);
                Assert.That(IO.Exists(dest), Is.True);
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
            using (var resizer = new ImageResizer(GetSource(filename)))
            {
                resizer.ResizeMode = mode;
                resizer.Width = 256;

                var ext  = IO.Get(filename).Extension;
                var dest = SavePath(resizer, "mode", ext);
                resizer.Save(dest);

                Assert.That(IO.Exists(dest), Is.True);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Save_Overwrite
        ///
        /// <summary>
        /// 上書きのテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("lena.png")]
        [TestCase("lena-24bpp.jpg")]
        public void Save_Overwrite(string filename)
        {
            using (var resizer = new ImageResizer(GetSource(filename)))
            {
                resizer.Width = 128;

                var ext = IO.Get(filename).Extension;
                var dest = SavePath(resizer, "overwrite", ext);
                resizer.Save(dest);
                resizer.Save(dest); // overwrite

                Assert.That(IO.Exists(dest), Is.True);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// AspectRatio
        ///
        /// <summary>
        /// 縦横比を取得するテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("lena.png",      1.000)]
        [TestCase("portrait.png",  1.146)]
        [TestCase("landscape.png", 3.813)]
        public void AspectRatio(string filename, double expected)
        {
            using (var resizer = new ImageResizer(GetSource(filename)))
            {
                Assert.That(resizer.AspectRatio, Is.EqualTo(expected).Within(0.01));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ColorDepth
        ///
        /// <summary>
        /// ビット深度を取得するテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("alpha.png",      32, 32)]
        [TestCase("lena.png",       32, 32)]
        [TestCase("lena-24bpp.jpg", 24, 24)]
        [TestCase("lena-24bpp.png", 24, 24)]
        [TestCase("lena-8bpp.png",   8, 32)]
        [TestCase("lena-4bpp.png",   4, 32)]
        [TestCase("gray-16bpp.png", 24, 24)]
        [TestCase("gray-8bpp.png",   8, 32)]
        [TestCase("gray-4bpp.png",   4, 32)]
        [TestCase("gray-1bpp.png",   1, 32)]
        public void ColorDepth(string filename, int depth, int expected)
        {
            using (var resizer = new ImageResizer(GetSource(filename)))
            {
                var actual = Image.GetPixelFormatSize(resizer.Resized.PixelFormat);
                Assert.That(resizer.ColorDepth, Is.EqualTo(depth));
                Assert.That(actual, Is.EqualTo(expected));
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
        public void Create_Null_Throws() => Assert.That(() =>
        {
            using (var resizer = new ImageResizer(default(Bitmap)))
            {
                Assert.Fail("never reached");
            }
        },
        Throws.TypeOf<ArgumentException>().And.Message.EqualTo("original"));

        #endregion

        #region Others

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

            return Get($"{string.Join("-", items)}{extension}");
        }

        #endregion
    }
}
