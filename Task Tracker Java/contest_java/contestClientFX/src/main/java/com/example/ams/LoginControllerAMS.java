package com.example.ams;

import com.example.*;
import com.example.domain.User;
import com.example.notification.Notification;
import javafx.event.ActionEvent;
import javafx.fxml.FXML;
import javafx.scene.Parent;
import javafx.scene.Scene;
import javafx.scene.control.*;
import javafx.stage.Stage;
import org.springframework.beans.factory.annotation.Autowired;

import java.io.IOException;
import java.util.Random;

public class LoginControllerAMS implements NotificationSubscriber {
    @FXML
    private Button loginButton;
    @FXML
    private Label errorLabel;
    @FXML
    private TextField usernameLoginTF;
    @FXML
    private PasswordField passwordLoginTF;
    private IContestServiceAMS server;
    private UserControllerAMS userController;
    Parent mainUserParent;
    private NotificationReceiver receiver;

    public LoginControllerAMS() {}

    @Autowired
    public LoginControllerAMS(IContestServiceAMS amsServer) {
        this.server = amsServer;
    }

    public void setServer(IContestServiceAMS server) {
        this.server = server;
    }

    public void setUserController(UserControllerAMS userController) {
        this.userController = userController;
    }

    public void setParent(Parent p) {
        mainUserParent = p;
    }

    @FXML
    private void onClickLogin(ActionEvent actionEvent) throws IOException {
        User loggedInUser = new User(new Random().nextInt(1000) + 300, usernameLoginTF.getText(), passwordLoginTF.getText());
        try {
            server.login(loggedInUser);
            userController.setReceiver(receiver);
            userController.setServer(server);
            Stage stage = new Stage();
            stage.setScene(new Scene(mainUserParent, 600, 400));
            stage.setTitle("Hello!");
            stage.show();
            userController.setLoggedInUser(loggedInUser);
            //receiver.start(this);
            Stage thisStage = (Stage) loginButton.getScene().getWindow();
            thisStage.close();
        } catch (ContestException cx) {
            Alert alert = new Alert(Alert.AlertType.INFORMATION);
            alert.setTitle("Contest Viewer");
            alert.setHeaderText("Authentication failure");
            alert.setContentText("Wrong username or password");
            alert.showAndWait();
        }
    }

    @Override
    public void notificationReceived(Notification notif) {}

    public void setReceiver(NotificationReceiver receiver) {
        this.receiver = receiver;
    }
}
