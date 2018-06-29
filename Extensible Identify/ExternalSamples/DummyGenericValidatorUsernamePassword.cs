using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using Safewhere.External.Authentication;

namespace Safewhere.External.Samples
{
    public class DummyGenericValidatorUsernamePassword : IGenericCredentialsValidator
    {
        const string ConnectionString = "ConnectionString";
        const string UserName = "username";
        const string Password = "password";

        public CredentialsValidationResult Validate(ControllerContext cc, IDictionary<string, string> input, IIdentifyLogWriter logWriter)
        {
            var validationResult = ValidateInput(input);

            LookupProtocolConnection(cc, logWriter);

            if (validationResult.ResultCode != CredentialsValidationResultCode.Success)
            {
                logWriter.WriteWarning("Validate the input failed. Error code: " + validationResult.ResultCode +
                                       ". ExternalErrorMessages: " + string.Join("\n-", validationResult.ExternalErrorMessages));
                validationResult.ShowErrorViewWhenResultCodeIsNotSuccess = true;
                return validationResult;
            }

            var connectionString = input[ConnectionString];
            try
            {
                var userRepository = new DummyUserRepository(connectionString);
                var username = input[UserName].Trim();
                var user = userRepository.GetUserByName(username);
                if (user == null)
                {
                    logWriter.WriteInformation("User not found. Search username: " + username);
                    validationResult.ResultCode = CredentialsValidationResultCode.UnknownUserName;
                }
                else
                {
                    logWriter.WriteInformation(string.Format(CultureInfo.InvariantCulture,
                                                             "User information: [UserName: {0}], [Password: {1}], [IsDisabled: {2}], [IsLocked: {3}]",
                                                             user.Username,
                                                             user.Password, user.IsDisabled, user.IsLocked));
                    var password = input[Password].Trim();
                    if (user.Password.Trim() != password)
                    {
                        validationResult.ExternalErrorMessages.Add("Password is invalid. Expected: " + user.Password);
                        validationResult.ResultCode = CredentialsValidationResultCode.IncorrectPassword;
                    }
                    else if (user.IsDisabled)
                    {
                        validationResult.ResultCode = CredentialsValidationResultCode.UserDisabled;
                    }
                    else if (user.IsLocked)
                    {
                        validationResult.ResultCode = CredentialsValidationResultCode.UserLocked;
                    }
                    else
                    {
                        validationResult.UserIdentity = user.Username;
                        validationResult.ResultCode = CredentialsValidationResultCode.Success;    
                    }
                }
            }
            catch (Exception ex)
            {
                logWriter.WriteError("An unexpected exception happends. Error message: " + ex.Message + ". StackTrace: " + ex.StackTrace);

                validationResult.ResultCode = CredentialsValidationResultCode.UnknownError;
                validationResult.ExternalErrorMessages.Add(ex.Message);
            }

            logWriter.WriteInformation("Validation with result code: " + validationResult.ResultCode + ". ExternalErrorMessages: " +
                                       string.Join("\n-", validationResult.ExternalErrorMessages));
            
            validationResult.ShowErrorViewWhenResultCodeIsNotSuccess = true;
            return validationResult;
        }

        /// <summary>
        /// It is an example how to access protocol connection id and entityId in external modules
        /// </summary>
        /// <param name="cc"></param>
        /// <param name="logWriter"></param>
        private void LookupProtocolConnection(ControllerContext cc, IIdentifyLogWriter logWriter)
        {
            const string temporaryContextKey = "ici_TemporaryProtocolContext";
            var httpContext = cc.HttpContext;
            dynamic temporaryContext = httpContext.Items[temporaryContextKey];
            dynamic contextIdKey = temporaryContext.ContextIdKey;
            string contextId = contextIdKey.ContextId;
            Guid protocolConnectionId = contextIdKey.ProtocolConnectionId;
            string protocolConnectionEntityId = contextIdKey.ProtocolConnectionEntityId;
            logWriter.WriteInformation(
                $"DummyGenericValidatorUsernamePassword. ContextId = '{contextId}'. Protocol connection id = '{protocolConnectionId}', entityId = '{protocolConnectionEntityId}'");
        }

        private static CredentialsValidationResult ValidateInput(IDictionary<string, string> input)
        {
            CredentialsValidationResult validationResult = new CredentialsValidationResult();
            if (!input.ContainsKey(ConnectionString))
            {
                validationResult.ExternalErrorMessages.Add("Missing connection string field");
                validationResult.ResultCode = CredentialsValidationResultCode.MissingRequiredFields;
                return validationResult;
            }

            if (!input.ContainsKey(UserName))
            {
                validationResult.ExternalErrorMessages.Add("Missing username field");
                validationResult.ResultCode = CredentialsValidationResultCode.MissingRequiredFields;
                return validationResult;
            }

            if (!input.ContainsKey(Password))
            {
                validationResult.ExternalErrorMessages.Add("Missing password field.");
                validationResult.ResultCode = CredentialsValidationResultCode.MissingRequiredFields;
                return validationResult;
            }

            validationResult.ResultCode = CredentialsValidationResultCode.Success;
            return validationResult;
        }
    }
}
