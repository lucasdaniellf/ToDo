import React from "react";
import classes from './style-components.module.css'

export const CardComponent = (props) => {
    return (
        <div className={classes.card}>
            {props.children}
        </div>
    );
}