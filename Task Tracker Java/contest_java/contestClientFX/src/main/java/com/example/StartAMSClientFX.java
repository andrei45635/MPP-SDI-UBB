package com.example;

import com.example.ams.LoginControllerAMS;
import com.example.ams.NotificationReceiver;
import com.example.ams.UserControllerAMS;
import javafx.application.Application;
import javafx.fxml.FXMLLoader;
import javafx.scene.Parent;
import javafx.scene.Scene;
import javafx.stage.Stage;
import org.springframework.context.support.ClassPathXmlApplicationContext;

import java.io.IOException;

public class StartAMSClientFX extends Application {
    private static ClassPathXmlApplicationContext context = null;
    @Override
    public void start(Stage primaryStage) throws IOException {
        if(context == null){
            context = new ClassPathXmlApplicationContext("spring-client.xml");
        }

        FXMLLoader loader = new FXMLLoader(getClass().getClassLoader().getResource("views/hello-view.fxml"));
        Parent root = loader.load();
        LoginControllerAMS ctrl = loader.getController();
        System.out.println(ctrl);
        context.refresh();
        ctrl.setServer(context.getBean("contestServer", IContestServiceAMS.class));
        ctrl.setReceiver(context.getBean("notificationReceiver", NotificationReceiver.class));
        FXMLLoader cloader = new FXMLLoader(getClass().getClassLoader().getResource("views/user-controller.fxml"));
        Parent croot = cloader.load();
        //UserControllerAMS userControllerAMS = context.getBean("userCtrl", UserControllerAMS.class);
        UserControllerAMS userControllerAMS = cloader.getController();

        ctrl.setUserController(userControllerAMS);
        ctrl.setParent(croot);

        primaryStage.setTitle("Contest Viewer");
        primaryStage.setScene(new Scene(root, 500, 300));
        primaryStage.show();
    }
}
