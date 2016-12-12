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
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using IoEx = Alphaleonis.Win32.Filesystem;

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
        public ImageResizer(string original)
        {
            using (var stream = IoEx.File.Open(original, System.IO.FileMode.Open))
            {
                var image = Image.FromStream(stream);
                if (image == null || image.Width < 1 || image.Height < 1) throw new ArgumentException();
                Original = Image.FromStream(stream);
            }
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
        /* ----------------------------------------------------------------- */
        public ImageResizer(System.IO.Stream original)
        {
            Original = Image.FromStream(original);
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
        /// <remarks>
        /// 指定された Image オブジェクトが NULL の場合、または Image
        /// オブジェクトの幅または高さが 0 の場合は例外が送出されます。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public ImageResizer(Image original)
        {
            if (original == null || original.Width < 1 || original.Height < 1) throw new ArgumentException();
            Original = original;
            Initialize();
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
        public Image Resized
        {
            get
            {
                if (_resized == null) _resized = Resize();
                return _resized;
            }
        }

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
            get { return _height; }
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

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// SetLongSide
        ///
        /// <summary>
        /// 長辺の長さを指定します。
        /// </summary>
        /// 
        /// <remarks>
        /// PreserveAspectRatio が true に設定されている場合、短辺の長さも
        /// 自動的に変更されます。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private void SetLongSide(int value)
        {
            if (Original.Width < Original.Height) Height = value;
            else Width = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetShortSide
        ///
        /// <summary>
        /// 短辺の長さを指定します。
        /// </summary>
        /// 
        /// <remarks>
        /// PreserveAspectRatio が true に設定されている場合、長辺の長さも
        /// 自動的に変更されます。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private void SetShortSide(int value)
        {
            if (Original.Width < Original.Height) Width = value;
            else Height = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// リサイズ後の Image オブジェクトをファイルに保存します。
        /// </summary>
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
        /* ----------------------------------------------------------------- */
        public void Save(string path, ImageFormat format)
        {
            using (var stream = IoEx.File.Create(path)) Save(stream, format);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// リサイズ後の Image オブジェクトをストリームに書き出します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Save(System.IO.Stream stream, ImageFormat format)
            => Resized.Save(stream, format);

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// リサイズ後の Image オブジェクトをファイルに保存します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Save(string path, ImageFormat format, EncoderParameters parameters)
            => Save(path,
            ImageCodecInfo.GetImageEncoders().First(c => c.FormatID == format.Guid),
            parameters
        );

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// リサイズ後の Image オブジェクトをストリームに書き出します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Save(System.IO.Stream stream, ImageFormat format, EncoderParameters parameters)
            => Save(stream,
            ImageCodecInfo.GetImageEncoders().First(c => c.FormatID == format.Guid),
            parameters
        );

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// リサイズ後の Image オブジェクトをファイルに保存します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Save(string path, ImageCodecInfo codec, EncoderParameters parameters)
        {
            using (var stream = IoEx.File.Create(path)) Save(stream, codec, parameters);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// リサイズ後の Image オブジェクトをストリームに書き出します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Save(System.IO.Stream stream, ImageCodecInfo codec, EncoderParameters parameters)
            => Resized.Save(stream, codec, parameters);

        #endregion

        #region Others

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
            if (ShrinkOnly && grow) return Original.Clone() as Image;

            var dest = new Bitmap(Width, Height, Original.PixelFormat);
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
        private void DisposeImage()
        {
            if (_resized == null) return;
            _resized.Dispose();
            _resized = null;
        }

        #endregion

        #region Fields
        private int _width;
        private int _height;
        private double _ratio; // 幅を基準とした縦横比
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
        Default,
        HighQuality,
        HighSpeed,
    }
}
