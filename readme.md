# lsFeed: Simple Feed Reader for Desktop
lsFeed は、デスクトップのブラウザでフィードを読むためのアプリです。


## デモ
[https://mamorum.github.io/lsFeed/Content/index.html](https://mamorum.github.io/lsFeed/Content/index.html)

デモには以下の制限があります。

- 閲覧できるフィードが固定されています。
- 設定画面が利用できません。


## 動作環境
以下の環境で動作します。

- .NET Framework 4.7.2 以上が動作
- ポート 8622 が未使用
- ブラウザ有り

ポート 8622 は、端末でサーバなどを起動してなければ気にしなくて大丈夫です。


## 動作確認
以下の環境で確認しています。

- .NET Framework 4.7.2
- Windows10 64bit
- Chrome 64bit

Windows10 の最新バージョンだと、.NET Framework のインストールは不要そうでした。


## インストール方法
以下の手順でインストールします。

1. GitHub の [リリースページ](https://github.com/mamorum/lsFeed/releases) を開きます。
2. 最新版の Zip（例：`lsFeed-1.0.1.zip`）をダウンロードします。
3. Zip をローカルの任意の場所に解凍します。

Zip を解凍してできた資源を、任意の場所に配置しても大丈夫です。


## 利用方法
アプリの使い方や終了方法は、以下のドキュメントに書いています。

1. [初回利用方法](./Manual/first-time.md)
2. [フィードの閲覧方法](./Manual/reading-feeds.md)
3. [フィードの設定方法](./Manual/setting-feeds.md)


## アンインストール方法
インストール時に Zip を解凍してできた資源を削除します。

具体的には、

- lsFeed.exe
- Content フォルダ（と、配下の資源）

を削除すれば大丈夫です。


## 設定ファイルの削除方法
以下の手順で、設定を削除することができます。

1. ユーザフォルダ（`%USERPROFILE%`）の`lsFeed` フォルダを開きます。
2. フォルダの中の `conf.json` を削除します。

アプリを利用しなくなる場合は、`lsFeed` フォルダも削除して頂いて
大丈夫です。
