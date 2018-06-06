# lsFeed: Simple Feed Reader for Desktop
lsFeed は、デスクトップのブラウザでフィードを読むためのアプリです。


## デモ
[https://mamorum.github.io/lsFeed/Content/index.html](https://mamorum.github.io/lsFeed/Content/index.html)

デモには以下の制限があります。

- 閲覧できるフィードが固定されています。
- 設定画面が利用できません。


## 動作環境
以下の環境で動作します。

- .NET Framework 4 以上が動作
- ポート 8622 が未使用
- ブラウザ有り

ポート 8622 は、端末でサーバなどを起動してなければ気にしなくて大丈夫です。


## 動作確認
以下の環境で確認しています。

- Windows10 64bit
- Chrome 64bit

Windows10 だと .NET Framework のインストールは不要そうでした。


## インストール方法
以下の手順でインストールします。

1. GitHub の [リリースページ](https://github.com/mamorum/lsFeed/releases) を開きます。
2. 最新版の Zip（例：`lsFeed-1.0.0.zip`）をダウンロードします。
3. Zip をローカルの任意の場所に解凍します。


## 利用方法
アプリの使い方や終了方法は、以下のドキュメントに書いています。

[https://web-dev.hatenablog.com/entry/oss/lsFeed/log/2018/0524](https://web-dev.hatenablog.com/entry/oss/lsFeed/log/2018/0524)


## アンインストール方法
インストール時に zip を解凍してできた資源を削除します。

具体的には、

- lsFeed.exe
- Content フォルダ（と、配下の資源）

を削除して頂ければ大丈夫です。

設定も削除したい場合は、ユーザフォルダ（`%USERPROFILE%`）にある、`lsFeed` フォルダを削除します。`lsFeed` フォルダ内にあるファイル `conf.json` も、一緒に削除して頂いて大丈夫です。
