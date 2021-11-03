dotnet-t4の使用例
===
dotnet-t4を使用してT4ファイル(*tt)からC#ソースコード(*cs)の使用例。  
自動生成したソースコードはGeneratedディレクトリに生成される。

## 実行環境
* .NET Core 3.1以上

## 実行方法
* dotnetコマンドを実行する場合
   1. (**初回のみ**)ツールのインストール  
      ```sh
      dotnet tool install dotnet-t4
      ```

   1. ビルド実行(T4ファイルからC#ソースコードの自動生成)  
      ```sh
      dotnet build
      ```

   1. 実行(test1_csの実行例をコンソール出力)  
      ```sh
      dotnet run
      ```

* Dockerを利用する場合
  1. docker_devに移動  
     ```sh
     cd  docker_dev
     ```

  1. (**初回のみ**)ビルド  
     ```sh
     docker-compose build
     ```

  1. コンテナ起動  
     ```sh
     docker-compose up -d
     ```

  1. コンテナに入る  
     ```sh
     docker exec -it docker_dev_t4_1 /bin/bash
     ```

  1. コンテナ内で実行 
     1. consoleディレクトリに移動  
     ```sh
     cd console
     ```

     1. ビルド実行(T4ファイルからC#ソースコードの自動生成)  
     ```sh
     dotnet build
     ```

     1. 実行(test1_csの実行例をコンソール出力)  
     ```sh
     dotnet run
     ```

  1. コンテナ停止・削除  
     ```sh
     docker-compose down
     ```

## 実装概要
```console.csproj```に下記を追加している。
```XML
<ItemGroup>
   <PackageReference Include="Mono.TextTemplating" Version="2.2.1" />
   <TextTemplate Include="**\*.tt" />
   <Generated Include="**\*.Generated.cs" />
</ItemGroup>

<!-- dotnet-t4を使ってT4ファイルからCSソースファイルを生成 -->
<Target Name="TextTemplateTransform" BeforeTargets="BeforeBuild">
   <Exec WorkingDirectory="$(ProjectDir)" Command="dotnet t4 %(TextTemplate.Identity) -c $(RootNameSpace).%(TextTemplate.Filename) -o Generated/%(TextTemplate.Filename).Generated.cs" />
</Target>

<!-- 自動生成したファイルを削除 -->
<Target Name="TextTemplateClean" AfterTargets="Clean">
   <Delete Files="@(Generated)" />
</Target>
```

## 参考サイト
* [.NET Core+VS CodeでもT4 テンプレートエンジンでコード生成したい！](https://qiita.com/nogic1008/items/2c4049d43a11e83df15b)