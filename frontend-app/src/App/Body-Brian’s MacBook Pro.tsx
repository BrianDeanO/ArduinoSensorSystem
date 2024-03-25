import React, { useEffect, useState } from "react";
import Graph from "./Graph.tsx";
import { SelectedDevice, SelectedSensor, SelectedTimeFrame, SelectedChannel } from "../interfaces";
import { localStorageTitles } from "../variables.js";

import Devices from "./Devices.tsx";
import Selectors from "./Selectors.tsx";
import ConfigureDevice from "./ConfigPages/ConfigureDevice.tsx";
import ConfigureSensor from "./ConfigPages/ConfigureSensor.tsx";
import ManageUsers from "./ConfigPages/ManageUsers.tsx";

interface BodyProps {
    loggedInUserID: number;
    isAdmin: boolean;
    manageUsers: ((isManagingUsers: boolean) => void);
    isManagingUsers: boolean;
    isLoggingOut: boolean;
}

const Body: React.FC<BodyProps>  = ({
    loggedInUserID,
    isAdmin,
    manageUsers,
    isManagingUsers,
    isLoggingOut
}: BodyProps) => {

    const deviceJSON = localStorage.getItem(localStorageTitles.selectedDevice);
    const sensorJSON = localStorage.getItem(localStorageTitles.selectedSensor);
    const timeFrameJSON = localStorage.getItem(localStorageTitles.selectedTimeFrame);
    const channelJSON = localStorage.getItem(localStorageTitles.selectedChannel);

    const selectedDevice: SelectedDevice = (deviceJSON !== null) ? JSON.parse(deviceJSON) : {};
    const selectedSensor: SelectedSensor = (sensorJSON !== null) ? JSON.parse(sensorJSON) : {};
    const parsedTimeFrame: SelectedTimeFrame = (timeFrameJSON !== null) ? JSON.parse(timeFrameJSON) : {};
    const parsedChannel: SelectedChannel = (channelJSON !== null) ? JSON.parse(channelJSON) : {};

    const [selectedDeviceID, setSelectedDeviceID] = useState((Object.keys(selectedDevice).length !== 0) ? selectedDevice.selectedDeviceID : 0);
    const [selectedSensorID, setSelectedSensorID] = useState((Object.keys(selectedSensor).length !== 0)  ? selectedSensor.selectedSensorID : 0);
    const [selectedTimeFrame, setSelectedTimeFrame] = useState((Object.keys(parsedTimeFrame).length !== 0) ? parsedTimeFrame.selectedTimeFrame : 0);
    const [selectedChannelID, setSelectedChannelID] = useState((Object.keys(parsedChannel).length !== 0) ? parsedChannel.selectedChannelID : -1);

    const [configuringDevice, setConfiguringDevice] = useState(false);
    const [configuringSensor, setConfiguringSensor] = useState(false);

    const [isAddingUser, setIsAddingUser] = useState(false);
    const [isUpdatingUser, setIsUpdatingUser] = useState(false);

    useEffect(() => {
        if(isLoggingOut) {
            setSelectedDeviceID(0);
            setSelectedTimeFrame(0);
            setSelectedSensorID(0);
            setSelectedChannelID(-1);
            setConfiguringSensor(false);
            setConfiguringDevice(false);
            setIsAddingUser(false);
            setIsUpdatingUser(false);
        }
    }, [isLoggingOut])

    useEffect(() => {
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
    }

    const selectDevice = (selectedDeviceID: number, resetSensorID: boolean) => {
        setSelectedDeviceID(selectedDeviceID);

        if(resetSensorID) {
            setSelectedSensorID(0);
            setSelectedChannelID(0);
            setSelectedTimeFrame(0);
        }
    }

    const selectChannel = (selectedChannelID: number) => {
        setSelectedChannelID(selectedChannelID);
    }

    const selectTimeFrame = (selectedTimeFrame: number) => {
        setSelectedTimeFrame(selectedTimeFrame);
    }

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
    }   

    const updateUser = (updatingUser: boolean) => {
        setIsUpdatingUser(updatingUser);
    }

    return (
        <div className="Body">
            <Devices 
                selectDevice={selectDevice} 
                loggedInUserID={loggedInUserID}
                selectedDeviceID={selectedDeviceID}
                configureDevice={configureDevice}
                isAdmin={isAdmin}
                configuringSensor={configuringSensor}
                isManagingUsers={isManagingUsers}
                isLoggingOut={isLoggingOut}
            />
            <div className="MainSelectorAndGraphBox">
                {
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
                            selectedChannelID={selectedChannelID}
                            selectTimeFrame={selectTimeFrame}
                            selectedTimeFrame={selectedTimeFrame}
                            selectChannel={selectChannel}
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

export default Body;