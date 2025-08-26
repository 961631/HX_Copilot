<%@ Page Title="新規端末情報登録" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" 
    CodeBehind="Top.aspx.cs" Inherits="HX.Terminal.Top" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <h2>新規端末情報登録</h2>
        <p class="lead">救急情報管理システム　みまもりホン後継機対応</p>
        
        <div class="row">
            <div class="col-md-4">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h4 class="panel-title">KDDI受領データ登録</h4>
                    </div>
                    <div class="panel-body">
                        <p>端末データとSIMデータのCSVファイルをアップロードして登録します。</p>
                        <div class="text-center">
                            <a href="<%= ResolveUrl("~/Terminal/Regist") %>" class="btn btn-primary btn-lg">
                                登録
                            </a>
                        </div>
                    </div>
                </div>
            </div>
            
            <div class="col-md-4">
                <div class="panel panel-info">
                    <div class="panel-heading">
                        <h4 class="panel-title">端末-SIM情報紐付け登録</h4>
                    </div>
                    <div class="panel-body">
                        <p>端末製造番号とSIM情報を紐付けて登録します。</p>
                        <div class="text-center">
                            <a href="<%= ResolveUrl("~/Terminal/Himoduke") %>" class="btn btn-info btn-lg">
                                登録
                            </a>
                        </div>
                    </div>
                </div>
            </div>
            
            <div class="col-md-4">
                <div class="panel panel-success">
                    <div class="panel-heading">
                        <h4 class="panel-title">SIM情報紐付け結果一覧</h4>
                    </div>
                    <div class="panel-body">
                        <p>登録済みの紐付け情報を一覧表示し、削除操作を行います。</p>
                        <div class="text-center">
                            <a href="<%= ResolveUrl("~/Terminal/Result") %>" class="btn btn-success btn-lg">
                                登録
                            </a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        
        <div class="row">
            <div class="col-md-12">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h4>機能説明</h4>
                    </div>
                    <div class="panel-body">
                        <h5>1. KDDI受領データ登録</h5>
                        <ul>
                            <li>端末データCSVファイルのアップロード</li>
                            <li>SIMデータCSVファイルのアップロード</li>
                            <li>ファイル形式チェック、重複チェックを実施</li>
                        </ul>
                        
                        <h5>2. 端末-SIM情報紐付け登録</h5>
                        <ul>
                            <li>端末製造番号（IMEI）15桁の入力</li>
                            <li>SIM情報（ICCID）19桁の入力</li>
                            <li>桁数チェック、重複チェックを実施</li>
                        </ul>
                        
                        <h5>3. SIM情報紐付け結果一覧</h5>
                        <ul>
                            <li>当日登録された紐付け情報の一覧表示</li>
                            <li>20件ごとのページング表示</li>
                            <li>削除機能（確認ダイアログ付き）</li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
        
        <div class="row">
            <div class="col-md-12 text-center">
                <h5>環境別URL</h5>
                <div class="well">
                    <strong>検証環境：</strong><br>
                    <a href="http://gcwwqv00.intra.secom.co.jp/HX/" target="_blank">
                        http://gcwwqv00.intra.secom.co.jp/HX/
                    </a><br><br>
                    <strong>本番環境：</strong><br>
                    <a href="http://gcwwpv00.intra.secom.co.jp/HX/" target="_blank">
                        http://gcwwpv00.intra.secom.co.jp/HX/
                    </a>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
