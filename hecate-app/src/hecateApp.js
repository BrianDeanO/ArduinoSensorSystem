import React, { useState, useEffect } from "react";
import HecateLogo  from "./images/Hecate_Logo.png";
import HecateHeader from "./HecateApp/hecateHeader.tsx";
import HecateBody  from "./HecateApp/HecateBody.tsx";
// import TemperatureVisualizationBox from "./Visualizations/visualizationBox";
import TemperatureVisualizationBox from "./HecateApp/Visualizations/TemperatureVisualizationBox.tsx";
import VisualizationBox from "./HecateApp/Visualizations/VisualizationBox.tsx";
import {BrowserRouter, Route, Routes, NavLink } from 'react-router-dom';
import GraphContainer from "./HecateApp/Visualizations/graphContainer";
import { sensorTable, version } from "./Variables.js";
import { userTable, userSensorEntriesTable, sensorDataTable } from "./Variables.js";



const HecateApp = () => {
    const userInfo = JSON.parse(localStorage.getItem("userInfo"));
    console.log('userInfo', userInfo)

    const [loggedIn, setLoggedIn] = useState((userInfo && (userInfo.currentUserID !== '')) ? true : false);
    const [selectedSensorInfo, setSelectedSensorInfo] = useState('---');
    const [timeType, setTimeType] = useState('');
    const [userName, setUserName] = useState(userInfo ? userInfo.currentUserName : '');
    const [password, setPassword] = useState('');
    const [userID, setUserID] = useState(userInfo ? userInfo.currentUserID : null);
    const [userSensors, setUserSensors] = useState(userInfo ? userInfo.sensors : []);
    // console.log('usernmae', userName);
    // console.log('password', password)

    console.log('selectedSensorInfo', selectedSensorInfo)

    function logInUser(userName, userPassword) {
        let foundUser = false;
        let tempUserID;
        let tempSensorArray = [];
        
        // console.log('LOGGING IN')
        // console.log('usernmae', userName);
        // console.log('password', userPassword)

        // console.log('user table', userTable);

        userTable.forEach((entry, index) => {
            if((entry.user_name === userName) && (entry.user_password === userPassword)) {
                tempUserID = entry.user_ID;
                foundUser = true;
                return;
            }
        });
        
        console.log('user ID', tempUserID);

        if(foundUser) {
            console.log(' userSensorEntriesTable', userSensorEntriesTable);
            userSensorEntriesTable.forEach((entry, index) => {
                console.log('entry', entry);
                if(entry.user_ID === tempUserID) {
                    tempSensorArray.push(entry.sensor_ID)
                }
            })
            console.log('sesnor  array', tempSensorArray);
            setUserID(tempUserID);
            setUserName(userName);
            setUserSensors(tempSensorArray);
            setLoggedIn(true);
        }
        console.log('final user id', userID);

        setPassword('');
    }

    function logOutUser() {
        setUserName('');
        setPassword('');
        setUserID('');
        setUserSensors([]);
        setSelectedSensorInfo('---')
        setLoggedIn(false);
    }

    useEffect(() => {
        localStorage.setItem("userInfo", JSON.stringify(
            {currentUserName: userName, currentUserID: userID, sensors: userSensors}));
    }, [userName, userID, userSensors]);

    return (
        <div className="MainPage">
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
                    <button 
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
                    </button>
                </div>
            </div>
            <div className="HecateBody">
                <div className="MainSensorBox">
                    <div className="sensorTypeSelectorBox">
                        <div className="sensorTypeSelectorText">Sensor</div>
                        <select 
                            className="sensorTypeSelector" 
                            id="sensorType"
                            onClick={(e) => {
                                console.log('e.target.value', e.target.value)
                                if(e.target.value === 0) {
                                    setSelectedSensorInfo('---');
                                } else {
                                    //@ts-ignore
                                    setSelectedSensorInfo(sensorTable[e.target.value - 1]);
                                }
                            }}>
                                <option value={0}>---</option>
                                {
                                    sensorTable.map((sensor, index) => {
                                        if(sensor.sensor_ID !== userSensors[index]) return null;
                                        return (
                                            <option value={sensor.sensor_ID} key={index}>
                                                {sensor.sensor_name}
                                            </option>
                                        )
                                    })
                                }
                        </select>
                    </div>
                    <div className="timeTypeSelectorBox">
                        <div className="timeTypeSelectorText">Time Frame</div>
                        <select 
                            className="timeTypeSelector" 
                            id="sensorType"
                            onClick={(e) => {
                                //@ts-ignore
                                setTimeType(e.target.value);
                            }}>
                                <option value={'---'}>---</option>
                                <option value={'Past Day'}>Past Day</option>
                                <option value={'Past Week'}>Past Week</option>
                                <option value={'Past Month'}>Past Month</option>
                                <option value={'Past Six Months'}>Past Six Months</option>
                                <option value={'Lifetime'}>Lifetime</option>
                        </select>
                    </div>
                </div>

                {((selectedSensorInfo !== '---') && (selectedSensorInfo !== undefined)) ? 
                    <VisualizationBox sensorInfo={selectedSensorInfo}/> : 
                    null}
            </div>
        </div>
    );
}

export default HecateApp;