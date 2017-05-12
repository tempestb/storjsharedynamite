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
        <div class="qsf-demo-canvas qsf-demo-canvas-overview">
            <p>Upload your log</p>
            <telerik:RadCloudUpload ID="RadCloudUpload1" runat="server" RenderMode="Lightweight" OnClientUploadFailed="onClientUploadFailed" OnFileUploaded="RadCloudUpload1_FileUploaded" ProviderType="Azure" MaxFileSize="20971520">
            </telerik:RadCloudUpload>

        </div>
        <div><br /><p>Once your upload completes.  Push Process to start checking your log.</p><asp:Button ID="btnProcess" runat="server" Text="Process" /></div>
        <div><br /><p>Node ID: <asp:Label ID="lblNodeID" runat="server" Text="..."></asp:Label></p></div>
        <div><br /><p>Delta: <asp:Label ID="lblDelta" runat="server" Text="..."></asp:Label></p></div>
        <div><br /><p>Status: <asp:Label ID="lblStatus" runat="server" Text="..."></asp:Label></p></div>
        <div><p>Receiving: <asp:Label ID="lblReceiving" runat="server" Text="..."></asp:Label></p></div>
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
