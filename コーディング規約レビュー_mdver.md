# C#コーディング規約レビュー結果（詳細版）

**対象プロジェクト:** HX_Copilot  
**レビュー日:** 2025年9月4日  
**対象ファイル:** Document、Testsフォルダを除くC#ソースコード  
**規約バージョン:** C#コーディング規約Ver1.2

## 1. フォーマット

### 1-1. スペース
**判定:** ✅ 遵守

**遵守状況:**
- 半角スペース使用、全角スペース未使用
- 演算子前後の適切なスペース配置
- コメント内のスペース使用が適切

**具体例:**
```csharp
// 良い例（Controllers/BaseController.cs 行40-42）
return userRole == ROLE_OFFICE_MANAGER || 
       userRole == ROLE_INDUSTRIAL_STAFF || 
       userRole == ROLE_ADMINISTRATOR;
```

### 1-2. インデント
**判定:** ✅ 遵守

**遵守状況:**
- タブ文字またはスペース4文字で統一されている

### 1-3. 改行
**判定:** ✅ 遵守

**遵守状況:**
- メソッド内での適切な改行が実施されている

### 1-4. 折返し
**判定:** ✅ 遵守

**遵守状況:**
- 長い行の適切な折り返し
- 2行目以降のインデント調整

### 1-5. {}
**判定:** ✅ 遵守

**遵守状況:**
- { } が別行記述され、適切にネストされている

### 1-6. ファイル構成
**判定:** ❌ 違反

**問題箇所:**
```csharp
// Models/TerminalRegistModel.cs 行69-75
        #endregion
    }
    }

    /// <summary>
    /// SIM情報登録モデル
    /// </summary>
    public class SimRegistModel
```

**違反内容:**
- 1つのファイルに3つのクラス（TerminalRegistModel、SimRegistModel、TerminalSimHimodukeModel）が定義されている
- 規約「1つのファイルに複数のクラス、インタフェース、列挙型を記述しない」に違反
- TerminalRegistModelクラスの終了時に }が重複している（構文エラー）

## 2. 宣言

### 2.1. 変数

#### 2.1.1. 命名規則
**判定:** ✅ 遵守

**遵守状況:**
- Pascal記法、camel記法が適切に使い分けられている

#### 2.1.2. ローカル変数
**判定:** ✅ 遵守

**遵守状況:**
- ローカル変数の命名が適切

#### 2.1.3. メンバ変数
**判定:** ❌ 違反

**問題箇所:**
```csharp
// Controllers/TerminalController.cs 行17-27
private readonly TerminalRegistBusinessLogic terminalLogic;
private readonly SimRegistBusinessLogic simLogic;
private readonly TerminalSimHimodukeBusinessLogic himodukeLogic;

// DataAccess/TerminalDataAccess.cs 行17
protected string connectionString;

// Models/ValidationResult.cs 行15
private readonly List<string> errors;
```

**違反内容:**
- メンバ変数にアンダースコア(_)プレフィックスが付与されていない
- 規約では「メンバ変数名の前にプレフィックスとして"_"を付加する」と明記されている

**正しい記述例:**
```csharp
private readonly TerminalRegistBusinessLogic _terminalLogic;
private readonly SimRegistBusinessLogic _simLogic;
protected string _connectionString;
private readonly List<string> _errors;
```

#### 2.1.4. 定数
**判定:** ✅ 遵守

**遵守状況:**
```csharp
// Controllers/BaseController.cs 行14-24
private const string ROLE_OFFICE_MANAGER = "21";
private const string ROLE_INDUSTRIAL_STAFF = "60";
private const string ROLE_ADMINISTRATOR = "99";
```
- 定数が全て大文字で記述されている

### 2.2. プロパティ
**判定:** 🟡 一部要改善

**遵守状況:**
- プロパティ名はPascal記法で適切

**改善点:**
```csharp
// Models/ValidationResult.cs 行36-39
public bool IsValid
{
    get { return !errors.Any(); }
}
```
- get/setアクセサの記述方法に一貫性がない
- 同一行記述と別行記述が混在している

### 2.3. メソッド
**判定:** ✅ 遵守

**遵守状況:**
- 動詞+名詞の形式で命名
- Pascal記法を使用

### 2.4. クラス
**判定:** ✅ 遵守

**遵守状況:**
- クラス名がPascal記法で記述されている

### 2.5. インタフェース
**判定:** ✅ 遵守

**遵守状況:**
- 独自インタフェースは未使用だが、.NET標準インタフェースを適切に使用

### 2.6. ネームスペース
**判定:** ✅ 遵守

**遵守状況:**
```csharp
namespace HX.Terminal
namespace HX.Terminal.Controllers
namespace HX.Terminal.Models
namespace HX.Terminal.BusinessLogic
namespace HX.Terminal.DataAccess
```
- 適切な階層構造

### 2.7. 列挙型
**判定:** ✅ 遵守（該当なし）

## 3. 禁止事項

### 3.1. public メンバフィールドの禁止
**判定:** ✅ 遵守

**遵守状況:**
- publicフィールドは使用されていない
- 外部アクセスが必要な場合はプロパティを使用

### 3.2. アクセス修飾子なしの宣言の禁止
**判定:** ✅ 遵守

**遵守状況:**
- すべての宣言で適切なアクセス修飾子が指定されている

### 3.3. コピー＆ペーストの禁止
**判定:** ❌ 違反

**問題箇所:**
```csharp
// Controllers/TerminalController.cs 行257-260, 行265-268
// 権限チェック
private bool CheckAuthorization()
{
    var userRole = GetCurrentUserRole();
    return userRole == "21" || userRole == "60" || userRole == "99";
}

// 権限チェック（内部メソッド）
private bool CheckAuthorization()
{
    return base.CheckAuthorization();
}
```

**違反内容:**
- 同じメソッド名で異なる実装が重複定義されている（コンパイルエラーの原因）
- 権限チェック処理が複数箇所に散在している

### 3.4. コードのコメントアウトの禁止
**判定:** ❌ 違反

**問題箇所:**
```csharp
// Global.asax.cs 行27-28
// ログ出力（log4netなどを使用）
// Logger.Error("Application Error", exception);
```

**違反内容:**
- コメントアウトされたコードが残存している
- 規約では「コメントアウトされたコードをそのまま残してはいけない」と明記されている

## 4. 例外処理

### 4.1. 例外とは
**判定:** ✅ 遵守

### 4.2. 例外処理の構文
**判定:** ✅ 遵守

**遵守状況:**
- try-catch-finally構文を適切に使用

### 4.3. 例外処理の原則
**判定:** 🟡 一部要改善

**問題箇所:**
```csharp
// Controllers/BaseController.cs 行53
catch (Exception)
{
    return false;
}
```

**改善点:**
- 例外の種類を特定せずに全例外をキャッチしている
- ログ出力などの適切な処理が不足

### 4.4. 例外処理による禁止事項
**判定:** ❌ 違反

**問題箇所:**
```csharp
// BusinessLogic/TerminalBusinessLogic.cs 行148, 292, 416, 444
catch (Exception ex)
{
    result.AddError($"ファイル処理中にエラーが発生しました：{ex.Message}");
    return result;
}

// Controllers/BaseController.cs 行53, 69, 85
catch (Exception)
{
    return false;
}
```

**違反内容:**
- `catch (Exception ex)`の使用（規約で明確に禁止）
- 規約では「catch (Exception ex) という catch 句の記述は禁止する」と明記されている
- 具体的な例外の種類を特定せずに全例外をキャッチしている

## 5. Disposeメソッド

### 5.1-5.3. IDisposableインタフェース
**判定:** ✅ 遵守

**遵守状況:**
- IDisposableを実装したクラス（SqlConnectionなど）を適切に使用

### 5.4. using ステートメント
**判定:** ✅ 遵守

**遵守状況:**
```csharp
// DataAccess/TerminalDataAccess.cs 行62-66
using (var connection = new SqlConnection(connectionString))
{
    connection.Open();
    // 処理
}
```
- usingステートメントが適切に使用されている

## 6. メソッドの行数制限

### 6.1. メソッドの行数
**判定:** 🟡 一部要改善

**問題箇所:**
```csharp
// BusinessLogic/TerminalBusinessLogic.cs ProcessTerminalCsvFileメソッド
// 約80行程度の長いメソッド（100行以内だが機能分割を推奨）
```

**改善点:**
- 100行は超えていないが、CSVファイル処理とバリデーションが1つのメソッドに集約されている
- 機能別のメソッド分割を検討すべき

## 重大な問題

### ❗ 構文エラー
**問題箇所:**
```csharp
// Models/TerminalRegistModel.cs 行69-71
        #endregion
    }
    }  // <- 余分な閉じ括弧
```
- クラス定義の終了で余分な閉じ括弧がある
- コンパイルエラーの原因となる

### ❗ メソッド名重複
**問題箇所:**
```csharp
// Controllers/TerminalController.cs
// CheckAuthorizationメソッドが同じクラス内で重複定義されている
```
- 同一クラス内で同じメソッド名の重複定義

## 総合評価

### ✅ 遵守項目（15項目）
- 基本フォーマット（インデント、改行、{}の使用）
- 命名規則（クラス、メソッド、プロパティ、定数）
- ネームスペース構成
- publicフィールドの禁止
- usingステートメントの適切な使用
- IDisposableリソースの適切な処理

### ❌ 違反項目（6項目）
1. **ファイル構成違反** - 1ファイル複数クラス定義
2. **メンバ変数命名規約違反** - アンダースコアプレフィックス未使用
3. **コメントアウトコード存在** - 削除が必要
4. **コピー＆ペースト違反** - 重複処理の存在
5. **例外処理禁止事項違反** - catch(Exception)の使用
6. **構文エラー** - 余分な閉じ括弧、メソッド名重複

### 🟡 改善推奨項目（3項目）
1. プロパティアクセサ記述方法の統一
2. 例外処理の詳細化
3. 長いメソッドの機能分割

## 優先度別改善アクション

### 🚨 緊急対応（コンパイルエラー修正）
1. Models/TerminalRegistModel.cs の余分な閉じ括弧削除
2. Controllers/TerminalController.cs の重複メソッド定義修正

### 🔴 高優先度（規約違反修正）
1. メンバ変数にアンダースコアプレフィックス追加
2. 1ファイル複数クラス定義の分離
3. catch(Exception)の具体的例外種別への変更
4. コメントアウトコードの削除

### 🟡 中優先度（品質向上）
1. 重複処理の共通メソッド化
2. 例外処理時のログ出力追加
3. 長いメソッドの機能分割

**全体評価:** 基本的な構造は良好だが、重要な規約違反と構文エラーが存在するため、早急な修正が必要。
