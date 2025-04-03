using Company.PL.Settings;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Company.PL.Helpers
{
    public class TwilioService(IOptions<TwilioSettings> _options) : ITwilioService
    {
        public MessageResource SendSms(Sms sms)
        {
            // INITIALIZE Connection
            TwilioClient.Init(_options.Value.AccountSID, _options.Value.AuthToken);

            // build Message
            var message = MessageResource.Create
            (
                body: sms.Body,
                from: new Twilio.Types.PhoneNumber(_options.Value.PhoneNumber),
                to: new Twilio.Types.PhoneNumber(sms.To)
            );

            //return message
            return message;
        }
    }
}
