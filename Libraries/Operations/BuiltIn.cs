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
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace Cube.Images.BuiltIn
{
    /* --------------------------------------------------------------------- */
    ///
    /// BuiltIn.Operations
    /// 
    /// <summary>
    /// System.Drawing.Image 等の .NET Framework にビルトインされた
    /// イメージクラスへの拡張メソッドを定義するクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class Operations
    {
        /* ----------------------------------------------------------------- */
        ///
        /// IsFormat
        ///
        /// <summary>
        /// Image オブジェクトが指定された ImageFormat かどうか判別します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static bool IsFormat(this Image src, ImageFormat format)
            => src?.RawFormat.Equals(format) ?? false;

        /* ----------------------------------------------------------------- */
        ///
        /// IsIndexedColor
        ///
        /// <summary>
        /// インデックスカラーかどうかを判別します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static bool IsIndexedColor(this Image src)
            => src != null && new PixelFormat[]
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
        /* ----------------------------------------------------------------- */
        public static int GetColorDepth(this Image src)
            => Image.GetPixelFormatSize(src?.PixelFormat ?? PixelFormat.Undefined);

        /* ----------------------------------------------------------------- */
        ///
        /// GetRgbFormat
        ///
        /// <summary>
        /// Image オブジェクトを RGB カラーで表現する場合の PixelFormat を
        /// 取得します。
        /// </summary>
        /// 
        /// <remarks>
        /// アルファチャンネルが必要な場合は 32bit ARGB、それ以外の場合は
        /// 24bit RGB に対応する値が返されます。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static PixelFormat GetRgbFormat(this Image src)
        {
            if (src == null) return PixelFormat.Undefined;

            switch (src.PixelFormat)
            {
                case PixelFormat.Format16bppRgb555:
                case PixelFormat.Format16bppRgb565:
                case PixelFormat.Format24bppRgb:
                case PixelFormat.Format32bppArgb:
                case PixelFormat.Format32bppPArgb:
                case PixelFormat.Format32bppRgb:
                case PixelFormat.Format48bppRgb:
                case PixelFormat.Format64bppArgb:
                case PixelFormat.Format64bppPArgb:
                    return src.PixelFormat;
                case PixelFormat.Format16bppGrayScale:
                    return PixelFormat.Format24bppRgb;
                case PixelFormat.Format16bppArgb1555:
                    return PixelFormat.Format32bppArgb;
                case PixelFormat.DontCare:
                case PixelFormat.Extended:
                case PixelFormat.Alpha:
                case PixelFormat.PAlpha:
                    return PixelFormat.Format32bppArgb;
                case PixelFormat.Indexed:
                case PixelFormat.Format1bppIndexed:
                case PixelFormat.Format4bppIndexed:
                case PixelFormat.Format8bppIndexed:
                    return PixelFormat.Format32bppArgb;
                default:
                    return PixelFormat.Format32bppArgb;
            }
        }
    }
}
