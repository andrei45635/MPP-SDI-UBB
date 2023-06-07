package com.example;

import com.example.domain.Participant;
import com.example.domain.Task;
import com.example.domain.User;
import com.example.dto.TaskDTO;
import com.example.enums.AgeGroup;
import com.example.enums.Type;

import java.io.IOException;
import java.util.List;

public interface IContestServiceAMS {
    List<Participant> getFilteredParticipants(Type type, AgeGroup ageGroup) throws ContestException;
    int getParticipantSize() throws ContestException;
    void register(String name, int age, Type type1, Type type2) throws ContestException, IOException;
    List<Task> getAllTasks() throws ContestException, IOException;
    boolean checkUserExists(String username, String password) throws ContestException;
    User findLoggedInUser(String username, String password) throws ContestException, IOException;
    void login(User user) throws ContestException, IOException;
    void logout(User user) throws ContestException;
    List<Task> getAllTasksWithMaxEnrolled() throws ContestException;
    int getEnrolled(AgeGroup ageGroup, Type type) throws ContestException;
    List<TaskDTO> getTasksEnrolled() throws ContestException;
}
