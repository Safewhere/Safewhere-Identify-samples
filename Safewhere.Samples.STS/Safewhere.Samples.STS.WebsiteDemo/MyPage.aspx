<%@ page language="C#" autoeventwireup="true" codebehind="MyPage.aspx.cs" inherits="Safewhere.Samples.STS.WebsiteDemo.WebForm1" masterpagefile="~/sp.Master" %>

<%@ import namespace="dk.nita.saml20.identity" %>
<%@ import namespace="dk.nita.saml20.Schema.Core" %>
<asp:content id="Content2" runat="server" contentplaceholderid="ContentPlaceHolder1">
    
    <% if (Saml20Identity.IsInitialized())
       { %>
    <div class="div-claim">
        <div class="div-metadata-mypage-title" >
            <span class="title"><b>My page:</b></span>
        </div>
        <b>WELCOME, <%= Saml20Identity.Current.Name + (Saml20Identity.Current.PersistentPseudonym != null ? " (Pseudonym is " + Saml20Identity.Current.PersistentPseudonym + ")" : String.Empty) %></b>
        <table>
            <thead>
                <tr>
                    <th style="width:50%">
                        <h4><b>Attribute name</b></h4>
                    </th>
                    <th  style="width:50%">
                        <h4><b>Attribute value</b></h4>
                    </th>
                </tr>
            </thead>
            <% foreach (SamlAttribute att in Saml20Identity.Current)
               { %>
            <tr>
                <td>
                    <%= att.Name %>
                </td>
                <td>
                    <%= att.AttributeValue.Length > 0 ? att.AttributeValue[0]:string.Empty %>
                </td>
            </tr>
            <% } %>
        
        </table>
        <div id="ClaimAppServiceResponse"  runat="server"></div>
        

        <div></div>


        <div><asp:Button Id="Ping" CssClass="btn btn-info" runat="server" Enabled="true" Text="Ping to Claim app service" OnClick="Btn_Ping_Click" />
            <asp:Button CssClass="btn btn-info" Id="btnLogoff" runat="server" Enabled="true" Text="Logoff" OnClick="Btn_Logoff_Click" /></div>
        <hr style="border-top:dotted; border-width:1px;" />
        <div>Relogin with IdP: 
        <asp:Button Id="Btn_Relogin" CssClass="btn btn-info" runat="server" Enabled="true" Text="ForceAuthn" OnClick="Btn_Relogin_Click" />
        <asp:Button Id="Btn_Passive" CssClass="btn btn-info" runat="server" Enabled="true" Text="Passive login" OnClick="Btn_Passive_Click" />
        <asp:Button Id="Btn_ReloginNoForceAuthn" CssClass="btn btn-info" runat="server" Enabled="true" Text="No ForceAuthn" OnClick="Btn_ReloginNoForceAuthn_Click" />
        <hr style="border-top:dotted;  border-width:1px;" />
        </div>
    </div>
    <% } %>

    
</asp:content>
