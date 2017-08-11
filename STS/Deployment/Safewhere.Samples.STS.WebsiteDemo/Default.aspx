<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" MasterPageFile="~/sp.Master" Inherits="Safewhere.Samples.STS.WebsiteDemo.Default" Title="Demo Service Provider" %>

<%@ Import Namespace="dk.nita.saml20.config" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="col-md-12">
            <div class="div-row-btn-mypage">
                <a href="MyPage.aspx" class="btn btn-info">Go to My Page</a>
            </div>
        </div>
    </div>
    <% if (string.IsNullOrEmpty(SAML20FederationConfig.GetConfig().Endpoints.MetadataLocation))
       { %>
    <div class="row">
        <div class="row-sm-height">
            <div class="col-md-12">
                <div class="div-metadata-error">
                    <h3><b>Metadata</b></h3>
                    The identity provider and the service provider must exchange metadata in order to establish SAML connections. 

                            You must add the <b>&lt;IDPEndPoints&gt;</b> tag to the <b>"<%= ConfigurationConstants.SectionNames.SAML20Federation %>"</b> section of the
                            application's configuration file in order to continue.
                </div>
            </div>
        </div>
    </div>
    <% }
       else if (certificateMissing)
       { %>
    <div class="row">
        <div class="row-sm-height">
            <div class="col-md-12">
                <div class="div-metadata-error">
                    <h3><b>Metadata</b></h3>
                    The identity provider and the service provider must exchange metadata in order to establish SAML connections. 

                            <div style="color: Red;">
                                The specified certificate could not be found. Please correct 
                            the certificate information in the "<%= ConfigurationConstants.SectionNames.Federation %>" section of the configuration file.
                            </div>
                </div>
            </div>
        </div>
    </div>
    <% }
       else
       { %>
    <div class="row">
        <div class="row-sm-height">
            <div class="col-md-9">
                <div class="div-metadata-path ">
                    <h3><b>Metadata</b></h3>
                    The identity provider and the service provider must exchange metadata in order to establish SAML connections. 
                            The Identity provider's metadata should be put in the directory 
                            <p><b>"<%= SAML20FederationConfig.GetConfig().Endpoints.MetadataLocation %>".</b></p>
                </div>
            </div>

            <div class="col-md-3">
                <div class="div-metadata-download">
                    <p>The metadata of the service provider can be downloaded </p>
                    <a href="metadata.ashx" class="btn btn-info" style="display: block; margin: 0 auto; width: 80px">here</a>
                </div>
            </div>
        </div>
    </div>

    <% } %>
</asp:Content>
