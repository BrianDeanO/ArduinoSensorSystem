import React, { useState, useEffect } from "react";
import HecateLogo  from "../images/Hecate_Logo.png";
import { version } from "../Variables";
import { CurrentUserType } from "../interfaces";

interface HeaderProps {
    logIn: (userID: number) => void;
}

const HecateHeader: React.FC<HeaderProps> = (
    {logIn}: HeaderProps
) => {
    let userJSON = localStorage.getItem("currentUser");
    const loggedInUser: CurrentUserType = (userJSON !== null) ? JSON.parse(userJSON) : {};

    const [loggedIn, setLoggedIn] = useState((loggedInUser && (loggedInUser.currentUserID !== 0)) ? true : false);
    const [userFirstName, setUserFirstName] = useState(loggedInUser ? loggedInUser.currentFirstName : '');
    const [userLastName, setUserLastName] = useState(loggedInUser ? loggedInUser.currentLastName : '');
    const [password, setPassword] = useState('');
    const [userID, setUserID] = useState(loggedInUser ? loggedInUser.currentUserID : 0);

    useEffect(() => {
        if(loggedIn) {
            logIn(userID);
        }
        localStorage.setItem("currentUser", JSON.stringify({
                currentUserID: userID,
                currentFirstName: userFirstName,
                currentLastName: userLastName
            }));
    }, [ userID, userFirstName, userLastName ]);

    function logInUser(userFName: string, userPassword: string) {
        console.log('logging in user');
        const tempUserID = 1;

        /**********
          USE AXIOS TO GET USER TABLE THEN CROSS REFERENCE TO LOG IN.
          OBTAINGED THE USER ID

        *************/

        setUserID(tempUserID);
        setUserFirstName(userFName);
        setUserLastName('Solo');

        // Calling the logIN function
        logIn(tempUserID);
    }

    function logOutUser() {
        setUserFirstName('');
        setUserLastName('');
        setPassword('');
        setUserID(0);
        setLoggedIn(false);

        logIn(0);
    }

    // function logInUser(userName, userPassword) {
    //     let foundUser = false;
    //     let tempUserID;
    //     let tempUserDevicesArray = [];
    //     // console.log('LOGGED IN?????', loggedIn)
        
    //     const fetchUserDeviceData = async (userId, deviceID, device) => {
    //         console.log('USRE DEVICE URL', `${apiInfo.USER_DEVICES.URL}/${userID}:${deviceID}`);

    //         await fetch(`${apiInfo.USER_DEVICES.URL}/${userID}:${deviceID}`)
    //             .then((response) => {return response.json().then((responseData) => {
    //                 console.log('responseData', responseData)

    //                 if(Object.keys(responseData).length !== 0) {
    //                     // console.log('PUSHING USER DEVICES ARRAY', responseData)
    //                     // console.log('DVICE PUSEHD', device)
    //                     tempUserDevicesArray.push(device);
    //                     // userDevices.push(device)
    //                     // setUserDevices(tempUserDevicesArray);
    
    //                     // deviceTable.forEach((device, deviceIndex) => {
    //                     //     if(device.device_ID === dEntry.device_ID) {
    //                     //         console.log('PUSH')
    //                     //         tempDeviceArray.push(device);
    //                     //     }
    //                     // })
    
    
    //                 }
    //                     }).catch((error) => {
    //                         console.log("Authorization Failed: " + error.message)
    //                 })
    //             });
    //     }


        
    //     // console.log('LOGGING IN')
    //     // console.log('usernmae', userName);
    //     // console.log('password', userPassword)

    //     // console.log('user table', userTable);

    //     users.forEach((entry, index) => {
    //         // console.log(index, 'entry', entry);
    //         if( (loggedIn && (entry.userName === userName)) || 
    //             (!loggedIn && ((entry.userName === userName) && (entry.userPassword === userPassword)))) {
    //                 console.log('FOUND')
    //                 tempUserID = entry.userID;
    //                 foundUser = true;
    //                 return;
    //         } 
    //         // if((entry.userName === userName) && (entry.userPassword === userPassword)) {

    //         // }
    //     });
        
    //     setUserID(tempUserID);
    //     setUserName(userName);
        
    //     // console.log('user ID', tempUserID);

    //     if(foundUser) {
    //         devices.forEach((dEntry, dIndex) => {
    //             // console.log('D entry', dEntry);
    //             // const tempUserDevice = fetchUserDevice(tempUserID, dEntry.deviceID);
    //             // fetchUserDeviceData(userID, dEntry.deviceID, dEntry);
    //         });

    //         // console.log('FINAL USER DEVICES array', tempUserDevicesArray);
    //         // setUserDevices(tempUserDevicesArray);

    //         // getAttachedSensors(tempUserDevicesArray);

    //         setLoggedIn(true);
    //     }
    //     // console.log('final user id', userID);

    //     setPassword('');
    // }

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
                            <div className="loggedInText"> {`${userFirstName}`} </div> :
                            <textarea
                                    className="loginInputTextArea"
                                    value={userFirstName}
                                    id={'NAME'}
                                    onChange={(e) => {setUserFirstName(e.target.value.toString());}}
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
            <button 
                className="mainLoginButton"
                onClick={(e) => {
                    if(loggedIn) {
                        logOutUser();
                    }
                    else {
                        logInUser((document.getElementById('NAME') as HTMLInputElement).value,
                                  (document.getElementById('PASWRD') as HTMLInputElement).value)
                    }
                }}>
                    {loggedIn ? 'Log Out' : 'Log In'}
            </button>
        </div>
    </div>
    )
}

export default HecateHeader;