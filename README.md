# NiconicoDiffer
ニコニコ動画で見たくないID、タグ、投稿者の動画をフィルタリングして総合ランキングを表示します。  
WPF製です。

## はじめに
VisualStudioなどでcloneを行い、ビルドするだけで検証できます。
WPF製なので、Windows環境で実行してください。

### インストール方法
ローカルの任意の場所に配置し、実行ファイルを起動するだけで使用できます。

### 使用方法
起動して、右上の`GetVideos`ボタンをクリックします。
総合ランキング（毎時）が表示されます。
![起動した状態](https://user-images.githubusercontent.com/39305262/52919128-43cedf80-3342-11e9-8c4d-224eddc3b728.png "起動した状態")
フィルタリングが上手くできていない場合は、もう一度クリックすると正常な表示になります。（非同期処理がうまくできてません…）

動画を選択し、動画ID、投稿者、タグに関して`もう表示しない`ボタンをクリックすると、次回から表示されません。

データを初期化する場合は、同ディレクトリに作成されるdataフォルダを削除してください。

## テストの実行
テストコードはありません。

## コントリビューション
特に考えていません。

## バージョン管理
特に使用しているツールなどはありません。

## 著者
* **Ginpaydo** - *原著者* - [Ginpaydo](https://github.com/ginpaydo)
このプロジェクトへの[貢献者](https://github.com/ginpaydo/NiconicoDiffer/contributors)のリストもご覧ください。

## ライセンス
このプロジェクトは MIT ライセンスの元にライセンスされています。 詳細は[LICENSE.md](LICENSE.md)をご覧ください。

## 謝辞
* 使いやすいAPIを提供してくれたニコニコ動画さん、ありがとうございました。
