# C#コーディング規約レビュー結果（修正版準拠）

**対象プロジェクト:** HX_Copilot  
**レビュー日:** 2025年9月4日  
**対象ファイル:** Document、Testsフォルダを除くC#ソースコード  
**規約バージョン:** C#コーディング規約Ver1.2（修正版）

## 1. フォーマット

### 1-1. スペース
**判定:** ✅ 遵守

**遵守状況:**
- 半角スペースのみ使用、全角スペース未使用
- 演算子前後の適切なスペース配置
- コメント "//" 直後のスペース使用
- カンマ後のスペース適切配置

**確認箇所:**
```csharp
// Controllers/BaseController.cs 行40-42
return userRole == ROLE_OFFICE_MANAGER || 
       userRole == ROLE_INDUSTRIAL_STAFF || 
       userRole == ROLE_ADMINISTRATOR;

// BusinessLogic/TerminalBusinessLogic.cs 行85
var columns = line.Split(',');
```

### 1-2. インデント
**判定:** ✅ 遵守

**遵守状況:**
- タブ（4文字スペース相当）を使用
- 一貫したインデント適用

### 1-3. 空行
**判定:** ✅ 遵守

**遵守状況:**
- メソッド内処理の切れ目で適切な空行使用
- 可読性向上に効果的

### 1-4. 改行
**判定:** ✅ 遵守

**遵守状況:**
- 長い式の適切な改行
- 2行目以降のインデント調整

### 1-5. {}
**判定:** ✅ 遵守

**遵守状況:**
- { } が宣言と別行で記述されている
- 改行後の記述が徹底されている

### 1-6. ファイル分け
**判定:** ❌ 重大な違反

**問題箇所:**
```csharp
// Models/TerminalRegistModel.cs 全体
namespace HX.Terminal.Models
{
    public class TerminalRegistModel { /* ... */ }
    public class SimRegistModel { /* ... */ }
    public class TerminalSimHimodukeModel { /* ... */ }
}
```

**違反内容:**
- **1つのファイルに3つのクラスが定義されている**
- 規約「ファイルは一つのファイルに複数のクラス、インタフェース、列挙体を記述せず、それぞれファイルとクラス、インタフェース、列挙体が1:1になるように作成する」に明確に違反

**構文エラーも併発:**
```csharp
// Models/TerminalRegistModel.cs 行69-71
        #endregion
    }
    }  // <- 余分な閉じ括弧でコンパイルエラー
```

## 2. 宣言

### 2.1. 変数

#### 2.1.1. 共通規則
**判定:** ✅ 遵守

**遵守状況:**
- 小文字から始まるcamelCase使用
- 単語区切りで大文字化
- 型プレフィックス未使用（規約通り）

#### 2.1.2. ローカル変数
**判定:** ✅ 遵守

**遵守状況:**
- 共通規則に従った命名

#### 2.1.3. メンバ変数
**判定:** ❌ 重大な違反

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
- **メンバ変数にアンダースコア(_)プレフィックスが付与されていない**
- 規約「メンバ変数を宣言する際には、ローカル変数と同様の命名規則により考えた変数名のプレフィックスに"_"を付与する」に明確に違反

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
**判定:** ✅ 遵守

**遵守状況:**
- Pascal記法で命名されている
- get/setアクセサの同一行記述が許可範囲内
- 規約「プロパティのgetアクセサとsetアクセサに限り、{}を同一行内に記述することを許可する」に準拠

**例:**
```csharp
// Models/TerminalRegistModel.cs
public DateTime CreatedDate { get; set; }
public string CreatedProgramId { get; set; }
```

### 2.3. メソッド
**判定:** ✅ 遵守

**遵守状況:**
- 動詞+名詞の形式で命名（GetUser、SetPassword等）
- Pascal記法使用

### 2.4. クラス
**判定:** ✅ 遵守

**遵守状況:**
- Pascal記法で命名
- 意味のあるクラス名

### 2.5. インタフェース
**判定:** ✅ 遵守

**遵守状況:**
- 独自インタフェース未使用だが、.NET標準インタフェース（IDisposable等）を適切に使用

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

### 2.7. 列挙体
**判定:** ✅ 遵守（該当なし）

## 3. 禁止事項

### 3.1. public メンバフィールドの禁止
**判定:** ✅ 遵守

**遵守状況:**
- publicフィールド未使用
- 外部アクセスはプロパティ経由

### 3.2. アクセス修飾子なしの宣言の禁止
**判定:** ✅ 遵守

**遵守状況:**
- 全ての宣言で適切なアクセス修飾子指定

### 3.3. コピー、ペーストの禁止
**判定:** ❌ 違反

**問題箇所:**
```csharp
// Controllers/TerminalController.cs 行257-268
// 同じメソッド名で異なる実装が重複定義（コンパイルエラー要因）
private bool CheckAuthorization() // 1回目の定義
{
    var userRole = GetCurrentUserRole();
    return userRole == "21" || userRole == "60" || userRole == "99";
}

private bool CheckAuthorization() // 2回目の定義（重複）
{
    return base.CheckAuthorization();
}
```

**違反内容:**
- 同一メソッド名の重複定義
- 権限チェック処理のコピー＆ペースト
- GetCurrentUserRoleメソッドが未定義でコンパイルエラー

### 3.4. コードのコメントアウトの禁止
**判定:** ❌ 違反

**問題箇所:**
```csharp
// Global.asax.cs 行27-28
// ログ出力（log4netなどを使用）
// Logger.Error("Application Error", exception);
```

**違反内容:**
- コメントアウトされたコードが残存
- 規約「コード中にコメントアウトしたコードをそのまま残してはいけない」に明確に違反

## 4. 例外処理

### 4.1. 例外とは
**判定:** ✅ 遵守

### 4.2. 例外処理の構文
**判定:** ✅ 遵守

**遵守状況:**
- try-catch-finally構文を適切に使用

### 4.3. 例外捕捉の原則
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
- 具体的な例外種別の特定が不十分
- 例外処理時の適切な処理（ログ出力等）が不足

### 4.4. 例外処理における禁止事項
**判定:** ❌ 重大な違反

#### throw ex; の禁止
**判定:** ✅ 遵守（該当なし）

#### catch (Exception ex) の禁止
**判定:** ❌ 明確な違反

**問題箇所:**
```csharp
// BusinessLogic/TerminalBusinessLogic.cs 行148
catch (Exception ex)
{
    result.AddError($"ファイル処理中にエラーが発生しました：{ex.Message}");
    return result;
}

// 同様の違反が行292, 416, 444でも発生

// Controllers/BaseController.cs 行53, 69, 85
catch (Exception)
{
    return false;
}
```

**違反内容:**
- **catch (Exception ex) の使用（規約で明確に禁止）**
- 規約「catch (Exception ex) という catch 句の記述は禁止する」に直接違反
- StackOverflowException, OutOfMemoryException等の致命的例外も捕捉してしまう

## 5. Disposeメソッド

### 5.1-5.3. IDisposableインタフェース
**判定:** ✅ 遵守

**遵守状況:**
- IDisposableを実装したクラス（SqlConnection等）を適切に使用

### 5.4. using ステートメント
**判定:** ✅ 遵守

**遵守状況:**
```csharp
// DataAccess/TerminalDataAccess.cs
using (var connection = new SqlConnection(connectionString))
{
    connection.Open();
    // 処理
}
```
- usingステートメントを適切に使用

## 6. メソッドの行数制限について

### メソッドの行数制限
**判定:** 🟡 一部要改善

**問題箇所:**
```csharp
// BusinessLogic/TerminalBusinessLogic.cs ProcessTerminalCsvFileメソッド
// 約80行程度（100行以内だが機能分割を推奨）
```

**改善点:**
- CSVファイル処理とバリデーションが1つのメソッドに集約
- 機能別メソッド分割を検討

## 🚨 コンパイルエラー要因

### 1. 構文エラー
```csharp
// Models/TerminalRegistModel.cs 行69-71
        #endregion
    }
    }  // <- 余分な閉じ括弧
```

### 2. メソッド名重複
```csharp
// Controllers/TerminalController.cs
// CheckAuthorizationメソッドが同じクラス内で重複定義
```

### 3. 未定義メソッド参照
```csharp
// Controllers/TerminalController.cs 行258
var userRole = GetCurrentUserRole(); // このメソッドが定義されていない
```

## 総合評価

### ✅ 遵守項目（12項目）
- スペース、インデント、改行、{}の使用方法
- 命名規則（クラス、メソッド、プロパティ、定数、ローカル変数）
- ネームスペース構成
- publicフィールドの禁止
- usingステートメントの適切使用
- アクセス修飾子の明示

### ❌ 重大な違反項目（5項目）
1. **ファイル分け違反** - 1ファイル複数クラス定義（規約1-6）
2. **メンバ変数命名違反** - アンダースコアプレフィックス未使用（規約2.1.3）
3. **コメントアウトコード存在** - 削除が必要（規約3.4）
4. **コピー＆ペースト違反** - 重複メソッド定義（規約3.3）
5. **例外処理禁止違反** - catch(Exception)の使用（規約4.4）

### 🚨 コンパイルエラー（3項目）
1. 余分な閉じ括弧による構文エラー
2. メソッド名重複によるコンパイルエラー
3. 未定義メソッド参照によるコンパイルエラー

### 🟡 改善推奨項目（2項目）
1. 例外処理の詳細化（ログ出力等）
2. 長いメソッドの機能分割

## 優先度別改善アクション

### 🚨 緊急対応（コンパイルエラー修正）
1. **Models/TerminalRegistModel.cs** - 余分な閉じ括弧削除
2. **Controllers/TerminalController.cs** - 重複メソッド定義修正
3. **Controllers/TerminalController.cs** - GetCurrentUserRoleメソッド実装

### 🔴 最優先対応（規約違反修正）
1. **ファイル分け修正** - 3つのクラスを個別ファイルに分離
2. **メンバ変数修正** - 全メンバ変数にアンダースコア(_)プレフィックス追加
3. **例外処理修正** - catch(Exception)を具体的例外種別に変更
4. **コメントアウトコード削除** - Global.asax.csの未使用コード除去

### 🟡 推奨対応（品質向上）
1. 例外処理時のログ出力機能追加
2. 長いメソッドの機能分割検討

**全体評価:** 規約準拠度が低く、コンパイルエラーも含む重要な問題が多数存在。緊急対応と最優先対応の実施が必要。
