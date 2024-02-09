import React, { useState, useEffect } from "react";
import HecateHeader from "./HecateHeader.tsx";
import HecateBody from "./HecateBody.tsx";
import { CurrentUserType, UserType } from ".interfaces.d.ts";
import { localStorageTitles } from "../Variables.js";

const HecateApp = () => {
    const [loggedInUserID, setLoggedInUserID] = useState(false);

    const userJSON = localStorage.getItem(localStorageTitles.currentUser);
    const loggedInUser = (userJSON !== null) ? JSON.parse(userJSON) : {};
    
    const [loggedIn, setLoggedIn] = useState((loggedInUser && (loggedInUser.currentUserID !== 0)) ? true : false);

    // useEffect(() => {
    //     if(loggedIn) {
    //         logIn(userID);
    //     }
    //     localStorage.setItem(localStorageTitles.currentUser, JSON.stringify({
    //             currentUserID: userID,
    //             currentFirstName: userFirstName,
    //             currentLastName: userLastName
    //         }));
    // }, [ userID, userFirstName, userLastName, loggedIn, logIn ]);


    WE ARE SETTING UP THE SETTING IN LOCAL Storage, SO WE CAN PASS THE SAVE UserType
        TOT HecateHeader. SINCE IT REFRESHES AFTER THE LOGIN AND DOESN'T SAVE THE DATA


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