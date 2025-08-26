/**
 * 端末管理システム用JavaScript
 */

$(document).ready(function() {
    // 共通初期化処理
    initializeTerminalSystem();
});

/**
 * システム初期化
 */
function initializeTerminalSystem() {
    // ファイルドラッグ&ドロップ対応
    initializeDragAndDrop();
    
    // 数字のみ入力制限
    initializeNumericInput();
    
    // フォームバリデーション
    initializeFormValidation();
    
    // メッセージ自動非表示
    initializeAutoHideMessages();
}

/**
 * ドラッグ&ドロップ初期化
 */
function initializeDragAndDrop() {
    $('.upload-area').on({
        dragover: function(e) {
            e.preventDefault();
            $(this).addClass('dragover');
        },
        dragleave: function(e) {
            e.preventDefault();
            $(this).removeClass('dragover');
        },
        drop: function(e) {
            e.preventDefault();
            $(this).removeClass('dragover');
            
            var files = e.originalEvent.dataTransfer.files;
            if (files.length > 0) {
                handleFileSelect(files[0], $(this).data('type'));
            }
        }
    });
}

/**
 * 数字のみ入力制限
 */
function initializeNumericInput() {
    $('input[data-numeric="true"]').on('input', function() {
        this.value = this.value.replace(/[^0-9]/g, '');
    });
    
    // 端末製造番号とSIM情報の自動フォーマット
    $('#terminalNo').on('input', function() {
        var value = this.value.replace(/[^0-9]/g, '');
        if (value.length > 15) {
            value = value.substring(0, 15);
        }
        this.value = value;
        
        // リアルタイムバリデーション
        if (value.length === 15) {
            $(this).removeClass('error').addClass('valid');
        } else {
            $(this).removeClass('valid').addClass('error');
        }
    });
    
    $('#icCardNo').on('input', function() {
        var value = this.value.replace(/[^0-9]/g, '');
        if (value.length > 19) {
            value = value.substring(0, 19);
        }
        this.value = value;
        
        // リアルタイムバリデーション
        if (value.length === 19) {
            $(this).removeClass('error').addClass('valid');
        } else {
            $(this).removeClass('valid').addClass('error');
        }
    });
}

/**
 * フォームバリデーション初期化
 */
function initializeFormValidation() {
    // 紐付け登録フォーム
    $('#himodukeForm').on('submit', function(e) {
        if (!validateHimodukeForm()) {
            e.preventDefault();
            return false;
        }
    });
}

/**
 * 紐付けフォームバリデーション
 */
function validateHimodukeForm() {
    var terminalNo = $('#terminalNo').val();
    var icCardNo = $('#icCardNo').val();
    var isValid = true;
    
    // 端末製造番号チェック
    if (!terminalNo || terminalNo.length !== 15) {
        showFieldError('terminalNo', '端末製造番号（IMEI）は15桁で入力してください。');
        isValid = false;
    } else {
        clearFieldError('terminalNo');
    }
    
    // SIM情報チェック
    if (!icCardNo || icCardNo.length !== 19) {
        showFieldError('icCardNo', 'SIM情報（ICCID）は19桁で入力してください。');
        isValid = false;
    } else {
        clearFieldError('icCardNo');
    }
    
    return isValid;
}

/**
 * フィールドエラー表示
 */
function showFieldError(fieldId, message) {
    var field = $('#' + fieldId);
    field.addClass('error');
    
    var errorElement = field.next('.field-error');
    if (errorElement.length === 0) {
        field.after('<div class="field-error text-danger">' + message + '</div>');
    } else {
        errorElement.text(message);
    }
}

/**
 * フィールドエラークリア
 */
function clearFieldError(fieldId) {
    var field = $('#' + fieldId);
    field.removeClass('error');
    field.next('.field-error').remove();
}

/**
 * メッセージ自動非表示初期化
 */
function initializeAutoHideMessages() {
    $('.alert').each(function() {
        var alert = $(this);
        if (alert.hasClass('alert-success')) {
            setTimeout(function() {
                alert.fadeOut();
            }, 5000);
        }
    });
}

/**
 * ファイル選択処理
 */
function handleFileSelect(file, type) {
    if (!validateFile(file)) {
        return;
    }
    
    var formData = new FormData();
    formData.append(type + 'File', file);
    
    uploadFile(formData, type);
}

/**
 * ファイルバリデーション
 */
function validateFile(file) {
    // ファイル拡張子チェック
    if (!file.name.toLowerCase().endsWith('.csv')) {
        showMessage('ファイル形式が正しくありません。CSVファイルをアップロードしてください。', 'error');
        return false;
    }
    
    // ファイルサイズチェック（10MB）
    if (file.size > 10 * 1024 * 1024) {
        showMessage('ファイルサイズが大きすぎます。10MB以下のファイルをアップロードしてください。', 'error');
        return false;
    }
    
    return true;
}

/**
 * ファイルアップロード
 */
function uploadFile(formData, type) {
    var url = type === 'terminal' 
        ? '/Terminal/UploadTerminalCsv'
        : '/Terminal/UploadSimCsv';
    
    // ローディング表示
    showLoading();
    
    $.ajax({
        url: url,
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        success: function(response) {
            hideLoading();
            if (response.success) {
                showMessage(response.message, 'success');
                // アップロード成功時の追加処理があれば記述
            } else {
                showMessage(response.message, 'error');
            }
        },
        error: function(xhr, status, error) {
            hideLoading();
            showMessage('アップロード処理中にエラーが発生しました。', 'error');
            console.error('Upload error:', error);
        }
    });
}

/**
 * メッセージ表示
 */
function showMessage(message, type) {
    var alertClass = type === 'success' ? 'alert-success' : 
                    type === 'warning' ? 'alert-warning' : 'alert-danger';
    
    var messageHtml = '<div class="alert ' + alertClass + ' alert-dismissible fade in" role="alert">' +
                      '<button type="button" class="close" data-dismiss="alert" aria-label="Close">' +
                      '<span aria-hidden="true">&times;</span>' +
                      '</button>' +
                      message + '</div>';
    
    // 既存のメッセージをクリア
    $('.alert').remove();
    
    // 新しいメッセージを表示
    $('.container').prepend(messageHtml);
    
    // 成功メッセージの場合は5秒後に自動非表示
    if (type === 'success') {
        setTimeout(function() {
            $('.alert-success').fadeOut();
        }, 5000);
    }
}

/**
 * ローディング表示
 */
function showLoading() {
    if ($('#loading').length === 0) {
        var loadingHtml = '<div id="loading" class="modal fade" data-backdrop="static" data-keyboard="false">' +
                         '<div class="modal-dialog modal-sm">' +
                         '<div class="modal-content">' +
                         '<div class="modal-body text-center">' +
                         '<div class="spinner"></div>' +
                         '<p>処理中...</p>' +
                         '</div></div></div></div>';
        $('body').append(loadingHtml);
    }
    $('#loading').modal('show');
}

/**
 * ローディング非表示
 */
function hideLoading() {
    $('#loading').modal('hide');
}

/**
 * 確認ダイアログ表示
 */
function showConfirmDialog(title, message, callback) {
    var dialogHtml = '<div id="confirmDialog" class="modal fade" tabindex="-1" role="dialog">' +
                     '<div class="modal-dialog" role="document">' +
                     '<div class="modal-content">' +
                     '<div class="modal-header">' +
                     '<button type="button" class="close" data-dismiss="modal">&times;</button>' +
                     '<h4 class="modal-title">' + title + '</h4>' +
                     '</div>' +
                     '<div class="modal-body">' +
                     '<p>' + message + '</p>' +
                     '</div>' +
                     '<div class="modal-footer">' +
                     '<button type="button" class="btn btn-danger" id="confirmOk">OK</button>' +
                     '<button type="button" class="btn btn-default" data-dismiss="modal">キャンセル</button>' +
                     '</div></div></div></div>';
    
    // 既存のダイアログを削除
    $('#confirmDialog').remove();
    
    // 新しいダイアログを追加
    $('body').append(dialogHtml);
    
    // OKボタンのイベント設定
    $('#confirmOk').on('click', function() {
        $('#confirmDialog').modal('hide');
        if (callback) callback();
    });
    
    // ダイアログを表示
    $('#confirmDialog').modal('show');
    
    // ダイアログが閉じられたら削除
    $('#confirmDialog').on('hidden.bs.modal', function() {
        $(this).remove();
    });
}

/**
 * CSVダウンロード
 */
function downloadCsv(data, filename) {
    var csv = arrayToCsv(data);
    var blob = new Blob([csv], { type: 'text/csv;charset=utf-8;' });
    var link = document.createElement('a');
    
    if (link.download !== undefined) {
        var url = URL.createObjectURL(blob);
        link.setAttribute('href', url);
        link.setAttribute('download', filename);
        link.style.visibility = 'hidden';
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
    }
}

/**
 * 配列をCSV形式に変換
 */
function arrayToCsv(data) {
    if (!data || data.length === 0) return '';
    
    var csv = '';
    data.forEach(function(row) {
        if (Array.isArray(row)) {
            csv += row.map(function(field) {
                return '"' + String(field).replace(/"/g, '""') + '"';
            }).join(',') + '\n';
        }
    });
    
    return csv;
}

/**
 * 日付フォーマット
 */
function formatDate(date, format) {
    if (!date) return '';
    
    var d = new Date(date);
    var year = d.getFullYear();
    var month = ('0' + (d.getMonth() + 1)).slice(-2);
    var day = ('0' + d.getDate()).slice(-2);
    var hour = ('0' + d.getHours()).slice(-2);
    var minute = ('0' + d.getMinutes()).slice(-2);
    var second = ('0' + d.getSeconds()).slice(-2);
    
    return format.replace('YYYY', year)
                 .replace('MM', month)
                 .replace('DD', day)
                 .replace('HH', hour)
                 .replace('mm', minute)
                 .replace('ss', second);
}

/**
 * ページ遷移
 */
function navigateTo(url) {
    window.location.href = url;
}

/**
 * トップページに戻る
 */
function goToTop() {
    navigateTo('/HX/Terminal/Top');
}

/**
 * メニューに戻る
 */
function goToMenu() {
    navigateTo('/HX/');
}
