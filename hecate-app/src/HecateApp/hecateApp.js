import React, { useState, useEffect } from "react";
import HecateHeader from "./HecateHeader.tsx";
import HecateBody from "./HecateBody.tsx";
import { ADMIN, localStorageTitles } from "../Variables.js";

const HecateApp = () => {
    const userJSON = localStorage.getItem(localStorageTitles.currentUser);
    const loggedInUser = (userJSON !== null) ? JSON.parse(userJSON) : {};

    console.log('loggedInUser', loggedInUser)
    
    const [loggedInUserID, setLoggedInUserID] = useState((Object.keys(loggedInUser).length !== 0) ? loggedInUser.currentUserID : 0);
    const [isAdmin, setIsAdmin] = useState((Object.keys(loggedInUser).length !== 0) ? loggedInUser.isAdmin : false);

    const logIn = (user) => {
        setLoggedInUserID(user.userID);
        setIsAdmin((user.userType ? (user.userType.toUpperCase() === ADMIN) : false));

        console.log('LGOGED IN SET', user);
        localStorage.setItem(localStorageTitles.currentUser, JSON.stringify({
            currentUserID: user.userID,
            currentFirstName: user.userFirstName,
            currentLastName: user.userLastName,
            isAdmin: (user.userType ? (user.userType.toUpperCase() === ADMIN) : false)
        }));

    }

    return (
        <div className="MainPage">
            <HecateHeader logIn={logIn} loggedInUser={loggedInUser} isAdmin={isAdmin}/>
            <HecateBody loggedInUserID={loggedInUserID} isAdmin={isAdmin}/>
        </div>
    )
}

export default HecateApp;