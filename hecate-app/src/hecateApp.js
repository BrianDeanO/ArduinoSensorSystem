import React, { useState, useEffect } from "react";
import HecateLogo  from "./images/Hecate_Logo.png";
import HecateHeader from "./HecateApp/HecateHeader";
import HecateBody  from "./HecateApp/HecateBody";
// import TemperatureVisualizationBox from "./Visualizations/visualizationBox";
import TemperatureVisualizationBox from "./HecateApp/Visualizations/TemperatureVisualizationBox.tsx";
import VisualizationBox from "./HecateApp/Visualizations/VisualizationBox.tsx";
import DataSelectors from "./HecateApp/Visualizations/DataSelectors.tsx"
import {BrowserRouter, Route, Routes, NavLink } from 'react-router-dom';
import GraphContainer from "./HecateApp/Visualizations/graphContainer";
import { 
    userTable, 
    deviceTable, 
    deviceInfoType,
    userDeviceEntriesTable, 
    deviceSensorEntriesTable,
    sensorTable, 
    sensorDataTable, 
    userDeviceEntries, 
    version,
    apiInfo
} from "./Variables.js";
import { 
    fetchList, 
    fetchUserList, 
    fetchDeviceList,
    fetchUserDevice,
    fetchUserDevicesList 
} from "./HecateApp/Helpers/FetchDataFunctions.tsx";

const HecateApp = () => {
    const userInfo = JSON.parse(localStorage.getItem("userInfo"));
    // const userInfo = {};
    // console.log('userInfo', userInfo)

    const [loggedIn, setLoggedIn] = useState((userInfo && (userInfo.currentUserID !== null)) ? true : false);
    const [selectedSensorInfo, setSelectedSensorInfo] = useState('---');
    const [selectedDevice, setSelectedDevice] = useState({});
    const [timeType, setTimeType] = useState('');
    const [userName, setUserName] = useState(userInfo ? userInfo.currentUserName : '');
    const [password, setPassword] = useState('');
    const [userID, setUserID] = useState(userInfo ? userInfo.currentUserID : null);
    const [users, setUsers] = useState([]);
    const [devices, setDevices] = useState([]);
    const [sensors, setSensors] = useState([]);
    const [sensorData, setSensorData] = useState([]);
    const [userDevices, setUserDevices] = useState([]);
    const [deviceSensors, setDeviceSensors] = useState([]);


    const [refreshInterval, setRefreshInterval] = useState(10000);
    // console.log('BEFORE DEVICES', devices);

    // console.log('totla refresh?????????')

    useEffect(() => {
        if(loggedIn) {
            console.log('userName', userName)
            logInUser(userName, password);
        }
    })


    useEffect(() => {
        const fetchData = async (apiURL, apiIndex) => {
            await fetch(apiURL)
                .then((response) => {return response.json().then((responseData) => {
                    // console.log('responseData', responseData)

                    switch(apiIndex) {
                        case apiInfo.USERS.INDEX:
                            setUsers(responseData);
                            break;

                        case apiInfo.DEVICES.INDEX:
                            setDevices(responseData);

                            // if(userID && loggedIn) {
                            //     const tempUserDeviceURL = `${apiInfo.USER_DEVICES.URL}/${userID}:${responseData.deviceID}`;
                            //     fetchData(tempUserDeviceURL, apiInfo.USER_DEVICES.INDEX);
                            // }
                            break;

                        case apiInfo.USER_DEVICES.INDEX:
                            // let tempUserDeviceArray = userDevices;
                            // console.log('PUSH USER DEVICES', apiURL)
                            // tempUserDeviceArray.push(responseData);
                            setUserDevices(responseData);
                            // console.log('NEW ARRAY', userDevices)
                            break;

                        case apiInfo.SENSORS.INDEX:
                            setSensors(responseData);
                            break;
                        case apiInfo.DEVICE_SENSORS.INDEX:
                            setDeviceSensors(responseData);
                            break;
                        case apiInfo.SENSOR_DATA.INDEX:
                            setSensorData(responseData);
                            break;                          

                        default:
                            break;
                    }
                        }).catch((error) => {
                            console.log("Authorization Failed: " + error.message)
                    })
                });
        }

        let interval = setInterval(() => {
            fetchData(apiInfo.USERS.URL, apiInfo.USERS.INDEX);
            fetchData(apiInfo.DEVICES.URL, apiInfo.DEVICES.INDEX);
            fetchData(apiInfo.USER_DEVICES.URL, apiInfo.USER_DEVICES.INDEX);
            fetchData(apiInfo.SENSORS.URL, apiInfo.SENSORS.INDEX);
            fetchData(apiInfo.DEVICE_SENSORS.URL, apiInfo.DEVICE_SENSORS.INDEX);
            fetchData(apiInfo.SENSOR_DATA.URL, apiInfo.SENSOR_DATA.INDEX);
        }, refreshInterval)

        fetchData(apiInfo.USERS.URL, apiInfo.USERS.INDEX);
        fetchData(apiInfo.DEVICES.URL, apiInfo.DEVICES.INDEX);
        fetchData(apiInfo.USER_DEVICES.URL, apiInfo.USER_DEVICES.INDEX);
        fetchData(apiInfo.SENSORS.URL, apiInfo.SENSORS.INDEX);
        fetchData(apiInfo.DEVICE_SENSORS.URL, apiInfo.DEVICE_SENSORS.INDEX);
        fetchData(apiInfo.SENSOR_DATA.URL, apiInfo.SENSOR_DATA.INDEX);

        return () => clearInterval(interval);
    },  [refreshInterval])

    console.log('DEVICES', devices);
    console.log('SENSORS', sensors);
    console.log('USERS', users);
    console.log('USER DEVICES', userDevices);
    console.log('DEVICE SENSORS', deviceSensors);
    console.log('SENSOR DATA', sensorData);



    function getAttachedSensors(selectedDeviceID) {
        console.log('selected device', selectedDeviceID);
        let tempSensorArray = [];
        deviceSensors.forEach((sEntry, sIndex) => {
            console.log('sEntry', sEntry);
            if(sEntry.deviceID === selectedDeviceID) {
                sensors.forEach((sensor, sensorIndex) => {
                    if(sensor.sensorID === sEntry.sensorID) {
                        console.log('PUSH')
                        tempSensorArray.push(sensor);
                    }
                })
            }

        })
        console.log('SESNORS ARRAy after full dentry', tempSensorArray);
        setSensors(tempSensorArray);
    }

    function getSelectedDevice(deviceID) {
        devices.forEach((device, index) => {
            if(device.deviceID === deviceID) {
                console.log('SET DEVICE', device);
                setSelectedDevice(device);
                getAttachedSensors(deviceID);
                return;
            }
        })
    }

    useEffect(() => {
        localStorage.setItem("userInfo", JSON.stringify(
            {
                currentUserName: userName, 
                currentUserID: userID, 
                // sensors: sensors, 
                // devices: devices, 
                // userDevices: userDevices
            }));
    }, [userName, userID]);

    const displayDeviceArray = [];
    devices.forEach((deviceEntry, dIndex) => {
        console.log('device', deviceEntry)
        userDevices.forEach((userDeviceEntry, uDIndex) => {
            if((userID === userDeviceEntry.userID) && (deviceEntry.deviceID === userDeviceEntry.deviceID)) {
                displayDeviceArray.push(deviceEntry);
            }
        })
    })

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
                <div className="MainDevicesBox">
                    <div className="deviceSelectorTitleText">Devices</div>
                    <div className="MainDeviceSelectorBox">
                            {
                                    displayDeviceArray.map((device, i) => {
                                        return (
                                            <span 
                                                className={((device.deviceID === selectedDevice.deviceID)) ? "SelectedDeviceNameBox" : "DeviceNameBox"}
                                                value={device.deviceID} 
                                                key={i} 
                                                onClick={(e) => {
                                                    console.log('e', e.target);
                                                    console.log('e.target.value', e.target.getAttribute("value"))
                                                    const tempDeviceID = parseInt(e.target.getAttribute("value"));
                                                    setSelectedSensorInfo('---');
        
                                                    // if((e.target.getAttribute("value")) === '---') {
                                                    //     setSelectedDevice('---');
                                                    // } else {
                                                    //     // setSelectedDevice(deviceTable[e.target.getAttribute("value") - 1]);
        
                                                    //     getSelectedDevice(e.target.getAttribute("value"));
                                                    //     getAttachedSensors(selectedDevice);
                                                    // }
        
                                                    getSelectedDevice(tempDeviceID);
                                                    console.log('selectedSensorInfo', selectedSensorInfo)
                                            }}>
                                                {device.deviceName}
                                            </span>
                                        )
                                    })
                        
                            }                
                    </div>
                </div>

                {/* {((selectedSensorInfo !== '---') && (selectedSensorInfo !== undefined)) ? 
                    <VisualizationBox sensorInfo={selectedSensorInfo}/> : 
                    null} */}
                {/* {((selectedDevice !== '---') && (selectedDevice !== undefined)) ? 
                    <DataSelectors sensorInfo={selectedSensorInfo}/> : 
                    null} */}

                <DataSelectors
                    selectedDevice={selectedDevice}
                    sensorDataTable={sensorDataTable}
                    sensors={sensors}
                />




            </div>
        </div>
    );
}

export default HecateApp;