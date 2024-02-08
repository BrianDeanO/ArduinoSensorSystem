import React, { useState, useEffect } from "react";
import HecateHeader from "./HecateHeader.tsx";
import HecateBody from "./HecateBody.tsx";

const HecateApp = () => {

    const [loggedIn, setLoggedIn] = useState(false);

    const logIn = (e) => {
        // setLoggedIn(e.target.value);
        setLoggedIn(true);
        console.log('LGOGED IN SET', e);
    }

    return (
        <div className="MainPage">
            <HecateHeader logIn={logIn} />
            <HecateBody loggedIn={loggedIn} />
        </div>
    )
}

export default HecateApp;