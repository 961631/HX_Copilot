<%@ page language="java" contentType="text/html; charset=UTF-8" pageEncoding="UTF-8"%>
<%@ taglib prefix="c" uri="http://java.sun.com/jsp/jstl/core" %>
<%@ taglib prefix="fmt" uri="http://java.sun.com/jsp/jstl/fmt" %>
<!DOCTYPE html>
<html lang="ja">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>SIM情報紐付け結果一覧 - 救急情報管理システム</title>
    
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
                    <h1><i class="fas fa-mobile-alt me-2"></i>救急情報管理システム - SIM情報紐付け結果一覧</h1>
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
                <a class="nav-link" href="/himoduke"><i class="fas fa-link me-1"></i>端末-SIM情報紐付け登録</a>
            </li>
            <li class="nav-item">
                <a class="nav-link active" href="/result"><i class="fas fa-list me-1"></i>SIM情報紐付け結果一覧</a>
            </li>
        </ul>

        <!-- Messages -->
        <c:if test="${not empty deleteSuccess}">
            <div class="alert alert-success alert-dismissible fade show" role="alert">
                <i class="fas fa-check-circle me-2"></i>${deleteSuccess}
                <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
            </div>
        </c:if>
        
        <c:if test="${not empty deleteError}">
            <div class="alert alert-danger alert-dismissible fade show" role="alert">
                <i class="fas fa-exclamation-circle me-2"></i>${deleteError}
                <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
            </div>
        </c:if>

        <!-- Summary Info -->
        <div class="row mb-4">
            <div class="col-12">
                <div class="card">
                    <div class="card-body">
                        <div class="row text-center">
                            <div class="col-md-4">
                                <i class="fas fa-calendar-day fa-2x text-primary mb-2"></i>
                                <h5>当日登録件数</h5>
                                <h3 class="text-primary">${totalElements}</h3>
                                <small class="text-muted">件</small>
                            </div>
                            <div class="col-md-4">
                                <i class="fas fa-file-alt fa-2x text-info mb-2"></i>
                                <h5>現在ページ</h5>
                                <h3 class="text-info">${currentPage + 1}</h3>
                                <small class="text-muted">/ ${totalPages} ページ</small>
                            </div>
                            <div class="col-md-4">
                                <i class="fas fa-list-ol fa-2x text-success mb-2"></i>
                                <h5>表示件数</h5>
                                <h3 class="text-success">${pageSize}</h3>
                                <small class="text-muted">件/ページ</small>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Data Table -->
        <div class="row">
            <div class="col-12">
                <div class="card">
                    <div class="card-header d-flex justify-content-between align-items-center">
                        <h4 class="mb-0"><i class="fas fa-table me-2"></i>紐付けデータ一覧</h4>
                        <div>
                            <a href="/himoduke" class="btn btn-primary btn-sm">
                                <i class="fas fa-plus me-1"></i>新規登録
                            </a>
                        </div>
                    </div>
                    <div class="card-body p-0">
                        <c:choose>
                            <c:when test="${himodukePage.totalElements > 0}">
                                <div class="table-responsive">
                                    <table class="table table-hover mb-0">
                                        <thead class="table-light">
                                            <tr>
                                                <th scope="col" class="text-center">#</th>
                                                <th scope="col"><i class="fas fa-mobile me-1"></i>端末製造番号(IMEI)</th>
                                                <th scope="col"><i class="fas fa-sim-card me-1"></i>ICカード番号(ICCID)</th>
                                                <th scope="col"><i class="fas fa-clock me-1"></i>登録日時</th>
                                                <th scope="col" class="text-center"><i class="fas fa-cogs me-1"></i>操作</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <c:forEach var="himoduke" items="${himodukePage.content}" varStatus="status">
                                                <tr>
                                                    <td class="text-center">
                                                        <span class="badge bg-secondary">${currentPage * pageSize + status.index + 1}</span>
                                                    </td>
                                                    <td>
                                                        <code class="text-primary">${himoduke.imei}</code>
                                                    </td>
                                                    <td>
                                                        <code class="text-success">${himoduke.iccid}</code>
                                                    </td>
                                                    <td>
                                                        <i class="fas fa-calendar-alt text-muted me-1"></i>
                                                        ${himoduke.createdAt}
                                                    </td>
                                                    <td class="text-center">
                                                        <button type="button" 
                                                                class="btn btn-danger btn-sm btn-delete"
                                                                data-id="${himoduke.id}"
                                                                data-imei="${himoduke.imei}"
                                                                data-iccid="${himoduke.iccid}">
                                                            <i class="fas fa-trash me-1"></i>削除
                                                        </button>
                                                    </td>
                                                </tr>
                                            </c:forEach>
                                        </tbody>
                                    </table>
                                </div>
                            </c:when>
                            <c:otherwise>
                                <div class="text-center py-5">
                                    <i class="fas fa-inbox fa-4x text-muted mb-3"></i>
                                    <h5 class="text-muted">データがありません</h5>
                                    <p class="text-muted">当日登録された紐付けデータはありません。</p>
                                    <a href="/himoduke" class="btn btn-primary">
                                        <i class="fas fa-plus me-1"></i>新規登録する
                                    </a>
                                </div>
                            </c:otherwise>
                        </c:choose>
                    </div>
                </div>
            </div>
        </div>

        <!-- Pagination -->
        <c:if test="${totalPages > 1}">
            <div class="row mt-4">
                <div class="col-12">
                    <nav aria-label="ページナビゲーション">
                        <ul class="pagination justify-content-center">
                            <!-- Previous page -->
                            <li class="page-item ${!hasPrevious ? 'disabled' : ''}">
                                <a class="page-link" href="?page=${previousPage}" aria-label="Previous">
                                    <span aria-hidden="true">&laquo;</span>
                                </a>
                            </li>
                            
                            <!-- Page numbers -->
                            <c:forEach begin="0" end="${totalPages - 1}" var="page">
                                <c:choose>
                                    <c:when test="${page == currentPage}">
                                        <li class="page-item active">
                                            <span class="page-link">${page + 1}</span>
                                        </li>
                                    </c:when>
                                    <c:otherwise>
                                        <li class="page-item">
                                            <a class="page-link" href="?page=${page}">${page + 1}</a>
                                        </li>
                                    </c:otherwise>
                                </c:choose>
                            </c:forEach>
                            
                            <!-- Next page -->
                            <li class="page-item ${!hasNext ? 'disabled' : ''}">
                                <a class="page-link" href="?page=${nextPage}" aria-label="Next">
                                    <span aria-hidden="true">&raquo;</span>
                                </a>
                            </li>
                        </ul>
                    </nav>
                </div>
            </div>
        </c:if>

        <!-- Instructions -->
        <div class="row mt-4">
            <div class="col-12">
                <div class="card">
                    <div class="card-header">
                        <h5 class="mb-0"><i class="fas fa-question-circle me-2"></i>操作方法</h5>
                    </div>
                    <div class="card-body">
                        <div class="row">
                            <div class="col-md-6">
                                <h6><i class="fas fa-eye text-info me-1"></i>表示機能</h6>
                                <ul class="list-unstyled">
                                    <li><i class="fas fa-check text-success me-1"></i>当日登録分のみ表示</li>
                                    <li><i class="fas fa-check text-success me-1"></i>20件/ページで表示</li>
                                    <li><i class="fas fa-check text-success me-1"></i>登録日時の降順で並び替え</li>
                                </ul>
                            </div>
                            <div class="col-md-6">
                                <h6><i class="fas fa-trash text-danger me-1"></i>削除機能</h6>
                                <ul class="list-unstyled">
                                    <li><i class="fas fa-check text-success me-1"></i>確認ダイアログ表示</li>
                                    <li><i class="fas fa-check text-success me-1"></i>物理削除実行</li>
                                    <li><i class="fas fa-check text-success me-1"></i>削除後に結果メッセージ表示</li>
                                </ul>
                            </div>
                        </div>
                        
                        <div class="mt-3 p-3 bg-light rounded">
                            <h6><i class="fas fa-keyboard text-primary me-1"></i>キーボードショートカット</h6>
                            <small>
                                <kbd>Ctrl</kbd> + <kbd>←</kbd>: 前のページ　　
                                <kbd>Ctrl</kbd> + <kbd>→</kbd>: 次のページ
                            </small>
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