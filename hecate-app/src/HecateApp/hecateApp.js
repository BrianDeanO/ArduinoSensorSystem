import React, { useState, useEffect } from "react";
import HecateHeader from "./hecateHeader.tsx";
import HecateBody from "./hecateBody.tsx";
import { ADMIN, localStorageTitles } from "../variables.js";

const HecateApp = () => {
    const userJSON = localStorage.getItem(localStorageTitles.currentUser);
    const loggedInUser = (userJSON !== null) ? JSON.parse(userJSON) : {};

    console.log('loggedInUser', loggedInUser)
    
    const [loggedInUserID, setLoggedInUserID] = useState((Object.keys(loggedInUser).length > 1) ? loggedInUser.currentUserID : 0);
    const [isAdmin, setIsAdmin] = useState((Object.keys(loggedInUser).length > 1) ? loggedInUser.isAdmin : false);
    // const [isAdmin, setIsAdmin] = useState(false);
    const [isManagingUsers, setIsManagingUsers] = useState(false);
    const [isLoggingOut, setIsLoggingOut] = useState(false);
    console.log('IS LOGGING OUT???', isLoggingOut)
    console.log('IS LOGGING OUT???', !isLoggingOut)

    const logIn = (user) => {
        console.log('LGOGED IN SET', user);

        if((Object.keys(user).length === 0)) {
            console.log('SETTING LOGING OUT TO TRUE', Object.keys(user).length)
            localStorage.setItem(localStorageTitles.currentUser, JSON.stringify({}));
            setIsLoggingOut(true);
            setIsManagingUsers(false);
        } else {
            console.log('ELSSSEEEE', Object.keys(user).length)
            localStorage.setItem(localStorageTitles.currentUser, JSON.stringify({
                currentUserID: user.userID,
                currentFirstName: user.userFirstName,
                currentLastName: user.userLastName,
                isAdmin: (user.userType ? (user.userType.toUpperCase() === ADMIN) : false)
            }));
            setIsLoggingOut(false);
        }
        setLoggedInUserID(user.userID);
        setIsAdmin((user.userType ? (user.userType.toUpperCase() === ADMIN) : false));
    }

    const manageUsers = (manageUsers) => {
        setIsManagingUsers(manageUsers);
    }

    return (
        <div className="MainPage">
            <HecateHeader 
                logIn={logIn} 
                loggedInUser={loggedInUser} 
                isAdmin={isAdmin} 
                manageUsers={manageUsers}
            />
            <HecateBody 
                loggedInUserID={loggedInUserID} 
                isAdmin={isAdmin} 
                manageUsers={manageUsers} 
                isManagingUsers={isManagingUsers}
                isLoggingOut={isLoggingOut}
            />
        </div>
    )
}

export default HecateApp;