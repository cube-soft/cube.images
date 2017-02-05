/* ------------------------------------------------------------------------- */
///
/// IconFactoryTest.cs
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
using NUnit.Framework;
using Cube.Images.Icons;

namespace Cube.Images.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// IconFactoryTest
    /// 
    /// <summary>
    /// IconFactory のテスト用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Parallelizable]
    [TestFixture]
    class IconFactoryTest
    {
        /* ----------------------------------------------------------------- */
        ///
        /// Create_StockIcon
        /// 
        /// <summary>
        /// システムで用意されているアイコンを生成するテストを行います。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase(StockIcons.Application, IconSize.Small,      ExpectedResult =  16)]
        [TestCase(StockIcons.Application, IconSize.Large,      ExpectedResult =  32)]
        [TestCase(StockIcons.Application, IconSize.ExtraLarge, ExpectedResult =  48)]
        [TestCase(StockIcons.Application, IconSize.Jumbo,      ExpectedResult = 256)]
        public int Create_StockIcon(StockIcons id, IconSize size)
            => IconFactory.Create(id, size).Width;

        /* ----------------------------------------------------------------- */
        ///
        /// Create_File
        /// 
        /// <summary>
        /// ファイルからアイコンを抽出して生成するテストを行います。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase(@"C:\Windows\notepad.exe", IconSize.Small,      ExpectedResult =  16)]
        [TestCase(@"C:\Windows\notepad.exe", IconSize.Large,      ExpectedResult =  32)]
        [TestCase(@"C:\Windows\notepad.exe", IconSize.ExtraLarge, ExpectedResult =  48)]
        [TestCase(@"C:\Windows\notepad.exe", IconSize.Jumbo,      ExpectedResult = 256)]
        public int Create_File(string path, IconSize size)
            => IconFactory.Create(path, size).Width;
    }
}
