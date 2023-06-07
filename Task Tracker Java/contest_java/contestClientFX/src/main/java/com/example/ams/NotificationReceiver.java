package com.example.ams;

import com.example.NotificationSubscriber;
import com.example.notification.Notification;
import org.springframework.jms.core.JmsOperations;

import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;
import java.util.concurrent.TimeUnit;

public class NotificationReceiver implements com.example.NotificationReceiver {
    private JmsOperations jmsOperations;
    private boolean running;

    public NotificationReceiver(JmsOperations operations) {
        jmsOperations = operations;
    }

    ExecutorService service;
    NotificationSubscriber subscriber;

    @Override
    public void start(NotificationSubscriber subscriber) {
        System.out.println("Starting notificaation receiver...");
        running = true;
        this.subscriber = subscriber;
        service = Executors.newSingleThreadExecutor();
        service.submit(this::run);
    }

    private void run() {
        while(running){
            Notification notification = (Notification) jmsOperations.receiveAndConvert();
            System.out.println("Received notification... " + notification);
            subscriber.notificationReceived(notification);
            System.out.println("after the subscriber receives the notification");
        }
    }


    @Override
    public void stop() {
        running = false;
        try{
            service.awaitTermination(100, TimeUnit.MILLISECONDS);
            service.shutdown();
        } catch (InterruptedException e) {
            e.printStackTrace();
        }
        System.out.println("Stopped notification receiver");
    }
}
