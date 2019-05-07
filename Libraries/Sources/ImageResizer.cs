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
using Cube.FileSystem;
using Cube.Mixin.Drawing;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading;

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
    public class ImageResizer : DisposableBase
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
        /// <param name="src">リサイズ対象ファイル</param>
        ///
        /* ----------------------------------------------------------------- */
        public ImageResizer(string src) : this(src, new IO()) { }

        /* ----------------------------------------------------------------- */
        ///
        /// ImageResizer
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="src">リサイズ対象ファイル</param>
        /// <param name="io">ファイル入出力用オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public ImageResizer(string src, IO io)
        {
            IO = io;
            using (var s = IO.OpenRead(src)) Original = Image.FromStream(s);
            Initialize();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ImageResizer
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="src">
        /// リサイズ対象となる画像ファイルを読み込むストリーム
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        public ImageResizer(System.IO.Stream src) : this(Image.FromStream(src)) { }

        /* ----------------------------------------------------------------- */
        ///
        /// ImageResizer
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="src">リサイズ対象となる画像</param>
        ///
        /// <remarks>
        /// 指定された Image オブジェクトが NULL の場合、または Image
        /// オブジェクトの幅または高さが 0 の場合は例外が送出されます。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public ImageResizer(Image src)
        {
            Original = src;
            Initialize();
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// IO
        ///
        /// <summary>
        /// Gets the I/O handler.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected IO IO { get; }

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
        public Image Resized => _resized ?? (_resized = Resize());

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
            get => _width;
            set
            {
                if (_width == value) return;
                DisposeImage();
                _width = value;
                if (!PreserveAspectRatio) return;
                _height = (int)(_width * _ratio);
            }
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
            get => _height;
            set
            {
                if (_height == value) return;
                DisposeImage();
                _height = value;
                if (!PreserveAspectRatio) return;
                _width = (int)(_height / _ratio);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// LongSide
        ///
        /// <summary>
        /// 長辺の長さを取得または指定します。
        /// </summary>
        ///
        /// <remarks>
        /// 長辺がどちらに当たるかは、Original のサイズを基に判別します。
        /// また、PreserveAspectRatio が true に設定されている場合、短辺の長さも
        /// 自動的に変更されます。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public int LongSide
        {
            get => (Original.Width < Original.Height) ? Height : Width;
            set
            {
                if (Original.Width < Original.Height) Height = value;
                else Width = value;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ShortSide
        ///
        /// <summary>
        /// 短辺の長さを取得または指定します。
        /// </summary>
        ///
        /// <remarks>
        /// 短辺がどちらに当たるかは、Original のサイズを基に判別します。
        /// また、PreserveAspectRatio が true に設定されている場合、長辺の長さも
        /// 自動的に変更されます。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public int ShortSide
        {
            get => (Original.Width < Original.Height) ? Width : Height;
            set
            {
                if (Original.Width < Original.Height) Width = value;
                else Height = value;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ResizeMode
        ///
        /// <summary>
        /// リサイズ方法を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ImageResizeMode ResizeMode { get; set; } = ImageResizeMode.Default;

        /* ----------------------------------------------------------------- */
        ///
        /// ShrinkOnly
        ///
        /// <summary>
        /// オリジナル画像の幅または高さよりリサイズ後の幅または高さが
        /// 大きい場合、リサイズ処理をスキップするかどうかを示す値を
        /// 取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool ShrinkOnly { get; set; } = false;

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
        /// <remarks>
        /// アスペクト比は短辺を 1 とした時の長辺の長さを表します。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public double AspectRatio => _ratio > 1.0 ? _ratio : 1 / _ratio;

        /* ----------------------------------------------------------------- */
        ///
        /// ColorDepth
        ///
        /// <summary>
        /// オリジナル画像のビット深度を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int ColorDepth => Original.GetColorDepth();

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
        /// <param name="path">保存パス</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Save(string path) => Save(path, Original.RawFormat);

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// リサイズ後の Image オブジェクトをストリームに書き出します。
        /// </summary>
        ///
        /// <param name="stream">書き込み用ストリーム</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Save(System.IO.Stream stream) => Save(stream, Original.RawFormat);

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// リサイズ後の Image オブジェクトをファイルに保存します。
        /// </summary>
        ///
        /// <param name="path">保存パス</param>
        /// <param name="format">保存する画像のフォーマット</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Save(string path, ImageFormat format)
        {
            using (var s = IO.Create(path)) Save(s, format);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// リサイズ後の Image オブジェクトをストリームに書き出します。
        /// </summary>
        ///
        /// <param name="stream">書き出し用ストリーム</param>
        /// <param name="format">保存する画像のフォーマット</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Save(System.IO.Stream stream, ImageFormat format) =>
            Resized.Save(stream, format);

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// リサイズ後の Image オブジェクトをファイルに保存します。
        /// </summary>
        ///
        /// <param name="path">保存パス</param>
        /// <param name="format">保存する画像のフォーマット</param>
        /// <param name="parameters">付随するパラメータ</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Save(string path, ImageFormat format, EncoderParameters parameters)
        {
            using (var s = IO.Create(path)) Save(s, format, parameters);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// リサイズ後の Image オブジェクトをストリームに書き出します。
        /// </summary>
        ///
        /// <param name="stream">書き出し用ストリーム</param>
        /// <param name="format">保存する画像のフォーマット</param>
        /// <param name="parameters">付随するパラメータ</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Save(System.IO.Stream stream, ImageFormat format, EncoderParameters parameters) =>
            Resized.Save(
                stream,
                ImageCodecInfo.GetImageEncoders().First(c => c.FormatID == format.Guid),
                parameters
            );

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// リソースオブジェクトを破棄します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                DisposeImage();
                Original?.Dispose();
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Initialize
        ///
        /// <summary>
        /// 初期化処理を実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Initialize()
        {
            if (Original == null) throw new ArgumentException("original");

            _width  = Original.Width;
            _height = Original.Height;
            _ratio  = _height / (double)_width;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Resize
        ///
        /// <summary>
        /// リサイズ処理を実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private Image Resize()
        {
            var grow = (Width > Original.Width || Height > Original.Height);
            if (ShrinkOnly && grow) return new Bitmap(Original);

            var dest = new Bitmap(Width, Height, Original.GetRgbFormat());
            using (var gs = Graphics.FromImage(dest))
            {
                SetResizeMode(gs);
                gs.DrawImage(Original, 0, 0, Width, Height);
            }
            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetResizeMode
        ///
        /// <summary>
        /// リサイズ方法を設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SetResizeMode(Graphics gs)
        {
            switch (ResizeMode)
            {
                case ImageResizeMode.HighQuality:
                    gs.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    gs.InterpolationMode  = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    gs.SmoothingMode      = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    break;
                case ImageResizeMode.HighSpeed:
                    gs.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
                    gs.InterpolationMode  = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                    gs.SmoothingMode      = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
                    break;
                case ImageResizeMode.Default:
                    gs.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.Default;
                    gs.InterpolationMode  = System.Drawing.Drawing2D.InterpolationMode.Default;
                    gs.SmoothingMode      = System.Drawing.Drawing2D.SmoothingMode.Default;
                    break;
                default:
                    break;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// DisposeImage
        ///
        /// <summary>
        /// リサイズ後の Image オブジェクトを破棄します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void DisposeImage() => Interlocked.Exchange(ref _resized, null)?.Dispose();

        #endregion

        #region Fields
        private int _width = 0;
        private int _height = 0;
        private double _ratio = 1.0; // 幅を基準とした縦横比
        private Image _resized;
        #endregion
    }

    /* --------------------------------------------------------------------- */
    ///
    /// ImageResizeMode
    ///
    /// <summary>
    /// リサイズ処理の方法を表す列挙型です。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public enum ImageResizeMode : uint
    {
        /// <summary>既定の方法</summary>
        Default,
        /// <summary>品質重視</summary>
        HighQuality,
        /// <summary>速度重視</summary>
        HighSpeed,
    }
}
