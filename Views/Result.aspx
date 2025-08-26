<%@ Page Title="SIM情報紐付け結果一覧" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" 
    CodeBehind="Result.aspx.cs" Inherits="HX.Terminal.Result" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <h2>SIM情報紐付け結果一覧</h2>
        
        <!-- メッセージ表示エリア -->
        <div id="messageArea" style="display: none;" class="alert">
            <span id="messageText"></span>
        </div>
        
        <% if (!string.IsNullOrEmpty(ViewBag.ErrorMessage as string)) { %>
            <div class="alert alert-danger">
                <%= ViewBag.ErrorMessage %>
            </div>
        <% } %>

        <div class="row">
            <div class="col-md-12">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h4>本日の紐付け登録一覧（<%= DateTime.Now.ToString("yyyy年MM月dd日") %>）</h4>
                    </div>
                    <div class="panel-body">
                        <% if (ViewBag.HasData == null || !(bool)ViewBag.HasData) { %>
                            <div class="text-center">
                                <p class="text-muted">
                                    <%= ViewBag.NoDataMessage ?? "本日登録されたデータはありません。" %>
                                </p>
                            </div>
                        <% } else { %>
                            <div class="table-responsive">
                                <table class="table table-striped table-bordered">
                                    <thead>
                                        <tr>
                                            <th>端末製造番号（IMEI）</th>
                                            <th>SIM情報（ICCID）</th>
                                            <th>登録日時</th>
                                            <th>操作</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <% foreach (var item in Model) { %>
                                            <tr>
                                                <td><%= item.TerminalNo %></td>
                                                <td><%= item.IcCardNo %></td>
                                                <td><%= item.RegistDate.ToString("yyyy/MM/dd HH:mm:ss") %></td>
                                                <td class="text-center">
                                                    <button type="button" class="btn btn-danger btn-sm" 
                                                            onclick="confirmDelete('<%= item.TerminalNo %>', '<%= item.IcCardNo %>')">
                                                        削除
                                                    </button>
                                                </td>
                                            </tr>
                                        <% } %>
                                    </tbody>
                                </table>
                            </div>

                            <!-- ページング（20件ごと） -->
                            <% 
                            int currentPage = ViewBag.CurrentPage ?? 1;
                            // 実際の実装では総件数を取得してページング処理を行う
                            %>
                            <div class="text-center">
                                <nav aria-label="Page navigation">
                                    <ul class="pagination">
                                        <% if (currentPage > 1) { %>
                                            <li class="page-item">
                                                <a class="page-link" href="?page=<%= currentPage - 1 %>">&laquo; 前へ</a>
                                            </li>
                                        <% } %>
                                        
                                        <li class="page-item active">
                                            <span class="page-link"><%= currentPage %></span>
                                        </li>
                                        
                                        <% if (Model.Count == 20) { // 20件表示されている場合は次ページがある可能性 %>
                                            <li class="page-item">
                                                <a class="page-link" href="?page=<%= currentPage + 1 %>">次へ &raquo;</a>
                                            </li>
                                        <% } %>
                                    </ul>
                                </nav>
                            </div>
                        <% } %>
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

    <!-- 削除確認モーダル -->
    <div id="deleteModal" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">削除確認</h4>
                </div>
                <div class="modal-body">
                    <p>該当の紐付け登録を削除してよろしいですか？</p>
                    <div class="well">
                        <strong>端末製造番号：</strong><span id="deleteTerminalNo"></span><br>
                        <strong>SIM情報：</strong><span id="deleteIcCardNo"></span>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" onclick="executeDelete()">OK</button>
                    <button type="button" class="btn btn-default" data-dismiss="modal">キャンセル</button>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        var deleteTerminalNo = '';
        var deleteIcCardNo = '';

        function confirmDelete(terminalNo, icCardNo) {
            deleteTerminalNo = terminalNo;
            deleteIcCardNo = icCardNo;
            
            document.getElementById('deleteTerminalNo').innerText = terminalNo;
            document.getElementById('deleteIcCardNo').innerText = icCardNo;
            
            $('#deleteModal').modal('show');
        }

        function executeDelete() {
            $.ajax({
                url: '<%= ResolveUrl("~/Terminal/DeleteHimoduke") %>',
                type: 'POST',
                data: {
                    terminalNo: deleteTerminalNo,
                    icCardNo: deleteIcCardNo
                },
                success: function(response) {
                    if (response.success) {
                        showMessage(response.message, 'success');
                        // ページをリロードして一覧を更新
                        setTimeout(function() {
                            location.reload();
                        }, 1500);
                    } else {
                        showMessage(response.message, 'error');
                    }
                    $('#deleteModal').modal('hide');
                },
                error: function() {
                    showMessage('削除処理中にエラーが発生しました。', 'error');
                    $('#deleteModal').modal('hide');
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
