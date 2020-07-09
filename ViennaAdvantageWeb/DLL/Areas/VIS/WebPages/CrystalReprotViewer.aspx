<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CrystalReprotViewer.aspx.cs" Inherits="VIS.WebPages.CrystalReprotViewer" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../../ViennaBase/Scripts/crystalreportviewers13/js/crviewer/crv.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
         <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="True" GroupTreeImagesFolderUrl="" BestFitPage="False" ToolbarImagesFolderUrl="" Width="100%" />

    </div>
      
    </form>
</body>
</html>
