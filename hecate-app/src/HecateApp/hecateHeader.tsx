import React, { SyntheticEvent, useState } from "react";
// /Users/deano/ArduinoSensorSystem/ArduinoSensorSystem/hecate-app/src/images/Hecate_Logo.png
import HecateLogo  from "../images/Hecate_Logo.png";
// import HecateLogo from "../images/"
import { version } from "../Variables";
import {userType} from "../interfaces";

interface HeaderProps {
    logIn: (e: SyntheticEvent) => void;
}

const HecateHeader: React.FC<HeaderProps> = (
    {logIn}: HeaderProps
) => {
    // const userInfo = JSON.parse(localStorage.getItem("userInfo"));
    const userInfo: userType = {
        currentUserID: 1,
        currentUserName: "Han"
    };
    // const [loggedIn, setLoggedIn] = useState((userInfo && (userInfo.currentUserID !== null)) ? true : false);
    const [selectedSensorInfo, setSelectedSensorInfo] = useState('---');
    const [selectedDevice, setSelectedDevice] = useState({});
    const [timeType, setTimeType] = useState('');
    const [userName, setUserName] = useState(userInfo ? userInfo.currentUserName : '');
    const [password, setPassword] = useState('');
    const [userID, setUserID] = useState(userInfo ? userInfo.currentUserID : null);
    const [loggedIn, setLoggedIn] = useState(false);

    return (
        <div className="MainHecateHeader">
        <div className="ImageBox">
            <img src={HecateLogo} className="HecateLogo" alt="Hecate Software Logo" />
            <div className="VersionBox">
                Version {version}
            </div>
        </div>
        <div className="MainHeaderLogin">
            <div className="mainLoginInputBox">
                <div className="userNameLoginBox">
                    <div className="loginHeaderText">
                        User Name
                    </div>
                    {
                        loggedIn ? 
                            <div className="loggedInText"> {userName} </div> :
                            <textarea
                                    className="loginInputTextArea"
                                    value={userName}
                                    id={'NAME'}
                                    onChange={(e) => {setUserName(e.target.value.toString());}}
                                    cols={1}
                                    rows={1}></textarea>
                    }
                </div>
                <div className="passwordLoginBox">
                    <div className="loginHeaderText">
                        Password
                    </div>
                    {
                        loggedIn ? 
                            <div className="loggedInText"> {password} </div> :
                            <textarea
                                    className="loginInputTextArea"
                                    value={password}
                                    id={'PASWRD'}
                                    onChange={(e) => {setPassword(e.target.value.toString());}}
                                    cols={1}
                                    rows={1}></textarea>
                    }

                </div>
            </div>
            {/* <button 
                className="mainLoginButton"
                onClick={(e) => {
                    if(loggedIn) {
                        logOutUser();
                    }
                    else {
                        logInUser(document.getElementById('NAME').value,
                                  document.getElementById('PASWRD').value)
                    }
                }}>
                    {loggedIn ? 'Log Out' : 'Log In'}
            </button> */}
            <button 
                    className="mainLoginButton"
                    onClick={(e) => {
                        logIn(e);
                    }}
                    // onClick={() => {
                    //     setLoggedIn(!loggedIn);
                    //     loggedIn ? 
                    //         alert("You have been logged out.") : 
                    //         alert("You have been logged in.")
                    // }}
                    >
                        {loggedIn ? 'Log Out': 'Log In'}
                </button>
        </div>
    </div>
    )
}

export default HecateHeader;