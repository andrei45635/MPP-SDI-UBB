import com.example.utils.AbstractServer;
import com.example.utils.ContestRpcAMSConcurrentServer;
import com.example.utils.ServerException;
import org.springframework.context.support.ClassPathXmlApplicationContext;

import java.util.Properties;

public class StartAMSRpcServer {
    public static void main(String[] args) {
        ClassPathXmlApplicationContext context = new ClassPathXmlApplicationContext("spring-server.xml");
        Properties props = context.getBean("jdbcProps", Properties.class);
        System.out.println("jdbcProps: " + props);
        AbstractServer server=context.getBean("contestTCPServer", ContestRpcAMSConcurrentServer.class);
        System.out.println(server);
        try {
            server.start();
        } catch (ServerException e) {
            System.err.println("Error starting the server" + e.getMessage());
        }
    }
}
