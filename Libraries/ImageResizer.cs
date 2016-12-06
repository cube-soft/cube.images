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

namespace Cube.Images
{
    /* --------------------------------------------------------------------- */
    ///
    /// ImageResizer
    /// 
    /// <summary>
    /// 画像をリサイズするためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ImageResizer
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ImageResizer
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ImageResizer(Image original)
        {
            Original = original;
            _width   = original?.Width  ?? 0;
            _height  = original?.Height ?? 0;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Original
        ///
        /// <summary>
        /// オリジナルの Image オブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Image Original { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Resized
        ///
        /// <summary>
        /// リサイズ後の Image オブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Image Resized { get; private set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Width
        ///
        /// <summary>
        /// リサイズ後の幅を取得または設定します。
        /// </summary>
        /// 
        /// <remarks>
        /// PreserveAspectRatio が true に設定されている場合、自動的に
        /// Height の値も変更されます。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Height
        ///
        /// <summary>
        /// リサイズ後の高さを取得または設定します。
        /// </summary>
        /// 
        /// <remarks>
        /// PreserveAspectRatio が true に設定されている場合、自動的に
        /// Height の値も変更されます。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// PreserveAspectRatio
        ///
        /// <summary>
        /// オリジナル画像のアスペクト比を維持したまたリサイズするか
        /// どうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool PreserveAspectRatio { get; set; } = true;

        /* ----------------------------------------------------------------- */
        ///
        /// AspectRatio
        ///
        /// <summary>
        /// オリジナル画像のアスペクト比（縦横比）を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public double AspectRatio => (
            Original?.Width < Original?.Height ?
            Original?.Height / (double)Original?.Width :
            Original?.Width  / (double)Original?.Width
        ) ?? 0.0;

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// リサイズ後の Image オブジェクトをファイルに保存します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Save(string path) => Resized?.Save(path);

        #endregion

        #region Fields
        private int _width;
        private int _height;
        #endregion
    }
}
