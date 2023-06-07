package com.example.repo.users;
import com.example.domain.User;
import com.example.repo.IRepository;

public interface UserRepository extends IRepository<Integer, User> {
    boolean findUser(String username, String password);
}
