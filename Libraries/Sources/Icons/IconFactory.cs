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
using System;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Cube.Images.Icons
{
    /* --------------------------------------------------------------------- */
    ///
    /// IconFactory
    ///
    /// <summary>
    /// Icon を生成するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class IconFactory
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// システムで用意されているアイコンを生成します。
        /// </summary>
        ///
        /// <param name="si">システムアイコンの ID</param>
        /// <param name="size">アイコンサイズ</param>
        ///
        /// <returns>アイコン</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static Icon Create(StockIcons si, IconSize size)
        {
            var info = new SHSTOCKICONINFO();
            info.cbSize = Marshal.SizeOf(info);
            Shell32.NativeMethods.SHGetStockIconInfo((uint)si,
                Shell32.NativeMethods.SHGFI_SYSICONINDEX, ref info);

            Shell32.NativeMethods.SHGetImageList((uint)size,
                Shell32.NativeMethods.IID_IImageList, out IImageList images);
            if (images == null) return null;

            var handle = IntPtr.Zero;
            images.GetIcon(info.iSysImageIndex,
                (int)Shell32.NativeMethods.ILD_TRANSPARENT, ref handle);
            return (handle != IntPtr.Zero) ? Icon.FromHandle(handle) : null;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// 指定されたファイルからアイコンを生成します。
        /// </summary>
        ///
        /// <param name="fi">ファイル情報</param>
        /// <param name="size">アイコンサイズ</param>
        ///
        /// <returns>アイコン</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static Icon Create(System.IO.FileInfo fi, IconSize size) =>
            Create(fi?.FullName, size);

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// 指定されたファイルからアイコンを生成します。
        /// </summary>
        ///
        /// <param name="fi">ファイル情報</param>
        /// <param name="size">アイコンサイズ</param>
        ///
        /// <returns>アイコン</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static Icon Create(Information fi, IconSize size) =>
            Create(fi?.FullName, size);

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// 指定されたファイルからアイコンを生成します。
        /// </summary>
        ///
        /// <param name="path">ファイルのパス</param>
        /// <param name="size">アイコンサイズ</param>
        ///
        /// <returns>アイコン</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static Icon Create(string path, IconSize size)
        {
            var info = new SHFILEINFO();
            Shell32.NativeMethods.SHGetFileInfo(path, 0, ref info, (uint)Marshal.SizeOf(info),
                Shell32.NativeMethods.SHGFI_SYSICONINDEX);

            Shell32.NativeMethods.SHGetImageList((uint)size,
                Shell32.NativeMethods.IID_IImageList, out IImageList images);
            if (images == null) return null;

            var handle = IntPtr.Zero;
            images.GetIcon(info.iIcon, (int)Shell32.NativeMethods.ILD_TRANSPARENT, ref handle);
            return (handle != IntPtr.Zero) ? Icon.FromHandle(handle) : null;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// 指定された Assembly オブジェクトからアイコンを生成します。
        /// </summary>
        ///
        /// <param name="asm">アセンブリ情報</param>
        /// <param name="size">アイコンサイズ</param>
        ///
        /// <returns>アイコン</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static Icon Create(Assembly asm, IconSize size) =>
            Create(asm?.Location, size);

        #endregion

        #region Extensions

        /* ----------------------------------------------------------------- */
        ///
        /// GetIcon
        ///
        /// <summary>
        /// システムで用意されているアイコンを取得します。
        /// </summary>
        ///
        /// <param name="si">システムアイコンの ID</param>
        /// <param name="size">アイコンサイズ</param>
        ///
        /// <returns>アイコン</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static Icon GetIcon(this StockIcons si, IconSize size) => Create(si, size);

        /* ----------------------------------------------------------------- */
        ///
        /// GetIcon
        ///
        /// <summary>
        /// 指定されたファイルからアイコンを取得します。
        /// </summary>
        ///
        /// <param name="fi">ファイル情報</param>
        /// <param name="size">アイコンサイズ</param>
        ///
        /// <returns>アイコン</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static Icon GetIcon(this System.IO.FileInfo fi, IconSize size) =>
            Create(fi, size);

        /* ----------------------------------------------------------------- */
        ///
        /// GetIcon
        ///
        /// <summary>
        /// 指定されたファイルからアイコンを取得します。
        /// </summary>
        ///
        /// <param name="fi">ファイル情報</param>
        /// <param name="size">アイコンサイズ</param>
        ///
        /// <returns>アイコン</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static Icon GetIcon(this Information fi, IconSize size) =>
            Create(fi, size);

        /* ----------------------------------------------------------------- */
        ///
        /// GetIcon
        ///
        /// <summary>
        /// Assembly オブジェクトに対応するアイコンを取得します。
        /// </summary>
        ///
        /// <param name="asm">アセンブリ情報</param>
        /// <param name="size">アイコンサイズ</param>
        ///
        /// <returns>アイコン</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static Icon GetIcon(this Assembly asm, IconSize size) => Create(asm, size);

        #endregion
    }
}
