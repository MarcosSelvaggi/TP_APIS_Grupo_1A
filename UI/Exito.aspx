<%@ Page Title="Éxito" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Exito.aspx.cs" Inherits="UI.Exito" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<%= ResolveUrl("~/Content/estilosExito.css") %>" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="contenedor">
        <h2 class="mensaje">¡Gracias por participar!</h2>
        <p>Te contactaremos en caso de salir ganador.</p>

        <div class="balloon" style="top: 150px; left: 20px;">
            <img src="https://img.icons8.com/?size=100&id=DzTqdrd0FxNy&format=png&color=000000" alt="Globo" />
        </div>
        <div class="balloon" style="top: 250px; left: 30px;">
            <img src="https://img.icons8.com/?size=100&id=DzTqdrd0FxNy&format=png&color=000000" alt="Globo" />
        </div>
        <div class="balloon" style="top: 150px; right: 20px;">
            <img src="https://img.icons8.com/?size=100&id=DzTqdrd0FxNy&format=png&color=000000" alt="Globo" />
        </div>
        <div class="balloon" style="top: 250px; right: 30px;">
            <img src="https://img.icons8.com/?size=100&id=DzTqdrd0FxNy&format=png&color=000000" alt="Globo" />
        </div>

        <div class="sparkle" style="top: 200px; left: 35%;">
            <img src="https://img.icons8.com/?size=100&id=x2R5bx7VULto&format=png&color=000000" alt="Chispas" />
        </div>
        <div class="sparkle" style="top: 200px; left: 60%;">
            <img src="https://img.icons8.com/?size=100&id=x2R5bx7VULto&format=png&color=000000" alt="Chispas" />
        </div>
        <div class="sparkle" style="top: 400px; left: 40%;">
            <img src="https://img.icons8.com/?size=100&id=x2R5bx7VULto&format=png&color=000000" alt="Chispas" />
        </div>
        <div class="sparkle" style="top: 400px; left: 60%;">
            <img src="https://img.icons8.com/?size=100&id=x2R5bx7VULto&format=png&color=000000" alt="Chispas" />
        </div>

        <br /><br />
        <asp:Button ID="btnVolverAlInicio" runat="server" Text="Volver al inicio" CssClass="btn btn-success btn-animated" OnClick="btnVolverAlInicio_Click" />
        <!-- <a href="Inicio.aspx" class="btn btn-success">Volver al inicio</a> -->
    </div>
</asp:Content>