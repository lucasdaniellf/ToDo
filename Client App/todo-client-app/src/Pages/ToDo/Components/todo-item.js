import { React } from 'react';
import { CardComponent } from '../../Style Components/Card';
import classes from './todo-item.module.css'

export const TodoItem = ({todo, updateTodoStatus}) => {
    const changeTodoStatus = (e) => {
        e.preventDefault()
        updateTodoStatus(todo.id)
    }

    return (
        <CardComponent>
            <div className={classes.myDiv}>
                <h1>{todo.title}</h1>
                <p>{todo.description}</p>
            </div>
            <div>
                <form onSubmit={(e) => changeTodoStatus(e)}>
                    <button 
                        className={classes.button}
                        id="button-todo-status">
                            {todo.isDone === 0 ? "Marcar como Concluído" : "Concluído"}
                    </button>
                </form>
            </div>
        </CardComponent>
    );
}