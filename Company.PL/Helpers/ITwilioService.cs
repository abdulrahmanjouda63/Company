using Twilio.Rest.Api.V2010.Account;

namespace Company.PL.Helpers
{
    public interface ITwilioService
    {
        public MessageResource SendSms(Sms sms);
    }
}
