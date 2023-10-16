import React from "react";
import HecateHeader from "./HecateApp/hecateHeader.tsx";
import HecateBody from "./HecateApp/hecateBody.tsx";

export const backgroundColor = 'rgb(235, 225, 225)';

const HecateApp = () => {
    return (
        <div className="MainPage">
            <HecateHeader />
            <HecateBody />
        </div>

    );
}

export default HecateApp;