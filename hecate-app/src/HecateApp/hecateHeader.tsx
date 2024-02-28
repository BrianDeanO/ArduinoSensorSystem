import React, { useState, useEffect } from "react";
import HecateLogo  from "../images/Hecate_Logo.png";
import { version, proxyURL, ADMIN, userTable } from "../variables.js";
import { CurrentUserType, UserType } from "../interfaces.js";
import { localStorageTitles } from "../variables.js";
import axios from "axios";

interface HeaderProps {
    logIn: (user: UserType) => void;
    loggedInUser: CurrentUserType;
    isAdmin: boolean;
    manageUsers: (isManagingUsers: boolean) => void;
}

const client = axios.create({
    baseURL: "http://localhost:5270/api" 
});

const HanSolo =     {
    userID: 1, 
    userType: "admin", 
    userFirstName: "Han", 
    userLastName: "Solo", 
    userPassword: "123", 
    userEmail: "",
    userPhone: "",
    userNotification: true
} as UserType

  
// const crypto = require('node:crypto');
// console.log("hashes", crypto.getHashes())

const HecateHeader: React.FC<HeaderProps> = ({
    logIn,
    loggedInUser,
    isAdmin,
    manageUsers
}: HeaderProps) => {

    const [loggedIn, setLoggedIn] = useState(((Object.keys(loggedInUser).length > 1) && (loggedInUser.currentUserID !== 0)) ? true : false);
    const [userFirstName, setUserFirstName] = useState((Object.keys(loggedInUser).length > 1) ? loggedInUser.currentFirstName : '');
    const [userLastName, setUserLastName] = useState((Object.keys(loggedInUser).length > 1) ? loggedInUser.currentLastName : '');
    const [userName, setUserName] = useState('');
    const [password, setPassword] = useState('');
    const [userID, setUserID] = useState((Object.keys(loggedInUser).length > 1) ? loggedInUser.currentUserID : 0);
    const [users, setUsers] = useState([] as UserType[]);

    async function logInUser(userName: string, userPassword: string) {
        console.log('logging in user', userName, userPassword);
        const userNames = userName.split('.');
        let tempUsers: UserType[] = [];
        let tempUser: UserType;

        if(userNames.length === 2) {
            await axios({
                method: 'get',
                url: `${proxyURL}/api/User/${userNames[0]}.${userNames[1]}:${userPassword}`,
            })
                .then(function (response) {
                    console.log('response', response);
                    // setUsers(response.data);
                    // tempUsers = response.data;
                    // console.log('FOUND USER', user);

                    tempUser = response.data;
                    console.log('FOUND USER', tempUser);
                    setUserID(tempUser.userID);
                    setUserFirstName(tempUser.userFirstName);
                    setUserLastName(tempUser.userLastName);
                    setLoggedIn(true);
                    logIn(tempUser);

                }).catch(error => {
                    console.log(error);
                })
    
            // console.log('tempUsers', tempUsers)
    
            // if(tempUsers.length > 0) {
            //     tempUsers.forEach((user, i) => {
            //         if((user.userFirstName === userNames[0]) && 
            //             (user.userLastName === userNames[1]) && 
            //             (user.userPassword === userPassword)
            //         ) {
            //             console.log('FOUND USER', user);
            //             setUserID(user.userID);
            //             setUserFirstName(user.userFirstName);
            //             setUserLastName(user.userLastName);
            //             setLoggedIn(true);
            //             logIn(user);
            //             return;
            //         }
            //     })
            // } else {
            //     console.log('ERROR COMMUNICATING TO API');
            // }
        }

    }

    function logOutUser() {
        setUserFirstName('');
        setUserLastName('');
        setUserName('');
        setPassword('');
        setUserID(0);
        setLoggedIn(false);
        logIn({} as UserType);
    }

    return (
        <div className="MainHecateHeader">
            <div className="ImageBox">
                <img src={HecateLogo} className="HecateLogo" alt="Hecate Software Logo" />
                <div className="VersionBox">
                    Version {version} {isAdmin ? ' - Admin View' : null}
                </div>
            </div>

            <div className="MainHeaderLogin">
                {
                    loggedIn ? 
                        <div className="loggedInUserMainBox">
                            <div className="loggedInText">
                                Welcome, {userFirstName} {userLastName}!
                            </div>
                        </div> :
                        <div className="mainLoginInputBox">
                            <div className="userNameLoginBox">
                                <div className="loginHeaderText">
                                    User Name
                                </div>
                                <textarea
                                    className="LoginUserNameTextArea"
                                    value={userName}
                                    id={'NAME'}
                                    onChange={(e) => {setUserName(e.target.value.toString());}}
                                    spellCheck={false}
                                    cols={1}
                                    rows={1}></textarea>
                            </div>
                            <div className="passwordLoginBox">
                                <div className="loginHeaderText">
                                    Password
                                </div>
                                <input
                                    className="LoginPasswordTextArea"
                                    value={password}
                                    id={'LOGIN_PASSWORD'}
                                    type={'password'}
                                    onChange={(e) => {setPassword(e.target.value.toString());}}></input>
                            </div>
                        </div>
                }

                <div className="UserLoginButtonSubBox">
                    {isAdmin ? 
                        <button 
                            className="ManageUserButton"
                            onClick={(e) => {
                                manageUsers(true);
                            }}>
                                Manage Users
                        </button> : null
                    }
                    <button 
                        className="LogInOutButton"
                        onClick={(e) => {
                            if(loggedIn) {
                                logOutUser();
                            }
                            else {
                                    ///
                                    ///
                                    //  NEED TO PASS IN  THE CONCATENATION
                                    //  THEN PARSE TO CHECK FRIST AND ASLT NAME
                                    //

                                logInUser(userName, password);
                                //           (document.getElementById('LOGIN_PASSWORD') as HTMLInputElement).value);
                                // logInUser((document.getElementById('NAME') as HTMLInputElement).value,
                                //           (document.getElementById('LOGIN_PASSWORD') as HTMLInputElement).value);
                                        // setLoggedIn(true);
                                        // logIn(HanSolo);
                            }
                        }}>
                            {loggedIn ? 'Log Out' : 'Log In'}
                    </button>
                </div>

            </div>
        </div>
    )
}

export default HecateHeader;