package com.example.ams;

import com.example.*;
import com.example.domain.User;
import com.example.dto.ParticipantDTO;
import com.example.dto.TaskDTO;
import com.example.enums.AgeGroup;
import com.example.enums.Type;
import com.example.notification.Notification;
import javafx.application.Platform;
import javafx.collections.FXCollections;
import javafx.collections.ObservableList;
import javafx.event.ActionEvent;
import javafx.fxml.FXML;
import javafx.fxml.FXMLLoader;
import javafx.scene.Parent;
import javafx.scene.Scene;
import javafx.scene.control.*;
import javafx.scene.control.cell.PropertyValueFactory;
import javafx.stage.Stage;

import java.io.IOException;
import java.net.URL;
import java.util.ArrayList;
import java.util.List;
import java.util.ResourceBundle;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

public class UserControllerAMS implements NotificationSubscriber {
    private User loggedInUser;
    private IContestServiceAMS server;
    @FXML
    private Label errorAge;
    @FXML
    private TextField registrationNameTF;
    @FXML
    private TextField ageRegistrationTF;
    @FXML
    private ComboBox<Type> task1ComboBox;
    @FXML
    private ComboBox<Type> task2ComboBox;
    @FXML
    private ComboBox<Type> taskComboBox;
    @FXML
    private ComboBox<AgeGroup> ageGroupComboBox;
    @FXML
    private TableView<ParticipantDTO> participantsTableView;
    @FXML
    private TableColumn<ParticipantDTO, String> nameParticipantColumn;
    @FXML
    private TableColumn<ParticipantDTO, Integer> ageParticipantColumn;
    @FXML
    private Button logoutButton;
    @FXML
    private TableView<TaskDTO> tableViewTasks;
    @FXML
    private TableColumn<TaskDTO, Integer> taskIDColumn;
    @FXML
    private TableColumn<TaskDTO, Type> typeColumn;
    @FXML
    private TableColumn<TaskDTO, AgeGroup> ageGroupColumn;
    @FXML
    private TableColumn<TaskDTO, Integer> enrolledColumn;

    private final ObservableList<TaskDTO> tasksModel = FXCollections.observableArrayList();
    private final ObservableList<ParticipantDTO> participantsModel = FXCollections.observableArrayList();
    private NotificationReceiver receiver;

    public UserControllerAMS() {}

    public UserControllerAMS(IContestServiceAMS server) throws ContestException {
        this.server = server;
    }

    public void setServer(IContestServiceAMS server) throws ContestException {
        this.server = server;
        receiver.start(this);
        ageGroupComboBox.setItems(FXCollections.observableArrayList(AgeGroup.YOUNG, AgeGroup.PRETEEN, AgeGroup.TEEN));
        taskComboBox.setItems(FXCollections.observableArrayList(Type.PAINTING, Type.POETRY, Type.TREASURE));
        task1ComboBox.setItems(FXCollections.observableArrayList(Type.PAINTING, Type.POETRY, Type.TREASURE));
        task2ComboBox.setItems(FXCollections.observableArrayList(Type.PAINTING, Type.POETRY, Type.TREASURE));
        //initModel();
        ageGroupComboBox.setItems(FXCollections.observableArrayList(AgeGroup.YOUNG, AgeGroup.PRETEEN, AgeGroup.TEEN));
        taskComboBox.setItems(FXCollections.observableArrayList(Type.PAINTING, Type.POETRY, Type.TREASURE));
        task1ComboBox.setItems(FXCollections.observableArrayList(Type.PAINTING, Type.POETRY, Type.TREASURE));
        task2ComboBox.setItems(FXCollections.observableArrayList(Type.PAINTING, Type.POETRY, Type.TREASURE));
        initModel();
        taskIDColumn.setCellValueFactory(new PropertyValueFactory<>("TaskID"));
        typeColumn.setCellValueFactory(new PropertyValueFactory<>("Type"));
        ageGroupColumn.setCellValueFactory(new PropertyValueFactory<>("AgeGroup"));
        enrolledColumn.setCellValueFactory(new PropertyValueFactory<>("Enrolled"));

        tableViewTasks.setItems(tasksModel);

        nameParticipantColumn.setCellValueFactory(new PropertyValueFactory<>("name"));
        ageParticipantColumn.setCellValueFactory(new PropertyValueFactory<>("age"));

        participantsTableView.setItems(participantsModel);
    }

    public void setLoggedInUser(User loggedInUser) {
        this.loggedInUser = loggedInUser;
    }

    @FXML
    private void onClickLogout(ActionEvent actionEvent) throws IOException {
        this.loggedInUser = null;
        FXMLLoader loader = new FXMLLoader(getClass().getResource("resources/views/hello-view.fxml"));
        Parent root = loader.load();
        LoginControllerAMS loginController = loader.getController();
        loginController.setServer(server);
        Stage stage = new Stage();
        stage.setScene(new Scene(root, 425, 350));
        stage.setTitle("Hello!");
        stage.show();
        Stage thisStage = (Stage) logoutButton.getScene().getWindow();
        receiver.stop();
        thisStage.close();
    }

    @FXML
    public void initialize(URL location, ResourceBundle resources) throws ContestException {
        ageGroupComboBox.setItems(FXCollections.observableArrayList(AgeGroup.YOUNG, AgeGroup.PRETEEN, AgeGroup.TEEN));
        taskComboBox.setItems(FXCollections.observableArrayList(Type.PAINTING, Type.POETRY, Type.TREASURE));
        task1ComboBox.setItems(FXCollections.observableArrayList(Type.PAINTING, Type.POETRY, Type.TREASURE));
        task2ComboBox.setItems(FXCollections.observableArrayList(Type.PAINTING, Type.POETRY, Type.TREASURE));
        initModel();
        taskIDColumn.setCellValueFactory(new PropertyValueFactory<>("TaskID"));
        typeColumn.setCellValueFactory(new PropertyValueFactory<>("Type"));
        ageGroupColumn.setCellValueFactory(new PropertyValueFactory<>("AgeGroup"));
        enrolledColumn.setCellValueFactory(new PropertyValueFactory<>("Enrolled"));

        tableViewTasks.setItems(tasksModel);

        nameParticipantColumn.setCellValueFactory(new PropertyValueFactory<>("name"));
        ageParticipantColumn.setCellValueFactory(new PropertyValueFactory<>("age"));

        participantsTableView.setItems(participantsModel);
    }

    @FXML
    private void onClickFilter(ActionEvent actionEvent) throws ContestException {
        Type selectedType = taskComboBox.getValue();
        AgeGroup selectedAgeGroup = ageGroupComboBox.getValue();
        System.out.println(server.getFilteredParticipants(selectedType, selectedAgeGroup).size());
        List<ParticipantDTO> participantDTOS = new ArrayList<>();
        for (var p : server.getFilteredParticipants(selectedType, selectedAgeGroup)) {
            participantDTOS.add(new ParticipantDTO(p.getName(), p.getAge()));
        }
        participantsModel.setAll(participantDTOS);
    }

    @FXML
    private void onClickRegister(ActionEvent actionEvent) throws IOException, ContestException {
        String inputName = registrationNameTF.getText();
        int age = Integer.parseInt(ageRegistrationTF.getText());
        if (age < 6 || age > 15) {
            errorAge.setText("Invalid age!");
            return;
        }
        Type type1 = task1ComboBox.getValue();
        Type type2 = task2ComboBox.getValue();
        server.register(inputName, age, type1, type2);
    }

    //@Override
    public void updateTaskList() {
        Platform.runLater(() -> {
            List<TaskDTO> tasks = new ArrayList<>();
            try {
                tasks = server.getTasksEnrolled();
                //tasksModel.setAll(server.getTasksEnrolled());
            } catch (ContestException e) {
                System.out.println("gresit in controller");
            }
            tasksModel.setAll(tasks);
            //tableViewTasks.setItems(tasksModel);
        });
    }

    private void initModel() throws ContestException {
        tasksModel.setAll(server.getTasksEnrolled());
    }

    public void setReceiver(NotificationReceiver receiver) {
        this.receiver = receiver;
    }

    @Override
    public void notificationReceived(Notification notif) {
        try {
            System.out.println("Notification received in controller " + notif.getType() + "...");
            Platform.runLater(() -> {
                System.out.println("IN PLATFORM RUN LATER !!!!!!!!!!!!");
                List<TaskDTO> tasks = new ArrayList<>();
                try {
                    tasks = server.getTasksEnrolled();
                    System.out.println("tasks in notification " + tasks);
                    //tasksModel.setAll(server.getTasksEnrolled());
                } catch (ContestException e) {
                    System.out.println("gresit in controller");
                }
                tasksModel.setAll(tasks);
                tableViewTasks.refresh();
                //tableViewTasks.setItems(tasksModel);
            });
        } catch (Exception e) {
            e.printStackTrace();
        }
    }
}
