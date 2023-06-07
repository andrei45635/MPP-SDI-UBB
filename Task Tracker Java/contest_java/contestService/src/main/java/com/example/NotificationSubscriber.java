package com.example;

import com.example.notification.Notification;

public interface NotificationSubscriber {
    void notificationReceived(Notification notif);
}
