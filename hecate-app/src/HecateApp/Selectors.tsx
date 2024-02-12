import React, { useEffect, useState, useCallback } from "react";
import Graph from "./Graph.tsx";
import { DeviceType, SensorType } from "../interfaces";
import { timeFrameConstants, proxyURL } from "../Variables.js";
import axios from "axios";

interface SelectorProps {
    selectedDeviceID: number;
    selectSensor: (selectedSensorID: number) => void;
    selectTimeFrame: (selectedTimeFrame: string) => void;
    selectedSensorID: number;
    // getSensorOverride: boolean
    addSensor: (addingSensor: boolean) => void;
}

const Selectors: React.FC<SelectorProps> = ({
    selectedDeviceID, 
    selectSensor,
    selectedSensorID,
    selectTimeFrame,
    // getSensorOverride
    addSensor
}: SelectorProps) => {    
    const [sensors, setSensors] = useState([] as SensorType[]);


    const getSensors = useCallback(async(selectedDeviceID: number) => {

        let tempSensors: SensorType[] = [];

        await axios({
            method: 'get',
            url: `${proxyURL}/api/Device/${selectedDeviceID}/Sensors`,
        })
        .then(function (response) {
            // console.log('response', response);
            // setSensors(response.data);
            tempSensors = response.data;
            // console.log('SENSORS FROM AXIOS', tempSensors)
        }).catch(error => {
            console.log(error);
        })

        setSensors(tempSensors);
        
        selectSensor(selectedSensorID || 0);

    }, [selectSensor, selectedSensorID])

    useEffect(() => {
        // if(getSensorOverride) {
        // }
        getSensors(selectedDeviceID);
    }, [ selectedDeviceID, getSensors ])

    // console.log('CURRENTLY SELECTED SNEOSRS ID', selectedSensorID)
    console.log('SENSORS - SELECTED SENSOR ID - ', selectedSensorID)

    return (
        <div className="SubSelectorsBox">
            <div className="AddSensorBox">
                <span 
                    className="AddSensorButton"
                    onClick={(e) => {
                        addSensor(true);
                }}>
                    Add New Sensor
                </span>   
            </div>
            <div className="sensorTypeSelectorBox">
                <div className="sensorTypeSelectorText">Sensor</div>
                <select 
                    className="sensorTypeSelector" 
                    id="sensorType"
                    onChange={(e) => {
                        const tempStringID = (e.target as HTMLSelectElement).value;
                        console.log('ON CHNACING SENSORS')
                        if(tempStringID === '---') {
                            selectSensor(0);
                        } else {
                            const tempSensorID = parseInt((tempStringID != null) ? tempStringID : "");
                            selectSensor(tempSensorID);
                        }
                    }}>
                        <option value={'---'} selected={selectedSensorID === 0}>---</option>
                        {
                            sensors.map((sensor, index) => {
                                return (
                                    <option value={sensor.sensorID} key={index} selected={sensor.sensorID === selectedSensorID}>
                                        {sensor.sensorName}
                                    </option>
                                )
                            })
                        }
                </select>
            </div>
            {/* <div className="channelTypeSelectorBox">
                <div className="channelTypeSelectorText">Sensor Channel</div>
                <select 
                    className="channelTypeSelector" 
                    id="channel"
                    onClick={(e) => {
                        //@ts-ignore
                        setSelectedChannel(parseInt(e.target.value));
                    }}>
                        <option value={0}>All</option>
                        {
                            channels.map((channel, index) => {
                                return (
                                    <option value={channel} key={channel}>
                                        {channel}
                                    </option>
                                )
                                
                            })
                        }
                </select>
            </div> */}
            <div className="timeTypeSelectorBox">
                <div className="timeTypeSelectorText">Time Frame</div>
                <select 
                    className="timeTypeSelector" 
                    id="sensorType"
                    defaultValue={'Lifetime'}
                    onChange={(e) => {
                        const timeFrameString = (e.target as HTMLSelectElement).value;
                        selectTimeFrame(timeFrameString);
                    }}>
                        {/* timeFrameConstants can be used here to subtract times? */}
                        <option value={'Past Day'}>Past Day</option>
                        <option value={'Past Week'}>Past Week</option>
                        <option value={'Past Month'}>Past Month</option>
                        <option value={'Past Six Months'}>Past Six Months</option>
                        <option value={'Lifetime'}>Lifetime</option>
                </select>
            </div>
        </div>
    )
}

export default Selectors;