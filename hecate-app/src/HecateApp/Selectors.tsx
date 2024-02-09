import React, { useEffect, useState, Component, SyntheticEvent } from "react";
import Graph from "./Graph.tsx";
import { DeviceType, SensorType } from "../interfaces";

interface SelectorProps {
    selectedDeviceID: number;
    selectSensor: (selectedSensorID: number) => void;
}

const Selectors: React.FC<SelectorProps> = ({
    selectedDeviceID, 
    selectSensor
}: SelectorProps) => {    

    const [sensors, setSensors] = useState([] as SensorType[]);
    let sensor = 'NOT CLIECKTE';
    

    useEffect(() => {
        
    }, [selectedDeviceID])

    const getSensors = (selectedSensorID: number) => {
        // setLoggedIn(e.target.value);
        // setSelectedSensorID(selectedSensorID);
        console.log('SENSOR SET IN BODY', selectedSensorID);

        /**********
          USE AXIOS TO GET SENSORS

        *************/


        const sensorArray = [
            {
                sensorID: 1,
                sensorName: 'SENSOR_1'
            },
            {
                sensorID: 2,
                sensorName: 'SENSOR_2'
            }
        ] as SensorType[];

        setSensors(sensorArray);
    }

    return (
        <div className="SubSelectorsBox">
            SUBslecogrs {`\n\n`}
            {`DEVICE ID - ${selectedDeviceID  || 0}`}
            {`\n`}
            <button 
                className="mainLoginButton"
                onClick={(e) => {
                    selectSensor(1);
                    sensor = 'sensors clciekd';
                }}
                >
                    {sensor}
            </button>
            
        </div>
    )
}

export default Selectors;