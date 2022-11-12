import { HubConnectionBuilder } from "@microsoft/signalr";

export class signalrClient{
    constructor(endpoint){
        this.connection = new HubConnectionBuilder()
                                    .withUrl(endpoint)
                                    .withAutomaticReconnect([0, 1000, 3000, 7000, 15000])
                                    .build()
    }

    addMethodToListen(name, callback){
        this.connection.on(name, callback)
    } 

    startConnection(){
        let flag = false;
        this.connection.start().then(() => {
            console.log("client connected")
            flag = true;
        }).catch((e) => {
            console.log(e)
        })

        return flag;
    }

    stopConnection(){
        this.connection.stop().then(() => {
            console.log("Connection with hub has been stopped")
        })

        return false
    }
}