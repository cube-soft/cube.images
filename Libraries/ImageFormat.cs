/* ------------------------------------------------------------------------- */
///
/// ImageFormat.cs
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
using System.Drawing.Imaging;

namespace Cube.Images
{
    /* --------------------------------------------------------------------- */
    ///
    /// Jpeg
    /// 
    /// <summary>
    /// JPEG のフォーマットに関わる設定を補助するためのメソッドを定義した
    /// クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public abstract class Jpeg
    {
        /* ----------------------------------------------------------------- */
        ///
        /// Format
        ///
        /// <summary>
        /// JPEG を表す ImageFormat オブジェクトを取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public static ImageFormat Format => ImageFormat.Jpeg;

        /* ----------------------------------------------------------------- */
        ///
        /// Quality
        ///
        /// <summary>
        /// JPEG の品質を表すオブジェクトを生成します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public static EncoderParameters Quality(long value)
        {
            var dest = new EncoderParameters(1);
            dest.Param[0] = new EncoderParameter(Encoder.Quality, value);
            return dest;
        }
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Png
    /// 
    /// <summary>
    /// PNG のフォーマットに関わる設定を補助するためのメソッドを定義した
    /// クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public abstract class Png
    {
        /* ----------------------------------------------------------------- */
        ///
        /// Format
        ///
        /// <summary>
        /// PNG を表す ImageFormat オブジェクトを取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public static ImageFormat Format => ImageFormat.Png;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Bmp
    /// 
    /// <summary>
    /// BMP のフォーマットに関わる設定を補助するためのメソッドを定義した
    /// クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public abstract class Bmp
    {
        /* ----------------------------------------------------------------- */
        ///
        /// Format
        ///
        /// <summary>
        /// BMP を表す ImageFormat オブジェクトを取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public static ImageFormat Format => ImageFormat.Bmp;
    }
}
