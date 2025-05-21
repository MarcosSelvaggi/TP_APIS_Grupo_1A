<%@ Page Title="Usuario" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Usuario.aspx.cs" Inherits="UI.Usuario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/scriptValidadores.js") %>"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row justify-content-center">
        <div class="col-3">
            <div class="mb-3">
                <label id="lblDocumento" runat="server" class="form-label">Documento</label>
                <asp:TextBox runat="server" ID="txtDocumento" CssClass="form-control" MaxLength="50" oninput="validarLongitud(this, 50, 'dniErrorMsj');" onkeypress="return soloNumeros(event);"></asp:TextBox>
                <%--<asp:TextBox runat="server" ID="txtDocumento" CssClass="form-control" MaxLength="50" oninput="validarLongitud(this, 50, 'dniErrorMsj');"></asp:TextBox>--%>
                <small id="dniErrorMsj" class="text-danger"></small>
            </div>
            <div class="mb-3">
                <asp:Button ID="btnBuscarDocumento" Text="Buscar Documento" OnClick="btnBuscarDocumento_Click" CssClass="btn btn-outline-primary w-100 me-1" runat="server" />
                <asp:Button ID="btnLimpiarDocumento" Text="Limpiar Documento" OnClick="btnLimpiarDocumento_Click" CssClass="btn btn-outline-primary w-100 me-1" runat="server" Visible="false" />
            </div>

            <div class="modal fade" id="buscarDocumentoModal" tabindex="-1" aria-labelledby="buscarDocumentoModalLabel" aria-hidden="true">
                <div class="modal-dialog modal-dialog-centered">
                    <div class="modal-content">
                        <div class="modal-header bg-success text-white">
                            <h5 class="modal-title" id="buscarDocumentoModalLabel" runat="server">Documento encontrado</h5>
                        </div>
                        <div class="modal-body">
                            <p id="pBusquedaDocumento" runat="server"></p>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-primary" data-bs-dismiss="modal">Aceptar</button>
                        </div>
                    </div>
                </div>
            </div>

            <div class="mb-3">
                <label for="txtNombre" class="form-label col-3">Nombre</label>
                <asp:TextBox runat="server" ID="txtNombre" CssClass="form-control col-3" ReadOnly="true" MaxLength="50" oninput="validarLongitud(this, 50, 'nombreErrorMsj');"></asp:TextBox>
                <small id="nombreErrorMsj" class="text-danger"></small>
            </div>
            <div class="mb-3">
                <label for="txtApellido" class="form-label">Apellido</label>
                <asp:TextBox runat="server" ID="txtApellido" CssClass="form-control" ReadOnly="true" MaxLength="50" oninput="validarLongitud(this, 50, 'apellidoErrorMsj');"></asp:TextBox>
                <small id="apellidoErrorMsj" class="text-danger"></small>
            </div>
            <div class="mb-3">
                <label for="txtEmail" class="form-label">Dirección de mail</label>
                <asp:TextBox runat="server" ID="txtEmail" TextMode="Email" CssClass="form-control" ReadOnly="true" MaxLength="50" oninput="validarLongitud(this, 50, 'mailErrorMsj');"></asp:TextBox>
                <small id="mailErrorMsj" class="text-danger"></small>
            </div>
            <div class="mb-3">
                <label for="txtDireccion" class="form-label">Dirección</label>
                <asp:TextBox runat="server" ID="txtDireccion" CssClass="form-control" ReadOnly="true" MaxLength="50" oninput="validarLongitud(this, 50, 'direccionErrorMsj');"></asp:TextBox>
                <small id="direccionErrorMsj" class="text-danger"></small>
            </div>
            <div class="mb-3">
                <label for="txtCiudad" class="form-label">Ciudad</label>
                <asp:TextBox runat="server" ID="txtCiudad" CssClass="form-control" ReadOnly="true" MaxLength="50" oninput="validarLongitud(this, 50, 'ciudadErrorMsj');"></asp:TextBox>
                <small id="ciudadErrorMsj" class="text-danger"></small>
            </div>
            <div class="mb-3">
                <label for="txtCP" class="form-label">Código postal</label>
                <asp:TextBox runat="server" ID="txtCP" CssClass="form-control" MaxLength="9" ReadOnly="false" oninput="validarCP(this);" onkeypress="return soloNumeros(event);" />
                <small id="cpErrorMsj" class="text-danger"></small>
            </div>
            <div class="mb-3">
                <asp:Button ID="btnAceptar" Text="Confirmar Datos" OnClick="btnAceptar_Click" CssClass="btn btn-primary" runat="server" />
                <asp:Button ID="btnModificarDatos" Text="Modificar Datos" OnClick="btnModificarDatos_Click" CssClass="btn btn-info" runat="server" Visible="false" />
            </div>
            <div class="mb-3">
                <label id="lblAceptar" runat="server" class="form-label"></label>
            </div>
        </div>
    </div>

    <div class="modal fade" id="datosErroneosModal" tabindex="-1" aria-labelledby="datosErroneosModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header bg-success text-white">
                    <h5 class="modal-title" id="datosErroneosModalLabel">❌ Error con los datos</h5>
                </div>
                <div class="modal-body">
                    <p>Por favor complete todos los campos con la información necesaria.</p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" data-bs-dismiss="modal">Aceptar</button>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="registroExitosoModal" tabindex="-1" aria-labelledby="registroExitosoModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header bg-success text-white">
                    <h5 class="modal-title" id="registroExitosoModalLabel">🎉 Registro Exitoso</h5>
                </div>
                <div class="modal-body">
                    <!--¡Sus datos fueron registrados correctamente! Serás redirigido en unos segundos.-->
                    <p>¡Sus datos fueron registrados correctamente!</p>
                    <p>Le hemos enviado un mail de notificacion.</p>
                    <p>Serás redirigido en unos segundos.</p>
                </div>
                <div class="modal-footer">
                    <a href="Exito.aspx" class="btn btn-success">Ir ahora</a>
                    <!--<a href="Inicio.aspx" class="btn btn-success">Ir ahora</a> -->
                </div>
            </div>
        </div>
    </div>

</asp:Content>
