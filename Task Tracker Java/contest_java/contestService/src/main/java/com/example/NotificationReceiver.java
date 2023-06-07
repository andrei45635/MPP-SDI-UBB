package com.example;

public interface NotificationReceiver {
    void start(NotificationSubscriber subscriber);
    void stop();
}
