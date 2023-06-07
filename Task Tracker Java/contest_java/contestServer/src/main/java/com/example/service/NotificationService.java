package com.example.service;

import com.example.IContestNotificationService;
import com.example.notification.Notification;
import com.example.notification.NotificationType;
import org.springframework.jms.core.JmsOperations;

public class NotificationService implements IContestNotificationService {
    private JmsOperations jmsOperations;

    public NotificationService(JmsOperations operations) {
        jmsOperations=operations;
    }

    @Override
    public void getUpdatedTasks() {
        System.out.println("Updated tasks notification");
        Notification notification = new Notification(NotificationType.GET_UPDATE_TASKS);
        jmsOperations.convertAndSend(notification);
        System.out.println("Sent message to ActiveMQ " + notification + "...");
    }
}
