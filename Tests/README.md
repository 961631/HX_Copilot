# 端末情報管理システム - テストコード実行ガイド

## 概要
このドキュメントでは、救急情報管理システム「みまもりホン後継機対応」の端末情報管理機能に対するテストコードの実行方法を説明します。

## テスト環境の準備

### 前提条件
- Visual Studio 2019 以上 または Visual Studio Code
- .NET Framework 4.8
- MSTest テストフレームワーク
- Moq モックライブラリ
- Oracle データベース（統合テスト用）

### 必要なNuGetパッケージ
```xml
<package id="MSTest.TestFramework" version="2.2.7" targetFramework="net48" />
<package id="MSTest.TestAdapter" version="2.2.7" targetFramework="net48" />
<package id="Moq" version="4.18.4" targetFramework="net48" />
<package id="Castle.Core" version="4.4.0" targetFramework="net48" />
```

## VSCodeでのテスト実行方法

### 1. 必要な拡張機能のインストール

VSCodeで以下の拡張機能をインストールしてください：

```
- C# Dev Kit (Microsoft)
- .NET Test Explorer (Microsoft)
- Test Explorer UI (Microsoft)
- NuGet Package Manager (Microsoft)
```

### 2. テストプロジェクトの設定

#### テストプロジェクト作成
```bash
# VSCode統合ターミナルで実行
cd your-project-directory
dotnet new mstest -n HX.Terminal.Tests
cd HX.Terminal.Tests
```

#### NuGetパッケージのインストール
```bash
dotnet add package MSTest.TestFramework --version 2.2.7
dotnet add package MSTest.TestAdapter --version 2.2.7
dotnet add package Moq --version 4.18.4
dotnet add package Castle.Core --version 4.4.0
```

#### プロジェクト参照の追加
```bash
dotnet add reference ../HX.Terminal.csproj
```

### 3. テスト実行コマンド

#### 全てのテストを実行
```bash
dotnet test
```

#### 特定のテストクラスを実行
```bash
dotnet test --filter "ClassName=TerminalRegistBusinessLogicTests"
```

#### 特定のテストメソッドを実行
```bash
dotnet test --filter "MethodName=ValidateCsvFile_正常なCSVファイルの場合_成功を返す"
```

#### カテゴリ別テスト実行
```bash
# 単体テストのみ実行
dotnet test --filter "TestCategory!=Integration&TestCategory!=Scenario"

# 統合テストのみ実行
dotnet test --filter "TestCategory=Integration"

# エラーハンドリングテストのみ実行
dotnet test --filter "TestCategory=ErrorHandling"
```

#### 詳細レポート付きテスト実行
```bash
dotnet test --logger "trx;LogFileName=TestResults.trx" --logger "html;LogFileName=TestResults.html"
```

### 4. VSCode Test Explorerの使用

1. **Test Explorerの表示**
   - `Ctrl+Shift+P` → `Test: Show Test Explorer`

2. **テストの実行**
   - Test Explorerで個別テストまたはテストクラスを右クリック
   - 「Run Test」または「Debug Test」を選択

3. **テスト結果の確認**
   - Test Explorer画面で成功/失敗を確認
   - 失敗したテストの詳細は下部の出力パネルで確認

### 5. デバッグ実行

#### テストのデバッグ
```bash
dotnet test --logger "console;verbosity=detailed" --filter "MethodName=テスト名"
```

#### VSCodeでのブレークポイントデバッグ
1. テストファイルでブレークポイントを設定
2. Test Explorerで「Debug Test」を選択
3. デバッガーが開始され、ブレークポイントで停止

## テストカテゴリの説明

### 単体テスト (Unit Tests)
- **BusinessLogic Tests**: ビジネスロジックの単体テスト
- **Controller Tests**: MVCコントローラーのテスト
- **Model Tests**: データモデルのテスト
- **DataAccess Tests**: データアクセス層のテスト

### 統合テスト (Integration Tests)
- **Integration**: データベースを含む完全な処理フローのテスト
- **ErrorHandling**: エラーハンドリングの統合テスト
- **Performance**: パフォーマンステスト
- **Security**: セキュリティテスト

### シナリオテスト (Scenario Tests)
- **Scenario**: ユーザーストーリーベースのテスト

## テストデータの準備

### CSVテストファイル
- `Tests/TestData/terminal_test_data.csv`: 端末データ用テストCSV
- `Tests/TestData/sim_test_data.csv`: SIMデータ用テストCSV

### データベーステストデータ
統合テストを実行する前に、テスト用データベースに以下のテーブルを作成してください：

```sql
-- テスト用テーブル作成例
CREATE TABLE HX.NEW_TERMINAL_REGIST_TEST AS SELECT * FROM HX.NEW_TERMINAL_REGIST WHERE 1=0;
CREATE TABLE HX.NEW_SIM_REGIST_TEST AS SELECT * FROM HX.NEW_SIM_REGIST WHERE 1=0;
CREATE TABLE HX.TERMINAL_SIM_HIMODUKE_TEST AS SELECT * FROM HX.TERMINAL_SIM_HIMODUKE WHERE 1=0;
```

## 継続的インテグレーション (CI) での実行

### Azure DevOps Pipeline
```yaml
- task: DotNetCoreCLI@2
  displayName: 'Run Tests'
  inputs:
    command: 'test'
    projects: '**/*Tests.csproj'
    arguments: '--logger trx --collect "Code coverage"'
```

### GitHub Actions
```yaml
- name: Test
  run: dotnet test --no-build --verbosity normal --logger "trx;LogFileName=TestResults.trx"
```

## トラブルシューティング

### よくある問題と解決策

#### 1. テストが見つからない場合
```bash
dotnet clean
dotnet build
dotnet test --list-tests
```

#### 2. データベース接続エラー
- `web.config`の接続文字列を確認
- テスト用データベースのアクセス権限を確認
- 統合テストは`Assert.Inconclusive()`でスキップされる設定になっています

#### 3. モックオブジェクトのエラー
- Moqのバージョン互換性を確認
- `using Moq;`が正しくインポートされているか確認

#### 4. ファイルパスエラー
- テストデータファイルのパスが正しいか確認
- ファイルのビルドアクションを「Content」に設定

## テスト結果の確認

### コンソール出力例
```
Test run for HX.Terminal.Tests.dll(.NETFramework,Version=v4.8)
Microsoft (R) Test Execution Command Line Tool Version 16.11.0

Starting test execution, please wait...
A total of 1 test files matched the specified pattern.

Test Run Successful.
Total tests: 25
     Passed: 22
     Failed: 0
     Skipped: 3
 Total time: 1.2345 Seconds
```

### カバレッジレポート
```bash
dotnet test --collect:"XPlat Code Coverage"
reportgenerator -reports:"TestResults/*/coverage.cobertura.xml" -targetdir:"coveragereport" -reporttypes:Html
```

## まとめ

このテストスイートは以下の観点から設計書の要件を検証します：

1. **機能テスト**: 設計書に記載された各機能が正常に動作することを確認
2. **バリデーションテスト**: 入力値チェック、桁数チェック、重複チェックの動作確認
3. **エラーハンドリングテスト**: 異常系の処理が適切に行われることを確認
4. **統合テスト**: データベースを含む実際の業務フローの確認
5. **セキュリティテスト**: SQLインジェクション対策などの確認

定期的にテストを実行し、システムの品質を維持してください。
