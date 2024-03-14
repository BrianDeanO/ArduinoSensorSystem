import React, { useState } from "react";
import HecateLogo  from "../images/Hecate_Logo.png";
import { version, proxyURL } from "../variables.js";
import { CurrentUserType, UserType } from "../interfaces.js";
import axios from "axios";

interface HeaderProps {
    logIn: ((user: UserType) => void);
    loggedInUser: CurrentUserType;
    isAdmin: boolean;
    manageUsers: ((isManagingUsers: boolean) => void);
}
var shajs = require('sha.js');

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

    async function logInUser(userName: string, userPassword: string) {
        const userNames = userName.split('.');
        let tempUser: UserType;
        const hashedPassword = shajs('sha256').update(userPassword).digest('hex');
        const hashedPassword1 = shajs('sha256').update('123').digest('hex');
        // const hashedPassword2 = shajs('sha256').update('456').digest('hex');
        // const hashedPassword3 = shajs('sha256').update('789').digest('hex');
        console.log('userPassword', userPassword)
        console.log('hashedPassword', hashedPassword)
        console.log('123', hashedPassword1)
        // console.log('hashedPassword2', hashedPassword2)
        // console.log('hashedPassword3', hashedPassword3)

        if(userNames.length === 2) {
            await axios({
                method: 'get',
                url: `${proxyURL}/api/User/${userNames[0]}.${userNames[1]}:${hashedPassword}`,
            })
                .then(function (response) {
                    tempUser = response.data;
                    if(tempUser.userFirstName) {
                        setUserFirstName(tempUser.userFirstName);
                        setUserLastName(tempUser.userLastName);
                        setLoggedIn(true);
                        logIn(tempUser);
                    }

                }).catch(error => {
                    console.log(error);
                })
        }
    }

    function logOutUser() {
        setUserFirstName('');
        setUserLastName('');
        setUserName('');
        setPassword('');
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
                            } else {
                                logInUser(userName, password);
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