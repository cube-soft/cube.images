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
using System.Reflection;
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
    [TestFixture]
    class IconFactoryTest
    {
        /* ----------------------------------------------------------------- */
        ///
        /// GetIcon_StockIcon
        /// 
        /// <summary>
        /// システムで用意されているアイコンを生成するテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase(StockIcons.Application, IconSize.Small,      ExpectedResult =  16)]
        [TestCase(StockIcons.Application, IconSize.Large,      ExpectedResult =  32)]
        [TestCase(StockIcons.Application, IconSize.ExtraLarge, ExpectedResult =  48)]
        [TestCase(StockIcons.Application, IconSize.Jumbo,      ExpectedResult = 256)]
        public int GetIcon_StockIcon(StockIcons id, IconSize size)
            => id.GetIcon(size).Width;

        /* ----------------------------------------------------------------- */
        ///
        /// GetIcon_FileInfo
        /// 
        /// <summary>
        /// ファイルからアイコンを抽出して生成するテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase(@"C:\Windows\notepad.exe", IconSize.Small,      ExpectedResult =  16)]
        [TestCase(@"C:\Windows\notepad.exe", IconSize.Large,      ExpectedResult =  32)]
        [TestCase(@"C:\Windows\notepad.exe", IconSize.ExtraLarge, ExpectedResult =  48)]
        [TestCase(@"C:\Windows\notepad.exe", IconSize.Jumbo,      ExpectedResult = 256)]
        public int GetIcon_FileInfo(string path, IconSize size)
            => new System.IO.FileInfo(path).GetIcon(size).Width;

        /* ----------------------------------------------------------------- */
        ///
        /// GetIcon_IInformation
        /// 
        /// <summary>
        /// ファイルからアイコンを抽出して生成するテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase(@"C:\Windows\notepad.exe", IconSize.Small,      ExpectedResult = 16)]
        [TestCase(@"C:\Windows\notepad.exe", IconSize.Large,      ExpectedResult = 32)]
        [TestCase(@"C:\Windows\notepad.exe", IconSize.ExtraLarge, ExpectedResult = 48)]
        [TestCase(@"C:\Windows\notepad.exe", IconSize.Jumbo,      ExpectedResult = 256)]
        public int GetIcon_IInformation(string path, IconSize size)
            => new Cube.FileSystem.AfsOperator().Get(path).GetIcon(size).Width;

        /* ----------------------------------------------------------------- */
        ///
        /// GetIcon_Assembly
        /// 
        /// <summary>
        /// Assembly オブジェクトからアイコンを抽出して生成するテストを
        /// 実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase(IconSize.Small,      ExpectedResult =  16)]
        [TestCase(IconSize.Large,      ExpectedResult =  32)]
        [TestCase(IconSize.ExtraLarge, ExpectedResult =  48)]
        [TestCase(IconSize.Jumbo,      ExpectedResult = 256)]
        public int GetIcon_Assembly(IconSize size)
            => Assembly.GetExecutingAssembly().GetIcon(size).Width;

        /* ----------------------------------------------------------------- */
        ///
        /// GetIcon_File_NotFound
        /// 
        /// <summary>
        /// 存在しないファイルを指定した時の挙動を確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void GetIcon_File_NotFound()
            => Assert.That(
                IconFactory.Create("dummy.exe", IconSize.ExtraLarge).Width,
                Is.EqualTo(48)
            );

        /* ----------------------------------------------------------------- */
        ///
        /// GetIcon_FileInfo_Null
        /// 
        /// <summary>
        /// FileInfo オブジェクトが null の場合の挙動を確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void GetIcon_FileInfo_Null()
            => Assert.That(
                IconFactory.Create(default(System.IO.FileInfo), IconSize.Large).Width,
                Is.EqualTo(32)
            );

        /* ----------------------------------------------------------------- */
        ///
        /// GetIcon_IInformation_Null
        /// 
        /// <summary>
        /// IInformation オブジェクトが null の場合の挙動を確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void GetIcon_IInformation_Null()
            => Assert.That(
                IconFactory.Create(default(Cube.FileSystem.IInformation), IconSize.Large).Width,
                Is.EqualTo(32)
            );

        /* ----------------------------------------------------------------- */
        ///
        /// GetIcon_Assembly
        /// 
        /// <summary>
        /// Assembly オブジェクトが null の場合の挙動を確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void GetIcon_Assembly_Null()
            => Assert.That(
                default(Assembly).GetIcon(IconSize.Small).Width,
                Is.EqualTo(16)
            );
    }
}
