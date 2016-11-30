using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Safewhere.External;
using Safewhere.External.Authentication;

namespace Safewhere.Samples.STS.GenericCredentialsValidator
{
    public class UsernamePasswordGenericCredentialsValidator : IGenericCredentialsValidator
    {
        const string ServiceIdentifier = "__serviceIdentifier";
        const string UserName = "username";
        const string Password = "password";
        const string AdditionalClaims = "additionalclaim";
        /// <summary>
        /// Validate credentials by generic provider
        /// </summary>
        /// <param name="cc">Notice that on STs the controller context is irrelevant</param>
        /// <param name="inputs"></param>
        /// <param name="logWriter"></param>
        /// <returns></returns>
        public CredentialsValidationResult Validate(ControllerContext cc, IDictionary<string, string> inputs, IIdentifyLogWriter logWriter)
        {
            var validationResult = ValidateInput(inputs);

            if (validationResult.ResultCode != CredentialsValidationResultCode.Success)
            {
                logWriter.WriteError("Validate the input failed. Error code: " + validationResult.ResultCode +
                                       ". ExternalErrorMessages: " + string.Join("\n-", validationResult.ExternalErrorMessages));
                validationResult.ShowErrorViewWhenResultCodeIsNotSuccess = false;
                return validationResult;
            }
            var username = inputs[UserName];
            var password = inputs[Password];
            var serviceIdentifier = inputs[ServiceIdentifier];
            if (VerifyIfUserNameIsCorrect(username))
            {
                return new CredentialsValidationResult {ResultCode = CredentialsValidationResultCode.UnknownUserName};
            }

            if (VerifyPasswordIsCorrect(password))
            {
                return new CredentialsValidationResult { ResultCode = CredentialsValidationResultCode.IncorrectPassword };
            }

            var logResult = new StringBuilder();
            logResult.AppendLine(
                $"Successfully validate generic credentials for username = '{username}' with service identifier ='{serviceIdentifier}'");
            logResult.AppendLine("");

            foreach (var input in inputs)
            {
                if (input.Key.StartsWith(AdditionalClaims, StringComparison.InvariantCultureIgnoreCase))
                {
                    logResult.AppendLine($"Additional claims received: type = '{input.Key}' - value ='{input.Value}'");
                }
            }
            logWriter.WriteInformation(logResult); 

            return new CredentialsValidationResult {ResultCode = CredentialsValidationResultCode.Success};
        }

        private bool VerifyPasswordIsCorrect(string password)
        {
            if (password.Equals("incorrectpasswords"))
            {
                return false;
            }
            return true;
        }

        private bool VerifyIfUserNameIsCorrect(string username)
        {
            if (username.Equals("unknownusername"))
            {
                return false;
            }
            return true;
        }

        private static CredentialsValidationResult ValidateInput(IDictionary<string, string> inputs)
        {
            CredentialsValidationResult validationResult = new CredentialsValidationResult();
            if (!inputs.ContainsKey(ServiceIdentifier))
            {
                validationResult.ExternalErrorMessages.Add("Missing service identifier field");
                validationResult.ResultCode = CredentialsValidationResultCode.MissingRequiredFields;
                return validationResult;
            }

            if (!inputs.ContainsKey(UserName))
            {
                validationResult.ExternalErrorMessages.Add("Missing username field");
                validationResult.ResultCode = CredentialsValidationResultCode.MissingRequiredFields;
                return validationResult;
            }

            if (!inputs.ContainsKey(Password))
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
