import React, { useCallback, useEffect, useMemo, useState } from "react";
import { RestApiClient } from "../../Services/rest-api-client/rest-api-client";
import { CreateTodoRequestsSettings } from "../../Services/rest-api-client/rest-api-factory-settings";
import { signalrClient } from "../../Services/signalr-client";
import { TodoItemGrid } from "./Components/todo-item-grid";
import classes from './todo.module.css'

export const TodoPage = () => {

    const [finishedTodos, setFinishedTodos] = useState([])
    const [unfinishedTodos, setUnfinishedTodos] = useState([])
    
    const apiClient = useMemo(() => new RestApiClient(CreateTodoRequestsSettings()),[]);
    const hub = useMemo(() => new signalrClient("http://localhost:5297/hub/todo?group=ToDo"), [])

    const changeTodoStatus = useCallback((id) => {
        apiClient.updateTodoStatus(id).then(
            () => {
                hub.connection.invoke("NotifyStatusChanged", parseInt(id))
            })
    },[apiClient, hub])

    useEffect(() => {
        const controller = new AbortController()

        let fetchTodos = async () => {
            try{
                let response = await apiClient.getTodos(controller.signal)
                let todoList = await response.json()
                
                setFinishedTodos(todoList.filter((x) => x.isDone === 1))
                setUnfinishedTodos(todoList.filter((x) => x.isDone === 0))
            } catch(error){
                if (error.name === 'AbortError') return;
                console.log("error "+ error) 
            }
        }
        
        fetchTodos()

        hub.addMethodToListen("NotifyStatusChanged", (t) => {
            console.log(`status changed for toDo ${t}`)
            fetchTodos()
        })

        hub.addMethodToListen("NotifyToDoUpdated", (t) => {
            console.log(`ToDo updated: ${t.id} - ${t.title}`)
            fetchTodos()
        })

        hub.addMethodToListen("NotifyToDoCreated", (t) => {
            console.log(`ToDo updated: ${t.id} - ${t.title}`)
            fetchTodos()
        })

        hub.addMethodToListen("NotifyToDoDeleted", (t) => {
            console.log(`ToDo deleted: ${t.id} - ${t.title}`)
            fetchTodos()
        })
        hub.startConnection();

        return () => {
            controller?.abort()
        }
    },[hub, apiClient])
    
    return (
        <div className={classes.todoSection}>
            <h1 className={classes.todoSectionTitle}>Tarefas a serem Concluidas:</h1>
            <TodoItemGrid todos={unfinishedTodos} 
                 updateTodoStatus={changeTodoStatus}/>
            <hr/>
            <h1 className={classes.todoSectionTitle}>Tarefas Finalizadas:</h1>
            <TodoItemGrid todos={finishedTodos} 
                 updateTodoStatus={changeTodoStatus}/>
        </div>
    );
}