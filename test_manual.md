# 端末情報管理システム - VSCodeテスト実行マニュアル

**プロジェクト名**: 救急情報管理システム　みまもりホン後継機対応  
**ドキュメント名**: VSCodeテスト実行マニュアル  
**作成者**: システム開発チーム  
**作成日**: 2025/8/27  
**最終更新**: 2025/8/27 (C#コーディング規約対応版)  
**対象**: 開発者・QAエンジニア

---

## 📋 目次
1. [環境準備](#環境準備)
2. [VSCode拡張機能のセットアップ](#vscode拡張機能のセットアップ)
3. [テストプロジェクトの構築](#テストプロジェクトの構築)
4. [VSCodeでのテスト実行](#vscodeでのテスト実行)
5. [デバッグ実行](#デバッグ実行)
6. [テスト結果の確認](#テスト結果の確認)
7. [C#コーディング規約対応テスト](#c-コーディング規約対応テスト)
8. [テストデータの活用](#テストデータの活用)
9. [トラブルシューティング](#トラブルシューティング)

---

## 🛠️ 環境準備

### 前提条件
以下の環境が整っていることを確認してください：

- **Visual Studio Code**: バージョン 1.70 以上
- **.NET Framework**: 4.8
- **PowerShell**: 5.1 以上（Windows環境）
- **Git**: バージョン管理用

### システム要件確認
```powershell
# PowerShellで実行
# .NET Framework バージョン確認
Get-ItemProperty "HKLM:SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\" -Name Release

# VSCode バージョン確認
code --version

# PowerShell バージョン確認
$PSVersionTable.PSVersion
```

---

## 🔧 VSCode拡張機能のセットアップ

### 1. 必須拡張機能のインストール

**方法1: VSCode拡張機能ビューから**
1. `Ctrl+Shift+X` で拡張機能ビューを開く
2. 以下の拡張機能を検索してインストール：

| 拡張機能名 | 発行者 | ID | 説明 |
|---|---|---|---|
| C# Dev Kit | Microsoft | `ms-dotnettools.csdevkit` | C#開発の統合環境 |
| .NET Test Explorer | Microsoft | `ms-dotnettools.dotnet-test-explorer` | テスト実行・結果表示 |
| Test Explorer UI | Holger Benl | `hbenl.vscode-test-explorer` | テストUI拡張 |
| NuGet Package Manager | Microsoft | `ms-dotnettools.nuget-package-manager` | NuGetパッケージ管理 |
| C# | Microsoft | `ms-dotnettools.csharp` | C#言語サポート |

**方法2: コマンドラインから一括インストール**
```powershell
# VSCode統合ターミナルまたはPowerShellで実行
code --install-extension ms-dotnettools.csdevkit
code --install-extension ms-dotnettools.dotnet-test-explorer
code --install-extension hbenl.vscode-test-explorer
code --install-extension ms-dotnettools.nuget-package-manager
code --install-extension ms-dotnettools.csharp
```

### 2. 拡張機能の設定

**settings.json 設定例**
```json
{
    "dotnet-test-explorer.testProjectPath": "Tests/**/*.csproj",
    "dotnet-test-explorer.useTreeView": true,
    "dotnet-test-explorer.showCodeLens": true,
    "dotnet-test-explorer.codeLensPattern": "**/*.cs",
    "csharp.unitTestDebuggingOptions": {
        "sourceFileMap": {
            "/src": "${workspaceFolder}"
        }
    }
}
```

---

## 🏗️ テストプロジェクトの構築

### 1. テストプロジェクトの作成

```powershell
# プロジェクトルートディレクトリに移動
cd "vscode-vfs://github/961631/hello-world-java"

# テストプロジェクト作成（既に存在する場合はスキップ）
dotnet new mstest -n HX.Terminal.Tests -o Tests

# テストプロジェクトディレクトリに移動
cd Tests
```

### 2. 必要なNuGetパッケージのインストール

```powershell
# テストフレームワーク
dotnet add package MSTest.TestFramework --version 2.2.7

# テストランナー
dotnet add package MSTest.TestAdapter --version 2.2.7

# モックライブラリ
dotnet add package Moq --version 4.18.4

# Moqの依存関係
dotnet add package Castle.Core --version 4.4.0

# Web関連テスト用
dotnet add package Microsoft.AspNet.Mvc --version 5.2.9
```

### 3. プロジェクト参照の追加

```powershell
# メインプロジェクトへの参照追加
dotnet add reference ../HX.Terminal.csproj

# プロジェクトファイルの確認
Get-Content HX.Terminal.Tests.csproj
```

### 4. プロジェクトのビルド確認

```powershell
# ソリューション全体のビルド
dotnet build

# テストプロジェクトのみビルド
dotnet build HX.Terminal.Tests.csproj
```

---

## 🚀 VSCodeでのテスト実行

### 1. Test Explorerの表示

**方法1: コマンドパレットから**
```
Ctrl+Shift+P → "Test: Show Test Explorer" を選択
```

**方法2: サイドバーから**
- サイドバーの「テスト」アイコンをクリック
- または `Ctrl+Shift+T` のショートカットキー

### 2. テストの発見と一覧表示

Test Explorerが開いたら：
1. **「Refresh Tests」**ボタンをクリック
2. テストが自動的に発見され、階層表示されます

```
📁 HX.Terminal.Tests
├── 📁 BusinessLogic
│   ├── 📄 TerminalRegistBusinessLogicTests
│   │   ├── ✓ ValidateCsvFile_正常なCSVファイルの場合_成功を返す
│   │   ├── ✓ ValidateCsvFile_ファイルが未選択の場合_エラーメッセージを返す
│   │   └── ...
│   ├── 📄 SimRegistBusinessLogicTests
│   └── 📄 TerminalSimHimodukeBusinessLogicTests
├── 📁 Controllers
├── 📁 Models
└── 📁 Integration
```

### 3. 全テストの実行

**方法1: Test Explorerから**
- ルートノード「HX.Terminal.Tests」を右クリック
- **「Run All Tests」**を選択

**方法2: コマンドラインから**
```powershell
# VSCode統合ターミナルで実行
dotnet test
```

**方法3: コマンドパレットから**
```
Ctrl+Shift+P → "Test: Run All Tests" を選択
```

### 4. 個別テストの実行

**特定のテストクラス実行**
- Test Explorerでテストクラスを右クリック
- **「Run Tests」**を選択

**特定のテストメソッド実行**
- 個別のテストメソッドを右クリック
- **「Run Test」**を選択

### 5. カテゴリ別テスト実行

```powershell
# 単体テストのみ実行（統合テストを除外）
dotnet test --filter "TestCategory!=Integration&TestCategory!=Scenario"

# 統合テストのみ実行
dotnet test --filter "TestCategory=Integration"

# エラーハンドリングテストのみ実行
dotnet test --filter "TestCategory=ErrorHandling"

# パフォーマンステストのみ実行
dotnet test --filter "TestCategory=Performance"

# シナリオテストのみ実行
dotnet test --filter "TestCategory=Scenario"
```

### 6. 詳細出力付きテスト実行

```powershell
# 詳細ログ付き実行
dotnet test --logger "console;verbosity=detailed"

# TRX形式のレポート出力
dotnet test --logger "trx;LogFileName=TestResults.trx"

# HTML形式のレポート出力
dotnet test --logger "html;LogFileName=TestResults.html"

# 複数形式同時出力
dotnet test --logger "trx;LogFileName=TestResults.trx" --logger "console;verbosity=normal"
```

---

## 🐛 デバッグ実行

### 1. ブレークポイントの設定

1. テストファイル（例：`TerminalRegistBusinessLogicTests.cs`）を開く
2. デバッグしたい行番号の左側をクリックしてブレークポイントを設定
3. 赤い丸（●）が表示されることを確認

### 2. デバッグ実行の開始

**方法1: Test Explorerから**
- 対象のテストを右クリック
- **「Debug Test」**を選択

**方法2: コードエディターから**
- テストメソッド上で `F5` キーを押す
- または `Ctrl+F5`（デバッグなし実行）

**方法3: コマンドラインから**
```powershell
# 特定テストのデバッグ実行
dotnet test --filter "MethodName=テストメソッド名" --logger "console;verbosity=detailed"
```

### 3. デバッグ中の操作

| キー | 動作 |
|---|---|
| `F10` | ステップオーバー（次の行へ） |
| `F11` | ステップイン（メソッド内部へ） |
| `Shift+F11` | ステップアウト（メソッドから抜ける） |
| `F5` | 続行（次のブレークポイントまで実行） |
| `Shift+F5` | デバッグ停止 |

### 4. 変数の確認

**デバッグ中に以下で変数を確認可能**：
- **VARIABLES**パネル：現在のスコープの変数
- **WATCH**パネル：監視する変数を追加
- **CALL STACK**パネル：呼び出し履歴
- **DEBUG CONSOLE**：式の実行

---

## 📊 テスト結果の確認

### 1. Test Explorerでの結果表示

テスト実行後、Test Explorerで以下の情報を確認できます：

- ✅ **成功**: 緑色のチェックマーク
- ❌ **失敗**: 赤色の×マーク  
- ⏭️ **スキップ**: 黄色の！マーク
- ⏱️ **実行時間**: 各テストの実行時間

### 2. 詳細な結果確認

**失敗したテストの詳細確認**：
1. 失敗したテストをクリック
2. **OUTPUT**パネルで詳細なエラーメッセージを確認
3. スタックトレースでエラー箇所を特定

### 3. コンソール出力の確認

```
Test Run Successful.
Total tests: 30
     Passed: 27
     Failed: 0  
     Skipped: 3
 Total time: 1.4567 Seconds

Tests by Category:
✅ Unit Tests: 18 passed
✅ Integration Tests: 6 passed  
✅ Scenario Tests: 3 passed
⏭️ Performance Tests: 3 skipped (requires database)

Individual Results:
✅ ValidateCsvFile_正常なCSVファイルの場合_成功を返す [1ms]
✅ ValidateCsvFile_ファイルが未選択の場合_エラーメッセージを返す [2ms]
✅ ProcessTerminalCsvFile_正常なCSVデータの場合_成功を返す [15ms]
✅ BaseController_認証チェック_管理者権限で成功 [5ms]
✅ ValidationResult_プロパティ初期化_正常に設定される [1ms]
⏭️ 端末データ登録_完全なワークフロー_正常に完了する [Skipped: データベース接続が必要]
```

### 4. テストカバレッジの確認

```powershell
# コードカバレッジ収集
dotnet test --collect:"XPlat Code Coverage"

# レポート生成（reportgeneratorがインストール済みの場合）
reportgenerator -reports:"TestResults/*/coverage.cobertura.xml" -targetdir:"coveragereport" -reporttypes:Html

# カバレッジレポートを開く
start coveragereport/index.html
```

---

## ❗ トラブルシューティング

### 1. テストが発見されない場合

**問題**: Test Explorerにテストが表示されない

**解決策**:
```powershell
# プロジェクトをクリーンアップ
dotnet clean

# 再ビルド
dotnet build

# テスト一覧表示で確認
dotnet test --list-tests

# VSCodeを再起動
# Ctrl+Shift+P → "Developer: Reload Window"
```

---

## 📚 C#コーディング規約対応テスト

### 1. 更新されたテストファイル一覧

#### **主要テストクラス** (C#コーディング規約適用済み)
- **`TerminalRegistBusinessLogicTests.cs`**: ビジネスロジック層の単体テスト
  - XML文書コメント完備
  - #region/#endregion によるコード整理
  - TestInitialize/TestCleanup パターン適用
  
- **`TerminalControllerTests.cs`**: コントローラー層の単体テスト
  - HTTPアクション別テストメソッド
  - モックオブジェクトの適切な使用
  - 例外処理テストの包括的実装

- **`BaseControllerTests.cs`**: 基盤認証機能テスト (**新規追加**)
  - ユーザー認証テスト
  - 権限チェックテスト
  - セッション管理テスト

- **`ValidationResultTests.cs`**: バリデーション機能テスト
  - プロパティ初期化テスト
  - エラー条件テスト
  - 境界値テスト

### 2. テストカテゴリ体系

```csharp
[TestCategory("UnitTest")]           // 単体テスト
[TestCategory("IntegrationTest")]    // 統合テスト  
[TestCategory("ScenarioTest")]       // シナリオテスト
[TestCategory("ErrorHandling")]      // エラーハンドリング
[TestCategory("Performance")]        // パフォーマンス
[TestCategory("Security")]           // セキュリティ
```

### 3. 規約適用後のテスト実行方法

**単体テストのみ実行** (新しいカテゴリ体系):
```powershell
# 単体テストのみ実行
dotnet test --filter "TestCategory=UnitTest"

# 統合テスト実行（データベース必要）
dotnet test --filter "TestCategory=IntegrationTest"

# セキュリティテスト実行
dotnet test --filter "TestCategory=Security"

# エラーハンドリングテスト実行
dotnet test --filter "TestCategory=ErrorHandling"
```

**詳細ログ出力** (XML文書コメント情報含む):
```powershell
# 詳細出力でテスト実行
dotnet test --logger "console;verbosity=detailed"

# テストメソッドの説明も含めて出力
dotnet test --logger "console;verbosity=diagnostic"
```

### 4. VSCode Test Explorerでの新機能

1. **テストカテゴリ別表示**: 
   - Test Explorerでカテゴリ別にグループ化表示
   - 各テストメソッドにXML文書コメントの説明表示

2. **リージョン別テスト実行**:
   - コード内の #region 単位でのテスト実行が可能
   - 初期化、実行、クリーンアップの段階別実行

3. **モック検証機能**:
   - Moq による依存関係テストの詳細ログ
   - アサーション失敗時の詳細なモック呼び出し履歴

---

## 🗂️ テストデータの活用

### 1. 更新されたテストデータファイル

#### **`Tests/TestData/terminal_test_data.csv`**
```csv
No,端末製造番号1,端末製造番号2,機器PINコード,納品予定日,商品コード,商品名
1,123456789012345,123456789012346,12345,2023/12/01,ABCD1234,テスト商品1
2,234567890123456,234567890123457,23456,2023/12/02,EFGH5678,テスト商品2
...（10件のテストデータ）
```

#### **`Tests/TestData/sim_test_data.csv`**  
```csv
No,端末電話番号,加入者コード,製造番号（ICカード）,ネットワーク暗証番号,PINロック解除,機器PINコード,納品予定日,受注日
1,08012345678,12345678,8981000000000000001,1234,ABC12345,12345,2023/12/01,2023/11/15
2,08023456789,23456789,8981000000000000002,2345,DEF23456,23456,2023/12/02,2023/11/16
...（10件のテストデータ）
```

### 2. データドリブンテストの実行

**CSVファイルを使ったテスト**:
```powershell
# テストデータファイル指定実行
dotnet test --filter "TestCategory=DataDriven"

# 特定のデータファイルのみテスト
dotnet test --filter "FullyQualifiedName~TerminalCsvFileTest"
```

### 3. テストデータ検証

**事前チェック**:
```powershell
# テストデータファイル存在確認
Test-Path "Tests/TestData/terminal_test_data.csv"
Test-Path "Tests/TestData/sim_test_data.csv"

# CSVファイル構造確認
Get-Content "Tests/TestData/terminal_test_data.csv" | Select-Object -First 3
```

---

### 2. NuGetパッケージエラー

**問題**: パッケージが見つからない、バージョン競合

**解決策**:
```powershell
# パッケージキャッシュをクリア
dotnet nuget locals all --clear

# パッケージ復元
dotnet restore

# パッケージ一覧確認
dotnet list package
```

### 3. データベース接続エラー

**問題**: 統合テストでDB接続失敗

**解決策**:
- `web.config`の接続文字列を確認
- Oracleクライアントがインストールされているか確認
- 統合テストは本来`Assert.Inconclusive()`でスキップされる設計

### 4. Moqエラー

**問題**: モックオブジェクト作成時のエラー

**解決策**:
```powershell
# Moqとその依存関係の確認
dotnet list package | Select-String -Pattern "Moq|Castle"

# 必要に応じてバージョン更新
dotnet add package Moq --version 4.18.4
dotnet add package Castle.Core --version 4.4.0
```

### 5. ファイルアクセスエラー

**問題**: テストデータファイルが見つからない

**解決策**:
```powershell
# テストデータファイルの存在確認
Get-ChildItem Tests/TestData/ -Recurse

# ファイルのビルドアクションを設定（.csprojファイル内）
# <Content Include="TestData\*.csv">
#   <CopyToOutputDirectory>Always</CopyToOutputDirectory>
# </Content>
```

### 6. VSCode拡張機能の問題

**問題**: テスト関連の拡張機能が正常に動作しない

**解決策**:
```powershell
# 拡張機能の再インストール
code --uninstall-extension ms-dotnettools.dotnet-test-explorer
code --install-extension ms-dotnettools.dotnet-test-explorer

# VSCodeの設定リセット
# settings.json の該当部分を削除後、再設定
```

---

## 📝 実行頻度とベストプラクティス

### 日常的な実行

**コード変更後**:
```powershell
# 影響範囲のテストのみ実行
dotnet test --filter "ClassName=変更したクラス名Tests"
```

**コミット前**:
```powershell
# 単体テスト全実行
dotnet test --filter "TestCategory!=Integration"
```

**プルリクエスト前**:
```powershell
# 全テスト実行
dotnet test
```

### CI/CD環境での実行

**Azure DevOps Pipeline**:
```yaml
- task: DotNetCoreCLI@2
  displayName: 'Run Tests'
  inputs:
    command: 'test'
    projects: 'Tests/**/*.csproj'
    arguments: '--logger trx --collect "Code coverage"'
```

**GitHub Actions**:
```yaml
- name: Test
  run: dotnet test --no-build --verbosity normal --logger "trx;LogFileName=TestResults.trx"
```

---

## 🎯 まとめ

このマニュアルに従って、以下のテスト実行が可能になります：

1. ✅ **単体テスト**: ビジネスロジック、コントローラー、モデルの個別テスト
2. ✅ **統合テスト**: データベースを含む完全なワークフローテスト  
3. ✅ **エラーハンドリングテスト**: 異常系処理の検証
4. ✅ **シナリオテスト**: ユーザーストーリーベースのテスト
5. ✅ **セキュリティテスト**: 認証・認可機能のテスト (**新規追加**)
6. ✅ **デバッグ実行**: ブレークポイントを使った詳細なデバッグ

**重要なポイント**:
- C#コーディング規約適用により、テストコードの品質が向上
- XML文書コメントにより、テストの目的と期待値が明確化
- #region/#endregion によるコード構造化で可読性向上
- 新しいテストカテゴリ体系で効率的なテスト実行が可能
- 更新されたテストデータファイルでより実用的なテストを実施
- テスト実行前に必ずプロジェクトをビルドする
- 統合テストはデータベース環境が必要
- エラーが発生した場合は、まずビルドエラーを確認する
- 定期的にテストを実行し、品質を維持する

**C#コーディング規約準拠による改善点**:
- **保守性向上**: XML文書化により、テストの意図が明確
- **可読性向上**: リージョン分割と適切な命名規約の適用
- **拡張性向上**: テストカテゴリ体系による柔軟なテスト実行
- **品質向上**: より包括的なテストカバレッジの実現

**問い合わせ先**:
テスト実行に関する問題は、開発チームまでお問い合わせください。

---

*最終更新日: 2025/8/27 (C#コーディング規約対応版)*
