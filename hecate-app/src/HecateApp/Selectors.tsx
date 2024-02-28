import React, { useEffect, useState, useCallback } from "react";
import Graph from "./Graph.tsx";
import { DeviceType, SensorType } from "../interfaces";
import { timeFrameConstants, proxyURL, sensorTable } from "../variables.js";
import axios from "axios";

interface SelectorProps {
    selectedDeviceID: number;
    selectSensor: (selectedSensorID: number, resetChannelID: boolean, resetTimeFrame: boolean) => void;
    selectTimeFrame: (selectedTimeFrame: number) => void;
    selectedTimeFrame: number;
    selectedSensorID: number;
    // getSensorOverride: boolean
    // addSensor: (addingSensor: boolean) => void;
    selectChannel: (selectedChannelID: number) => void;
    isAdmin: boolean;
    configureSensor: (configuringSensor: boolean) => void;
    isLoggingOut: boolean;
}

const Selectors: React.FC<SelectorProps> = ({
    selectedDeviceID, 
    selectSensor,
    selectedSensorID,
    selectTimeFrame,
    selectedTimeFrame,
    selectChannel,
    // addSensor,
    isAdmin,
    configureSensor,
    isLoggingOut
}: SelectorProps) => {
    const [sensors, setSensors] = useState([] as SensorType[]);
    const [channels, setChannels] = useState([] as number[]);
    // const [selectedChannels, setSelectedChannels] = useState([] as number[]);

    console.log("SELECTORS")
    console.log('selectedDeviceID',selectedDeviceID)
    console.log('selectedSensorID',selectedSensorID)
    console.log('selectedTimeFrame',selectedTimeFrame)

    const getSensors = useCallback(async(selectedDeviceID: number) => {

        let tempSensors: SensorType[] = [];
        let tempChannels: number[] = [];

        if(selectedDeviceID !== 0 && !isLoggingOut) {
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
    
            tempSensors.forEach((sensor, i) => {
                tempChannels.push(sensor.channelCount);
            })
            console.log('tempChannels', tempChannels)
    
            tempChannels.forEach((sensor, i) => {
                console.log('CHANNEL ??????????', i)
            })
    
            setChannels(tempChannels);
            
            selectSensor(selectedSensorID || 0, false, false);
        } else {
            setSensors(tempSensors);
            setChannels(tempChannels);
        }

    }, [selectSensor, selectedSensorID, isLoggingOut])

    useEffect(() => {
        // if(getSensorOverride) {
        // }
        if(isLoggingOut) {
            selectSensor(0, true, true);
        }
        getSensors(selectedDeviceID);
        // setSelectedChannels(new Array(channels[selectedSensorID - 1]));
        // setSensors(sensorTable as SensorType[]);
    }, [ selectedDeviceID, getSensors, isLoggingOut, selectSensor])

    // console.log('CURRENTLY SELECTED SNEOSRS ID', selectedSensorID)
    // console.log('SENSORS - SELECTED SENSOR ID - ', selectedSensorID)

    // console.log('Array(channels[selectedSensorID - 1])', Array(channels[selectedSensorID - 1]));



    return (
        <div className="SubSelectorsBox">
            <div className="sensorSelectorMainBox">
                <div className="sensorTypeSelectorBox">
                    <div className="sensorTypeSelectorText">Sensor</div>
                    <select 
                        className="sensorTypeSelector" 
                        id="sensorType"
                        onChange={(e) => {
                            const tempStringID = (e.target as HTMLSelectElement).value;
                            // console.log('ON CHNACING SENSORS')
                            if(tempStringID === '---') {
                                selectSensor(0, true, true);
                            } else {
                                const tempSensorID = parseInt((tempStringID != null) ? tempStringID : "");
                                selectSensor(tempSensorID, true, true);
                                sensors.forEach((sensor, i) => {
                                    if(sensor.sensorID === tempSensorID) {
                                        setChannels(new Array(sensor.channelCount));
                                    }
                                })
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
                {
                    (isAdmin && (selectedSensorID !== 0)) ? 
                    <span 
                        className="ConfigureSensorButton"
                        data-value={selectedDeviceID}
                        onClick={(e) => {
                            // const tempStringID = (e.target as HTMLElement).getAttribute("data-value");
                            configureSensor(true);
                    }}>
                        Configure Sensor
                    </span> : null
                }
            </div>
            
            <div className="channelTypeSelectorBox">
                <div className="channelTypeSelectorText">Sensor Channel</div>
                <select 
                    className="channelTypeSelector" 
                    id="channel"
                    onChange={(e) => {
                        const tempStringID = (e.target as HTMLSelectElement).value;
                        if(tempStringID === '---') {
                            selectChannel(0);
                        } else {
                            const tempChannelID = parseInt((tempStringID != null) ? tempStringID : "");
                            console.log('SELECTINC CAHNNEL', tempChannelID)
                            selectChannel(tempChannelID);
                        }
                    }}>
                        
                        <option value={'---'} selected={(channels.length === 0) || !(channels)}>---</option>
                        {
                            (selectedSensorID !== 0) ? 
                                channels.map((channel, i) => {
                                    return(
                                        <option value={i+1} key={i} >
                                            {i + 1}
                                        </option>
                                    )
                                })
                                : null
                        }
                </select>
            </div> 
            
            <div className="timeTypeSelectorBox">
                <div className="timeTypeSelectorText">Time Frame</div>
                <select 
                    className="timeTypeSelector" 
                    id="sensorType"
                    onChange={(e) => {
                        const timeFrameString = (e.target as HTMLSelectElement).value;
                        const tempTime = parseInt((timeFrameString != null) ? timeFrameString : "");
                        selectTimeFrame(tempTime);
                    }}>
                        <option value={timeFrameConstants.DAY} selected={selectedTimeFrame === timeFrameConstants.DAY}>
                            Past Day
                        </option>
                        <option value={timeFrameConstants.WEEK} selected={selectedTimeFrame === timeFrameConstants.WEEK}>
                            Past Week
                        </option>
                        <option value={timeFrameConstants.MONTH} selected={selectedTimeFrame === timeFrameConstants.MONTH}>
                            Past Month
                        </option>
                        <option value={timeFrameConstants.SIX_MONTHS} selected={selectedTimeFrame === timeFrameConstants.SIX_MONTHS}>
                            Past Six Months
                        </option>
                        <option value={timeFrameConstants.YEAR} selected={selectedTimeFrame === timeFrameConstants.YEAR}>
                            Past Year
                        </option>
                        <option 
                            value={0}
                            selected={selectedTimeFrame === 0}>
                            Lifetime
                        </option>
                </select>
            </div>
        </div>
    )
}

export default Selectors;