using DrTech.Amal.SQLModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace DrTech.Amal.Notifications
{
    public class BaseNotifyEvent
    {

        public EmailNotification PrepareEmailNotification<T>(List<EmailNotificationEvent> _Event, T _Obj, string _UserID, string emails)
        {
            EmailNotification mdlEmailNotification = new EmailNotification();
            mdlEmailNotification.EmailTo = emails;
            mdlEmailNotification.EmailSubject = FillPlaceHoldersWithEmailValues<T>(_Event[0].EmailTemplateSubject, _Obj);
            mdlEmailNotification.EmailBody = FillPlaceHoldersWithEmailValues<T>(_Event[0].EmailTemplateBody, _Obj);
            mdlEmailNotification.Status = 0;
            return mdlEmailNotification;

        }


        public SMSNotification PrepareSMSNotification<T>(List<SMSNotificationEvent> _Event, T _Obj, string Mobile)
        {
            SMSNotification mdlSMSNotification = new SMSNotification();
            mdlSMSNotification.MobileNumber = Mobile;
            mdlSMSNotification.SMSText = FillPlaceHoldersWithEmailValues<T>(_Event[0].SMSTemplateBody, _Obj);
            mdlSMSNotification.Status = 0;
            return mdlSMSNotification;

        }


        public ExpoPushNotificationEvent PreparePushNotification<T>(ExpoPushNotificationEvent _Event, T _Obj)
        {
            ExpoPushNotificationEvent mdlSMSNotification = new ExpoPushNotificationEvent();
            mdlSMSNotification.Body = FillPlaceHoldersWithEmailValues<T>(_Event.Body, _Obj);
            return mdlSMSNotification;

        }

        public string FillPlaceHoldersWithEmailValues<T>(string _Template, T _Obj)
        {
            foreach (var item in typeof(T).GetProperties())
            {
                if (_Template != null && _Template.Contains(item.Name))
                    _Template = _Template.Replace("$$" + item.Name + "$$", Convert.ToString(item.GetValue(_Obj)));
            }
            return _Template;
        }

    }
}
