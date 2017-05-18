<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="StorjshareDynamite._Default" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="Telerik.Web.UI.Skins" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">



    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
    </telerik:RadAjaxManager>
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
        <Scripts>
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
        </Scripts>
    </telerik:RadScriptManager>
    <telerik:RadStyleSheetManager ID="RadStyleSheetManager1" runat="server">
    </telerik:RadStyleSheetManager>
    <div class="demo-container size-narrow">
    <div><p>Welcome to StorjshareDynamite.  This will check your log for any connectivity issues.  Note, it is ideal that you stop your node, delete your log, start your node and then wait at least 15 minutes (One hour is better) to check your log.  Old logs may result in incorrect information being displayed regarding issues you've previously corrected.</p></div>
        <div class="qsf-demo-canvas qsf-demo-canvas-overview">
            <p><b>Upload your log here:</b></p>
            <telerik:RadCloudUpload ID="RadCloudUpload1" runat="server" RenderMode="Lightweight" OnClientUploadFailed="onClientUploadFailed" OnFileUploaded="RadCloudUpload1_FileUploaded" ProviderType="Azure" MaxFileSize="52428800">
            </telerik:RadCloudUpload>

        </div>
        <div><br /><p>Once your upload completes.  Push Process to start checking your log.</p><asp:Button ID="btnProcess" runat="server" Text="Process" /></div>
        <div><p><br /><b>Node ID:</b> <asp:Label ID="lblNodeID" runat="server" Text=""></asp:Label></p></div>
        <div><b>Last Seen:</b> <asp:Label ID="lblLastSeen" runat="server" Text=""></asp:Label></div>
        <div><b>Port:</b> <asp:Label ID="lblPort" runat="server" Text=""></asp:Label></div>
        <div><b>Address:</b> <asp:Label ID="lblAddress" runat="server" Text=""></asp:Label></div>
        <div><b>User Agent:</b> <asp:Label ID="lblUserAgent" runat="server" Text=""></asp:Label></div>
        <div><b>Protocol:</b> <asp:Label ID="lblProtocol" runat="server" Text=""></asp:Label></div>
        <div><b>Response Time:</b> <asp:Label ID="lblResponseTime" runat="server" Text=""></asp:Label></div>
        <div><b>Last Timeout:</b> <asp:Label ID="lblLastTimeout" runat="server" Text=""></asp:Label></div>
        <div><b>Timeout Rate:</b> <asp:Label ID="lblTimeoutRate" runat="server" Text=""></asp:Label></div>
        <div><asp:Label ID="lblOnline" runat="server" Text=""></asp:Label></div>
        <div><p><br /><b>Delta:</b> <asp:Label ID="lblDelta" runat="server" Text="..."></asp:Label></p></div>
        <div><p><b>NTP:</b><asp:Label ID="lblNTP" runat="server" Text=""></asp:Label></p></div>
        <div><p><b>Status:</b> <asp:Label ID="lblStatus" runat="server" Text="..."></asp:Label></p></div>
        <div><p><asp:Label ID="lblUPnP" runat="server" Text=""></asp:Label></p></div>
        <div><p><b>Receiving:</b> <asp:Label ID="lblReceiving" runat="server" Text="..."></asp:Label></p></div>
        <div><asp:Label ID="lblOFFERS" runat="server" Text=""></asp:Label></div>

    </div>

    <telerik:RadScriptBlock runat="server">
        <script type="text/javascript">

            function onClientUploadFailed(sender, eventArgs) {
                alert(eventArgs.get_message())
            }

        </script>
    </telerik:RadScriptBlock>



</asp:Content>
