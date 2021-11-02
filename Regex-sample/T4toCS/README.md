簡易T4コンバーター「T4toCS」
===
T4テンプレート(*tt)を元にC#ファイル(*.cs)を出力するツール  
正規表現を使用して「TransformText」メソッドを出力する簡易的なT4変換を行っている。

## 実行環境
* .NET Core 3.1以上

## 実行パラメータ
```dotnet T4toCS T4FilePath NameSpaceName ClassName OutputPath```
* T4FilePath  
   T4ファイルパス(*.tt)

* NameSpaceName  
   C#ソースコードに付与するネームスペース名

* ClassName  
   C#ソースコードに付与するクラス名

* OutputPath  
   出力ファイルパス(*cs)

## 実行例
```sh
dotnet run test1_cs.cs t4_practice test1_cs Generated/test1_cs.Generated.cs
```

## 実装概要
下記手順で実行する。
1. 入力パラメータ「T4FilePath」を読み込み下記を実行
   1. 下記設定を削除  
      * &lt;#@ template ～ #&gt;
      * &lt;#@ assembly ～ #&gt;
      * &lt;#@ output ～ #&gt;

   1. using作成  
      &lt;#@ import namespace="～" #&gt;を```using ～;```に変換

   1. パラメータ作成  
      &lt;#= ～ #&gt;を```$"{～}"```(C#の文字列補間)に変更

1. 入力パラメータを使って```namespace.ClassName#TransformText()```を組み立て