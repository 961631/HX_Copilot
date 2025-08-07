<%@ page language="java" contentType="text/html; charset=UTF-8" pageEncoding="UTF-8"%>
<%@ taglib prefix="c" uri="http://java.sun.com/jsp/jstl/core" %>
<!DOCTYPE html>
<html lang="ja">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>救急情報管理システム - 新規端末情報登録</title>
    
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
                    <h1><i class="fas fa-mobile-alt me-2"></i>救急情報管理システム - 新規端末情報登録</h1>
                </div>
            </div>
        </div>
    </div>

    <!-- Navigation -->
    <div class="container">
        <ul class="nav nav-tabs mb-4">
            <li class="nav-item">
                <a class="nav-link active" href="/"><i class="fas fa-home me-1"></i>トップ</a>
            </li>
            <li class="nav-item">
                <a class="nav-link" href="/regist"><i class="fas fa-upload me-1"></i>KDDI受領データ登録</a>
            </li>
            <li class="nav-item">
                <a class="nav-link" href="/himoduke"><i class="fas fa-link me-1"></i>端末-SIM情報紐付け登録</a>
            </li>
            <li class="nav-item">
                <a class="nav-link" href="/result"><i class="fas fa-list me-1"></i>SIM情報紐付け結果一覧</a>
            </li>
        </ul>

        <!-- Main Content -->
        <div class="row">
            <div class="col-12">
                <div class="card">
                    <div class="card-header">
                        <h3 class="mb-0"><i class="fas fa-tachometer-alt me-2"></i>システム概要</h3>
                    </div>
                    <div class="card-body">
                        <p class="lead">このシステムは、みまもりホン後継機対応の新規端末情報登録を行うためのシステムです。</p>
                        
                        <div class="row mt-4">
                            <div class="col-md-6 mb-3">
                                <div class="card h-100">
                                    <div class="card-body text-center">
                                        <i class="fas fa-upload fa-3x text-primary mb-3"></i>
                                        <h5 class="card-title">KDDI受領データ登録</h5>
                                        <p class="card-text">端末データとSIMデータをCSVファイルから一括登録します。</p>
                                        <a href="/regist" class="btn btn-primary">
                                            <i class="fas fa-upload me-1"></i>データ登録画面へ
                                        </a>
                                    </div>
                                </div>
                            </div>
                            
                            <div class="col-md-6 mb-3">
                                <div class="card h-100">
                                    <div class="card-body text-center">
                                        <i class="fas fa-link fa-3x text-success mb-3"></i>
                                        <h5 class="card-title">端末-SIM情報紐付け登録</h5>
                                        <p class="card-text">端末製造番号(IMEI)とICカード番号(ICCID)を手動で紐付けます。</p>
                                        <a href="/himoduke" class="btn btn-success">
                                            <i class="fas fa-link me-1"></i>紐付け登録画面へ
                                        </a>
                                    </div>
                                </div>
                            </div>
                        </div>
                        
                        <div class="row mt-3">
                            <div class="col-md-6 mx-auto">
                                <div class="card">
                                    <div class="card-body text-center">
                                        <i class="fas fa-list fa-3x text-info mb-3"></i>
                                        <h5 class="card-title">SIM情報紐付け結果一覧</h5>
                                        <p class="card-text">当日登録された紐付けデータの一覧を確認・削除できます。</p>
                                        <a href="/result" class="btn btn-info">
                                            <i class="fas fa-list me-1"></i>結果一覧画面へ
                                        </a>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        
        <!-- System Information -->
        <div class="row mt-4">
            <div class="col-12">
                <div class="card">
                    <div class="card-header">
                        <h5 class="mb-0"><i class="fas fa-info-circle me-2"></i>利用可能権限</h5>
                    </div>
                    <div class="card-body">
                        <div class="row">
                            <div class="col-md-4">
                                <div class="text-center p-3 border rounded">
                                    <i class="fas fa-user-shield fa-2x text-primary mb-2"></i>
                                    <h6>権限 21</h6>
                                    <small class="text-muted">ココセコム事務センター責任者</small>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="text-center p-3 border rounded">
                                    <i class="fas fa-industry fa-2x text-success mb-2"></i>
                                    <h6>権限 60</h6>
                                    <small class="text-muted">セコム工業</small>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="text-center p-3 border rounded">
                                    <i class="fas fa-crown fa-2x text-warning mb-2"></i>
                                    <h6>権限 99</h6>
                                    <small class="text-muted">管理者権限</small>
                                </div>
                            </div>
                        </div>
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