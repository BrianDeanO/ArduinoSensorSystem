import React, { useEffect, useState, Component, SyntheticEvent } from "react";
// import Selectors from "./DataAndTimeSelectors/selectors.js";
import TemperatureVisualizationBox from "./Visualizations/TemperatureVisualizationBox.js";
import Graph from "./Graph.tsx";
// import { variables, dataObject } from "../variables.js";
import { DeviceType, SensorType, SelectedDevice, SelectedSensor, SelectedTimeFrame } from "../interfaces";
import { localStorageTitles } from "../Variables.js";
import NewDevice from "./NewDevice.tsx";

import Devices from "./Devices.tsx";
import Selectors from "./Selectors.tsx";
import NewSensor from "./NewSensor.tsx";

interface HecateBodyProps {
    loggedInUserID: number;
}

const HecateBody: React.FC<HecateBodyProps>  = ({
    loggedInUserID
}: HecateBodyProps) => {

    const deviceJSON = localStorage.getItem(localStorageTitles.selectedDevice);
    const sensorJSON = localStorage.getItem(localStorageTitles.selectedSensor);
    const timeFrameJSON = localStorage.getItem(localStorageTitles.selectedTimeFrame);
    // const newDeviceJSON = localStorage.getItem(localStorageTitles.selectedTimeFrame); LATER ??????????????????????

    const selectedDevice: SelectedDevice = (deviceJSON !== null) ? JSON.parse(deviceJSON) : {};
    const selectedSensor: SelectedSensor = (sensorJSON !== null) ? JSON.parse(sensorJSON) : {};
    const parsedTimeFrame: SelectedTimeFrame = (timeFrameJSON !== null) ? JSON.parse(timeFrameJSON) : {};

    const [selectedDeviceID, setSelectedDeviceID] = useState((Object.keys(selectedDevice).length !== 0) ? selectedDevice.selectedDeviceID : 0);
    const [selectedSensorID, setSelectedSensorID] = useState((Object.keys(selectedSensor).length !== 0)  ? selectedSensor.selectedSensorID : 0);
    const [selectedTimeFrame, setSelectedTimeFrame] = useState((Object.keys(parsedTimeFrame).length !== 0) ? parsedTimeFrame.selectedTimeFrame : '---');


    // const [getSensorOverride, setGetSensorOverride] = useState(true);
    const [addingNewDevice, setAddingNewDevice] = useState(false);
    const [addingNewSensor, setAddingNewSensor] = useState(false);


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

    const selectSensor = (selectedSensorID: number) => {
        setSelectedSensorID(selectedSensorID);
        setSelectedTimeFrame('---');
        console.log('SENSOR SET IN BODY', selectedSensorID);
    }

    const selectDevice = (selectedDeviceID: number, resetSensorID: boolean) => {
        if(!addingNewDevice) {
            setSelectedDeviceID(selectedDeviceID);
            setSelectedTimeFrame('---');
            // setGetSensorOverride(true);
    
            if(resetSensorID) {
                setSelectedSensorID(0);
            }
            
            console.log('DEVICE SET IN BODY', selectedDeviceID);
            console.log('SENSOR RESET?', selectedSensorID);
        }
    }

    const selectTimeFrame = (selectedTimeFrame: string) => {
        setSelectedTimeFrame(selectedTimeFrame);
        console.log('SSETTING TIME FRAME IN BODY', selectedDeviceID);
    }

    const addDevice = (addingDevice: boolean) => {
        setAddingNewDevice(addingDevice);
        setSelectedDeviceID(0);
        setSelectedTimeFrame('---');
        setSelectedSensorID(0);
    }

    const addSensor = (addingSensor: boolean) => {
        setAddingNewSensor(addingSensor);
        setSelectedDeviceID(0);
        setSelectedTimeFrame('---');
        setSelectedSensorID(0);
    }

    console.log('BODY - SELECTED SENSOR ID - ', selectedSensorID)

    return (
        <div className="HecateBody">
            <Devices 
                selectDevice={selectDevice} 
                loggedInUserID={loggedInUserID}
                selectedDeviceID={selectedDeviceID}
                addDevice={addDevice}
            />
            <div className="MainSelectorAndGraphBox">
                {
                    addingNewDevice ? 
                        <NewDevice addDevice={addDevice}/> :
                    addingNewSensor ? 
                        <NewSensor addSensor={addSensor} /> :
                    <>
                        <Selectors 
                            selectedDeviceID={selectedDeviceID} 
                            selectSensor={selectSensor}
                            selectedSensorID={selectedSensorID}
                            selectTimeFrame={selectTimeFrame}
                            addSensor={addSensor}
                            // getSensorOverride={getSensorOverride}
                        />
                        <Graph 
                            selectedSensorID={selectedSensorID}
                            selectedTimeFrame={selectedTimeFrame}
                        />
                    </>

                }
            </div>
        </div>  
    )
}

export default HecateBody;