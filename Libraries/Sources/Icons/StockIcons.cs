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
namespace Cube.Images.Icons
{
    /* --------------------------------------------------------------------- */
    ///
    /// StockIcons
    ///
    /// <summary>
    /// システムで用意されているアイコン一覧を定義した列挙型です。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public enum StockIcons : uint
    {
        /// <summary>関連付けされていないファイル</summary>
        DocumentNotAssociated = 0,
        /// <summary>関連付けされたファイル</summary>
        DocumentAssociated = 1,
        /// <summary>アプリケーション</summary>
        Application = 2,
        /// <summary>フォルダ</summary>
        Folder = 3,
        /// <summary>開かれたフォルダ</summary>
        FolderOpen = 4,
        /// <summary>5.25 インチのフロッピー</summary>
        Floppy525 = 5,
        /// <summary>3.5 インチのフロッピー</summary>
        Floppy35 = 6,
        /// <summary>リムーバブルドライブ</summary>
        RemovableDrive = 7,
        /// <summary>ローカルドライブ</summary>
        FixedDrive = 8,
        /// <summary>ネットワークドライブ</summary>
        NetworkDrive = 9,
        /// <summary>未接続状態のネットワークドライブ</summary>
        NetworkDriveDisconnected = 10,
        /// <summary>CD ドライブ</summary>
        CdDrive = 11,
        /// <summary>RAM ドライブ</summary>
        RamDrive = 12,
        /// <summary>インターネット</summary>
        World = 13,
        /// <summary>サーバ</summary>
        Server = 15,
        /// <summary>プリンタ</summary>
        Printer = 16,
        /// <summary>ネットワーク</summary>
        Network = 17,
        /// <summary>検索</summary>
        Find = 22,
        /// <summary>ヘルプ</summary>
        Help = 23,
        /// <summary>共有</summary>
        Share = 28,
        /// <summary>リンク</summary>
        Link = 29,
        /// <summary>低速ファイル</summary>
        SlowFile = 30,
        /// <summary>空のゴミ箱</summary>
        Recycle = 31,
        /// <summary>ゴミ箱</summary>
        RecycleFull = 32,
        /// <summary>Audio 用 CD メディア</summary>
        AudioCdMedia = 40,
        /// <summary>ロック状態</summary>
        Lock = 47,
        /// <summary>自動リスト</summary>
        AutoList = 49,
        /// <summary>ネットワークプリンタ</summary>
        NetworkPrinter = 50,
        /// <summary>共有サーバ</summary>
        ServerShare = 51,
        /// <summary>FAX プリンタ</summary>
        FaxPrinter = 52,
        /// <summary>ネットワーク FAX プリンタ</summary>
        NetworkFaxPrinter = 53,
        /// <summary>ファイルに出力</summary>
        PrintToFile = 54,
        /// <summary>スタック</summary>
        Stack = 55,
        /// <summary>SVCD メディア</summary>
        SvcdMedia = 56,
        /// <summary>フォルダ</summary>
        StuffedFolder = 57,
        /// <summary>不明なドライブ</summary>
        UnknownDrive = 58,
        /// <summary>DVD ドライブ</summary>
        DvdDrive = 59,
        /// <summary>DVD メディア</summary>
        DvdMedia = 60,
        /// <summary>DVD-RAM メディア</summary>
        DvdRamMedia = 61,
        /// <summary>DVD-RW メディア</summary>
        DvdRwMedia = 62,
        /// <summary>DVD-R メディア</summary>
        DvdRMedia = 63,
        /// <summary>DVD ROM メディア</summary>
        DvdRomMedia = 64,
        /// <summary>CD+ メディア</summary>
        CdPlusMedia = 65,
        /// <summary>CD-RW メディア</summary>
        CdRwMedia = 66,
        /// <summary>CD-R メディア</summary>
        CdRMedia = 67,
        /// <summary>メディアに保存中</summary>
        Burning = 68,
        /// <summary>空の CD メディア</summary>
        BlankCdMedia = 69,
        /// <summary>CD ROM メディア</summary>
        CdRomMedia = 70,
        /// <summary>音楽ファイル</summary>
        AudioFiles = 71,
        /// <summary>画像ファイル</summary>
        ImageFiles = 72,
        /// <summary>動画ファイル</summary>
        VideoFiles = 73,
        /// <summary>マルチメディアファイル</summary>
        MixedFiles = 74,
        /// <summary>戻る</summary>
        FolderBack = 75,
        /// <summary>進む</summary>
        FolderFront = 76,
        /// <summary>シールドアイコン</summary>
        Shield = 77,
        /// <summary>警告</summary>
        Warning = 78,
        /// <summary>情報</summary>
        Information = 79,
        /// <summary>エラー</summary>
        Error = 80,
        /// <summary>鍵</summary>
        Key = 81,
        /// <summary>ソフトウェア</summary>
        Software = 82,
        /// <summary>名前を変更</summary>
        Rename = 83,
        /// <summary>削除</summary>
        Delete = 84,
        /// <summary>音楽 DVD メディア</summary>
        AudioDvdMedia = 85,
        /// <summary>映像 DVD メディア</summary>
        MovieDvdMedia = 86,
        /// <summary>CD メディア</summary>
        EnhancedCdMedia = 87,
        /// <summary>DVD メディア</summary>
        EnhancedDvdMedia = 88,
        /// <summary>HD-DVD メディア</summary>
        HdDvdMedia = 89,
        /// <summary>BluRay メディア</summary>
        BluRayMedia = 90,
        /// <summary>VCD メディア</summary>
        VcdMedia = 91,
        /// <summary>DVD+R メディア</summary>
        DvdPlusRMedia = 92,
        /// <summary>DVD+RW メディア</summary>
        DvdPlusRwMedia = 93,
        /// <summary>デスクトップ</summary>
        Desktop = 94,
        /// <summary>モバイル</summary>
        Mobile = 95,
        /// <summary>ユーザ</summary>
        Users = 96,
        /// <summary>スマートメディア</summary>
        SmartMedia = 97,
        /// <summary>コンパクトフラッシュ</summary>
        CompactFlash = 98,
        /// <summary>携帯電話</summary>
        CellPhone = 99,
        /// <summary>カメラ</summary>
        Camera = 100,
        /// <summary>ビデオカメラ</summary>
        VideoCamera = 101,
        /// <summary>音楽プレーヤー</summary>
        AudioPlayer = 102,
        /// <summary>ネットワークに接続済み</summary>
        NetworkConnect = 103,
        /// <summary>インターネット</summary>
        Internet = 104,
        /// <summary>ZIP</summary>
        Zip = 105,
        /// <summary>設定</summary>
        Settings = 106,
        /// <summary>最後のアイコン</summary>
        MaxIcons = 107
    }
}
