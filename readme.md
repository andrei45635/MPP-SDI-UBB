### **_Problem statement:_**

The organizers of a children's contest use a software system to register participants. At different offices
from the city the system is used. The person in each office uses a desktop application with the following
functionalities:
1. **_Login_**. After successful login, a new window opens showing the available samples
   register the children (drawing, treasure hunt, poem) and the corresponding age category: 6-8 years, 9-11 years,
   12-15 years at which a participant can register and the number of children already registered for each test and category
   of age.
2. **_Search_**. After successful login, the office person can search the participants registered in a particular
   sample and age group. The application will display in another list/table the names of the participants in that test and
   their age.
3. **_Registration_**. A participant can register for a maximum of 2 tests. When registering, the person at the office enters
   the child's name, his age and the tests he wants to participate in. After enrolling a child, all
   the people from the other offices see the updated list of samples and the number of copies registered for each
   sample.
4. **_Logout_**.

### _**_Technologies used_**_:
* Java 17, JavaFX 17
* C# 11
* Gradle
* SQLite, JDBC
* ApacheMQ
* ReactJS, Vite




### **_Notes:_**
Task Tracker is a semester project which aims to provide users with a clear and robust way to organize tasks for a contest. It's written in Java and C#, making it possible to host a server in either Java or C# and also open clients in both languages.
Both implementations share an SQLite database which holds records for all users, participants and tasks for a contest. 
