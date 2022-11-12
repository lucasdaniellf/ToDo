import React from "react"
import classes from './style-components.module.css'

export const GridComponent = (props) => {
    return (
        <div className={classes.grid}>
            {props.children}
        </div>
    );
}