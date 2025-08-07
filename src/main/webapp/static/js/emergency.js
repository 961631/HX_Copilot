// Emergency Information Management System JavaScript

$(document).ready(function() {
    
    // File upload drag and drop functionality
    $('.file-upload-area').on('dragover', function(e) {
        e.preventDefault();
        $(this).addClass('drag-over');
    });
    
    $('.file-upload-area').on('dragleave', function(e) {
        e.preventDefault();
        $(this).removeClass('drag-over');
    });
    
    $('.file-upload-area').on('drop', function(e) {
        e.preventDefault();
        $(this).removeClass('drag-over');
        
        var files = e.originalEvent.dataTransfer.files;
        if (files.length > 0) {
            var fileInput = $(this).find('input[type="file"]');
            fileInput[0].files = files;
            updateFileDisplay(fileInput[0]);
        }
    });
    
    // File input change handler
    $('input[type="file"]').on('change', function() {
        updateFileDisplay(this);
    });
    
    // Delete confirmation dialog
    $('.btn-delete').on('click', function(e) {
        e.preventDefault();
        
        var id = $(this).data('id');
        var imei = $(this).data('imei');
        var iccid = $(this).data('iccid');
        
        if (confirm('以下の紐付けデータを削除してもよろしいですか？\n\nIMEI: ' + imei + '\nICCID: ' + iccid)) {
            // Create form and submit
            var form = $('<form method="post" action="/result/delete"></form>');
            form.append('<input type="hidden" name="id" value="' + id + '">');
            form.append('<input type="hidden" name="page" value="' + getCurrentPage() + '">');
            $('body').append(form);
            form.submit();
        }
    });
    
    // Form submission loading state
    $('form').on('submit', function() {
        var submitBtn = $(this).find('button[type="submit"]');
        submitBtn.prop('disabled', true);
        submitBtn.addClass('loading');
        submitBtn.find('.loading-spinner').show();
    });
    
    // Auto-hide alerts after 5 seconds
    $('.alert').each(function() {
        var alert = $(this);
        if (!alert.hasClass('alert-danger')) {
            setTimeout(function() {
                alert.fadeOut();
            }, 5000);
        }
    });
    
    // Input validation
    $('#imei').on('input', function() {
        validateImei($(this));
    });
    
    $('#iccid').on('input', function() {
        validateIccid($(this));
    });
    
    // Page navigation keyboard shortcuts
    $(document).on('keydown', function(e) {
        if (e.ctrlKey) {
            switch(e.key) {
                case 'ArrowLeft':
                    e.preventDefault();
                    $('.pagination .page-item:not(.disabled) .page-link[aria-label="Previous"]').click();
                    break;
                case 'ArrowRight':
                    e.preventDefault();
                    $('.pagination .page-item:not(.disabled) .page-link[aria-label="Next"]').click();
                    break;
            }
        }
    });
});

// Update file display
function updateFileDisplay(fileInput) {
    var file = fileInput.files[0];
    var displayArea = $(fileInput).closest('.file-upload-area');
    
    if (file) {
        displayArea.find('.file-name').text(file.name);
        displayArea.find('.file-size').text('(' + formatFileSize(file.size) + ')');
        displayArea.addClass('file-selected');
    } else {
        displayArea.find('.file-name').text('ファイルを選択してください');
        displayArea.find('.file-size').text('');
        displayArea.removeClass('file-selected');
    }
}

// Format file size
function formatFileSize(bytes) {
    if (bytes === 0) return '0 Bytes';
    
    var k = 1024;
    var sizes = ['Bytes', 'KB', 'MB', 'GB'];
    var i = Math.floor(Math.log(bytes) / Math.log(k));
    
    return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
}

// Get current page number
function getCurrentPage() {
    var urlParams = new URLSearchParams(window.location.search);
    return urlParams.get('page') || 0;
}

// Validate IMEI
function validateImei(input) {
    var value = input.val();
    var isValid = /^\d{15}$/.test(value);
    
    if (value && !isValid) {
        input.addClass('is-invalid');
        input.siblings('.invalid-feedback').text('15桁の数字で入力してください');
    } else {
        input.removeClass('is-invalid');
    }
    
    return isValid;
}

// Validate ICCID
function validateIccid(input) {
    var value = input.val();
    var isValid = /^\d{19}$/.test(value);
    
    if (value && !isValid) {
        input.addClass('is-invalid');
        input.siblings('.invalid-feedback').text('19桁の数字で入力してください');
    } else {
        input.removeClass('is-invalid');
    }
    
    return isValid;
}

// Validate form before submission
function validateHimodukeForm() {
    var imeiValid = validateImei($('#imei'));
    var iccidValid = validateIccid($('#iccid'));
    
    return imeiValid && iccidValid;
}