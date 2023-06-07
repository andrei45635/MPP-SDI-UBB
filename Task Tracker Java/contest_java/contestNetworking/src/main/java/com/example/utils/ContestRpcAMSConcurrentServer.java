package com.example.utils;

import com.example.IContestServiceAMS;
import com.example.network.ams.ContestClientAMSReflectionWorker;

import java.net.Socket;

public class ContestRpcAMSConcurrentServer extends AbsConcurrentServer {
    private IContestServiceAMS contestServer;

    public ContestRpcAMSConcurrentServer(int port, IContestServiceAMS contestServer) {
        super(port);
        this.contestServer = contestServer;
        System.out.println("Contest- ContestRpcAMSConcurrentServer port " + port);
    }

    @Override
    protected Thread createWorker(Socket client) {
        ContestClientAMSReflectionWorker worker = new ContestClientAMSReflectionWorker(contestServer, client);
        Thread tw = new Thread(worker);
        return tw;
    }
}
