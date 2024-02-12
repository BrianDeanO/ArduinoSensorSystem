import React, { useState, useEffect } from "react";
import HecateHeader from "./HecateHeader.tsx";
import HecateBody from "./HecateBody.tsx";
import { localStorageTitles } from "../Variables.js";

const HecateApp = () => {
    const userJSON = localStorage.getItem(localStorageTitles.currentUser);
    const loggedInUser = (userJSON !== null) ? JSON.parse(userJSON) : {};

    console.log('loggedInUser', loggedInUser)
    
    const [loggedInUserID, setLoggedInUserID] = useState((Object.keys(loggedInUser).length !== 0) ? loggedInUser.currentUserID : 0);

    const logIn = (user) => {
        setLoggedInUserID(user.userID);

        localStorage.setItem(localStorageTitles.currentUser, JSON.stringify({
            currentUserID: user.userID,
            currentFirstName: user.userFirstName,
            currentLastName: user.userLastName
        }));

        console.log('LGOGED IN SET', user);
    }

    return (
        <div className="MainPage">
            <HecateHeader logIn={logIn} loggedInUser={loggedInUser}/>
            <HecateBody loggedInUserID={loggedInUserID} />
        </div>
    )
}

export default HecateApp;