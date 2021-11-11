WPFでの動的DLL読み込み実装例
===
WPFで所定のディレクトリに存在するdllを読み込み、  
所定の処理を実行するサンプルとなる。

## 実行環境
* .NET Core 3.1以上
* **WPFのみWindows上でビルド・実行が必要**

## 実行
* Windowで実行する場合
  1. 読み込み対象のDLLをビルドする。
        ```bat
        cd src/LoadTargets
        ./build.bat
        ```

  1. WPFを実行する。
        ```bat
        cd ../WPF
        dotnet run
        ```

* Dockerを利用する場合
  1. docker_devに移動  
     ```sh
     cd docker_dev
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
     1. 読み込み対象のDLLをビルドする。
        ```bat
        cd src/LoadTargets
        ./build.sh
        ```

     1. WPFのビルド **※現時点ではエラーとなる**  
        ```sh
        dotnet build
        ```

  1. コンテナ停止・削除  
     ```sh
     docker-compose down
     ```