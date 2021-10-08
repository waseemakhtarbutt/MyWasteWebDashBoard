using DrTech.Amal.Common.Helpers;
using DrTech.Amal.SQLDataAccess;
using DrTech.Amal.SQLModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrTech.Amal.Notifications
{
    public class PushNotificationEvent : BaseNotifyEvent
    {
        ContextDB db;
        public Dictionary<string, object> Parameters { get; set; }
        public PushNotificationEvent()
        {
            this.Parameters = new Dictionary<string, object>();
            db = new ContextDB();
        }     
        public bool AddPushNotifyEvent(long EventID, string Parameter)
        {
            try
            {
                bool isAdded = false;
                ExpoPushNotificationEvent mdlTemplate = db.Repository<ExpoPushNotificationEvent>().GetAll().Where(x => x.EventID == EventID && x.IsActive == true).FirstOrDefault();

                if (mdlTemplate != null)
                {
                    if (mdlTemplate.EventID == (int)NotificationEventConstants.PushNotification.Reduce)
                    {
                        string GreenPoints = Parameters["GP"].ToString();
                        int UserID = Convert.ToInt32(Parameter.ToString());
                        User userinfo = db.Repository<User>().GetAll().Where(x => x.ID == UserID).FirstOrDefault();
                        userinfo.GreenPoints = Convert.ToInt32(GreenPoints);
                        ExpoPushNotificationEvent mdlReducePush = PreparePushNotification(mdlTemplate, userinfo);
                        mdlReducePush.NavigateTo = mdlTemplate.NavigateTo;
                        mdlReducePush.Title = mdlTemplate.Title;
                        ExpoPushHelper.SendPushNotification(userinfo.DeviceToken, mdlReducePush);
                    }
                    else if(mdlTemplate.EventID == (int)NotificationEventConstants.PushNotification.Refuse)
                    {
                        string GreenPoints = Parameters["GP"].ToString();
                        int UserID = Convert.ToInt32(Parameter.ToString());
                        User userinfo = db.Repository<User>().GetAll().Where(x => x.ID == UserID).FirstOrDefault();
                        userinfo.GreenPoints = Convert.ToInt32(GreenPoints);
                        ExpoPushNotificationEvent mdlRefusePush = PreparePushNotification(mdlTemplate, userinfo);
                        mdlRefusePush.NavigateTo = mdlTemplate.NavigateTo;
                        mdlRefusePush.Title = mdlTemplate.Title;
                        ExpoPushHelper.SendPushNotification(userinfo.DeviceToken, mdlRefusePush);
                    }
                    else if(mdlTemplate.EventID == (int)NotificationEventConstants.PushNotification.Report)
                    {
                        string GreenPoints = Parameters["GP"].ToString();
                        int UserID = Convert.ToInt32(Parameter.ToString());
                        User userinfo = db.Repository<User>().GetAll().Where(x => x.ID == UserID).FirstOrDefault();
                        userinfo.GreenPoints = Convert.ToInt32(GreenPoints);
                        ExpoPushNotificationEvent mdlReportPush = PreparePushNotification(mdlTemplate, userinfo);
                        mdlReportPush.NavigateTo = mdlTemplate.NavigateTo;
                        mdlReportPush.Title = mdlTemplate.Title;
                        ExpoPushHelper.SendPushNotification(userinfo.DeviceToken, mdlReportPush);
                    }
                    else if(mdlTemplate.EventID == (int)NotificationEventConstants.PushNotification.Replant)
                    {
                        string GreenPoints = Parameters["GP"].ToString();
                        int UserID = Convert.ToInt32(Parameter.ToString());
                        User userinfo = db.Repository<User>().GetAll().Where(x => x.ID == UserID).FirstOrDefault();
                        userinfo.GreenPoints = Convert.ToInt32(GreenPoints);
                        ExpoPushNotificationEvent mdlReplantPush = PreparePushNotification(mdlTemplate, userinfo);
                        mdlReplantPush.NavigateTo = mdlTemplate.NavigateTo;
                        mdlReplantPush.Title = mdlTemplate.Title;
                        ExpoPushHelper.SendPushNotification(userinfo.DeviceToken, mdlReplantPush);
                    }
                    else if (mdlTemplate.EventID == (int)NotificationEventConstants.PushNotification.Reuse)
                    {
                        string GreenPoints = Parameters["GP"].ToString();
                        int UserID = Convert.ToInt32(Parameter.ToString());
                        User userinfo = db.Repository<User>().GetAll().Where(x => x.ID == UserID).FirstOrDefault();
                        userinfo.GreenPoints = Convert.ToInt32(GreenPoints);
                        ExpoPushNotificationEvent mdlReusePush = PreparePushNotification(mdlTemplate, userinfo);
                        mdlReusePush.NavigateTo = mdlTemplate.NavigateTo;
                        mdlReusePush.Title = mdlTemplate.Title;
                        ExpoPushHelper.SendPushNotification(userinfo.DeviceToken, mdlReusePush);
                    }
                    else if (mdlTemplate.EventID == (int)NotificationEventConstants.PushNotification.ReuseDeclined)
                    {
                        int UserID = Convert.ToInt32(Parameter.ToString());
                        User userinfo = db.Repository<User>().GetAll().Where(x => x.ID == UserID).FirstOrDefault();
                        ExpoPushNotificationEvent mdlReuseDeclinedPush = PreparePushNotification(mdlTemplate, userinfo);
                        mdlReuseDeclinedPush.NavigateTo = mdlTemplate.NavigateTo;
                        mdlReuseDeclinedPush.Title = mdlTemplate.Title;
                        ExpoPushHelper.SendPushNotification(userinfo.DeviceToken, mdlReuseDeclinedPush);
                    }
                    else if (mdlTemplate.EventID == (int)NotificationEventConstants.PushNotification.ReplantDeclined)
                    {
                        int UserID = Convert.ToInt32(Parameter.ToString());
                        User userinfo = db.Repository<User>().GetAll().Where(x => x.ID == UserID).FirstOrDefault();
                        ExpoPushNotificationEvent mdlReplantDeclinedPush = PreparePushNotification(mdlTemplate, userinfo);
                        mdlReplantDeclinedPush.NavigateTo = mdlTemplate.NavigateTo;
                        mdlReplantDeclinedPush.Title = mdlTemplate.Title;
                        ExpoPushHelper.SendPushNotification(userinfo.DeviceToken, mdlReplantDeclinedPush);
                    }
                   else if (mdlTemplate.EventID == (int)NotificationEventConstants.PushNotification.ReportDeclined)
                    {
                        int UserID = Convert.ToInt32(Parameter.ToString());
                        User userinfo = db.Repository<User>().GetAll().Where(x => x.ID == UserID).FirstOrDefault();
                        ExpoPushNotificationEvent mdlReportDeclinedPush = PreparePushNotification(mdlTemplate, userinfo);
                        mdlReportDeclinedPush.NavigateTo = mdlTemplate.NavigateTo;
                        mdlReportDeclinedPush.Title = mdlTemplate.Title;
                        ExpoPushHelper.SendPushNotification(userinfo.DeviceToken, mdlReportDeclinedPush);
                    }
                   else if (mdlTemplate.EventID == (int)NotificationEventConstants.PushNotification.RefuseDeclined)
                    {
                        int UserID = Convert.ToInt32(Parameter.ToString());
                        User userinfo = db.Repository<User>().GetAll().Where(x => x.ID == UserID).FirstOrDefault();
                        ExpoPushNotificationEvent mdlRefuseDeclinedPush = PreparePushNotification(mdlTemplate, userinfo);
                        mdlRefuseDeclinedPush.NavigateTo = mdlTemplate.NavigateTo;
                        mdlRefuseDeclinedPush.Title = mdlTemplate.Title;
                        ExpoPushHelper.SendPushNotification(userinfo.DeviceToken, mdlRefuseDeclinedPush);
                    }
                   else if (mdlTemplate.EventID == (int)NotificationEventConstants.PushNotification.ReduceDeclined)
                    {
                        int UserID = Convert.ToInt32(Parameter.ToString());
                        User userinfo = db.Repository<User>().GetAll().Where(x => x.ID == UserID).FirstOrDefault();
                        ExpoPushNotificationEvent mdlReduceDeclinedPush = PreparePushNotification(mdlTemplate, userinfo);
                        mdlReduceDeclinedPush.NavigateTo = mdlTemplate.NavigateTo;
                        mdlReduceDeclinedPush.Title = mdlTemplate.Title;
                        ExpoPushHelper.SendPushNotification(userinfo.DeviceToken, mdlReduceDeclinedPush);
                    }

                    else if (mdlTemplate.EventID == (int)NotificationEventConstants.PushNotification.General)
                    {
                        int UserID = Convert.ToInt32(Parameter.ToString());
                        List<User> userinfo = db.Repository<User>().GetAll().ToList().Where(x=>x.DeviceToken != null).ToList();
                        ExpoPushHelper.SendBatchNotification(userinfo, mdlTemplate);

                    }
                    else if (mdlTemplate.EventID == (int)NotificationEventConstants.PushNotification.Recycle)
                    {
                        string GreenPoints = Parameters["GP"].ToString();
                        int UserID = Convert.ToInt32(Parameter.ToString());
                        User userinfo = db.Repository<User>().GetAll().Where(x => x.ID == UserID).FirstOrDefault();
                        userinfo.GreenPoints = Convert.ToInt32(GreenPoints);
                        ExpoPushNotificationEvent mdlRecyclePush = PreparePushNotification(mdlTemplate,userinfo);
                        mdlRecyclePush.NavigateTo = mdlTemplate.NavigateTo;
                        mdlRecyclePush.Title = mdlTemplate.Title;
                        ExpoPushHelper.SendPushNotification(userinfo.DeviceToken, mdlRecyclePush);
                   }

                    else if (mdlTemplate.EventID == (int)NotificationEventConstants.PushNotification.Regift)
                    {
                        string GreenPoints = Parameters["GP"].ToString();
                        int UserID = Convert.ToInt32(Parameter.ToString());
                        User userinfo = db.Repository<User>().GetAll().Where(x => x.ID == UserID).FirstOrDefault();
                        userinfo.GreenPoints = Convert.ToInt32(GreenPoints);
                        ExpoPushNotificationEvent mdlRegiftPush = PreparePushNotification(mdlTemplate, userinfo);
                        mdlRegiftPush.NavigateTo = mdlTemplate.NavigateTo;
                        mdlRegiftPush.Title = mdlTemplate.Title;
                        ExpoPushHelper.SendPushNotification(userinfo.DeviceToken, mdlRegiftPush);

                    }
                    else if (mdlTemplate.EventID == (int)NotificationEventConstants.PushNotification.RegiftCollected)
                    {
                        int UserID = Convert.ToInt32(Parameter.ToString());
                        string AmalID = Parameters["AmalID"].ToString();
                        User userinfo = db.Repository<User>().GetAll().Where(x => x.ID == UserID).FirstOrDefault();
                        ExpoPushNotificationEvent mdlReplantDeclinedPush = PreparePushNotification(mdlTemplate, userinfo);
                        mdlReplantDeclinedPush.NavigateTo = mdlTemplate.NavigateTo;
                        mdlReplantDeclinedPush.Title = mdlTemplate.Title;
                        mdlReplantDeclinedPush.AmalID = Convert.ToInt32(AmalID);
                        ExpoPushHelper.SendPushNotification(userinfo.DeviceToken, mdlReplantDeclinedPush);
                    }
                    else if (mdlTemplate.EventID == (int)NotificationEventConstants.PushNotification.RegiftDelivered)
                    {
                        int UserID = Convert.ToInt32(Parameter.ToString());
                        string AmalID = Parameters["AmalID"].ToString();
                        User userinfo = db.Repository<User>().GetAll().Where(x => x.ID == UserID).FirstOrDefault();
                        ExpoPushNotificationEvent mdlReplantDeclinedPush = PreparePushNotification(mdlTemplate, userinfo);
                        mdlReplantDeclinedPush.NavigateTo = mdlTemplate.NavigateTo;
                        mdlReplantDeclinedPush.Title = mdlTemplate.Title;
                        mdlReplantDeclinedPush.AmalID = Convert.ToInt32(AmalID);
                        ExpoPushHelper.SendPushNotification(userinfo.DeviceToken, mdlReplantDeclinedPush);
                    }
                    else if (mdlTemplate.EventID == (int)NotificationEventConstants.PushNotification.RecycleCollected)
                    {
                        int UserID = Convert.ToInt32(Parameter.ToString());
                        string AmalID = Parameters["AmalID"].ToString();
                        User userinfo = db.Repository<User>().GetAll().Where(x => x.ID == UserID).FirstOrDefault();
                        ExpoPushNotificationEvent mdlReplantDeclinedPush = PreparePushNotification(mdlTemplate, userinfo);
                        mdlReplantDeclinedPush.NavigateTo = mdlTemplate.NavigateTo;
                        mdlReplantDeclinedPush.Title = mdlTemplate.Title;
                        mdlReplantDeclinedPush.AmalID = Convert.ToInt32(AmalID);
                        ExpoPushHelper.SendPushNotification(userinfo.DeviceToken, mdlReplantDeclinedPush);
                    }
                    else if (mdlTemplate.EventID == (int)NotificationEventConstants.PushNotification.RecycleDelivered)
                    {
                        int UserID = Convert.ToInt32(Parameter.ToString());
                        string AmalID = Parameters["AmalID"].ToString();
                        User userinfo = db.Repository<User>().GetAll().Where(x => x.ID == UserID).FirstOrDefault();
                        ExpoPushNotificationEvent mdlReplantDeclinedPush = PreparePushNotification(mdlTemplate, userinfo);
                        mdlReplantDeclinedPush.NavigateTo = mdlTemplate.NavigateTo;
                        mdlReplantDeclinedPush.Title = mdlTemplate.Title;
                        mdlReplantDeclinedPush.AmalID = Convert.ToInt32(AmalID);
                        ExpoPushHelper.SendPushNotification(userinfo.DeviceToken, mdlReplantDeclinedPush);
                    }

                    else if (mdlTemplate.EventID == (int)NotificationEventConstants.PushNotification.Bin)
                    {
                        int UserID = Convert.ToInt32(Parameter.ToString());
                        User userinfo = db.Repository<User>().GetAll().Where(x => x.ID == UserID).FirstOrDefault();
                        ExpoPushNotificationEvent mdlReplantDeclinedPush = PreparePushNotification(mdlTemplate, userinfo);
                        mdlReplantDeclinedPush.NavigateTo = mdlTemplate.NavigateTo;
                        mdlReplantDeclinedPush.Title = mdlTemplate.Title;
                        ExpoPushHelper.SendPushNotification(userinfo.DeviceToken, mdlReplantDeclinedPush);
                    }
                    else if (mdlTemplate.EventID == (int)NotificationEventConstants.PushNotification.RegiftCollectedRedirect)
                    {
                        int UserID = Convert.ToInt32(Parameter.ToString());
                        User userinfo = db.Repository<User>().GetAll().Where(x => x.ID == UserID).FirstOrDefault();
                        ExpoPushNotificationEvent mdlRegiftRedirect = PreparePushNotification(mdlTemplate, userinfo);
                        mdlRegiftRedirect.NavigateTo = mdlTemplate.NavigateTo;
                        mdlRegiftRedirect.Title = mdlTemplate.Title;
                        ExpoPushHelper.SendPushNotification(userinfo.DeviceToken, mdlRegiftRedirect);
                    }
                    else if (mdlTemplate.EventID == (int)NotificationEventConstants.PushNotification.RcycleCollectedRedirect)
                    {
                        int UserID = Convert.ToInt32(Parameter.ToString());
                        User userinfo = db.Repository<User>().GetAll().Where(x => x.ID == UserID).FirstOrDefault();
                        ExpoPushNotificationEvent mdlRecycleRedirect = PreparePushNotification(mdlTemplate, userinfo);
                        mdlRecycleRedirect.NavigateTo = mdlTemplate.NavigateTo;
                        mdlRecycleRedirect.Title = mdlTemplate.Title;
                        ExpoPushHelper.SendPushNotification(userinfo.DeviceToken, mdlRecycleRedirect);
                    }
                    else if (mdlTemplate.EventID == (int)NotificationEventConstants.PushNotification.NotifyStudentOnSuspention)
                    {
                        int UserID = Convert.ToInt32(Parameter.ToString());
                        string GPNName = Parameters["GPN"].ToString();
                        User userinfo = db.Repository<User>().GetAll().Where(x => x.ID == UserID).FirstOrDefault();
                        userinfo.Status = GPNName;
                        ExpoPushNotificationEvent mdlStudentSuspensionRedirect = PreparePushNotification(mdlTemplate, userinfo);
                      //  mdlStudentSuspensionRedirect.NavigateTo = mdlTemplate.NavigateTo;
                        mdlStudentSuspensionRedirect.Title = mdlTemplate.Title;
                        ExpoPushHelper.SendPushNotification(userinfo.DeviceToken, mdlStudentSuspensionRedirect);
                    }
                    else if (mdlTemplate.EventID == (int)NotificationEventConstants.PushNotification.NotifyStaffOnSuspention)
                    {
                        int UserID = Convert.ToInt32(Parameter.ToString());
                        string GPNName = Parameters["GPN"].ToString();
                        User userinfo = db.Repository<User>().GetAll().Where(x => x.ID == UserID).FirstOrDefault();
                        userinfo.Status = GPNName;
                        ExpoPushNotificationEvent mdlStaffSuspensionRedirect = PreparePushNotification(mdlTemplate, userinfo);
                       // mdlStaffSuspensionRedirect.NavigateTo = mdlTemplate.NavigateTo;
                        mdlStaffSuspensionRedirect.Title = mdlTemplate.Title;
                        ExpoPushHelper.SendPushNotification(userinfo.DeviceToken, mdlStaffSuspensionRedirect);
                    }
                    else if (mdlTemplate.EventID == (int)NotificationEventConstants.PushNotification.NotifyEmployeeOnSuspention)
                    {
                        int UserID = Convert.ToInt32(Parameter.ToString());
                        string GPNName = Parameters["GPN"].ToString();
                        User userinfo = db.Repository<User>().GetAll().Where(x => x.ID == UserID).FirstOrDefault();
                        userinfo.Status = GPNName;
                        ExpoPushNotificationEvent mdlEmployeeSuspensionRedirect = PreparePushNotification(mdlTemplate, userinfo);
                       // mdlEmployeeSuspensionRedirect.NavigateTo = mdlTemplate.NavigateTo;
                        mdlEmployeeSuspensionRedirect.Title = mdlTemplate.Title;
                        ExpoPushHelper.SendPushNotification(userinfo.DeviceToken, mdlEmployeeSuspensionRedirect);
                    }
                    else if (mdlTemplate.EventID == (int)NotificationEventConstants.PushNotification.NotifyMemberOnSuspention)
                    {
                        int UserID = Convert.ToInt32(Parameter.ToString());
                        string GPNName = Parameters["GPN"].ToString();
                        User userinfo = db.Repository<User>().GetAll().Where(x => x.ID == UserID).FirstOrDefault();
                        userinfo.Status = GPNName;
                        ExpoPushNotificationEvent MemberSuspensionRedirect = PreparePushNotification(mdlTemplate, userinfo);
                       // MemberSuspensionRedirect.NavigateTo = mdlTemplate.NavigateTo;
                        MemberSuspensionRedirect.Title = mdlTemplate.Title;
                        ExpoPushHelper.SendPushNotification(userinfo.DeviceToken, MemberSuspensionRedirect);
                    }
                }
                return isAdded;
            }
            catch (Exception ex)
            {
                Common.ServerResponse.ServiceResponse.LogError(ex);
                return false;
            }
        }
    }
}
