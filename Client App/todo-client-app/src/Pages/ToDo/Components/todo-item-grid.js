import React from "react"
import { TodoItem } from "./todo-item"
import { GridComponent } from "../../Style Components/Grid"

export const TodoItemGrid = ({todos, updateTodoStatus}) => {
    return (
        <GridComponent>
            {todos.map(t => 
                <TodoItem key={t.id} todo={t} updateTodoStatus={updateTodoStatus}/>
            )}
        </GridComponent>
    ) 
}