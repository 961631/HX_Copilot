<%@ Page Title="KDDI受領データ登録" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" 
    CodeBehind="Regist.aspx.cs" Inherits="HX.Terminal.Regist" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <h2>KDDI受領データ登録</h2>
        
        <!-- メッセージ表示エリア -->
        <div id="messageArea" style="display: none;" class="alert">
            <span id="messageText"></span>
        </div>

        <div class="row">
            <div class="col-md-6">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h4>端末データ登録</h4>
                    </div>
                    <div class="panel-body">
                        <div class="form-group">
                            <button type="button" class="btn btn-primary" onclick="openFileDialog('terminal')">
                                端末データ登録
                            </button>
                        </div>
                        <div class="form-group">
                            <input type="file" id="terminalFileInput" style="display: none;" accept=".csv" 
                                   onchange="uploadFile('terminal')" />
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-md-6">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h4>SIMデータ登録</h4>
                    </div>
                    <div class="panel-body">
                        <div class="form-group">
                            <button type="button" class="btn btn-primary" onclick="openFileDialog('sim')">
                                SIMデータ登録
                            </button>
                        </div>
                        <div class="form-group">
                            <input type="file" id="simFileInput" style="display: none;" accept=".csv" 
                                   onchange="uploadFile('sim')" />
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-md-12 text-center">
                <button type="button" class="btn btn-default" onclick="goToMenu()">
                    メニューへ戻る
                </button>
            </div>
        </div>
    </div>

    <!-- ファイルアップロード用ポップアップ -->
    <div id="uploadModal" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title" id="modalTitle">ファイルアップロード</h4>
                </div>
                <div class="modal-body">
                    <form id="uploadForm" enctype="multipart/form-data">
                        <div class="form-group">
                            <label>ファイル選択：</label>
                            <input type="file" id="modalFileInput" class="form-control" accept=".csv" />
                        </div>
                    </form>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" onclick="processUpload()">アップロード</button>
                    <button type="button" class="btn btn-secondary" onclick="clearFile()">クリア</button>
                    <button type="button" class="btn btn-default" data-dismiss="modal">キャンセル</button>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        var currentUploadType = '';

        function openFileDialog(type) {
            currentUploadType = type;
            document.getElementById('modalTitle').innerText = 
                type === 'terminal' ? '端末データ登録' : 'SIMデータ登録';
            document.getElementById('modalFileInput').value = '';
            $('#uploadModal').modal('show');
        }

        function processUpload() {
            var fileInput = document.getElementById('modalFileInput');
            if (!fileInput.files.length) {
                showMessage('ファイルが選択されていません。', 'error');
                return;
            }

            var formData = new FormData();
            formData.append(currentUploadType + 'File', fileInput.files[0]);

            var url = currentUploadType === 'terminal' 
                ? '<%= ResolveUrl("~/Terminal/UploadTerminalCsv") %>'
                : '<%= ResolveUrl("~/Terminal/UploadSimCsv") %>';

            $.ajax({
                url: url,
                type: 'POST',
                data: formData,
                processData: false,
                contentType: false,
                success: function(response) {
                    if (response.success) {
                        showMessage(response.message, 'success');
                    } else {
                        showMessage(response.message, 'error');
                    }
                    $('#uploadModal').modal('hide');
                },
                error: function() {
                    showMessage('アップロード処理中にエラーが発生しました。', 'error');
                    $('#uploadModal').modal('hide');
                }
            });
        }

        function clearFile() {
            document.getElementById('modalFileInput').value = '';
        }

        function uploadFile(type) {
            var fileInput = document.getElementById(type + 'FileInput');
            if (!fileInput.files.length) return;

            var formData = new FormData();
            formData.append(type + 'File', fileInput.files[0]);

            var url = type === 'terminal' 
                ? '<%= ResolveUrl("~/Terminal/UploadTerminalCsv") %>'
                : '<%= ResolveUrl("~/Terminal/UploadSimCsv") %>';

            $.ajax({
                url: url,
                type: 'POST',
                data: formData,
                processData: false,
                contentType: false,
                success: function(response) {
                    if (response.success) {
                        showMessage(response.message, 'success');
                    } else {
                        showMessage(response.message, 'error');
                    }
                    fileInput.value = ''; // ファイル選択をクリア
                },
                error: function() {
                    showMessage('アップロード処理中にエラーが発生しました。', 'error');
                }
            });
        }

        function showMessage(message, type) {
            var messageArea = document.getElementById('messageArea');
            var messageText = document.getElementById('messageText');
            
            messageText.innerText = message;
            messageArea.className = 'alert ' + (type === 'success' ? 'alert-success' : 'alert-danger');
            messageArea.style.display = 'block';
            
            // 5秒後にメッセージを非表示にする
            setTimeout(function() {
                messageArea.style.display = 'none';
            }, 5000);
        }

        function goToMenu() {
            window.location.href = '<%= ResolveUrl("~/HX/") %>';
        }
    </script>
</asp:Content>
