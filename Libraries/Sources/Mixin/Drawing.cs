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
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace Cube.Mixin.Drawing
{
    /* --------------------------------------------------------------------- */
    ///
    /// Extension
    ///
    /// <summary>
    /// Provides extende methods of classes in the System.Drawing namespace.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class Extension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// IsFormat
        ///
        /// <summary>
        /// Image オブジェクトが指定されたイメージフォーマとかどうか
        /// 判別します。
        /// </summary>
        ///
        /// <param name="src">Image オブジェクト</param>
        /// <param name="format">イメージフォーマット</param>
        ///
        /// <returns>
        /// 指定されたイメージフォーマットに合致するかどうかを示す値
        /// </returns>
        ///
        /* ----------------------------------------------------------------- */
        public static bool IsFormat(this Image src, ImageFormat format) =>
            src?.RawFormat.Equals(format) ?? false;

        /* ----------------------------------------------------------------- */
        ///
        /// IsIndexedColor
        ///
        /// <summary>
        /// インデックスカラーかどうかを判別します。
        /// </summary>
        ///
        /// <param name="src">Image オブジェクト</param>
        ///
        /// <returns>インデックスカラーかどうかを示す値</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static bool IsIndexedColor(this Image src) =>
            src != null && new PixelFormat[]
            {
                PixelFormat.Format1bppIndexed,
                PixelFormat.Format4bppIndexed,
                PixelFormat.Format8bppIndexed,
                PixelFormat.Indexed,
            }.Any(x => x == src.PixelFormat);

        /* ----------------------------------------------------------------- */
        ///
        /// GetColorDepth
        ///
        /// <summary>
        /// Image オブジェクトのビット深度を取得します。
        /// </summary>
        ///
        /// <param name="src">Image オブジェクト</param>
        ///
        /// <returns>ビット深度</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static int GetColorDepth(this Image src) =>
            Image.GetPixelFormatSize(src?.PixelFormat ?? PixelFormat.Undefined);

        /* ----------------------------------------------------------------- */
        ///
        /// GetImageFormat
        ///
        /// <summary>
        /// Image オブジェクトに対応する ImageFormat オブジェクトを
        /// 取得します。システムに登録されているオブジェクトと一致した
        /// 場合、RawFormat の代わりにそれらが返されます。
        /// </summary>
        ///
        /// <param name="src">Image オブジェクト</param>
        ///
        /// <returns>ImageFormat オブジェクト</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static ImageFormat GetImageFormat(this Image src)
        {
            if (src == null) return default;
            var cmp = GetImageFormats();
            return cmp.FirstOrDefault(e => src.IsFormat(e)) ?? src.RawFormat;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetRgbFormat
        ///
        /// <summary>
        /// Image オブジェクトを RGB カラーで表現した時の PixelFormat を
        /// 取得します。
        /// </summary>
        ///
        /// <param name="src">Image オブジェクト</param>
        ///
        /// <returns>
        /// RGB カラーで表現する場合の PixelFormat
        /// </returns>
        ///
        /* ----------------------------------------------------------------- */
        public static PixelFormat GetRgbFormat(this Image src)
        {
            if (src == null) return PixelFormat.Undefined;
            return GetRgbFormats().Contains(src.PixelFormat) ?
                   src.PixelFormat :
                   PixelFormat.Format32bppArgb;
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// GetImageFormats
        ///
        /// <summary>
        /// システムに登録されている ImageFormat オブジェクト一覧を取得
        /// します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static IList<ImageFormat> GetImageFormats() => _fmt ?? (
            _fmt = new List<ImageFormat>
            {
                ImageFormat.Jpeg,
                ImageFormat.Png,
                ImageFormat.Gif,
                ImageFormat.Bmp,
                ImageFormat.MemoryBmp,
                ImageFormat.Icon,
                ImageFormat.Tiff,
                ImageFormat.Wmf,
                ImageFormat.Emf,
                ImageFormat.Exif,
            }
        );

        /* ----------------------------------------------------------------- */
        ///
        /// GetRgbFormats
        ///
        /// <summary>
        /// RGB カラーを表す PixelFormat オブジェクト一覧を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static HashSet<PixelFormat> GetRgbFormats() => _rgb ?? (
            _rgb = new HashSet<PixelFormat>
            {
                PixelFormat.Format16bppRgb555,
                PixelFormat.Format16bppRgb565,
                PixelFormat.Format24bppRgb,
                PixelFormat.Format32bppArgb,
                PixelFormat.Format32bppPArgb,
                PixelFormat.Format32bppRgb,
                PixelFormat.Format48bppRgb,
                PixelFormat.Format64bppArgb,
                PixelFormat.Format64bppPArgb,
            }
        );

        #endregion

        #region Fields
        private static IList<ImageFormat> _fmt;
        private static HashSet<PixelFormat> _rgb;
        #endregion
    }
}
