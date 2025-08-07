<%@ page language="java" contentType="text/html; charset=UTF-8" pageEncoding="UTF-8"%>
<%@ taglib prefix="c" uri="http://java.sun.com/jsp/jstl/core" %>
<!DOCTYPE html>
<html lang="ja">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>KDDI受領データ登録 - 救急情報管理システム</title>
    
    <!-- Bootstrap CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css" rel="stylesheet">
    <link href="/static/css/emergency.css" rel="stylesheet">
</head>
<body>
    <!-- Header -->
    <div class="header">
        <div class="container">
            <div class="row">
                <div class="col-12">
                    <h1><i class="fas fa-mobile-alt me-2"></i>救急情報管理システム - KDDI受領データ登録</h1>
                </div>
            </div>
        </div>
    </div>

    <!-- Navigation -->
    <div class="container">
        <ul class="nav nav-tabs mb-4">
            <li class="nav-item">
                <a class="nav-link" href="/"><i class="fas fa-home me-1"></i>トップ</a>
            </li>
            <li class="nav-item">
                <a class="nav-link active" href="/regist"><i class="fas fa-upload me-1"></i>KDDI受領データ登録</a>
            </li>
            <li class="nav-item">
                <a class="nav-link" href="/himoduke"><i class="fas fa-link me-1"></i>端末-SIM情報紐付け登録</a>
            </li>
            <li class="nav-item">
                <a class="nav-link" href="/result"><i class="fas fa-list me-1"></i>SIM情報紐付け結果一覧</a>
            </li>
        </ul>

        <!-- Messages -->
        <c:if test="${not empty terminalSuccess}">
            <div class="alert alert-success alert-dismissible fade show" role="alert">
                <i class="fas fa-check-circle me-2"></i>${terminalSuccess}
                <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
            </div>
        </c:if>
        
        <c:if test="${not empty terminalError}">
            <div class="alert alert-danger alert-dismissible fade show" role="alert">
                <i class="fas fa-exclamation-circle me-2"></i>${terminalError}
                <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
            </div>
        </c:if>
        
        <c:if test="${not empty terminalErrors}">
            <div class="alert alert-danger alert-dismissible fade show" role="alert">
                <h6 class="alert-heading"><i class="fas fa-exclamation-triangle me-2"></i>端末データ登録エラー</h6>
                <div class="error-list">
                    <c:forEach var="error" items="${terminalErrors}">
                        <div>${error}</div>
                    </c:forEach>
                </div>
                <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
            </div>
        </c:if>
        
        <c:if test="${not empty simSuccess}">
            <div class="alert alert-success alert-dismissible fade show" role="alert">
                <i class="fas fa-check-circle me-2"></i>${simSuccess}
                <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
            </div>
        </c:if>
        
        <c:if test="${not empty simError}">
            <div class="alert alert-danger alert-dismissible fade show" role="alert">
                <i class="fas fa-exclamation-circle me-2"></i>${simError}
                <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
            </div>
        </c:if>
        
        <c:if test="${not empty simErrors}">
            <div class="alert alert-danger alert-dismissible fade show" role="alert">
                <h6 class="alert-heading"><i class="fas fa-exclamation-triangle me-2"></i>SIMデータ登録エラー</h6>
                <div class="error-list">
                    <c:forEach var="error" items="${simErrors}">
                        <div>${error}</div>
                    </c:forEach>
                </div>
                <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
            </div>
        </c:if>

        <!-- Main Content -->
        <div class="row">
            <!-- Terminal Data Registration -->
            <div class="col-md-6 mb-4">
                <div class="card">
                    <div class="card-header">
                        <h4 class="mb-0"><i class="fas fa-mobile me-2"></i>端末データ登録</h4>
                    </div>
                    <div class="card-body">
                        <form action="/regist/terminal" method="post" enctype="multipart/form-data">
                            <div class="file-upload-area mb-3">
                                <input type="file" name="terminalFile" id="terminalFile" accept=".csv" class="d-none" required>
                                <label for="terminalFile" class="d-block cursor-pointer">
                                    <i class="fas fa-cloud-upload-alt fa-3x mb-3 text-muted"></i>
                                    <div class="file-name">端末データCSVファイルを選択してください</div>
                                    <div class="file-size text-muted"></div>
                                    <small class="text-muted">クリックまたはドラッグ&ドロップでファイルを選択</small>
                                </label>
                            </div>
                            
                            <div class="mb-3">
                                <h6><i class="fas fa-info-circle me-1"></i>CSV形式</h6>
                                <div class="bg-light p-3 rounded">
                                    <small>
                                        <strong>ヘッダー行:</strong> 端末製造番号,機器PINコード,商品コード<br>
                                        <strong>データ例:</strong> 123456789012345,12345,ABC12345<br>
                                        <strong>要件:</strong><br>
                                        ・端末製造番号: 15桁の数字<br>
                                        ・機器PINコード: 5桁の数字<br>
                                        ・商品コード: 8桁の英数字
                                    </small>
                                </div>
                            </div>
                            
                            <button type="submit" class="btn btn-primary">
                                <i class="fas fa-upload me-1"></i>端末データ登録
                                <span class="loading-spinner spinner-border spinner-border-sm ms-2" role="status"></span>
                            </button>
                        </form>
                    </div>
                </div>
            </div>

            <!-- SIM Data Registration -->
            <div class="col-md-6 mb-4">
                <div class="card">
                    <div class="card-header">
                        <h4 class="mb-0"><i class="fas fa-sim-card me-2"></i>SIMデータ登録</h4>
                    </div>
                    <div class="card-body">
                        <form action="/regist/sim" method="post" enctype="multipart/form-data">
                            <div class="file-upload-area mb-3">
                                <input type="file" name="simFile" id="simFile" accept=".csv" class="d-none" required>
                                <label for="simFile" class="d-block cursor-pointer">
                                    <i class="fas fa-cloud-upload-alt fa-3x mb-3 text-muted"></i>
                                    <div class="file-name">SIMデータCSVファイルを選択してください</div>
                                    <div class="file-size text-muted"></div>
                                    <small class="text-muted">クリックまたはドラッグ&ドロップでファイルを選択</small>
                                </label>
                            </div>
                            
                            <div class="mb-3">
                                <h6><i class="fas fa-info-circle me-1"></i>CSV形式</h6>
                                <div class="bg-light p-3 rounded">
                                    <small>
                                        <strong>ヘッダー行:</strong> 電話番号,加入者コード,ICカード番号,ネットワーク暗証番号,SIMロック解除コード<br>
                                        <strong>データ例:</strong> 09012345678,ABC12345,1234567890123456789,1234,ABC12345<br>
                                        <strong>要件:</strong><br>
                                        ・電話番号: 11桁の数字<br>
                                        ・加入者コード: 8桁の英数字<br>
                                        ・ICカード番号: 19桁の数字<br>
                                        ・ネットワーク暗証番号: 4桁の数字<br>
                                        ・SIMロック解除コード: 8桁の英数字
                                    </small>
                                </div>
                            </div>
                            
                            <button type="submit" class="btn btn-primary">
                                <i class="fas fa-upload me-1"></i>SIMデータ登録
                                <span class="loading-spinner spinner-border spinner-border-sm ms-2" role="status"></span>
                            </button>
                        </form>
                    </div>
                </div>
            </div>
        </div>

        <!-- Usage Notes -->
        <div class="row">
            <div class="col-12">
                <div class="card">
                    <div class="card-header">
                        <h5 class="mb-0"><i class="fas fa-exclamation-triangle me-2"></i>注意事項</h5>
                    </div>
                    <div class="card-body">
                        <ul class="mb-0">
                            <li>CSVファイルは UTF-8 エンコーディングで保存してください。</li>
                            <li>ファイルサイズは最大 10MB までアップロード可能です。</li>
                            <li>重複するデータがある場合、エラーとして表示されます。</li>
                            <li>データ形式が正しくない場合、該当行のエラー詳細が表示されます。</li>
                            <li>アップロード処理中はページを閉じたり更新したりしないでください。</li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Footer -->
    <footer class="mt-5 py-4 bg-light">
        <div class="container text-center">
            <p class="mb-0 text-muted">© 2023 救急情報管理システム - セコム工業株式会社</p>
        </div>
    </footer>

    <!-- Bootstrap JS -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <script src="https://code.jquery.com/jquery-3.7.0.min.js"></script>
    <script src="/static/js/emergency.js"></script>
</body>
</html>