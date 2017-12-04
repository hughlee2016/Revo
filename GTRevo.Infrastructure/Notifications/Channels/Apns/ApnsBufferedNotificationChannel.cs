﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PushSharp.Apple;

namespace GTRevo.Infrastructure.Notifications.Channels.Apns
{
    public class ApnsBufferedNotificationChannel : IBufferedNotificationChannel
    {
        private readonly IApnsNotificationFormatter[] pushNotificationFormatters;
        private readonly IApnsBrokerDispatcher apnsBrokerDispatcher;

        public ApnsBufferedNotificationChannel(
            IApnsNotificationFormatter[] pushNotificationFormatters,
            IApnsBrokerDispatcher apnsBrokerDispatcher)
        {
            this.pushNotificationFormatters = pushNotificationFormatters;
            this.apnsBrokerDispatcher = apnsBrokerDispatcher;
        }

        public async Task SendNotificationsAsync(IEnumerable<INotification> notifications)
        {
            IEnumerable<WrappedApnsNotification> apnsNotifications = null;
            foreach (IApnsNotificationFormatter formatter in pushNotificationFormatters)
            {
                IEnumerable<WrappedApnsNotification> pushNotifications = await formatter.FormatPushNotification(notifications);
                apnsNotifications = apnsNotifications?.Concat(pushNotifications) ?? pushNotifications;
            }

            if (apnsNotifications != null)
            {
                apnsBrokerDispatcher.QueueNotifications(apnsNotifications);
            }
        }
    }
}
