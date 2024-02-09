import React, { useEffect, useState, Component, SyntheticEvent } from "react";
// import Selectors from "./DataAndTimeSelectors/selectors.js";
import TemperatureVisualizationBox from "./Visualizations/TemperatureVisualizationBox.js";
import Graph from "./Graph.tsx";
// import { variables, dataObject } from "../variables.js";
import { DeviceType, SensorType, SelectedDevice, SelectedSensor } from "../interfaces";
import { localStorageTitles } from "../Variables.js";

import Devices from "./Devices.tsx";
import Selectors from "./Selectors.tsx";

interface HecateBodyProps {
    loggedInUserID: number;
}

const HecateBody: React.FC<HecateBodyProps>  = ({
    loggedInUserID
}: HecateBodyProps) => {

    const deviceJSON = localStorage.getItem(localStorageTitles.selectedDevice);
    const sensorJSON = localStorage.getItem(localStorageTitles.selectedSensor);

    const selectedDevice: SelectedDevice = (deviceJSON !== null) ? JSON.parse(deviceJSON) : {};
    const selectedSensor: SelectedSensor = (sensorJSON !== null) ? JSON.parse(sensorJSON) : {};


    const [selectedDeviceID, setSelectedDeviceID] = useState(selectedDevice ? selectedDevice.selectedDeviceID : 0);
    const [selectedSensorID, setSelectedSensorID] = useState(selectedSensor ? selectedSensor.selectedSensorID : 0);

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

    const selectSensor = (selectedSensorID: number) => {
        setSelectedSensorID(selectedSensorID);
        console.log('SENSOR SET IN BODY', selectedSensorID);
    }

    const selectDevice = (selectedDeviceID: number) => {
        setSelectedDeviceID(selectedDeviceID);
        
        console.log('DEVICE SET IN BODY', selectedDeviceID);

        setSelectedSensorID(0);
    }

    return (
        <div className="HecateBody">
            <Devices 
                selectDevice={selectDevice} 
                loggedInUserID={loggedInUserID}
                selectedDeviceID={selectedDeviceID}
            />
            <div className="MainSelectorAndGraphBox">
                <Selectors 
                    selectedDeviceID={selectedDeviceID} 
                    selectSensor={selectSensor}
                />
                <Graph selectedSensorID={selectedSensorID}/>
            </div>
        </div>  
    )
}

export default HecateBody;