import React, { useEffect, useState, useCallback, Component, SyntheticEvent } from "react";
// import Selectors from "./DataAndTimeSelectors/selectors.js";
import TemperatureVisualizationBox from "./Visualizations/TemperatureVisualizationBox.js";
import Graph from "./Graph.tsx";
// import { variables, dataObject } from "../variables.js";
import { DeviceType, SensorType, SelectedDevice, SelectedSensor, SelectedTimeFrame, SelectedChannel } from "../interfaces";
import { localStorageTitles } from "../variables.js";
import NewDevice from "./AddNewPages/NewDevice.tsx";

import Devices from "./Devices.tsx";
import Selectors from "./Selectors.tsx";
import NewSensor from "./AddNewPages/NewSensor.tsx";
import ConfigureDevice from "./ConfigPages/ConfigureDevice.tsx";
import ConfigureSensor from "./ConfigPages/ConfigureSensor.tsx";
import ManageUsers from "./ManageUsers/ManageUsers.tsx";

interface HecateBodyProps {
    loggedInUserID: number;
    isAdmin: boolean;
    manageUsers: (isManagingUsers: boolean) => void;
    isManagingUsers: boolean;
    isLoggingOut: boolean;
}

const HecateBody: React.FC<HecateBodyProps>  = ({
    loggedInUserID,
    isAdmin,
    manageUsers,
    isManagingUsers,
    isLoggingOut
}: HecateBodyProps) => {

    const deviceJSON = localStorage.getItem(localStorageTitles.selectedDevice);
    const sensorJSON = localStorage.getItem(localStorageTitles.selectedSensor);
    const timeFrameJSON = localStorage.getItem(localStorageTitles.selectedTimeFrame);
    const channelJSON = localStorage.getItem(localStorageTitles.selectedChannel);

    console.log('READING IN DEVICE JSON', deviceJSON)

    const selectedDevice: SelectedDevice = (deviceJSON !== null) ? JSON.parse(deviceJSON) : {};
    const selectedSensor: SelectedSensor = (sensorJSON !== null) ? JSON.parse(sensorJSON) : {};
    const parsedTimeFrame: SelectedTimeFrame = (timeFrameJSON !== null) ? JSON.parse(timeFrameJSON) : {};
    const parsedChannel: SelectedChannel = (channelJSON !== null) ? JSON.parse(channelJSON) : {};

    const [selectedDeviceID, setSelectedDeviceID] = useState((Object.keys(selectedDevice).length !== 0) ? selectedDevice.selectedDeviceID : 0);
    const [selectedSensorID, setSelectedSensorID] = useState((Object.keys(selectedSensor).length !== 0)  ? selectedSensor.selectedSensorID : 0);
    const [selectedTimeFrame, setSelectedTimeFrame] = useState((Object.keys(parsedTimeFrame).length !== 0) ? parsedTimeFrame.selectedTimeFrame : 0);
    const [selectedChannelID, setSelectedChannelID] = useState((Object.keys(parsedChannel).length !== 0) ? parsedChannel.selectedChannelID : -1);

    // const [getSensorOverride, setGetSensorOverride] = useState(true);
    // const [addingNewDevice, setAddingNewDevice] = useState(false);
    // const [addingNewSensor, setAddingNewSensor] = useState(false);
    const [configuringDevice, setConfiguringDevice] = useState(false);
    const [configuringSensor, setConfiguringSensor] = useState(false);

    const [isAddingUser, setIsAddingUser] = useState(false);
    const [isUpdatingUser, setIsUpdatingUser] = useState(false);

    console.log('loggedInUserID', loggedInUserID);

    // const resetState = useCallback((isManagingUsers: boolean, isLoggingOut: boolean) => {
    //     console.log('DOING THIS USE EFFECT')
    //     // if((isManagingUsers && selectedDeviceID !== 0) || isLoggingOut) {
    //     if(isManagingUsers || isLoggingOut) {
    //         setSelectedDeviceID(0);
    //         setSelectedTimeFrame(0);
    //         setSelectedSensorID(0);
    //         setSelectedChannelID(0);
    //     }
    // }, [])

    // resetState(isManagingUsers, isLoggingOut);

    useEffect(() => {
        console.log('BODY - is logging out????', isLoggingOut)
        if(isLoggingOut) {
            setSelectedDeviceID(0);
            setSelectedTimeFrame(0);
            setSelectedSensorID(0);
            setSelectedChannelID(0);
        }
    }, [isLoggingOut])

    useEffect(() => {
        console.log('setting selected evice ', selectedDeviceID)
        localStorage.setItem(localStorageTitles.selectedDevice, JSON.stringify({
            selectedDeviceID: selectedDeviceID
        }));
    }, [ selectedDeviceID ]);

    useEffect(() => {
        localStorage.setItem(localStorageTitles.selectedSensor, JSON.stringify({
            selectedSensorID: selectedSensorID
        }));
    }, [ selectedSensorID ]);

    useEffect(() => {
        localStorage.setItem(localStorageTitles.selectedTimeFrame, JSON.stringify({
            selectedTimeFrame: selectedTimeFrame
        }));
    }, [ selectedTimeFrame ]);

    useEffect(() => {
        localStorage.setItem(localStorageTitles.selectedChannel, JSON.stringify({
            selectedChannelID: selectedChannelID
        }));
    }, [ selectedChannelID ]);

    const selectSensor = (selectedSensorID: number, resetChannelID: boolean, resetTimeFrame: boolean) => {
        setSelectedSensorID(selectedSensorID);
        if(resetChannelID) {
            setSelectedChannelID(0);
        }
        if(resetTimeFrame) {
            setSelectedTimeFrame(0);
        }
        console.log('SENSOR SET IN BODY', selectedSensorID);
    }

    const selectDevice = (selectedDeviceID: number, resetSensorID: boolean) => {
        // if(!addingNewDevice) {

            
        //     // console.log('SENSOR RESET?', selectedSensorID);
        // }

        // setSelectedTimeFrame(0);


        console.log('DEVICE SET IN BODY', selectedDeviceID);
        setSelectedDeviceID(selectedDeviceID);

        if(resetSensorID) {
            setSelectedSensorID(0);
            setSelectedChannelID(0);
            setSelectedTimeFrame(0);
        }


        // if(!isManagingUsers) {
        //     console.log('DEVICE SET IN BODY', selectedDeviceID);
        //     setSelectedDeviceID(selectedDeviceID);

        //     if(resetSensorID) {
        //         setSelectedSensorID(0);
        //         setSelectedChannelID(0);
        //         setSelectedTimeFrame(0);
        //     }
        //     // manageUsers(false);
        // }
        // setGetSensorOverride(true);


    }

    const selectChannel = (selectedChannelID: number) => {
        setSelectedChannelID(selectedChannelID);
        console.log('SSETTING CHANNEL IN BODY', selectedDeviceID);
    }

    const selectTimeFrame = (selectedTimeFrame: number) => {
        setSelectedTimeFrame(selectedTimeFrame);
        console.log('SSETTING TIME FRAME IN BODY', selectedDeviceID);
    }

    // const addDevice = (addingDevice: boolean, reset: boolean) => {
    //     setAddingNewDevice(addingDevice);
    //     if(reset) {
    //         resetValues();
    //     }
    // }

    // const addSensor = (addingSensor: boolean) => {
    //     setAddingNewSensor(addingSensor);
    //     resetValues();
    // }

    const configureDevice = (configuringDevice: boolean, resetDevices: boolean) => {
        setConfiguringDevice(configuringDevice);
        setSelectedTimeFrame(0);
        setSelectedSensorID(0);
        setSelectedChannelID(0);
        
        if(resetDevices) {
            setSelectedDeviceID(0);
        }
    }

    const configureSensor = (configuringSensor: boolean) => {
        setConfiguringSensor(configuringSensor);
        setSelectedTimeFrame(0);
        setSelectedChannelID(0);
    }

    const addUser = (addingUser: boolean) => {
        setIsAddingUser(addingUser);
        console.log('adduser', addingUser)
    }   

    const updateUser = (updatingUser: boolean) => {
        setIsUpdatingUser(updatingUser);
        console.log('udpateUser', updatingUser)
    }

    // console.log('BODY - SELECTED SENSOR ID - ', selectedSensorID)

    return (
        <div className="HecateBody">
            <Devices 
                selectDevice={selectDevice} 
                loggedInUserID={loggedInUserID}
                selectedDeviceID={selectedDeviceID}
                // addDevice={addDevice}
                configureDevice={configureDevice}
                isAdmin={isAdmin}
                configuringSensor={configuringSensor}
                isManagingUsers={isManagingUsers}
                isLoggingOut={isLoggingOut}
            />
            <div className="MainSelectorAndGraphBox">
                {
                    // addingNewDevice ? 
                    //     <NewDevice addDevice={addDevice}/> :
                    // addingNewSensor ? 
                    //     <NewSensor addSensor={addSensor} /> :
                    isManagingUsers ? 
                        <ManageUsers 
                            manageUsers={manageUsers} 
                            isManagingUsers={isManagingUsers}
                            addUser={addUser}
                            isAddingUser={isAddingUser}
                            updateUser={updateUser}
                            isUpdatingUser={isUpdatingUser}
                        /> :
                    configuringDevice ?
                        <ConfigureDevice selectedDeviceID ={selectedDeviceID} configureDevice={configureDevice} /> :
                    configuringSensor ?
                        <ConfigureSensor selectedSensorID ={selectedSensorID} configureSensor={configureSensor} /> :

                    <>
                        <Selectors 
                            selectedDeviceID={selectedDeviceID} 
                            selectSensor={selectSensor}
                            selectedSensorID={selectedSensorID}
                            selectTimeFrame={selectTimeFrame}
                            selectedTimeFrame={selectedTimeFrame}
                            selectChannel={selectChannel}
                            // addSensor={addSensor}
                            isAdmin={isAdmin}
                            configureSensor={configureSensor}
                            isLoggingOut={isLoggingOut}
                        />
                        <Graph 
                            selectedSensorID={selectedSensorID}
                            selectedChannelID={selectedChannelID}
                            selectedTimeFrame={selectedTimeFrame}
                            isLoggingOut={isLoggingOut}
                        />
                    </>

                }
            </div>
        </div>  
    )
}

export default HecateBody;