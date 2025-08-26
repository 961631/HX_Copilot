<%@ Page Title="端末-SIM情報紐付け登録" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" 
    CodeBehind="Himoduke.aspx.cs" Inherits="HX.Terminal.Himoduke" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <h2>端末-SIM情報紐付け登録</h2>
        
        <!-- メッセージ表示エリア -->
        <% if (!string.IsNullOrEmpty(ViewBag.SuccessMessage as string)) { %>
            <div class="alert alert-success">
                <%= ViewBag.SuccessMessage %>
            </div>
        <% } %>
        
        <% if (!string.IsNullOrEmpty(ViewBag.ErrorMessage as string)) { %>
            <div class="alert alert-danger">
                <%= ViewBag.ErrorMessage %>
            </div>
        <% } %>

        <div class="row">
            <div class="col-md-8 col-md-offset-2">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h4>端末-SIM情報入力</h4>
                    </div>
                    <div class="panel-body">
                        <form method="post" action="<%= ResolveUrl("~/Terminal/RegisterHimoduke") %>">
                            <div class="form-group">
                                <label for="terminalNo">端末製造番号（IMEI）:</label>
                                <input type="text" id="terminalNo" name="terminalNo" class="form-control" 
                                       maxlength="15" placeholder="15桁の端末製造番号を入力してください" 
                                       autofocus required />
                                <small class="form-text text-muted">※15桁で入力してください</small>
                            </div>
                            
                            <div class="form-group">
                                <label for="icCardNo">SIM情報（ICCID）:</label>
                                <input type="text" id="icCardNo" name="icCardNo" class="form-control" 
                                       maxlength="19" placeholder="19桁のSIM情報を入力してください" 
                                       required />
                                <small class="form-text text-muted">※19桁で入力してください</small>
                            </div>
                            
                            <div class="form-group text-center">
                                <button type="submit" class="btn btn-primary">登録</button>
                                <button type="button" class="btn btn-secondary" onclick="clearForm()">クリア</button>
                            </div>
                        </form>
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

    <script type="text/javascript">
        function clearForm() {
            document.getElementById('terminalNo').value = '';
            document.getElementById('icCardNo').value = '';
            document.getElementById('terminalNo').focus();
        }

        function goToMenu() {
            window.location.href = '<%= ResolveUrl("~/HX/") %>';
        }

        // 数字のみ入力制限
        document.getElementById('terminalNo').addEventListener('input', function(e) {
            this.value = this.value.replace(/[^0-9]/g, '');
        });

        document.getElementById('icCardNo').addEventListener('input', function(e) {
            this.value = this.value.replace(/[^0-9]/g, '');
        });

        // フォーム送信時のバリデーション
        document.querySelector('form').addEventListener('submit', function(e) {
            var terminalNo = document.getElementById('terminalNo').value;
            var icCardNo = document.getElementById('icCardNo').value;
            
            if (terminalNo.length !== 15) {
                alert('端末製造番号（IMEI）は15桁で入力してください。');
                e.preventDefault();
                return false;
            }
            
            if (icCardNo.length !== 19) {
                alert('SIM情報（ICCID）は19桁で入力してください。');
                e.preventDefault();
                return false;
            }
            
            return true;
        });
    </script>
</asp:Content>
