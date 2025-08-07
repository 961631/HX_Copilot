<%@ page language="java" contentType="text/html; charset=UTF-8" pageEncoding="UTF-8"%>
<%@ taglib prefix="c" uri="http://java.sun.com/jsp/jstl/core" %>
<!DOCTYPE html>
<html lang="ja">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>端末-SIM情報紐付け登録 - 救急情報管理システム</title>
    
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
                    <h1><i class="fas fa-mobile-alt me-2"></i>救急情報管理システム - 端末-SIM情報紐付け登録</h1>
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
                <a class="nav-link" href="/regist"><i class="fas fa-upload me-1"></i>KDDI受領データ登録</a>
            </li>
            <li class="nav-item">
                <a class="nav-link active" href="/himoduke"><i class="fas fa-link me-1"></i>端末-SIM情報紐付け登録</a>
            </li>
            <li class="nav-item">
                <a class="nav-link" href="/result"><i class="fas fa-list me-1"></i>SIM情報紐付け結果一覧</a>
            </li>
        </ul>

        <!-- Messages -->
        <c:if test="${not empty success}">
            <div class="alert alert-success alert-dismissible fade show" role="alert">
                <i class="fas fa-check-circle me-2"></i>${success}
                <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
            </div>
        </c:if>
        
        <c:if test="${not empty errors}">
            <div class="alert alert-danger alert-dismissible fade show" role="alert">
                <h6 class="alert-heading"><i class="fas fa-exclamation-triangle me-2"></i>入力エラー</h6>
                <div class="error-list">
                    <c:forEach var="error" items="${errors}">
                        <div>${error}</div>
                    </c:forEach>
                </div>
                <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
            </div>
        </c:if>

        <!-- Main Content -->
        <div class="row justify-content-center">
            <div class="col-md-8">
                <div class="card">
                    <div class="card-header">
                        <h4 class="mb-0"><i class="fas fa-link me-2"></i>端末-SIM情報紐付け登録</h4>
                    </div>
                    <div class="card-body">
                        <form action="/himoduke/register" method="post" onsubmit="return validateHimodukeForm()">
                            <div class="row">
                                <div class="col-md-6 mb-3">
                                    <label for="imei" class="form-label">
                                        <i class="fas fa-mobile me-1"></i>端末製造番号 (IMEI) <span class="text-danger">*</span>
                                    </label>
                                    <input type="text" 
                                           class="form-control" 
                                           id="imei" 
                                           name="imei" 
                                           value="${inputImei}"
                                           placeholder="123456789012345"
                                           maxlength="15"
                                           required>
                                    <div class="invalid-feedback"></div>
                                    <small class="form-text text-muted">15桁の数字で入力してください</small>
                                </div>
                                
                                <div class="col-md-6 mb-3">
                                    <label for="iccid" class="form-label">
                                        <i class="fas fa-sim-card me-1"></i>ICカード番号 (ICCID) <span class="text-danger">*</span>
                                    </label>
                                    <input type="text" 
                                           class="form-control" 
                                           id="iccid" 
                                           name="iccid" 
                                           value="${inputIccid}"
                                           placeholder="1234567890123456789"
                                           maxlength="19"
                                           required>
                                    <div class="invalid-feedback"></div>
                                    <small class="form-text text-muted">19桁の数字で入力してください</small>
                                </div>
                            </div>
                            
                            <div class="d-grid gap-2 d-md-flex justify-content-md-end">
                                <button type="reset" class="btn btn-secondary me-md-2">
                                    <i class="fas fa-undo me-1"></i>クリア
                                </button>
                                <button type="submit" class="btn btn-primary">
                                    <i class="fas fa-save me-1"></i>紐付け登録
                                    <span class="loading-spinner spinner-border spinner-border-sm ms-2" role="status"></span>
                                </button>
                            </div>
                        </form>
                    </div>
                </div>

                <!-- Input Specifications -->
                <div class="card mt-4">
                    <div class="card-header">
                        <h5 class="mb-0"><i class="fas fa-info-circle me-2"></i>入力仕様</h5>
                    </div>
                    <div class="card-body">
                        <div class="row">
                            <div class="col-md-6">
                                <h6><i class="fas fa-mobile text-primary me-1"></i>端末製造番号 (IMEI)</h6>
                                <ul class="list-unstyled">
                                    <li><i class="fas fa-check text-success me-1"></i>15桁の数字のみ</li>
                                    <li><i class="fas fa-check text-success me-1"></i>重複チェック実施</li>
                                    <li><i class="fas fa-check text-success me-1"></i>必須入力</li>
                                </ul>
                                <div class="bg-light p-2 rounded">
                                    <small><strong>例:</strong> 123456789012345</small>
                                </div>
                            </div>
                            
                            <div class="col-md-6">
                                <h6><i class="fas fa-sim-card text-primary me-1"></i>ICカード番号 (ICCID)</h6>
                                <ul class="list-unstyled">
                                    <li><i class="fas fa-check text-success me-1"></i>19桁の数字のみ</li>
                                    <li><i class="fas fa-check text-success me-1"></i>重複チェック実施</li>
                                    <li><i class="fas fa-check text-success me-1"></i>必須入力</li>
                                </ul>
                                <div class="bg-light p-2 rounded">
                                    <small><strong>例:</strong> 1234567890123456789</small>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Process Flow -->
                <div class="card mt-4">
                    <div class="card-header">
                        <h5 class="mb-0"><i class="fas fa-route me-2"></i>処理フロー</h5>
                    </div>
                    <div class="card-body">
                        <div class="row text-center">
                            <div class="col-md-3 mb-3">
                                <div class="bg-primary text-white p-3 rounded">
                                    <i class="fas fa-keyboard fa-2x mb-2"></i>
                                    <div>1. 入力</div>
                                    <small>IMEI・ICCID入力</small>
                                </div>
                            </div>
                            <div class="col-md-3 mb-3">
                                <div class="bg-info text-white p-3 rounded">
                                    <i class="fas fa-check-double fa-2x mb-2"></i>
                                    <div>2. 検証</div>
                                    <small>形式・重複チェック</small>
                                </div>
                            </div>
                            <div class="col-md-3 mb-3">
                                <div class="bg-success text-white p-3 rounded">
                                    <i class="fas fa-database fa-2x mb-2"></i>
                                    <div>3. 登録</div>
                                    <small>データベース保存</small>
                                </div>
                            </div>
                            <div class="col-md-3 mb-3">
                                <div class="bg-warning text-white p-3 rounded">
                                    <i class="fas fa-list fa-2x mb-2"></i>
                                    <div>4. 確認</div>
                                    <small>結果一覧で確認</small>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Quick Links -->
                <div class="card mt-4">
                    <div class="card-body text-center">
                        <h6 class="mb-3"><i class="fas fa-external-link-alt me-2"></i>関連機能</h6>
                        <a href="/regist" class="btn btn-outline-primary me-2">
                            <i class="fas fa-upload me-1"></i>データ登録
                        </a>
                        <a href="/result" class="btn btn-outline-info">
                            <i class="fas fa-list me-1"></i>結果一覧
                        </a>
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